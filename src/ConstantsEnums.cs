using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Linq
{
	/// <summary>
	/// Describes the type of change the entity will undergo when submitted to the database.
	/// </summary>
	public enum ChangeAction
	{
		/// <summary>
		/// The entity will not be submitted.
		/// </summary>
		None = 0,
		/// <summary>
		/// The entity will be deleted.
		/// </summary>
		Delete,
		/// <summary>
		/// The entity will be inserted.
		/// </summary>
		Insert,
		/// <summary>
		/// The entity will be updated.
		/// </summary>
		Update
	}


	internal enum SqlMultiplexerOptionType
	{
		None,
		EnableBigJoin
	}

	internal enum SqlParameterType
	{
		Value,
		UserArgument,
		PreviousResult
	}


	/// <summary>
	/// Used to specify how a submit should behave when one
	/// or more updates fail due to optimistic concurrency
	/// conflicts.
	/// </summary>
	public enum ConflictMode
	{
		/// <summary>
		/// Fail immediately when the first change conflict is encountered.
		/// </summary>
		FailOnFirstConflict,
		/// <summary>
		/// Only fail after all changes have been attempted.
		/// </summary>
		ContinueOnConflict
	}

	/// <summary>
	/// Used to specify a value synchronization strategy. 
	/// </summary>
	public enum RefreshMode
	{
		/// <summary>
		/// Keep the current values.
		/// </summary>
		KeepCurrentValues,
		/// <summary>
		/// Current values that have been changed are not modified, but
		/// any unchanged values are updated with the current database
		/// values.  No changes are lost in this merge.
		/// </summary>
		KeepChanges,
		/// <summary>
		/// All current values are overwritten with current database values,
		/// regardless of whether they have been changed.
		/// </summary>
		OverwriteCurrentValues
	}


	/// <summary>
	/// Flags control the format of string returned by ToQueryString().
	/// </summary>
	[Flags]
	internal enum QueryFormatOptions
	{
		None = 0,
		SuppressSize = 1
	}


	/// <summary>
	/// Enum to specify the type category of a DB type
	/// </summary>
	internal enum TypeCategory
	{
		Numeric,
		Char,
		Text,
		Binary,
		Image,
		Xml,
		DateTime,
		UniqueIdentifier,
		Variant,
		Udt
	}


	/// <summary>
	/// Enum to specify the type of a SqlNode, which is the node type used to build a processed Expressions tree with
	/// </summary>
	internal enum SqlNodeType
	{
		Add,
		Alias,
		AliasRef,
		And,
		Assign,
		Avg,
		Between,
		BitAnd,
		BitNot,
		BitOr,
		BitXor,
		Block,
		ClientArray,
		ClientCase,
		ClientParameter,
		ClientQuery,
		ClrLength,
		Coalesce,
		Column,
		ColumnRef,
		Concat,
		Convert,
		Count,
		Delete,
		DiscriminatedType,
		DiscriminatorOf,
		Div,
		DoNotVisit,
		Element,
		ExprSet,
		EQ,
		EQ2V,
		Exists,
		FunctionCall,
		In,
		IncludeScope,
		IsNotNull,
		IsNull,
		LE,
		Lift,
		Link,
		Like,
		LongCount,
		LT,
		GE,
		Grouping,
		GT,
		Insert,
		Join,
		JoinedCollection,
		Max,
		MethodCall,
		Member,
		MemberAssign,
		Min,
		Mod,
		Mul,
		Multiset,
		NE,
		NE2V,
		Negate,
		New,
		Not,
		Not2V,
		Nop,
		Or,
		OptionalValue,
		OuterJoinedValue,
		Parameter,
		Property,
		Row,
		RowNumber,
		ScalarSubSelect,
		SearchedCase,
		Select,
		SharedExpression,
		SharedExpressionRef,
		SimpleCase,
		SimpleExpression,
		Stddev,
		StoredProcedureCall,
		Sub,
		Sum,
		Table,
		TableValuedFunctionCall,
		Treat,
		TypeCase,
		Union,
		Update,
		UserColumn,
		UserQuery,
		UserRow,
		Variable,
		Value,
		ValueOf
	}

	internal enum InternalExpressionType
	{
		Known = 2000,
		LinkedTable = 2001
	}

	/// <summary>
	/// These are application types used to represent types used during intermediate
	/// stages of the query building process.
	/// </summary>
	internal enum ConverterSpecialTypes
	{
		Row,
		Table
	}

	internal enum SqlOrderType
	{
		Ascending,
		Descending
	}

	internal enum SqlOrderingType
	{
		Default,
		Never,
		Blocked,
		Always
	}

	internal enum SqlJoinType
	{
		Cross,
		Inner,
		LeftOuter,
		CrossApply,
		OuterApply
	}


	[Flags]
	internal enum ConverterStrategy
	{
		Default = 0x0,
		SkipWithRowNumber = 0x1,
		CanUseScopeIdentity = 0x2,
		CanUseOuterApply = 0x4,
		CanUseRowStatus = 0x8,
		CanUseJoinOn = 0x10,  // Whether or not to use ON clause of JOIN.
		CanOutputFromInsert = 0x20
	}

	internal enum MethodSupport
	{
		None,         // Unsupported method
		MethodGroup,  // One or more overloads of the method are supported
		Method        // The particular method form specified is supported (stronger)
	}

	/// <summary>
	/// Flags that control the optimization of SQL produced.
	/// Only optimization flags should go here because QA will be looking
	/// here to see what optimizations they need to test.
	/// </summary>
	[Flags]
	internal enum OptimizationFlags
	{
		None = 0,
		SimplifyCaseStatements = 1,
		OptimizeLinkExpansions = 2,
		All = SimplifyCaseStatements | OptimizeLinkExpansions
	}
}
