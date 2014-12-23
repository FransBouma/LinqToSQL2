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
	internal class ParameterMapping
	{
		string name;
		string parameterName;
		string dbType;
		MappingParameterDirection direction;

		internal string Name
		{
			get { return this.name; }
			set { this.name = value; }
		}

		internal string ParameterName
		{
			get { return this.parameterName; }
			set { this.parameterName = value; }
		}

		internal string DbType
		{
			get { return this.dbType; }
			set { this.dbType = value; }
		}

		public string XmlDirection
		{
			get { return this.direction == MappingParameterDirection.In ? null : this.direction.ToString(); }
			set
			{
				this.direction = (value == null)
					? MappingParameterDirection.In
					: (MappingParameterDirection)Enum.Parse(typeof(MappingParameterDirection), value, true);
			}
		}

		public MappingParameterDirection Direction
		{
			get { return this.direction; }
			set { this.direction = value; }
		}
	}
}

