using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.Linq.Provider.Common;
using System.Data.Linq.Provider.Interfaces;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace System.Data.Linq.Provider.Common
{
	internal class ExecuteResult : IExecuteResult, IDisposable
	{
		DbCommand command;
		ReadOnlyCollection<SqlParameterInfo> parameters;
		IObjectReaderSession session;
		int iReturnParameter = -1;
		object value;
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "[....]: used in an assert in ReturnValue.set")]
		bool useReturnValue;
		bool isDisposed;

		internal ExecuteResult(DbCommand command, ReadOnlyCollection<SqlParameterInfo> parameters, IObjectReaderSession session, object value, bool useReturnValue)
			: this(command, parameters, session)
		{
			this.value = value;
			this.useReturnValue = useReturnValue;
			if(this.command != null && this.parameters != null && useReturnValue)
			{
				iReturnParameter = GetParameterIndex("@RETURN_VALUE");
			}
		}

		internal ExecuteResult(DbCommand command, ReadOnlyCollection<SqlParameterInfo> parameters, IObjectReaderSession session)
		{
			this.command = command;
			this.parameters = parameters;
			this.session = session;
		}

		internal ExecuteResult(DbCommand command, ReadOnlyCollection<SqlParameterInfo> parameters, IObjectReaderSession session, object value)
			: this(command, parameters, session, value, false)
		{
		}

		[SuppressMessage("Microsoft.Maintainability", "CA1500:VariableNamesShouldNotMatchFieldNames", MessageId = "value", Justification = "FxCop Error -- False positive during code analysis")]
		public object ReturnValue
		{
			get
			{
				if(this.iReturnParameter >= 0)
				{
					return this.GetParameterValue(this.iReturnParameter);
				}
				return this.value;
			}
			internal set
			{
				Debug.Assert(!useReturnValue);
				this.value = value;
			}
		}

		private int GetParameterIndex(string paramName)
		{
			int idx = -1;
			for(int i = 0, n = this.parameters.Count; i < n; i++)
			{
				if(String.Compare(parameters[i].Parameter.Name, paramName, StringComparison.OrdinalIgnoreCase) == 0)
				{
					idx = i;
					break;
				}
			}
			return idx;
		}

		internal object GetParameterValue(string paramName)
		{
			int idx = GetParameterIndex(paramName);
			if(idx >= 0)
			{
				return GetParameterValue(idx);
			}
			return null;
		}

		public object GetParameterValue(int parameterIndex)
		{
			if(this.parameters == null || parameterIndex < 0 || parameterIndex > this.parameters.Count)
			{
				throw Error.ArgumentOutOfRange("parameterIndex");
			}

			// ADO.NET providers require all results to be read before output parameters are visible
			if(this.session != null && !this.session.IsBuffered)
			{
				this.session.Buffer();
			}

			SqlParameterInfo pi = this.parameters[parameterIndex];
			object parameterValue = this.command.Parameters[parameterIndex].Value;
			if(parameterValue == DBNull.Value) parameterValue = null;
			if(parameterValue != null && parameterValue.GetType() != pi.Parameter.ClrType)
			{
				return DBConvert.ChangeType(parameterValue, pi.Parameter.ClrType);
			}

			return parameterValue;
		}

		public void Dispose()
		{
			if(!this.isDisposed)
			{
				// Technically, calling GC.SuppressFinalize is not required because the class does not
				// have a finalizer, but it does no harm, protects against the case where a finalizer is added
				// in the future, and prevents an FxCop warning.
				GC.SuppressFinalize(this);
				this.isDisposed = true;
				if(this.session != null)
				{
					this.session.Dispose();
				}
			}
		}
	}
}