using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Common
{
	internal class SqlParameterInfo
	{
		#region Member Declarations
		private SqlParameter parameter;
		private object value;
		private Delegate accessor;
		#endregion

		internal SqlParameterInfo(SqlParameter parameter, Delegate accessor)
		{
			this.parameter = parameter;
			this.accessor = accessor;
		}
		internal SqlParameterInfo(SqlParameter parameter, object value)
		{
			this.parameter = parameter;
			this.value = value;
		}
		internal SqlParameterInfo(SqlParameter parameter)
		{
			this.parameter = parameter;
		}

		internal SqlParameterType Type
		{
			get
			{
				if(this.accessor != null)
				{
					return SqlParameterType.UserArgument;
				}
#warning [FB] REFACTOR: SQL SERVER SPECIFIC 
				if(this.parameter.Name == "@ROWCOUNT")
				{
					return SqlParameterType.PreviousResult;
				}
				return SqlParameterType.Value;
			}
		}
		internal SqlParameter Parameter
		{
			get { return this.parameter; }
		}
		internal Delegate Accessor
		{
			get { return this.accessor; }
		}
		internal object Value
		{
			get { return this.value; }
		}
	}
}