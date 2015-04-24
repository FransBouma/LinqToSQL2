using System.Data.Linq.Provider.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace System.Data.Linq.DbEngines.SqlServer
{

	internal static class SqlTypeSystem
	{
#warning REFACTORING CANDIDATE FOR #23
		internal static TypeSystemProvider Create2000Provider()
		{
			return new Sql2000Provider();
		}

		internal static TypeSystemProvider Create2005Provider()
		{
			return new Sql2005Provider();
		}

		internal static TypeSystemProvider Create2008Provider()
		{
			return new Sql2008Provider();
		}

		internal static TypeSystemProvider CreateCEProvider()
		{
			return new SqlCEProvider();
		}

		// class constructor will cause static initialization to be deferred
		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "Static constructors provide deferred execution, which is desired.")]
		static SqlTypeSystem() { }

		internal static ProviderType Create(SqlDbType type, int size)
		{
			return new SqlType(type, size);
		}

		internal static ProviderType Create(SqlDbType type, int precision, int scale)
		{
			if(type != SqlDbType.Decimal && precision == 0 && scale == 0)
			{
				return Create(type);
			}
			else if(type == SqlDbType.Decimal && precision == ProviderConstants.DefaultDecimalPrecision && scale == ProviderConstants.DefaultDecimalScale)
			{
				return Create(type);
			}
			return new SqlType(type, precision, scale);
		}

		[SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "These issues are related to our use of if-then and case statements for node types, which adds to the complexity count however when reviewed they are easy to navigate and understand.")]
		internal static ProviderType Create(SqlDbType type)
		{
			switch(type)
			{
				case SqlDbType.BigInt: return ProviderConstants.BigIntType;
				case SqlDbType.Bit: return ProviderConstants.BitType;
				case SqlDbType.Char: return ProviderConstants.CharType;
				case SqlDbType.DateTime: return ProviderConstants.DateTimeType;
				case SqlDbType.Date: return ProviderConstants.DateType;
				case SqlDbType.Time: return ProviderConstants.TimeType;
				case SqlDbType.DateTime2: return ProviderConstants.DateTime2Type;
				case SqlDbType.DateTimeOffset: return ProviderConstants.DateTimeOffsetType;
				case SqlDbType.Decimal: return ProviderConstants.DefaultDecimalType;
				case SqlDbType.Float: return ProviderConstants.FloatType;
				case SqlDbType.Int: return ProviderConstants.IntType;
				case SqlDbType.Money: return ProviderConstants.MoneyType;
				case SqlDbType.Real: return ProviderConstants.RealType;
				case SqlDbType.UniqueIdentifier: return ProviderConstants.UniqueIdentifierType;
				case SqlDbType.SmallDateTime: return ProviderConstants.SmallDateTimeType;
				case SqlDbType.SmallInt: return ProviderConstants.SmallIntType;
				case SqlDbType.SmallMoney: return ProviderConstants.SmallMoneyType;
				case SqlDbType.Timestamp: return ProviderConstants.TimestampType;
				case SqlDbType.TinyInt: return ProviderConstants.TinyIntType;
				case SqlDbType.Xml: return ProviderConstants.XmlType;
				case SqlDbType.Text: return ProviderConstants.TextType;
				case SqlDbType.NText: return ProviderConstants.NTextType;
				case SqlDbType.Image: return ProviderConstants.ImageType;
				default:
					return new SqlType(type);
			}
		}

		[SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "These issues are related to our use of if-then and case statements for node types, which adds to the complexity count however when reviewed they are easy to navigate and understand.")]
		internal static Type GetClosestRuntimeType(SqlDbType sqlDbType)
		{
			switch(sqlDbType)
			{
				case SqlDbType.Int:
					return typeof(int);
				case SqlDbType.BigInt:
					return typeof(long);
				case SqlDbType.Bit:
					return typeof(bool);
				case SqlDbType.Date:
				case SqlDbType.SmallDateTime:
				case SqlDbType.DateTime:
				case SqlDbType.DateTime2:
					return typeof(DateTime);
				case SqlDbType.DateTimeOffset:
					return typeof(DateTimeOffset);
				case SqlDbType.Time:
					return typeof(TimeSpan);
				case SqlDbType.Float:
					return typeof(double);
				case SqlDbType.Real:
					return typeof(float);
				case SqlDbType.Binary:
				case SqlDbType.Image:
				case SqlDbType.Timestamp:
				case SqlDbType.VarBinary:
					return typeof(byte[]);
				case SqlDbType.Decimal:
				case SqlDbType.Money:
				case SqlDbType.SmallMoney:
					return typeof(decimal);
				case SqlDbType.Char:
				case SqlDbType.NChar:
				case SqlDbType.NText:
				case SqlDbType.NVarChar:
				case SqlDbType.Xml:
				case SqlDbType.VarChar:
				case SqlDbType.Text:
					return typeof(string);
				case SqlDbType.UniqueIdentifier:
					return typeof(Guid);
				case SqlDbType.SmallInt:
					return typeof(short);
				case SqlDbType.TinyInt:
					return typeof(byte);
				case SqlDbType.Udt:
					// Udt type is not handled.
					throw Error.UnexpectedTypeCode(SqlDbType.Udt);
				case SqlDbType.Variant:
				default:
					return typeof(object);
			}
		}

	}
}
