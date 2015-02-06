using System.Collections.Generic;
using System.Data.Common;
using System.Data.Linq.Provider.Common;
using System.Data.Linq.Provider.NodeTypes;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace System.Data.Linq.DbEngines.SqlServer
{
	internal abstract class SqlServerProviderBase : TypeSystemProvider
	{
		protected Dictionary<int, SqlType> applicationTypes = new Dictionary<int, SqlType>();

		internal override ProviderType GetApplicationType(int index)
		{
			if(index < 0)
				throw Error.ArgumentOutOfRange("index");
			SqlType result = null;
			if(!applicationTypes.TryGetValue(index, out result))
			{
				result = new SqlType(index);
				applicationTypes.Add(index, result);
			}
			return result;
		}

		[SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "These issues are related to our use of if-then and case statements for node types, which adds to the complexity count however when reviewed they are easy to navigate and understand.")]
		internal override ProviderType Parse(string stype)
		{
			// parse type...
			string typeName = null;
			string param1 = null;
			string param2 = null;
			int paren = stype.IndexOf('(');
			int space = stype.IndexOf(' ');
			int end = (paren != -1 && space != -1) ? Math.Min(space, paren)
				: (paren != -1) ? paren
					: (space != -1) ? space
						: -1;
			if(end == -1)
			{
				typeName = stype;
				end = stype.Length;
			}
			else
			{
				typeName = stype.Substring(0, end);
			}
			int start = end;
			if(start < stype.Length && stype[start] == '(')
			{
				start++;
				end = stype.IndexOf(',', start);
				if(end > 0)
				{
					param1 = stype.Substring(start, end - start);
					start = end + 1;
					end = stype.IndexOf(')', start);
					param2 = stype.Substring(start, end - start);
				}
				else
				{
					end = stype.IndexOf(')', start);
					param1 = stype.Substring(start, end - start);
				}
				start = end++;
			}

			#region Special case type mappings
			if(String.Compare(typeName, "rowversion", StringComparison.OrdinalIgnoreCase) == 0)
			{
				typeName = "Timestamp";
			}

			if(String.Compare(typeName, "numeric", StringComparison.OrdinalIgnoreCase) == 0)
			{
				typeName = "Decimal";
			}

			if(String.Compare(typeName, "sql_variant", StringComparison.OrdinalIgnoreCase) == 0)
			{
				typeName = "Variant";
			}

			if(String.Compare(typeName, "filestream", StringComparison.OrdinalIgnoreCase) == 0)
			{
				typeName = "Binary";
			}
			#endregion

			// since we're going to parse the enum value below, we verify
			// here that it is defined.  For example, types like table, cursor
			// are not valid.
			if(!Enum.GetNames(typeof(SqlDbType)).Select(n => n.ToUpperInvariant()).Contains(typeName.ToUpperInvariant()))
			{
				throw Error.InvalidProviderType(typeName);
			}

			int p1 = 0;
			int p2 = 0;
			SqlDbType dbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), typeName, true);
			if(param1 != null)
			{
				if(String.Compare(param1.Trim(), "max", StringComparison.OrdinalIgnoreCase) == 0)
				{
					p1 = ProviderConstants.LargeTypeSizeIndicator;
				}
				else
				{
					p1 = Int32.Parse(param1, Globalization.CultureInfo.InvariantCulture);
					if(p1 == Int32.MaxValue)
						p1 = ProviderConstants.LargeTypeSizeIndicator;
				}
			}

			if(param2 != null)
			{
				if(String.Compare(param2.Trim(), "max", StringComparison.OrdinalIgnoreCase) == 0)
				{
					p2 = ProviderConstants.LargeTypeSizeIndicator;
				}
				else
				{
					p2 = Int32.Parse(param2, Globalization.CultureInfo.InvariantCulture);
					if(p2 == Int32.MaxValue)
						p2 = ProviderConstants.LargeTypeSizeIndicator;

				}
			}

			switch(dbType)
			{
				case SqlDbType.Binary:
				case SqlDbType.Char:
				case SqlDbType.NChar:
				case SqlDbType.NVarChar:
				case SqlDbType.VarBinary:
				case SqlDbType.VarChar:
					return SqlTypeSystem.Create(dbType, p1);
				case SqlDbType.Decimal:
				case SqlDbType.Real:
				case SqlDbType.Float:
					return SqlTypeSystem.Create(dbType, p1, p2);
				case SqlDbType.Timestamp:
				default:
					return SqlTypeSystem.Create(dbType);
			}
		}

		// Returns true if the type provider supports MAX types (e.g NVarChar(MAX))
		protected abstract bool SupportsMaxSize { get; }

		/// <summary>
		/// For types with size, determine the closest matching type for the information
		/// specified, promoting to the appropriate large type as needed.  If no size is
		/// specified, we use the max.
		/// </summary>
		protected ProviderType GetBestType(SqlDbType targetType, int? size)
		{
			// determine max size for the specified db type
			int maxSize = 0;
			switch(targetType)
			{
				case SqlDbType.NChar:
				case SqlDbType.NVarChar:
					maxSize = 4000;
					break;
				case SqlDbType.Char:
				case SqlDbType.VarChar:
				case SqlDbType.Binary:
				case SqlDbType.VarBinary:
					maxSize = 8000;
					break;
			};

			if(size.HasValue)
			{
				if(size.Value <= maxSize)
				{
					return SqlTypeSystem.Create(targetType, size.Value);
				}
				else
				{
					return GetBestLargeType(SqlTypeSystem.Create(targetType));
				}
			}

			// if the type provider supports MAX types, return one, otherwise use
			// the maximum size determined above
			return SqlTypeSystem.Create(targetType, SupportsMaxSize ? ProviderConstants.LargeTypeSizeIndicator : maxSize);
		}

		internal override void InitializeParameter(ProviderType type, DbParameter parameter, object value)
		{
			SqlType sqlType = (SqlType)type;
			if(sqlType.IsRuntimeOnlyType)
			{
				throw Error.BadParameterType(sqlType.GetClosestRuntimeType());
			}
			SqlClient.SqlParameter sParameter = parameter as SqlClient.SqlParameter;
			if(sParameter != null)
			{
				sParameter.SqlDbType = sqlType.SqlDbType;
				if(sqlType.HasPrecisionAndScale)
				{
					sParameter.Precision = (byte)sqlType.Precision;
					sParameter.Scale = (byte)sqlType.Scale;
				}
			}
			else
			{
				PropertyInfo piSqlDbType = parameter.GetType().GetProperty("SqlDbType");
				if(piSqlDbType != null)
				{
					piSqlDbType.SetValue(parameter, sqlType.SqlDbType, null);
				}
				if(sqlType.HasPrecisionAndScale)
				{
					PropertyInfo piPrecision = parameter.GetType().GetProperty("Precision");
					if(piPrecision != null)
					{
						piPrecision.SetValue(parameter, Convert.ChangeType(sqlType.Precision, piPrecision.PropertyType, CultureInfo.InvariantCulture), null);
					}
					PropertyInfo piScale = parameter.GetType().GetProperty("Scale");
					if(piScale != null)
					{
						piScale.SetValue(parameter, Convert.ChangeType(sqlType.Scale, piScale.PropertyType, CultureInfo.InvariantCulture), null);
					}
				}
			}
			parameter.Value = GetParameterValue(sqlType, value);

			int? determinedSize = DetermineParameterSize(sqlType, parameter);
			if(determinedSize.HasValue)
			{
				parameter.Size = determinedSize.Value;
			}
		}

		internal virtual int? DetermineParameterSize(SqlType declaredType, DbParameter parameter)
		{
			// Output parameters and input-parameters of a fixed-size should be specifically set if value fits.
			bool isInputParameter = parameter.Direction == ParameterDirection.Input;
			if(!isInputParameter || declaredType.IsFixedSize)
			{
				if(declaredType.Size.HasValue && parameter.Size <= declaredType.Size || declaredType.IsLargeType)
				{
					return declaredType.Size.Value;
				}
			}

			// Preserve existing provider & server-driven behaviour for all other cases.
			return null;
		}

		protected int? GetLargestDeclarableSize(SqlType declaredType)
		{
			switch(declaredType.SqlDbType)
			{
				case SqlDbType.Image:
				case SqlDbType.Binary:
				case SqlDbType.VarChar:
					return 8000;
				case SqlDbType.NVarChar:
					return 4000;
				default:
					return null;
			}
		}

		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Unknown reason.")]
		protected object GetParameterValue(SqlType type, object value)
		{
			if(value == null)
			{
				return DBNull.Value;
			}
			else
			{
				Type vType = value.GetType();
				Type pType = type.GetClosestRuntimeType();
				if(pType == vType)
				{
					return value;
				}
				else
				{
					return DBConvert.ChangeType(value, pType);
				}
			}
		}

		[SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "These issues are related to our use of if-then and case statements for node types, which adds to the complexity count however when reviewed they are easy to navigate and understand.")]
		internal override ProviderType PredictTypeForUnary(SqlNodeType unaryOp, ProviderType operandType)
		{
			switch(unaryOp)
			{
				case SqlNodeType.Not:
				case SqlNodeType.Not2V:
				case SqlNodeType.IsNull:
				case SqlNodeType.IsNotNull:
					return ProviderConstants.BitType;
				case SqlNodeType.Negate:
				case SqlNodeType.BitNot:
				case SqlNodeType.ValueOf:
				case SqlNodeType.Treat:
				case SqlNodeType.OuterJoinedValue:
					return operandType;
				case SqlNodeType.Count:
					return this.From(typeof(int));
				case SqlNodeType.LongCount:
					return this.From(typeof(long));
				case SqlNodeType.Min:
				case SqlNodeType.Max:
					return operandType;
				case SqlNodeType.Sum:
				case SqlNodeType.Avg:
				case SqlNodeType.Stddev:
					return this.MostPreciseTypeInFamily(operandType);
				case SqlNodeType.ClrLength:
					if(operandType.IsLargeType)
					{
						return this.From(typeof(long)); // SqlDbType.BigInt
					}
					else
					{
						return this.From(typeof(int)); // SqlDbType.Int
					}
				default:
					throw Error.UnexpectedNode(unaryOp);
			}
		}

		[SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "These issues are related to our use of if-then and case statements for node types, which adds to the complexity count however when reviewed they are easy to navigate and understand.")]
		internal override ProviderType PredictTypeForBinary(SqlNodeType binaryOp, ProviderType leftType, ProviderType rightType)
		{
			SqlType highest;

			if(leftType.IsSameTypeFamily(this.From(typeof(string))) && rightType.IsSameTypeFamily(this.From(typeof(string))))
			{
				highest = (SqlType)this.GetBestType(leftType, rightType);
			}
			else
			{
				int coercionPrecedence = leftType.ComparePrecedenceTo(rightType);
				highest = (SqlType)(coercionPrecedence > 0 ? leftType : rightType);
			}

			switch(binaryOp)
			{
				case SqlNodeType.Add:
				case SqlNodeType.Sub:
				case SqlNodeType.Mul:
				case SqlNodeType.Div:
				case SqlNodeType.BitAnd:
				case SqlNodeType.BitOr:
				case SqlNodeType.BitXor:
				case SqlNodeType.Mod:
				case SqlNodeType.Coalesce:
					return highest;
				case SqlNodeType.Concat:
					// When concatenating two types with size, the result type after
					// concatenation must have a size equal to the sum of the two sizes.
					if(highest.HasSizeOrIsLarge)
					{
						// get the best type, specifying null for size so we get
						// the maximum allowable size
						ProviderType concatType = this.GetBestType(highest.SqlDbType, null);

						if((!leftType.IsLargeType && leftType.Size.HasValue) &&
						   (!rightType.IsLargeType && rightType.Size.HasValue))
						{
							// If both types are not large types and have size, and the
							// size is less than the default size, return the shortened type.
							int concatSize = leftType.Size.Value + rightType.Size.Value;
							if((concatSize < concatType.Size) || concatType.IsLargeType)
							{
								return GetBestType(highest.SqlDbType, concatSize);
							}
						}

						return concatType;
					}
					return highest;
				case SqlNodeType.And:
				case SqlNodeType.Or:
				case SqlNodeType.LT:
				case SqlNodeType.LE:
				case SqlNodeType.GT:
				case SqlNodeType.GE:
				case SqlNodeType.EQ:
				case SqlNodeType.NE:
				case SqlNodeType.EQ2V:
				case SqlNodeType.NE2V:
					return ProviderConstants.IntType;
				default:
					throw Error.UnexpectedNode(binaryOp);
			}
		}

		internal override ProviderType MostPreciseTypeInFamily(ProviderType type)
		{
			SqlType sqlType = (SqlType)type;
			switch(sqlType.SqlDbType)
			{
				case SqlDbType.TinyInt:
				case SqlDbType.SmallInt:
				case SqlDbType.Int:
					return From(typeof(int));
				case SqlDbType.SmallMoney:
				case SqlDbType.Money:
					return SqlTypeSystem.Create(SqlDbType.Money);
				case SqlDbType.Real:
				case SqlDbType.Float:
					return From(typeof(double));
				case SqlDbType.Date:
				case SqlDbType.Time:
				case SqlDbType.SmallDateTime:
				case SqlDbType.DateTime:
				case SqlDbType.DateTime2:
					return From(typeof(DateTime));
				case SqlDbType.DateTimeOffset:
					return From(typeof(DateTimeOffset));
				default:
					return type;
			}
		}

		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Unknown reason.")]
		private ProviderType[] GetArgumentTypes(SqlFunctionCall fc)
		{
			ProviderType[] array = new ProviderType[fc.Arguments.Count];
			for(int i = 0, n = array.Length; i < n; i++)
			{
				array[i] = fc.Arguments[i].SqlType;
			}
			return array;
		}

		/// <summary>
		/// Gives the return type of a SQL function which has as first argument something of this SqlType.
		/// For some functions (e.g. trigonometric functions) the return type is independent of the argument type;
		/// in those cases this method returns null and the type was already set correctly in the MethodCallConverter.
		/// </summary>
		/// <param name="functionCall">The SqlFunctionCall node.</param>
		/// <returns>null: Don't change type, otherwise: Set return type to this ProviderType</returns>
		[SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "These issues are related to our use of if-then and case statements for node types, which adds to the complexity count however when reviewed they are easy to navigate and understand.")]
		internal override ProviderType ReturnTypeOfFunction(SqlFunctionCall functionCall)
		{
			var argumentTypes = this.GetArgumentTypes(functionCall);

			SqlType arg0 = (SqlType)argumentTypes[0];
			SqlType arg1 = argumentTypes.Length > 1 ? (SqlType)argumentTypes[1] : (SqlType)null;

			switch(functionCall.Name)
			{
				case "LEN":
				case "DATALENGTH":
					switch(arg0.SqlDbType)
					{
						case SqlDbType.NVarChar:
						case SqlDbType.VarChar:
						case SqlDbType.VarBinary:
							if(arg0.IsLargeType)
							{
								return SqlTypeSystem.Create(SqlDbType.BigInt);
							}
							else
							{
								return SqlTypeSystem.Create(SqlDbType.Int);
							}
						default:
							return SqlTypeSystem.Create(SqlDbType.Int);
					}
				case "ABS":
				case "SIGN":
				case "ROUND":
				case "CEILING":
				case "FLOOR":
				case "POWER":
					switch(arg0.SqlDbType)
					{
						case SqlDbType.TinyInt:
						case SqlDbType.Int:
						case SqlDbType.SmallInt:
							return SqlTypeSystem.Create(SqlDbType.Int);
						case SqlDbType.Float:
						case SqlDbType.Real:
							return SqlTypeSystem.Create(SqlDbType.Float);
						default:
							return arg0;
					}
				case "PATINDEX":
				case "CHARINDEX":
					if(arg1.IsLargeType)
						return SqlTypeSystem.Create(SqlDbType.BigInt);
					return SqlTypeSystem.Create(SqlDbType.Int);
				case "SUBSTRING":
					if(functionCall.Arguments[2].NodeType == SqlNodeType.Value)
					{
						SqlValue val = (SqlValue)functionCall.Arguments[2];

						if(val.Value is int)
						{
							switch(arg0.SqlDbType)
							{
								case SqlDbType.NVarChar:
								case SqlDbType.NChar:
								case SqlDbType.VarChar:
								case SqlDbType.Char:
									return SqlTypeSystem.Create(arg0.SqlDbType, (int)val.Value);
								default:
									return null;
							}
						}
					}
					switch(arg0.SqlDbType)
					{
						case SqlDbType.NVarChar:
						case SqlDbType.NChar:
							return SqlTypeSystem.Create(SqlDbType.NVarChar);
						case SqlDbType.VarChar:
						case SqlDbType.Char:
							return SqlTypeSystem.Create(SqlDbType.VarChar);
						default:
							return null;
					}
				case "STUFF":
					// if the stuff call is an insertion  and is strictly additive
					// (no deletion of characters) the return type is the same as 
					// a concatenation
					if(functionCall.Arguments.Count == 4)
					{
						SqlValue delLength = functionCall.Arguments[2] as SqlValue;
						if(delLength != null && (int)delLength.Value == 0)
						{
							return PredictTypeForBinary(SqlNodeType.Concat,
								functionCall.Arguments[0].SqlType, functionCall.Arguments[3].SqlType);
						}
					}
					return null;
				case "LOWER":
				case "UPPER":
				case "RTRIM":
				case "LTRIM":
				case "INSERT":
				case "REPLACE":
				case "LEFT":
				case "RIGHT":
				case "REVERSE":
					return arg0;
				default:
					return null;
			}
		}

		internal override ProviderType ChangeTypeFamilyTo(ProviderType type, ProviderType toType)
		{
			// if this is already the same family, do nothing
			if(type.IsSameTypeFamily(toType))
				return type;

			// otherwise as a default return toType
			// but look for special cases we have to convert carefully
			if(type.IsApplicationType || toType.IsApplicationType)
				return toType;
			SqlType sqlToType = (SqlType)toType;
			SqlType sqlThisType = (SqlType)type;

			if(sqlToType.Category == TypeCategory.Numeric && sqlThisType.Category == TypeCategory.Char)
			{
				switch(sqlThisType.SqlDbType)
				{
					case SqlDbType.NChar:
						return SqlTypeSystem.Create(SqlDbType.Int);
					case SqlDbType.Char:
						return SqlTypeSystem.Create(SqlDbType.SmallInt);
					default:
						return toType;
				}
			}
			else
			{
				return toType;
			}
		}

		[SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Justification = "[....]: Cast is dependent on node type and casts do not happen unecessarily in a single code path.")]
		internal override ProviderType GetBestType(ProviderType typeA, ProviderType typeB)
		{
			// first determine the type precedence
			SqlType bestType = (SqlType)(typeA.ComparePrecedenceTo(typeB) > 0 ? typeA : typeB);

			// if one of the types is a not a server type, return
			// that type
			if(typeA.IsApplicationType || typeA.IsRuntimeOnlyType)
			{
				return typeA;
			}
			if(typeB.IsApplicationType || typeB.IsRuntimeOnlyType)
			{
				return typeB;
			}

			SqlType sqlTypeA = (SqlType)typeA;
			SqlType sqlTypeB = (SqlType)typeB;
			if(sqlTypeA.HasPrecisionAndScale && sqlTypeB.HasPrecisionAndScale && bestType.SqlDbType == SqlDbType.Decimal)
			{
				int p0 = sqlTypeA.Precision;
				int s0 = sqlTypeA.Scale;
				int p1 = sqlTypeB.Precision;
				int s1 = sqlTypeB.Scale;
				// precision and scale may be zero if this is an unsized type.
				if(p0 == 0 && s0 == 0 && p1 == 0 && s1 == 0)
				{
					return SqlTypeSystem.Create(bestType.SqlDbType);
				}
				else if(p0 == 0 && s0 == 0)
				{
					return SqlTypeSystem.Create(bestType.SqlDbType, p1, s1);
				}
				else if(p1 == 0 && s1 == 0)
				{
					return SqlTypeSystem.Create(bestType.SqlDbType, p0, s0);
				}
				// determine best scale/precision
				int bestLeft = Math.Max(p0 - s0, p1 - s1);
				int bestRight = Math.Max(s0, s1);
				int precision = Math.Min(bestLeft + bestRight, 38);
				return SqlTypeSystem.Create(bestType.SqlDbType, precision, /*scale*/bestRight);
			}
			else
			{
				// determine the best size
				int? bestSize = null;

				if(sqlTypeA.Size.HasValue && sqlTypeB.Size.HasValue)
				{
					bestSize = (sqlTypeB.Size > sqlTypeA.Size) ? sqlTypeB.Size : sqlTypeA.Size;
				}

				if(sqlTypeB.Size.HasValue && sqlTypeB.Size.Value == ProviderConstants.LargeTypeSizeIndicator
				   || sqlTypeA.Size.HasValue && sqlTypeA.Size.Value == ProviderConstants.LargeTypeSizeIndicator)
				{
					// the large type size trumps all
					bestSize = ProviderConstants.LargeTypeSizeIndicator;
				}

				bestType = new SqlType(bestType.SqlDbType, bestSize);
			}

			return bestType;
		}

		internal override ProviderType From(object o)
		{
			Type clrType = (o != null) ? o.GetType() : typeof(object);
			if(clrType == typeof(string))
			{
				string str = (string)o;
				return From(clrType, str.Length);
			}
			else if(clrType == typeof(bool))
			{
				return From(typeof(int));
			}
			else if(clrType.IsArray)
			{
				Array arr = (Array)o;
				return From(clrType, arr.Length);
			}
			else if(clrType == typeof(decimal))
			{
				decimal d = (decimal)o;
				// The CLR stores the scale of a decimal value in bits
				// 16 to 23 (i.e., mask 0x00FF0000) of the fourth int. 
				int scale = (Decimal.GetBits(d)[3] & 0x00FF0000) >> 16;
				return From(clrType, scale);
			}
			else
			{
				return From(clrType);
			}
		}

		internal override ProviderType From(Type type)
		{
			return From(type, null);
		}

		internal override ProviderType From(Type type, int? size)
		{
			return From(type, size);
		}
	}
}