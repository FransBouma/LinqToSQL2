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
	internal sealed class MappedParameter : MetaParameter
	{
		private ParameterInfo parameterInfo;
		private ParameterMapping map;

		public MappedParameter(ParameterInfo parameterInfo, ParameterMapping map)
		{
			this.parameterInfo = parameterInfo;
			this.map = map;
		}
		public override ParameterInfo Parameter
		{
			get { return this.parameterInfo; }
		}
		public override string Name
		{
			get { return this.parameterInfo.Name; }
		}
		public override string MappedName
		{
			get { return this.map.Name; }
		}
		public override Type ParameterType
		{
			get { return this.parameterInfo.ParameterType; }
		}
		public override string DbType
		{
			get { return this.map.DbType; }
		}
	}
}

