using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Data.Linq.Provider;
using System.Data.Linq.SqlClient;
using System.Threading;
using System.Runtime.Versioning;
using LinqToSqlShared.Mapping;
using System.Runtime.CompilerServices;

namespace System.Data.Linq.Mapping
{
	internal class MappedFunction : MetaFunction
	{
		MetaModel model;
		FunctionMapping map;
		MethodInfo method;
		ReadOnlyCollection<MetaParameter> parameters;
		MetaParameter returnParameter;
		ReadOnlyCollection<MetaType> rowTypes;
		static ReadOnlyCollection<MetaParameter> _emptyParameters = new List<MetaParameter>(0).AsReadOnly();
		static ReadOnlyCollection<MetaType> _emptyTypes = new List<MetaType>(0).AsReadOnly();

		[ResourceExposure(ResourceScope.Assembly | ResourceScope.Machine)] // map parameter contains type names.
		[ResourceConsumption(ResourceScope.Assembly | ResourceScope.Machine)] // FindType method call.
		internal MappedFunction(MappedMetaModel model, FunctionMapping map, MethodInfo method)
		{
			this.model = model;
			this.map = map;
			this.method = method;
			this.rowTypes = _emptyTypes;

			if(map.Types.Count == 0 && this.method.ReturnType == typeof(IMultipleResults))
			{
				throw Error.NoResultTypesDeclaredForFunction(method.Name);
			}
			else if(map.Types.Count > 1 && this.method.ReturnType != typeof(IMultipleResults))
			{
				throw Error.TooManyResultTypesDeclaredForFunction(method.Name);
			}
			else if(map.Types.Count == 1 && this.method.ReturnType != typeof(IMultipleResults))
			{
				Type elementType = TypeSystem.GetElementType(method.ReturnType);
				this.rowTypes = new List<MetaType>(1) { this.GetMetaType(map.Types[0], elementType) }.AsReadOnly();
			}
			else if(map.Types.Count > 0)
			{
				List<MetaType> rowTypes = new List<MetaType>();
				foreach(TypeMapping rtm in map.Types)
				{
					Type elementType = model.FindType(rtm.Name);
					if(elementType == null)
					{
						throw Error.CouldNotFindElementTypeInModel(rtm.Name);
					}
					MetaType mt = this.GetMetaType(rtm, elementType);
					// Only add unique meta types
					if(!rowTypes.Contains(mt))
					{
						rowTypes.Add(mt);
					}
				}
				this.rowTypes = rowTypes.AsReadOnly();
			}
			else if(map.FunReturn != null)
			{
				this.returnParameter = new MappedReturnParameter(method.ReturnParameter, map.FunReturn);
			}

			// Parameters.
			ParameterInfo[] pis = this.method.GetParameters();
			if(pis.Length > 0)
			{
				List<MetaParameter> mps = new List<MetaParameter>(pis.Length);
				if(this.map.Parameters.Count != pis.Length)
				{
					throw Error.IncorrectNumberOfParametersMappedForMethod(this.map.MethodName);
				}
				for(int i = 0; i < pis.Length; i++)
				{
					mps.Add(new MappedParameter(pis[i], this.map.Parameters[i]));
				}
				this.parameters = mps.AsReadOnly();
			}
			else
			{
				this.parameters = _emptyParameters;
			}
		}
		/// <summary>
		/// For the specified type, if it is a mapped type, use the Table
		/// metatype to get the correct inheritance metatype,
		/// otherwise create a new meta type.
		[ResourceExposure(ResourceScope.Assembly | ResourceScope.Machine)] // Parameter contains various type references.
		[ResourceConsumption(ResourceScope.Assembly | ResourceScope.Machine)] // MappedRootType constructor call.        
		private MetaType GetMetaType(TypeMapping tm, Type elementType)
		{
			MetaTable tbl = model.GetTable(elementType);
			if(tbl != null)
			{
				return tbl.RowType.GetInheritanceType(elementType);
			}
			return new MappedRootType((MappedMetaModel)model, null, tm, elementType);
		}
		public override ReadOnlyCollection<MetaParameter> Parameters
		{
			get { return this.parameters; }
		}
		public override string MappedName
		{
			get { return this.map.Name; }
		}
		public override MethodInfo Method
		{
			get { return this.method; }
		}
		public override MetaModel Model
		{
			get { return this.model; }
		}
		public override string Name
		{
			get { return this.method.Name; }
		}
		public override bool IsComposable
		{
			get { return this.map.IsComposable; }
		}
		public override MetaParameter ReturnParameter
		{
			get { return this.returnParameter; }
		}
		public override bool HasMultipleResults
		{
			get { return this.method.ReturnType == typeof(IMultipleResults); }
		}
		public override ReadOnlyCollection<MetaType> ResultRowTypes
		{
			get { return this.rowTypes; }
		}
	}
}

