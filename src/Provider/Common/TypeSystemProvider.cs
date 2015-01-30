using System;
using System.Collections.Generic;
using System.Data.Linq.Provider.NodeTypes;
using System.Text;

namespace System.Data.Linq.Provider.Common {

    /// <summary>
    /// Abstracts the provider side type system. Encapsulates:
    /// - Mapping from runtime types to provider types.
    /// - Parsing type strings in the provider's language.
    /// - Handling application defined (opaque) types.
    /// - Type coercion precedence rules.
    /// - Type family organization.
    /// </summary>
    internal abstract class TypeSystemProvider {

        internal abstract ProviderType PredictTypeForUnary(SqlNodeType unaryOp, ProviderType operandType);
        internal abstract ProviderType PredictTypeForBinary(SqlNodeType binaryOp, ProviderType leftType, ProviderType rightType);

        /// <summary>
        /// Return the provider type corresponding to the given clr type.
        /// </summary>
        internal abstract ProviderType From(Type runtimeType);

        /// <summary>
        /// Return the provider type corresponding to the given object instance.
        /// </summary>
        internal abstract ProviderType From(object o);

        /// <summary>
        /// Return the provider type corresponding to the given clr type and size.
        /// </summary>
        internal abstract ProviderType From(Type type, int? size);

        /// <summary>
        /// Return a type by parsing a string. The format of the string is
        /// provider specific.
        /// </summary>
        internal abstract ProviderType Parse(string text);

        /// <summary>
        /// Return a type understood only by the application.
        /// Each call with the same index will return the same ProviderType.
        /// </summary>
        internal abstract ProviderType GetApplicationType(int index);

        /// <summary>
        /// Returns the most precise type in the family of the type given.
        /// A family is a group types that serve similar functions. For example,
        /// in SQL SmallInt and Int are part of one family.
        /// </summary>
        internal abstract ProviderType MostPreciseTypeInFamily(ProviderType type);

        /// <summary>
        /// For LOB data types that have large type equivalents, this function returns the equivalent large
        /// data type.  If the type is not an LOB or cannot be converted, the function returns the current type.
        /// For example SqlServer defines the 'Image' LOB type, whose large type equivalent is VarBinary(MAX).
        /// </summary>
        internal abstract ProviderType GetBestLargeType(ProviderType type);

        /// <summary>
        /// Returns a type that can be used to hold values for both the current
        /// type and the specified type without data loss.
        /// </summary>
        internal abstract ProviderType GetBestType(ProviderType typeA, ProviderType typeB);

        internal abstract ProviderType ReturnTypeOfFunction(SqlFunctionCall functionCall);

        /// <summary>
        /// Get a type that can hold the same information but belongs to a different type family.
        /// For example, to represent a SQL NChar as an integer type, we need to use the type int.
        /// (SQL smallint would not be able to contain characters with unicode >32768)
        /// </summary>
        /// <param name="type">Type of the target type family</param>
        /// <returns>Smallest type of target type family that can hold equivalent information</returns>
        internal abstract ProviderType ChangeTypeFamilyTo(ProviderType type, ProviderType typeWithFamily);

        internal abstract void InitializeParameter(ProviderType type, System.Data.Common.DbParameter parameter, object value);
    }
}
