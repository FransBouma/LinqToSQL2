using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Data.Linq;
using System.Threading;
using LinqToSqlShared.Mapping;
using System.Runtime.CompilerServices;
using System.Data.Linq.Provider.Common;

namespace System.Data.Linq.Mapping
{
	internal sealed class AttributedMetaFunction : MetaFunction
	{
		private AttributedMetaModel model;
		private MethodInfo methodInfo;
		private FunctionAttribute functionAttrib;
		private MetaParameter returnParameter;
		private ReadOnlyCollection<MetaParameter> parameters;
		private ReadOnlyCollection<MetaType> rowTypes;
		static ReadOnlyCollection<MetaParameter> _emptyParameters = new List<MetaParameter>(0).AsReadOnly();
		static ReadOnlyCollection<MetaType> _emptyTypes = new List<MetaType>(0).AsReadOnly();

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="metaType">The parent meta type.</param>
		/// <param name="mi">The method info.</param>
		public AttributedMetaFunction(AttributedMetaModel model, MethodInfo mi)
		{
			this.model = model;
			this.methodInfo = mi;
			this.rowTypes = _emptyTypes;

			this.functionAttrib = Attribute.GetCustomAttribute(mi, typeof(FunctionAttribute), false) as FunctionAttribute;
			System.Diagnostics.Debug.Assert(functionAttrib != null);

			// Gather up all mapped results
			ResultTypeAttribute[] attrs = (ResultTypeAttribute[])Attribute.GetCustomAttributes(mi, typeof(ResultTypeAttribute));
			if(attrs.Length == 0 && mi.ReturnType == typeof(IMultipleResults))
			{
				throw Error.NoResultTypesDeclaredForFunction(mi.Name);
			}
			else if(attrs.Length > 1 && mi.ReturnType != typeof(IMultipleResults))
			{
				throw Error.TooManyResultTypesDeclaredForFunction(mi.Name);
			}
			else if(attrs.Length <= 1 && mi.ReturnType.IsGenericType &&
					 (mi.ReturnType.GetGenericTypeDefinition() == typeof(IEnumerable<>) ||
					  mi.ReturnType.GetGenericTypeDefinition() == typeof(ISingleResult<>) ||
					  mi.ReturnType.GetGenericTypeDefinition() == typeof(IQueryable<>)))
			{
				Type elementType = TypeSystem.GetElementType(mi.ReturnType);
				this.rowTypes = new List<MetaType>(1) { this.GetMetaType(elementType) }.AsReadOnly();
			}
			else if(attrs.Length > 0)
			{
				List<MetaType> rowTypes = new List<MetaType>();
				foreach(ResultTypeAttribute rat in attrs)
				{
					Type type = rat.Type;
					MetaType mt = this.GetMetaType(type);
					// Only add unique meta types
					if(!rowTypes.Contains(mt))
					{
						rowTypes.Add(mt);
					}
				}
				this.rowTypes = rowTypes.AsReadOnly();
			}
			else
			{
				this.returnParameter = new AttributedMetaParameter(this.methodInfo.ReturnParameter);
			}

			// gather up all meta parameter
			ParameterInfo[] pis = mi.GetParameters();
			if(pis.Length > 0)
			{
				List<MetaParameter> mps = new List<MetaParameter>(pis.Length);
				for(int i = 0, n = pis.Length; i < n; i++)
				{
					AttributedMetaParameter metaParam = new AttributedMetaParameter(pis[i]);
					mps.Add(metaParam);
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
		/// </summary>
		private MetaType GetMetaType(Type type)
		{
			// call no-lock version of GetTable since this function is called only in constructor
			// and constructor is only called by function that already has a lock.
			MetaTable tbl = model.GetTableNoLocks(type);
			if(tbl != null)
			{
				return tbl.RowType.GetInheritanceType(type);
			}
			return new AttributedRootType(model, null, type);
		}

		public override MetaModel Model
		{
			get { return this.model; }
		}
		public override MethodInfo Method
		{
			get { return this.methodInfo; }
		}
		public override string Name
		{
			get { return this.methodInfo.Name; }
		}
		public override string MappedName
		{
			get
			{
				if(!string.IsNullOrEmpty(this.functionAttrib.Name))
				{
					return this.functionAttrib.Name;
				}
				return this.methodInfo.Name;
			}
		}
		public override bool IsComposable
		{
			get { return this.functionAttrib.IsComposable; }
		}
		public override ReadOnlyCollection<MetaParameter> Parameters
		{
			get { return this.parameters; }
		}
		public override MetaParameter ReturnParameter
		{
			get { return this.returnParameter; }
		}
		public override bool HasMultipleResults
		{
			get { return this.methodInfo.ReturnType == typeof(IMultipleResults); }
		}
		public override ReadOnlyCollection<MetaType> ResultRowTypes
		{
			get { return this.rowTypes; }
		}
	}
}

