using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Globalization;

namespace LinqToSqlShared.Mapping {

    /// <summary>
    /// Shared rules governing the mapping system.
	/// DatabaseMapping and related classes represent a parsed version of the
	/// XML mapping string. This unvalidated intermediate representation is 
	/// necessary because unused mappings are intentially never validated.
	/// </summary>
    internal static class MappingSystem {
        /// <summary>
        /// Return true if this is a clr type supported as an inheritance discriminator.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static bool IsSupportedDiscriminatorType(Type type) {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                type = type.GetGenericArguments()[0];
            }
            switch (Type.GetTypeCode(type)) {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Char:
                case TypeCode.String:
                case TypeCode.Boolean:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Return true if this is a clr type supported as an inheritance discriminator.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static bool IsSupportedDiscriminatorType(SqlDbType type) {
            switch (type) {
                case SqlDbType.BigInt:
                case SqlDbType.Bit:
                case SqlDbType.Char:
                case SqlDbType.Int:
                case SqlDbType.NChar:
                case SqlDbType.NVarChar:
                case SqlDbType.SmallInt:
                case SqlDbType.TinyInt:
                case SqlDbType.VarChar:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Return true if this is a CLR type supported as an identity member.  Since identity
        /// management (caching) relies on key members being hashable, only types implementing
        /// GetHashCode are supported.  Also, the runtime relies on identity members being comparable,
        /// so only types implementing Equals are supported.
        /// </summary>
        internal static bool IsSupportedIdentityType(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = type.GetGenericArguments()[0];
            }
            if (type == typeof(Guid) || type == typeof(DateTime) || type == typeof(DateTimeOffset) ||
                type == typeof(TimeSpan) || type == typeof(Binary))
            {
                return true;
            }
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Char:
                case TypeCode.String:
                case TypeCode.Boolean:
                case TypeCode.Decimal:
                case TypeCode.Single:
                case TypeCode.Double:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Types that do not support comparison cannot be used as primary keys.  The
        /// database will restrict this, but we can't rely on that, since it is possible
        /// to create a key mapping to a column that isn't truly a key in the DB.
        /// </summary>
        internal static bool IsSupportedIdentityType(SqlDbType type)
        {
            switch (type) {
                case SqlDbType.NText:
                case SqlDbType.Image:
                case SqlDbType.Xml:
                case SqlDbType.Text:
                case SqlDbType.Variant:
                case SqlDbType.Udt:
                    return false;
                default:
                    return true;
            }
        }

    }
}
