using System.Data.Linq.Provider.Common;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace System.Data.Linq.DbEngines.SqlServer
{
	internal class SqlType : ProviderType
	{
		protected SqlDbType sqlDbType;

		internal SqlType(SqlDbType type)
		{
			this.sqlDbType = type;
		}

		internal SqlType(SqlDbType type, int? size)
		{
			this.sqlDbType = type;
			this.size = size;
		}

		internal SqlType(SqlDbType type, int precision, int scale)
		{
			Diagnostics.Debug.Assert(scale <= precision);
			this.sqlDbType = type;
			this.precision = precision;
			this.scale = scale;
		}

		internal SqlType(Type type)
		{
			this.RuntimeOnlyType = type;
		}

		internal SqlType(int applicationTypeIndex)
		{
			this.ApplicationTypeIndex = applicationTypeIndex;
		}

		internal override bool IsUnicodeType
		{
			get
			{
				switch(this.SqlDbType)
				{
					case SqlDbType.NChar:
					case SqlDbType.NText:
					case SqlDbType.NVarChar:
						return true;
					default:
						return false;
				}
			}
		}
		internal override ProviderType GetNonUnicodeEquivalent()
		{
			if(IsUnicodeType)
			{
				switch(this.SqlDbType)
				{
					case SqlDbType.NChar:
						return new SqlType(SqlDbType.Char, this.Size);
					case SqlDbType.NText:
						return new SqlType(SqlDbType.Text);
					case SqlDbType.NVarChar:
						return new SqlType(SqlDbType.VarChar, this.Size);
					default:
						// can't happen
						return this;
				}
			}
			return this;
		}


		/// <summary>
		/// Determines if it is safe to suppress size specifications for
		/// the operand of a cast/convert.  For example, when casting to string,
		/// all these types have length less than the default sized used by SqlServer,
		/// so the length specification can be omitted without fear of truncation.
		/// </summary>
		internal override bool CanSuppressSizeForConversionToString
		{
			get
			{
				int defaultSize = 30;

				if(this.IsLargeType)
				{
					return false;
				}
				if(!this.IsChar && !this.IsString && this.IsFixedSize && this.Size > 0 /*&& this.Size != LargeTypeSizeIndicator*/)
				{ // commented out because LargeTypeSizeIndicator == -1
					return (this.Size < defaultSize);
				}
				switch(this.SqlDbType)
				{
					case SqlDbType.BigInt: // -2^63 (-9,223,372,036,854,775,808) to 2^63-1 (9,223,372,036,854,775,807)
					case SqlDbType.Bit: // 0 or 1
					case SqlDbType.Int: // -2^31 (-2,147,483,648) to 2^31-1 (2,147,483,647)
					case SqlDbType.SmallInt: // -2^15 (-32,768) to 2^15-1 (32,767)
					case SqlDbType.TinyInt: // 0 to 255
					case SqlDbType.Money: // -922,337,203,685,477.5808 to 922,337,203,685,477.5807
					case SqlDbType.SmallMoney: // -214,748.3648 to 214,748.3647
					case SqlDbType.Float: // -1.79E+308 to -2.23E-308, 0 and 2.23E-308 to 1.79E+308
					case SqlDbType.Real: // -3.40E+38 to -1.18E-38, 0 and 1.18E-38 to 3.40E+38
						return true;
					default:
						return false;
				};
			}
		}

		internal override int ComparePrecedenceTo(ProviderType type)
		{
			SqlType sqlProviderType = (SqlType)type;
			// Highest precedence is given to server-known types. Converting to a client-only type is
			// impossible when the conversion is present in a SQL query.
			int p0 = IsTypeKnownByProvider ? GetTypeCoercionPrecedence(this.SqlDbType) : Int32.MinValue;
			int p1 = sqlProviderType.IsTypeKnownByProvider ? GetTypeCoercionPrecedence(sqlProviderType.SqlDbType) : Int32.MinValue;
			return p0.CompareTo(p1);
		}


		internal override bool SupportsComparison
		{
			get
			{
				switch(this.sqlDbType)
				{
					case SqlDbType.NText:
					case SqlDbType.Image:
					case SqlDbType.Xml:
					case SqlDbType.Text:
						return false;
					default:
						return true;
				}
			}
		}

		internal override bool SupportsLength
		{
			get
			{
				switch(this.sqlDbType)
				{
					case SqlDbType.NText:
					case SqlDbType.Image:
					case SqlDbType.Xml:
					case SqlDbType.Text:
						return false;
					default:
						return true;
				}
			}
		}

		/// <summary>
		/// Returns true if the given values will be equal to eachother on the server for this type.
		/// </summary>
		internal override bool AreValuesEqual(object o1, object o2)
		{
			if(o1 == null || o2 == null)
			{
				return false;
			}
			switch(this.sqlDbType)
			{
				case SqlDbType.Char:
				case SqlDbType.NChar:
				case SqlDbType.NVarChar:
				case SqlDbType.VarChar:
				case SqlDbType.Text:
					string s1 = o1 as string;
					if(s1 != null)
					{
						string s2 = o2 as string;
						if(s2 != null)
						{
							return s1.TrimEnd(' ').Equals(s2.TrimEnd(' '), StringComparison.Ordinal);
						}
					}
					break;
			}
			return o1.Equals(o2);
		}

		internal override bool IsLargeType
		{
			get
			{
				switch(this.sqlDbType)
				{
					case SqlDbType.NText:
					case SqlDbType.Image:
					case SqlDbType.Xml:
					case SqlDbType.Text:
						return true;
					case SqlDbType.NVarChar:
					case SqlDbType.VarChar:
					case SqlDbType.VarBinary:
						return (this.size == ProviderConstants.LargeTypeSizeIndicator);
					default:
						return false;
				}
			}
		}

		internal override bool HasPrecisionAndScale
		{
			get
			{
				switch(SqlDbType)
				{
					case SqlDbType.Decimal:
					case SqlDbType.Float:
					case SqlDbType.Real:
					case SqlDbType.Money:      // precision and scale are fixed at 19,4
					case SqlDbType.SmallMoney: // precision and scale are fixed at 10,4
					case SqlDbType.DateTime2:
					case SqlDbType.DateTimeOffset:
					case SqlDbType.Time:
						return true;
					default:
						return false;
				}
			}
		}

		internal override Type GetClosestRuntimeType()
		{
			if(this.RuntimeOnlyType != null)
			{
				return this.RuntimeOnlyType;
			}
			return SqlTypeSystem.GetClosestRuntimeType(this.sqlDbType);
		}


		internal override string ToQueryString()
		{
			return ToQueryString(QueryFormatOptions.None);
		}

		[SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "These issues are related to our use of if-then and case statements for node types, which adds to the complexity count however when reviewed they are easy to navigate and understand.")]
		internal override string ToQueryString(QueryFormatOptions formatFlags)
		{
			if(this.RuntimeOnlyType != null)
			{
				return this.RuntimeOnlyType.ToString();
			}
			StringBuilder sb = new StringBuilder();

			switch(sqlDbType)
			{
				case SqlDbType.BigInt:
				case SqlDbType.Bit:
				case SqlDbType.Date:
				case SqlDbType.Time:
				case SqlDbType.DateTime:
				case SqlDbType.DateTime2:
				case SqlDbType.DateTimeOffset:
				case SqlDbType.Int:
				case SqlDbType.Money:
				case SqlDbType.SmallDateTime:
				case SqlDbType.SmallInt:
				case SqlDbType.SmallMoney:
				case SqlDbType.Timestamp:
				case SqlDbType.TinyInt:
				case SqlDbType.UniqueIdentifier:
				case SqlDbType.Xml:
				case SqlDbType.Image:
				case SqlDbType.NText:
				case SqlDbType.Text:
				case SqlDbType.Udt:
					sb.Append(sqlDbType.ToString());
					break;
				case SqlDbType.Variant:
					sb.Append("sql_variant");
					break;
				case SqlDbType.Binary:
				case SqlDbType.Char:
				case SqlDbType.NChar:
					sb.Append(sqlDbType);
					if((formatFlags & QueryFormatOptions.SuppressSize) == 0)
					{
						sb.Append("(");
						sb.Append(size);
						sb.Append(")");
					}
					break;
				case SqlDbType.NVarChar:
				case SqlDbType.VarBinary:
				case SqlDbType.VarChar:
					sb.Append(sqlDbType);
					if((size.HasValue && size != 0) && (formatFlags & QueryFormatOptions.SuppressSize) == 0)
					{
						sb.Append("(");
						if(size == ProviderConstants.LargeTypeSizeIndicator)
						{
							sb.Append("MAX");
						}
						else
						{
							sb.Append(size);
						}
						sb.Append(")");
					}
					break;
				case SqlDbType.Decimal:
				case SqlDbType.Float:
				case SqlDbType.Real:
					sb.Append(sqlDbType);
					if(precision != 0)
					{
						sb.Append("(");
						sb.Append(precision);
						if(scale != 0)
						{
							sb.Append(",");
							sb.Append(scale);
						}
						sb.Append(")");
					}
					break;
			}
			return sb.ToString();
		}


		internal override bool IsFixedSize
		{
			get
			{
				switch(this.sqlDbType)
				{
					case SqlDbType.NText:
					case SqlDbType.Text:
					case SqlDbType.NVarChar:
					case SqlDbType.VarChar:
					case SqlDbType.Image:
					case SqlDbType.VarBinary:
					case SqlDbType.Xml:
						return false;
					default:
						return true;
				}
			}
		}

		internal SqlDbType SqlDbType
		{
			get { return sqlDbType; }
		}

		internal override bool IsOrderable
		{
			get
			{
				if(this.IsRuntimeOnlyType) return false; // must be a SQL type

				switch(this.sqlDbType)
				{
					case SqlDbType.Image:
					case SqlDbType.Text:
					case SqlDbType.NText:
					case SqlDbType.Xml:
						return false;
					default:
						return true;
				}
			}
		}

		internal override bool IsGroupable
		{
			get
			{
				if(this.IsRuntimeOnlyType) return false; // must be a SQL type

				switch(this.sqlDbType)
				{
					case SqlDbType.Image:
					case SqlDbType.Text:
					case SqlDbType.NText:
					case SqlDbType.Xml:
						return false;
					default:
						return true;
				}
			}
		}

		internal override bool IsNumeric
		{
			get
			{
				if(IsApplicationType || IsRuntimeOnlyType)
				{
					return false;
				}
				switch(SqlDbType)
				{
					case SqlDbType.Bit:
					case SqlDbType.TinyInt:
					case SqlDbType.SmallInt:
					case SqlDbType.Int:
					case SqlDbType.BigInt:
					case SqlDbType.Decimal:
					case SqlDbType.Float:
					case SqlDbType.Real:
					case SqlDbType.Money:
					case SqlDbType.SmallMoney:
						return true;
					default:
						return false;
				};
			}
		}

		internal override bool IsChar
		{
			get
			{
				if(IsApplicationType || IsRuntimeOnlyType)
					return false;
				switch(SqlDbType)
				{
					case SqlDbType.Char:
					case SqlDbType.NChar:
					case SqlDbType.NVarChar:
					case SqlDbType.VarChar:
						return Size == 1;
					default:
						return false;
				}
			}
		}

		internal override bool IsString
		{
			get
			{
				if(IsApplicationType || IsRuntimeOnlyType)
					return false;
				switch(SqlDbType)
				{
					case SqlDbType.Char:
					case SqlDbType.NChar:
					case SqlDbType.NVarChar:
					case SqlDbType.VarChar:
						// -1 is used for large types to represent MAX
						return Size == 0 || Size > 1 || Size == ProviderConstants.LargeTypeSizeIndicator;
					case SqlDbType.Text:
					case SqlDbType.NText:
						return true;
					default:
						return false;
				}
			}
		}

		public override bool Equals(object obj)
		{
			if((object)this == obj)
				return true;

			SqlType that = obj as SqlType;
			if(that == null)
				return false;

			return
				this.RuntimeOnlyType == that.RuntimeOnlyType &&
				this.ApplicationTypeIndex == that.ApplicationTypeIndex &&
				this.sqlDbType == that.sqlDbType &&
				this.Size == that.Size &&
				this.precision == that.precision &&
				this.scale == that.scale;
		}

		public override int GetHashCode()
		{
			// Make up a hash function that will atleast not treat precision and scale
			// as interchangeable. This may need a smarter replacement if certain hash
			// buckets get too full.
			int hash = 0;
			if(this.RuntimeOnlyType != null)
			{
				hash = this.RuntimeOnlyType.GetHashCode();
			}
			else if(this.ApplicationTypeIndex != null)
			{
				hash = this.ApplicationTypeIndex.Value;
			}
			return hash ^ this.sqlDbType.GetHashCode() ^ (this.Size ?? 0) ^ (this.Precision) ^ (this.Scale << 8);
		}

		[SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "These issues are related to our use of if-then and case statements for node types, which adds to the complexity count however when reviewed they are easy to navigate and understand.")]
		private static int GetTypeCoercionPrecedence(SqlDbType type)
		{
			switch(type)
			{
				case SqlDbType.Binary: return 0;
				case SqlDbType.VarBinary: return 1;
				case SqlDbType.VarChar: return 2;
				case SqlDbType.Char: return 3;
				case SqlDbType.NChar: return 4;
				case SqlDbType.NVarChar: return 5;
				case SqlDbType.UniqueIdentifier: return 6;
				case SqlDbType.Timestamp: return 7;
				case SqlDbType.Image: return 8;
				case SqlDbType.Text: return 9;
				case SqlDbType.NText: return 10;
				case SqlDbType.Bit: return 11;
				case SqlDbType.TinyInt: return 12;
				case SqlDbType.SmallInt: return 13;
				case SqlDbType.Int: return 14;
				case SqlDbType.BigInt: return 15;
				case SqlDbType.SmallMoney: return 16;
				case SqlDbType.Money: return 17;
				case SqlDbType.Decimal: return 18;
				case SqlDbType.Real: return 19;
				case SqlDbType.Float: return 20;
				case SqlDbType.Date: return 21;
				case SqlDbType.Time: return 22;
				case SqlDbType.SmallDateTime: return 23;
				case SqlDbType.DateTime: return 24;
				case SqlDbType.DateTime2: return 25;
				case SqlDbType.DateTimeOffset: return 26;
				case SqlDbType.Xml: return 27;
				case SqlDbType.Variant: return 28;
				case SqlDbType.Udt: return 29;
				default:
					throw Error.UnexpectedTypeCode(type);
			}
		}


		[SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "These issues are related to our use of if-then and case statements for node types, which adds to the complexity count however when reviewed they are easy to navigate and understand.")]
		internal override TypeCategory Category
		{
			get
			{
				switch(this.SqlDbType)
				{
					case SqlDbType.Bit:
					case SqlDbType.TinyInt:
					case SqlDbType.SmallInt:
					case SqlDbType.Int:
					case SqlDbType.BigInt:
					case SqlDbType.Decimal:
					case SqlDbType.Float:
					case SqlDbType.Real:
					case SqlDbType.Money:
					case SqlDbType.SmallMoney:
						return TypeCategory.Numeric;
					case SqlDbType.Char:
					case SqlDbType.NChar:
					case SqlDbType.VarChar:
					case SqlDbType.NVarChar:
						return TypeCategory.Char;
					case SqlDbType.Text:
					case SqlDbType.NText:
						return TypeCategory.Text;
					case SqlDbType.Binary:
					case SqlDbType.VarBinary:
					case SqlDbType.Timestamp:
						return TypeCategory.Binary;
					case SqlDbType.Image:
						return TypeCategory.Image;
					case SqlDbType.Xml:
						return TypeCategory.Xml;
					case SqlDbType.Date:
					case SqlDbType.Time:
					case SqlDbType.DateTime:
					case SqlDbType.DateTime2:
					case SqlDbType.DateTimeOffset:
					case SqlDbType.SmallDateTime:
						return TypeCategory.DateTime;
					case SqlDbType.UniqueIdentifier:
						return TypeCategory.UniqueIdentifier;
					case SqlDbType.Variant:
						return TypeCategory.Variant;
					case SqlDbType.Udt:
						return TypeCategory.Udt;
					default:
						throw Error.UnexpectedTypeCode(this);
				}
			}
		}
	}
}