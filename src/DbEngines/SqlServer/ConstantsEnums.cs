using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Linq.DbEngines.SqlServer
{
	internal enum SqlServerProviderMode
	{
		NotYetDecided,
		Sql2000,
		Sql2005,
		Sql2008,
		SqlCE
		// Add more here.
	}

	internal class ProviderConstants
	{
		// -1 is used to indicate a MAX size.  In ADO.NET, -1 is specified as the size
		// on a SqlParameter with SqlDbType = VarChar to indicate VarChar(MAX).
		internal const short LargeTypeSizeIndicator = -1;

		/*
		 * For simple types with no size, precision or scale, reuse a static set of SqlDataTypes.
		 * This has two advantages: Fewer allocations of SqlType and faster comparison of
		 * one type to another.
		 */
		internal const int DefaultDecimalPrecision = 29;
		internal const int DefaultDecimalScale = 4;

		static internal readonly SqlType BigIntType = new SqlType(SqlDbType.BigInt);
		static internal readonly SqlType BitType = new SqlType(SqlDbType.Bit);
		static internal readonly SqlType CharType = new SqlType(SqlDbType.Char);
		static internal readonly SqlType DateTimeType = new SqlType(SqlDbType.DateTime);
		static internal readonly SqlType DateType = new SqlType(SqlDbType.Date);
		static internal readonly SqlType TimeType = new SqlType(SqlDbType.Time);
		static internal readonly SqlType DateTime2Type = new SqlType(SqlDbType.DateTime2);
		static internal readonly SqlType DateTimeOffsetType = new SqlType(SqlDbType.DateTimeOffset);
		static internal readonly SqlType DefaultDecimalType = new SqlType(SqlDbType.Decimal, DefaultDecimalPrecision, DefaultDecimalScale);
		static internal readonly SqlType FloatType = new SqlType(SqlDbType.Float);
		static internal readonly SqlType IntType = new SqlType(SqlDbType.Int);
		static internal readonly SqlType MoneyType = new SqlType(SqlDbType.Money, 19, 4);
		static internal readonly SqlType RealType = new SqlType(SqlDbType.Real);
		static internal readonly SqlType UniqueIdentifierType = new SqlType(SqlDbType.UniqueIdentifier);
		static internal readonly SqlType SmallDateTimeType = new SqlType(SqlDbType.SmallDateTime);
		static internal readonly SqlType SmallIntType = new SqlType(SqlDbType.SmallInt);
		static internal readonly SqlType SmallMoneyType = new SqlType(SqlDbType.SmallMoney, 10, 4);
		static internal readonly SqlType TimestampType = new SqlType(SqlDbType.Timestamp);
		static internal readonly SqlType TinyIntType = new SqlType(SqlDbType.TinyInt);
		static internal readonly SqlType XmlType = new SqlType(SqlDbType.Xml, LargeTypeSizeIndicator);
		static internal readonly SqlType TextType = new SqlType(SqlDbType.Text, LargeTypeSizeIndicator);
		static internal readonly SqlType NTextType = new SqlType(SqlDbType.NText, LargeTypeSizeIndicator);
		static internal readonly SqlType ImageType = new SqlType(SqlDbType.Image, LargeTypeSizeIndicator);

	}
}
