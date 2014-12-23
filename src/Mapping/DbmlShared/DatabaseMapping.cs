using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Globalization;

namespace LinqToSqlShared.Mapping
{
	/// <summary>
	/// DatabaseMapping and related classes represent a parsed version of the
	/// XML mapping string. This unvalidated intermediate representation is 
	/// necessary because unused mappings are intentially never validated.
	/// </summary>
	internal class DatabaseMapping
	{
		string databaseName;
		string provider;
		List<TableMapping> tables;
		List<FunctionMapping> functions;

		internal DatabaseMapping()
		{
			this.tables = new List<TableMapping>();
			this.functions = new List<FunctionMapping>();
		}

		internal string DatabaseName
		{
			get { return this.databaseName; }
			set { this.databaseName = value; }
		}

		internal string Provider
		{
			get { return this.provider; }
			set { this.provider = value; }
		}

		internal List<TableMapping> Tables
		{
			get { return this.tables; }
		}

		internal List<FunctionMapping> Functions
		{
			get { return this.functions; }
		}

		internal TableMapping GetTable(string tableName)
		{
			foreach(TableMapping tmap in this.tables)
			{
				if(string.Compare(tmap.TableName, tableName, StringComparison.Ordinal) == 0)
					return tmap;
			}
			return null;
		}

		internal TableMapping GetTable(Type rowType)
		{
			foreach(TableMapping tableMap in this.tables)
			{
				if(this.IsType(tableMap.RowType, rowType))
				{
					return tableMap;
				}
			}
			return null;
		}

		private bool IsType(TypeMapping map, Type type)
		{
			if(string.Compare(map.Name, type.Name, StringComparison.Ordinal) == 0
				|| string.Compare(map.Name, type.FullName, StringComparison.Ordinal) == 0
				|| string.Compare(map.Name, type.AssemblyQualifiedName, StringComparison.Ordinal) == 0)
				return true;
			foreach(TypeMapping subMap in map.DerivedTypes)
			{
				if(this.IsType(subMap, type))
					return true;
			}
			return false;
		}

		internal FunctionMapping GetFunction(string functionName)
		{
			foreach(FunctionMapping fmap in this.functions)
			{
				if(string.Compare(fmap.Name, functionName, StringComparison.Ordinal) == 0)
					return fmap;
			}
			return null;
		}
	}
}

