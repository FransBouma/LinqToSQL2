using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Data;
using System.Data.Common;
using System.Data.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Data.Linq.Provider.Common
{
	using System.Diagnostics;
	using System.Data.Linq.Provider.Interfaces;
	using System.Data.Linq.Provider.NodeTypes;
	using System.Data.Linq.Provider.Visitors;

	internal class ObjectReaderCompiler : IObjectReaderCompiler
	{
		#region Member Declarations
		private Type dataReaderType;
		private IDataServices services;

		private MethodInfo miDRisDBNull;
		private MethodInfo miBRisDBNull;
		private FieldInfo readerField;
		private FieldInfo bufferReaderField;

		private FieldInfo ordinalsField;
		private FieldInfo globalsField;
		private FieldInfo argsField;
		#endregion

		#region DEBUG related capture code
#if DEBUG
		static AssemblyBuilder captureAssembly;
		static ModuleBuilder captureModule;
		static string captureAssemblyFilename;
		static int iCaptureId;

		internal static int GetNextId()
		{
			return iCaptureId++;
		}

		internal static ModuleBuilder CaptureModule
		{
			get { return captureModule; }
		}

		[ResourceExposure(ResourceScope.Machine)] // filename parameter later used by other methods.
		internal static void StartCaptureToFile(string filename)
		{
			if(captureAssembly == null)
			{
				string dir = System.IO.Path.GetDirectoryName(filename);
				if(dir.Length == 0) dir = null;
				string name = System.IO.Path.GetFileName(filename);
				AssemblyName assemblyName = new AssemblyName(System.IO.Path.GetFileNameWithoutExtension(name));
				captureAssembly = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Save, dir);
				captureModule = captureAssembly.DefineDynamicModule(name);
				captureAssemblyFilename = filename;
			}
		}

		[ResourceExposure(ResourceScope.None)] // Exposure is via StartCaptureToFile method.
		[ResourceConsumption(ResourceScope.Machine, ResourceScope.Machine)] // Assembly.Save method call.
		internal static void StopCapture()
		{
			if(captureAssembly != null)
			{
				captureAssembly.Save(captureAssemblyFilename);
				captureAssembly = null;
			}
		}

		internal static void SetMaxReaderCacheSize(int size)
		{
			if(size <= 1)
			{
				throw Error.ArgumentOutOfRange("size");
			}
			maxReaderCacheSize = size;
		}
#endif
		#endregion

		#region Statics
		static LocalDataStoreSlot cacheSlot = Thread.AllocateDataSlot();
		static int maxReaderCacheSize = 10;
		#endregion

		/// <summary>
		/// Static Ctor to make sure the static vars are properly initialized for multi-threading purposes. 
		/// </summary>
		static ObjectReaderCompiler()
		{
		}

		internal ObjectReaderCompiler(Type dataReaderType, IDataServices services)
		{
			this.dataReaderType = dataReaderType;
			this.services = services;

			this.miDRisDBNull = dataReaderType.GetMethod("IsDBNull", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			this.miBRisDBNull = typeof(DbDataReader).GetMethod("IsDBNull", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

			Type orbType = typeof(ObjectMaterializer<>).MakeGenericType(this.dataReaderType);
			this.ordinalsField = orbType.GetField("Ordinals", BindingFlags.Instance | BindingFlags.Public);
			this.globalsField = orbType.GetField("Globals", BindingFlags.Instance | BindingFlags.Public);
			this.argsField = orbType.GetField("Arguments", BindingFlags.Instance | BindingFlags.Public);
			this.readerField = orbType.GetField("DataReader", BindingFlags.Instance | BindingFlags.Public);
			this.bufferReaderField = orbType.GetField("BufferReader", BindingFlags.Instance | BindingFlags.Public);

			System.Diagnostics.Debug.Assert(
				this.miDRisDBNull != null &&
				this.miBRisDBNull != null &&
				this.readerField != null &&
				this.bufferReaderField != null &&
				this.ordinalsField != null &&
				this.globalsField != null &&
				this.argsField != null
			);
		}

		[ResourceExposure(ResourceScope.None)] // Consumed by Thread.AllocateDataSource result being unique.
		[ResourceConsumption(ResourceScope.AppDomain, ResourceScope.AppDomain)] // Thread.GetData method call.
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public IObjectReaderFactory Compile(SqlExpression expression, Type elementType)
		{
			object mapping = this.services.Context.Mapping.Identity;
			DataLoadOptions options = this.services.Context.LoadOptions;
			IObjectReaderFactory factory = null;
			ObjectReaderFactoryCache cache = null;
			bool canBeCompared = SqlProjectionComparer.CanBeCompared(expression);
			if(canBeCompared)
			{
				cache = (ObjectReaderFactoryCache)Thread.GetData(cacheSlot);
				if(cache == null)
				{
					cache = new ObjectReaderFactoryCache(maxReaderCacheSize);
					Thread.SetData(cacheSlot, cache);
				}
				factory = cache.GetFactory(elementType, this.dataReaderType, mapping, options, expression);
			}
			if(factory == null)
			{
				DynamicTypeGenerator gen = new DynamicTypeGenerator(this, elementType);
#if DEBUG
				if(ObjectReaderCompiler.CaptureModule != null)
				{
					this.CompileCapturedMethod(gen, expression, elementType);
				}
#endif
				DynamicMethod dm = this.CompileDynamicMethod(gen, expression, elementType);
				Type fnMatType = typeof(Func<,>).MakeGenericType(typeof(ObjectMaterializer<>).MakeGenericType(this.dataReaderType), elementType);
				var fnMaterialize = (Delegate)dm.CreateDelegate(fnMatType);

				Type factoryType = typeof(ObjectReaderFactory<,>).MakeGenericType(this.dataReaderType, elementType);
				factory = (IObjectReaderFactory)Activator.CreateInstance(
					factoryType, BindingFlags.Instance | BindingFlags.NonPublic, null,
					new object[] { fnMaterialize, gen.NamedColumns, gen.Globals, gen.Locals }, null
					);

				if(canBeCompared)
				{
					expression = new SourceExpressionRemover().VisitExpression(expression);
					cache.AddFactory(elementType, this.dataReaderType, mapping, options, expression, factory);
				}
			}
			return factory;
		}


		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public IObjectReaderSession CreateSession(DbDataReader reader, IReaderProvider provider, object[] parentArgs, object[] userArgs, ICompiledSubQuery[] subQueries)
		{
			Type sessionType = typeof(ObjectReaderSession<>).MakeGenericType(this.dataReaderType);
			return (IObjectReaderSession)Activator.CreateInstance(sessionType, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null,
				new object[] { reader, provider, parentArgs, userArgs, subQueries }, null);
		}

#if DEBUG
		private void CompileCapturedMethod(DynamicTypeGenerator gen, SqlExpression expression, Type elementType)
		{
			TypeBuilder tb = ObjectReaderCompiler.CaptureModule.DefineType("reader_type_" + ObjectReaderCompiler.GetNextId());
			MethodBuilder mb = tb.DefineMethod(
				"Read_" + elementType.Name,
				MethodAttributes.Static | MethodAttributes.Public,
				CallingConventions.Standard,
				elementType,
				new Type[] { typeof(ObjectMaterializer<>).MakeGenericType(this.dataReaderType) }
				);
			gen.GenerateBody(mb.GetILGenerator(), (SqlExpression)SqlDuplicator.Copy(expression));
			tb.CreateType();
		}
#endif

		private DynamicMethod CompileDynamicMethod(DynamicTypeGenerator gen, SqlExpression expression, Type elementType)
		{
			Type objectReaderType = typeof(ObjectMaterializer<>).MakeGenericType(this.dataReaderType);
			DynamicMethod dm = new DynamicMethod(
				"Read_" + elementType.Name,
				elementType,
				new Type[] { objectReaderType },
				true
				);
			gen.GenerateBody(dm.GetILGenerator(), expression);
			return dm;
		}

		#region Property Declarations
		internal Type DataReaderType
		{
			get { return this.dataReaderType; }
		}

		internal FieldInfo ReaderField
		{
			get { return this.readerField; }
		}

		internal FieldInfo BufferReaderField
		{
			get { return this.bufferReaderField; }
		}

		internal FieldInfo OrdinalsField
		{
			get { return this.ordinalsField; }
		}

		internal FieldInfo ArgsField
		{
			get { return this.argsField; }
		}

		internal FieldInfo GlobalsField
		{
			get { return this.globalsField; }
		}

		internal IDataServices Services
		{
			get { return this.services; }
		}

		internal MethodInfo DataReaderIsDBNullMethod
		{
			get { return this.miDRisDBNull; }
		}

		internal MethodInfo BufferReaderIsDBNullMethod
		{
			get { return this.miBRisDBNull; }
		}
		#endregion
	}
}
