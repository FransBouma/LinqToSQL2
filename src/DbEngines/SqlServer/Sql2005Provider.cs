using System.Data.Common;
using System.Data.Linq.Provider.Common;
using System.Diagnostics.CodeAnalysis;

namespace System.Data.Linq.DbEngines.SqlServer
{
	internal class Sql2005Provider : SqlServerProviderBase
	{
		[SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "These issues are related to our use of if-then and case statements for node types, which adds to the complexity count however when reviewed they are easy to navigate and understand.")]
		internal override ProviderType From(Type type, int? size)
		{
			if(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
				type = type.GetGenericArguments()[0];
			TypeCode tc = System.Type.GetTypeCode(type);
			switch(tc)
			{
				case TypeCode.Boolean:
					return SqlTypeSystem.Create(SqlDbType.Bit);
				case TypeCode.Byte:
					return SqlTypeSystem.Create(SqlDbType.TinyInt);
				case TypeCode.SByte:
				case TypeCode.Int16:
					return SqlTypeSystem.Create(SqlDbType.SmallInt);
				case TypeCode.Int32:
				case TypeCode.UInt16:
					return SqlTypeSystem.Create(SqlDbType.Int);
				case TypeCode.Int64:
				case TypeCode.UInt32:
					return SqlTypeSystem.Create(SqlDbType.BigInt);
				case TypeCode.UInt64:
					return SqlTypeSystem.Create(SqlDbType.Decimal, 20, 0);
				case TypeCode.Decimal:
					return SqlTypeSystem.Create(SqlDbType.Decimal, 29, size ?? 4);
				case TypeCode.Double:
					return SqlTypeSystem.Create(SqlDbType.Float);
				case TypeCode.Single:
					return SqlTypeSystem.Create(SqlDbType.Real);
				case TypeCode.Char:
					return SqlTypeSystem.Create(SqlDbType.NChar, 1);
				case TypeCode.String:
					return GetBestType(SqlDbType.NVarChar, size);
				case TypeCode.DateTime:
					return SqlTypeSystem.Create(SqlDbType.DateTime);
				case TypeCode.Object:
				{
					if(type == typeof(Guid))
						return SqlTypeSystem.Create(SqlDbType.UniqueIdentifier);
					if(type == typeof(byte[]) || type == typeof(Binary))
						return GetBestType(SqlDbType.VarBinary, size);
					if(type == typeof(char[]))
						return GetBestType(SqlDbType.NVarChar, size);
					if(type == typeof(TimeSpan))
						return SqlTypeSystem.Create(SqlDbType.BigInt);
					if(type == typeof(System.Xml.Linq.XDocument) ||
					   type == typeof(System.Xml.Linq.XElement))
						return ProviderConstants.XmlType;
					// else UDT?
					return new SqlType(type);
				}
				default:
					throw Error.UnexpectedTypeCode(tc);
			}
		}

		internal override ProviderType GetBestLargeType(ProviderType type)
		{
			SqlType sqlType = (SqlType)type;
			switch(sqlType.SqlDbType)
			{
				case SqlDbType.NText:
				case SqlDbType.NChar:
				case SqlDbType.NVarChar:
					return SqlTypeSystem.Create(SqlDbType.NVarChar, ProviderConstants.LargeTypeSizeIndicator);
				case SqlDbType.Text:
				case SqlDbType.Char:
				case SqlDbType.VarChar:
					return SqlTypeSystem.Create(SqlDbType.VarChar, ProviderConstants.LargeTypeSizeIndicator);
				case SqlDbType.Image:
				case SqlDbType.Binary:
				case SqlDbType.VarBinary:
					return SqlTypeSystem.Create(SqlDbType.VarBinary, ProviderConstants.LargeTypeSizeIndicator);
			}
			return type;
		}

		internal override int? DetermineParameterSize(SqlType declaredType, DbParameter parameter)
		{
			int? parameterSize = base.DetermineParameterSize(declaredType, parameter);
			if(parameterSize.HasValue)
			{
				return parameterSize;
			}

			// Engine team have recommended we use 4k/8k block sizes rather than -1 providing the
			// data fits within that size.
			int? largestDeclarableSize = GetLargestDeclarableSize(declaredType);
			if(largestDeclarableSize.HasValue && largestDeclarableSize.Value >= parameter.Size)
			{
				return largestDeclarableSize.Value;
			}

			// Providers that support the maximum size large type indicator should fall to that.
			return ProviderConstants.LargeTypeSizeIndicator;
		}


		protected override bool SupportsMaxSize
		{
			get { return true; }
		}

	}
}