using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.SqlClient;
using System.Threading;
using System.Runtime.Versioning;
using LinqToSqlShared.Mapping;
using System.Runtime.CompilerServices;

namespace System.Data.Linq.Mapping
{
	internal sealed class MappedTable : MetaTable
	{
		MappedMetaModel model;
		TableMapping mapping;
		MetaType rowType;
		bool hasMethods;
		MethodInfo insertMethod;
		MethodInfo updateMethod;
		MethodInfo deleteMethod;

		[ResourceExposure(ResourceScope.Assembly | ResourceScope.Machine)] // Parameter contains various type references.
		[ResourceConsumption(ResourceScope.Assembly | ResourceScope.Machine)] // MappedRootType constructor call.
		internal MappedTable(MappedMetaModel model, TableMapping mapping, Type rowType)
		{
			this.model = model;
			this.mapping = mapping;
			this.rowType = new MappedRootType(model, this, mapping.RowType, rowType);
		}
		public override MetaModel Model
		{
			get { return this.model; }
		}
		public override string TableName
		{
			get { return this.mapping.TableName; }
		}
		public override MetaType RowType
		{
			get { return this.rowType; }
		}
		public override MethodInfo InsertMethod
		{
			get
			{
				this.InitMethods();
				return this.insertMethod;
			}
		}
		public override MethodInfo UpdateMethod
		{
			get
			{
				this.InitMethods();
				return this.updateMethod;
			}
		}
		public override MethodInfo DeleteMethod
		{
			get
			{
				this.InitMethods();
				return this.deleteMethod;
			}
		}
		private void InitMethods()
		{
			if(!this.hasMethods)
			{
				this.insertMethod = MethodFinder.FindMethod(
					this.model.ContextType,
					"Insert" + rowType.Name,
					BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
					new Type[] { rowType.Type }
					);
				this.updateMethod = MethodFinder.FindMethod(
					this.model.ContextType,
					"Update" + rowType.Name,
					BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
					new Type[] { rowType.Type }
					);
				this.deleteMethod = MethodFinder.FindMethod(
					this.model.ContextType,
					"Delete" + rowType.Name,
					BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
					new Type[] { rowType.Type }
					);
				this.hasMethods = true;
			}
		}
	}
}

