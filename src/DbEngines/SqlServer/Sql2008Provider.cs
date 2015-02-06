using System.Data.Linq.Provider.Common;
using System.Diagnostics.CodeAnalysis;

namespace System.Data.Linq.DbEngines.SqlServer
{
	internal class Sql2008Provider : Sql2005Provider
	{
		[SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "These issues are related to our use of if-then and case statements for node types, which adds to the complexity count however when reviewed they are easy to navigate and understand.")]
		internal override ProviderType From(Type type, int? size)
		{
			if(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
				type = type.GetGenericArguments()[0];

			// Retain mappings for DateTime and TimeSpan; add one for the new DateTimeOffset type.
			//
			if(System.Type.GetTypeCode(type) == TypeCode.Object &&
			   type == typeof(DateTimeOffset))
			{
				return SqlTypeSystem.Create(SqlDbType.DateTimeOffset);
			}

			return base.From(type, size);
		}
	}
}