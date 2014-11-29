//////////////////////////////////
//
//  This is a generated file and for now it's copied from the .NET 3.5 assembly as the original resx file isn't available 
//  in the reference source.
//
//////////////////////////////////

using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Transactions;
namespace System.Data.Linq.SqlClient
{
	internal static class Error
	{
		internal static Exception VbLikeDoesNotSupportMultipleCharacterRanges()
		{
			return new ArgumentException(Strings.VbLikeDoesNotSupportMultipleCharacterRanges);
		}
		internal static Exception VbLikeUnclosedBracket()
		{
			return new ArgumentException(Strings.VbLikeUnclosedBracket);
		}
		internal static Exception UnrecognizedProviderMode(object p0)
		{
			return new InvalidOperationException(Strings.UnrecognizedProviderMode(p0));
		}
		internal static Exception CompiledQueryCannotReturnType(object p0)
		{
			return new InvalidOperationException(Strings.CompiledQueryCannotReturnType(p0));
		}
		internal static Exception ArgumentEmpty(object p0)
		{
			return new ArgumentException(Strings.ArgumentEmpty(p0));
		}
		internal static Exception ProviderCannotBeUsedAfterDispose()
		{
			return new ObjectDisposedException(Strings.ProviderCannotBeUsedAfterDispose);
		}
		internal static Exception ArgumentTypeMismatch(object p0)
		{
			return new ArgumentException(Strings.ArgumentTypeMismatch(p0));
		}
		internal static Exception ContextNotInitialized()
		{
			return new InvalidOperationException(Strings.ContextNotInitialized);
		}
		internal static Exception CouldNotDetermineSqlType(object p0)
		{
			return new InvalidOperationException(Strings.CouldNotDetermineSqlType(p0));
		}
		internal static Exception CouldNotDetermineDbGeneratedSqlType(object p0)
		{
			return new InvalidOperationException(Strings.CouldNotDetermineDbGeneratedSqlType(p0));
		}
		internal static Exception CouldNotDetermineCatalogName()
		{
			return new InvalidOperationException(Strings.CouldNotDetermineCatalogName);
		}
		internal static Exception CreateDatabaseFailedBecauseOfClassWithNoMembers(object p0)
		{
			return new InvalidOperationException(Strings.CreateDatabaseFailedBecauseOfClassWithNoMembers(p0));
		}
		internal static Exception CreateDatabaseFailedBecauseOfContextWithNoTables(object p0)
		{
			return new InvalidOperationException(Strings.CreateDatabaseFailedBecauseOfContextWithNoTables(p0));
		}
		internal static Exception CreateDatabaseFailedBecauseSqlCEDatabaseAlreadyExists(object p0)
		{
			return new InvalidOperationException(Strings.CreateDatabaseFailedBecauseSqlCEDatabaseAlreadyExists(p0));
		}
		internal static Exception DistributedTransactionsAreNotAllowed()
		{
			return new TransactionPromotionException(Strings.DistributedTransactionsAreNotAllowed);
		}
		internal static Exception InvalidConnectionArgument(object p0)
		{
			return new ArgumentException(Strings.InvalidConnectionArgument(p0));
		}
		internal static Exception CannotEnumerateResultsMoreThanOnce()
		{
			return new InvalidOperationException(Strings.CannotEnumerateResultsMoreThanOnce);
		}
		internal static Exception IifReturnTypesMustBeEqual(object p0, object p1)
		{
			return new NotSupportedException(Strings.IifReturnTypesMustBeEqual(p0, p1));
		}
		internal static Exception MethodNotMappedToStoredProcedure(object p0)
		{
			return new InvalidOperationException(Strings.MethodNotMappedToStoredProcedure(p0));
		}
		internal static Exception ResultTypeNotMappedToFunction(object p0, object p1)
		{
			return new InvalidOperationException(Strings.ResultTypeNotMappedToFunction(p0, p1));
		}
		internal static Exception ToStringOnlySupportedForPrimitiveTypes()
		{
			return new NotSupportedException(Strings.ToStringOnlySupportedForPrimitiveTypes);
		}
		internal static Exception TransactionDoesNotMatchConnection()
		{
			return new InvalidOperationException(Strings.TransactionDoesNotMatchConnection);
		}
		internal static Exception UnexpectedTypeCode(object p0)
		{
			return new InvalidOperationException(Strings.UnexpectedTypeCode(p0));
		}
		internal static Exception UnsupportedDateTimeConstructorForm()
		{
			return new NotSupportedException(Strings.UnsupportedDateTimeConstructorForm);
		}
		internal static Exception UnsupportedDateTimeOffsetConstructorForm()
		{
			return new NotSupportedException(Strings.UnsupportedDateTimeOffsetConstructorForm);
		}
		internal static Exception UnsupportedStringConstructorForm()
		{
			return new NotSupportedException(Strings.UnsupportedStringConstructorForm);
		}
		internal static Exception UnsupportedTimeSpanConstructorForm()
		{
			return new NotSupportedException(Strings.UnsupportedTimeSpanConstructorForm);
		}
		internal static Exception UnsupportedTypeConstructorForm(object p0)
		{
			return new NotSupportedException(Strings.UnsupportedTypeConstructorForm(p0));
		}
		internal static Exception WrongNumberOfValuesInCollectionArgument(object p0, object p1, object p2)
		{
			return new ArgumentException(Strings.WrongNumberOfValuesInCollectionArgument(p0, p1, p2));
		}
		internal static Exception MemberCannotBeTranslated(object p0, object p1)
		{
			return new NotSupportedException(Strings.MemberCannotBeTranslated(p0, p1));
		}
		internal static Exception NonConstantExpressionsNotSupportedFor(object p0)
		{
			return new NotSupportedException(Strings.NonConstantExpressionsNotSupportedFor(p0));
		}
		internal static Exception MathRoundNotSupported()
		{
			return new NotSupportedException(Strings.MathRoundNotSupported);
		}
		internal static Exception SqlMethodOnlyForSql(object p0)
		{
			return new NotSupportedException(Strings.SqlMethodOnlyForSql(p0));
		}
		internal static Exception NonConstantExpressionsNotSupportedForRounding()
		{
			return new NotSupportedException(Strings.NonConstantExpressionsNotSupportedForRounding);
		}
		internal static Exception CompiledQueryAgainstMultipleShapesNotSupported()
		{
			return new NotSupportedException(Strings.CompiledQueryAgainstMultipleShapesNotSupported);
		}
		internal static Exception IndexOfWithStringComparisonArgNotSupported()
		{
			return new NotSupportedException(Strings.IndexOfWithStringComparisonArgNotSupported);
		}
		internal static Exception LastIndexOfWithStringComparisonArgNotSupported()
		{
			return new NotSupportedException(Strings.LastIndexOfWithStringComparisonArgNotSupported);
		}
		internal static Exception ConvertToCharFromBoolNotSupported()
		{
			return new NotSupportedException(Strings.ConvertToCharFromBoolNotSupported);
		}
		internal static Exception ConvertToDateTimeOnlyForDateTimeOrString()
		{
			return new NotSupportedException(Strings.ConvertToDateTimeOnlyForDateTimeOrString);
		}
		internal static Exception SkipIsValidOnlyOverOrderedQueries()
		{
			return new InvalidOperationException(Strings.SkipIsValidOnlyOverOrderedQueries);
		}
		internal static Exception SkipRequiresSingleTableQueryWithPKs()
		{
			return new NotSupportedException(Strings.SkipRequiresSingleTableQueryWithPKs);
		}
		internal static Exception NoMethodInTypeMatchingArguments(object p0)
		{
			return new InvalidOperationException(Strings.NoMethodInTypeMatchingArguments(p0));
		}
		internal static Exception CannotConvertToEntityRef(object p0)
		{
			return new InvalidOperationException(Strings.CannotConvertToEntityRef(p0));
		}
		internal static Exception ExpressionNotDeferredQuerySource()
		{
			return new InvalidOperationException(Strings.ExpressionNotDeferredQuerySource);
		}
		internal static Exception DeferredMemberWrongType()
		{
			return new InvalidOperationException(Strings.DeferredMemberWrongType);
		}
		internal static Exception ArgumentWrongType(object p0, object p1, object p2)
		{
			return new ArgumentException(Strings.ArgumentWrongType(p0, p1, p2));
		}
		internal static Exception ArgumentWrongValue(object p0)
		{
			return new ArgumentException(Strings.ArgumentWrongValue(p0));
		}
		internal static Exception BadProjectionInSelect()
		{
			return new InvalidOperationException(Strings.BadProjectionInSelect);
		}
		internal static Exception InvalidReturnFromSproc(object p0)
		{
			return new InvalidOperationException(Strings.InvalidReturnFromSproc(p0));
		}
		internal static Exception WrongDataContext()
		{
			return new InvalidOperationException(Strings.WrongDataContext);
		}
		internal static Exception BinaryOperatorNotRecognized(object p0)
		{
			return new InvalidOperationException(Strings.BinaryOperatorNotRecognized(p0));
		}
		internal static Exception CannotAggregateType(object p0)
		{
			return new NotSupportedException(Strings.CannotAggregateType(p0));
		}
		internal static Exception CannotCompareItemsAssociatedWithDifferentTable()
		{
			return new InvalidOperationException(Strings.CannotCompareItemsAssociatedWithDifferentTable);
		}
		internal static Exception CannotDeleteTypesOf(object p0)
		{
			return new InvalidOperationException(Strings.CannotDeleteTypesOf(p0));
		}
		internal static Exception ClassLiteralsNotAllowed(object p0)
		{
			return new InvalidOperationException(Strings.ClassLiteralsNotAllowed(p0));
		}
		internal static Exception ClientCaseShouldNotHold(object p0)
		{
			return new InvalidOperationException(Strings.ClientCaseShouldNotHold(p0));
		}
		internal static Exception ClrBoolDoesNotAgreeWithSqlType(object p0)
		{
			return new InvalidOperationException(Strings.ClrBoolDoesNotAgreeWithSqlType(p0));
		}
		internal static Exception ColumnCannotReferToItself()
		{
			return new InvalidOperationException(Strings.ColumnCannotReferToItself);
		}
		internal static Exception ColumnClrTypeDoesNotAgreeWithExpressionsClrType()
		{
			return new InvalidOperationException(Strings.ColumnClrTypeDoesNotAgreeWithExpressionsClrType);
		}
		internal static Exception ColumnIsDefinedInMultiplePlaces(object p0)
		{
			return new InvalidOperationException(Strings.ColumnIsDefinedInMultiplePlaces(p0));
		}
		internal static Exception ColumnIsNotAccessibleThroughGroupBy(object p0)
		{
			return new InvalidOperationException(Strings.ColumnIsNotAccessibleThroughGroupBy(p0));
		}
		internal static Exception ColumnIsNotAccessibleThroughDistinct(object p0)
		{
			return new InvalidOperationException(Strings.ColumnIsNotAccessibleThroughDistinct(p0));
		}
		internal static Exception ColumnReferencedIsNotInScope(object p0)
		{
			return new InvalidOperationException(Strings.ColumnReferencedIsNotInScope(p0));
		}
		internal static Exception ConstructedArraysNotSupported()
		{
			return new NotSupportedException(Strings.ConstructedArraysNotSupported);
		}
		internal static Exception ParametersCannotBeSequences()
		{
			return new NotSupportedException(Strings.ParametersCannotBeSequences);
		}
		internal static Exception CapturedValuesCannotBeSequences()
		{
			return new NotSupportedException(Strings.CapturedValuesCannotBeSequences);
		}
		internal static Exception CouldNotAssignSequence(object p0, object p1)
		{
			return new InvalidOperationException(Strings.CouldNotAssignSequence(p0, p1));
		}
		internal static Exception CouldNotTranslateExpressionForReading(object p0)
		{
			return new InvalidOperationException(Strings.CouldNotTranslateExpressionForReading(p0));
		}
		internal static Exception CouldNotGetClrType()
		{
			return new InvalidOperationException(Strings.CouldNotGetClrType);
		}
		internal static Exception CouldNotGetSqlType()
		{
			return new InvalidOperationException(Strings.CouldNotGetSqlType);
		}
		internal static Exception CouldNotHandleAliasRef(object p0)
		{
			return new InvalidOperationException(Strings.CouldNotHandleAliasRef(p0));
		}
		internal static Exception DidNotExpectAs(object p0)
		{
			return new InvalidOperationException(Strings.DidNotExpectAs(p0));
		}
		internal static Exception DidNotExpectTypeBinding()
		{
			return new InvalidOperationException(Strings.DidNotExpectTypeBinding);
		}
		internal static Exception DidNotExpectTypeChange(object p0, object p1)
		{
			return new InvalidOperationException(Strings.DidNotExpectTypeChange(p0, p1));
		}
		internal static Exception EmptyCaseNotSupported()
		{
			return new InvalidOperationException(Strings.EmptyCaseNotSupported);
		}
		internal static Exception ExpectedNoObjectType()
		{
			return new InvalidOperationException(Strings.ExpectedNoObjectType);
		}
		internal static Exception ExpectedBitFoundPredicate()
		{
			return new ArgumentException(Strings.ExpectedBitFoundPredicate);
		}
		internal static Exception ExpectedClrTypesToAgree(object p0, object p1)
		{
			return new InvalidOperationException(Strings.ExpectedClrTypesToAgree(p0, p1));
		}
		internal static Exception ExpectedPredicateFoundBit()
		{
			return new ArgumentException(Strings.ExpectedPredicateFoundBit);
		}
		internal static Exception ExpectedQueryableArgument(object p0, object p1, object p2)
		{
			return new ArgumentException(Strings.ExpectedQueryableArgument(p0, p1, p2));
		}
		internal static Exception InvalidGroupByExpressionType(object p0)
		{
			return new NotSupportedException(Strings.InvalidGroupByExpressionType(p0));
		}
		internal static Exception InvalidGroupByExpression()
		{
			return new NotSupportedException(Strings.InvalidGroupByExpression);
		}
		internal static Exception InvalidOrderByExpression(object p0)
		{
			return new NotSupportedException(Strings.InvalidOrderByExpression(p0));
		}
		internal static Exception Impossible()
		{
			return new InvalidOperationException(Strings.Impossible);
		}
		internal static Exception InfiniteDescent()
		{
			return new InvalidOperationException(Strings.InfiniteDescent);
		}
		internal static Exception InvalidFormatNode(object p0)
		{
			return new InvalidOperationException(Strings.InvalidFormatNode(p0));
		}
		internal static Exception InvalidReferenceToRemovedAliasDuringDeflation()
		{
			return new InvalidOperationException(Strings.InvalidReferenceToRemovedAliasDuringDeflation);
		}
		internal static Exception InvalidSequenceOperatorCall(object p0)
		{
			return new InvalidOperationException(Strings.InvalidSequenceOperatorCall(p0));
		}
		internal static Exception ParameterNotInScope(object p0)
		{
			return new InvalidOperationException(Strings.ParameterNotInScope(p0));
		}
		internal static Exception MemberAccessIllegal(object p0, object p1, object p2)
		{
			return new InvalidOperationException(Strings.MemberAccessIllegal(p0, p1, p2));
		}
		internal static Exception MemberCouldNotBeTranslated(object p0, object p1)
		{
			return new InvalidOperationException(Strings.MemberCouldNotBeTranslated(p0, p1));
		}
		internal static Exception MemberNotPartOfProjection(object p0, object p1)
		{
			return new InvalidOperationException(Strings.MemberNotPartOfProjection(p0, p1));
		}
		internal static Exception MethodHasNoSupportConversionToSql(object p0)
		{
			return new NotSupportedException(Strings.MethodHasNoSupportConversionToSql(p0));
		}
		internal static Exception MethodFormHasNoSupportConversionToSql(object p0, object p1)
		{
			return new NotSupportedException(Strings.MethodFormHasNoSupportConversionToSql(p0, p1));
		}
		internal static Exception UnableToBindUnmappedMember(object p0, object p1, object p2)
		{
			return new InvalidOperationException(Strings.UnableToBindUnmappedMember(p0, p1, p2));
		}
		internal static Exception QueryOperatorNotSupported(object p0)
		{
			return new NotSupportedException(Strings.QueryOperatorNotSupported(p0));
		}
		internal static Exception QueryOperatorOverloadNotSupported(object p0)
		{
			return new NotSupportedException(Strings.QueryOperatorOverloadNotSupported(p0));
		}
		internal static Exception ReaderUsedAfterDispose()
		{
			return new InvalidOperationException(Strings.ReaderUsedAfterDispose);
		}
		internal static Exception RequiredColumnDoesNotExist(object p0)
		{
			return new InvalidOperationException(Strings.RequiredColumnDoesNotExist(p0));
		}
		internal static Exception SimpleCaseShouldNotHold(object p0)
		{
			return new InvalidOperationException(Strings.SimpleCaseShouldNotHold(p0));
		}
		internal static Exception TypeBinaryOperatorNotRecognized()
		{
			return new InvalidOperationException(Strings.TypeBinaryOperatorNotRecognized);
		}
		internal static Exception UnexpectedNode(object p0)
		{
			return new InvalidOperationException(Strings.UnexpectedNode(p0));
		}
		internal static Exception UnexpectedFloatingColumn()
		{
			return new InvalidOperationException(Strings.UnexpectedFloatingColumn);
		}
		internal static Exception UnexpectedSharedExpression()
		{
			return new InvalidOperationException(Strings.UnexpectedSharedExpression);
		}
		internal static Exception UnexpectedSharedExpressionReference()
		{
			return new InvalidOperationException(Strings.UnexpectedSharedExpressionReference);
		}
		internal static Exception UnhandledBindingType(object p0)
		{
			return new InvalidOperationException(Strings.UnhandledBindingType(p0));
		}
		internal static Exception UnhandledStringTypeComparison()
		{
			return new NotSupportedException(Strings.UnhandledStringTypeComparison);
		}
		internal static Exception UnhandledMemberAccess(object p0, object p1)
		{
			return new InvalidOperationException(Strings.UnhandledMemberAccess(p0, p1));
		}
		internal static Exception UnmappedDataMember(object p0, object p1, object p2)
		{
			return new InvalidOperationException(Strings.UnmappedDataMember(p0, p1, p2));
		}
		internal static Exception UnrecognizedExpressionNode(object p0)
		{
			return new InvalidOperationException(Strings.UnrecognizedExpressionNode(p0));
		}
		internal static Exception ValueHasNoLiteralInSql(object p0)
		{
			return new InvalidOperationException(Strings.ValueHasNoLiteralInSql(p0));
		}
		internal static Exception UnionIncompatibleConstruction()
		{
			return new NotSupportedException(Strings.UnionIncompatibleConstruction);
		}
		internal static Exception UnionDifferentMembers()
		{
			return new NotSupportedException(Strings.UnionDifferentMembers);
		}
		internal static Exception UnionDifferentMemberOrder()
		{
			return new NotSupportedException(Strings.UnionDifferentMemberOrder);
		}
		internal static Exception UnionOfIncompatibleDynamicTypes()
		{
			return new NotSupportedException(Strings.UnionOfIncompatibleDynamicTypes);
		}
		internal static Exception UnionWithHierarchy()
		{
			return new NotSupportedException(Strings.UnionWithHierarchy);
		}
		internal static Exception UnhandledExpressionType(object p0)
		{
			return new ArgumentException(Strings.UnhandledExpressionType(p0));
		}
		internal static Exception IntersectNotSupportedForHierarchicalTypes()
		{
			return new NotSupportedException(Strings.IntersectNotSupportedForHierarchicalTypes);
		}
		internal static Exception ExceptNotSupportedForHierarchicalTypes()
		{
			return new NotSupportedException(Strings.ExceptNotSupportedForHierarchicalTypes);
		}
		internal static Exception NonCountAggregateFunctionsAreNotValidOnProjections(object p0)
		{
			return new NotSupportedException(Strings.NonCountAggregateFunctionsAreNotValidOnProjections(p0));
		}
		internal static Exception GroupingNotSupportedAsOrderCriterion()
		{
			return new NotSupportedException(Strings.GroupingNotSupportedAsOrderCriterion);
		}
		internal static Exception SelectManyDoesNotSupportStrings()
		{
			return new ArgumentException(Strings.SelectManyDoesNotSupportStrings);
		}
		internal static Exception SequenceOperatorsNotSupportedForType(object p0)
		{
			return new NotSupportedException(Strings.SequenceOperatorsNotSupportedForType(p0));
		}
		internal static Exception SkipNotSupportedForSequenceTypes()
		{
			return new NotSupportedException(Strings.SkipNotSupportedForSequenceTypes);
		}
		internal static Exception ComparisonNotSupportedForType(object p0)
		{
			return new NotSupportedException(Strings.ComparisonNotSupportedForType(p0));
		}
		internal static Exception QueryOnLocalCollectionNotSupported()
		{
			return new NotSupportedException(Strings.QueryOnLocalCollectionNotSupported);
		}
		internal static Exception UnsupportedNodeType(object p0)
		{
			return new NotSupportedException(Strings.UnsupportedNodeType(p0));
		}
		internal static Exception TypeColumnWithUnhandledSource()
		{
			return new InvalidOperationException(Strings.TypeColumnWithUnhandledSource);
		}
		internal static Exception GeneralCollectionMaterializationNotSupported()
		{
			return new NotSupportedException(Strings.GeneralCollectionMaterializationNotSupported);
		}
		internal static Exception TypeCannotBeOrdered(object p0)
		{
			return new InvalidOperationException(Strings.TypeCannotBeOrdered(p0));
		}
		internal static Exception InvalidMethodExecution(object p0)
		{
			return new InvalidOperationException(Strings.InvalidMethodExecution(p0));
		}
		internal static Exception SprocsCannotBeComposed()
		{
			return new InvalidOperationException(Strings.SprocsCannotBeComposed);
		}
		internal static Exception InsertItemMustBeConstant()
		{
			return new NotSupportedException(Strings.InsertItemMustBeConstant);
		}
		internal static Exception UpdateItemMustBeConstant()
		{
			return new NotSupportedException(Strings.UpdateItemMustBeConstant);
		}
		internal static Exception CouldNotConvertToPropertyOrField(object p0)
		{
			return new InvalidOperationException(Strings.CouldNotConvertToPropertyOrField(p0));
		}
		internal static Exception BadParameterType(object p0)
		{
			return new NotSupportedException(Strings.BadParameterType(p0));
		}
		internal static Exception CannotAssignToMember(object p0)
		{
			return new InvalidOperationException(Strings.CannotAssignToMember(p0));
		}
		internal static Exception MappedTypeMustHaveDefaultConstructor(object p0)
		{
			return new InvalidOperationException(Strings.MappedTypeMustHaveDefaultConstructor(p0));
		}
		internal static Exception UnsafeStringConversion(object p0, object p1)
		{
			return new FormatException(Strings.UnsafeStringConversion(p0, p1));
		}
		internal static Exception CannotAssignNull(object p0)
		{
			return new InvalidOperationException(Strings.CannotAssignNull(p0));
		}
		internal static Exception ProviderNotInstalled(object p0, object p1)
		{
			return new InvalidOperationException(Strings.ProviderNotInstalled(p0, p1));
		}
		internal static Exception InvalidProviderType(object p0)
		{
			return new NotSupportedException(Strings.InvalidProviderType(p0));
		}
		internal static Exception InvalidDbGeneratedType(object p0)
		{
			return new NotSupportedException(Strings.InvalidDbGeneratedType(p0));
		}
		internal static Exception DatabaseDeleteThroughContext()
		{
			return new InvalidOperationException(Strings.DatabaseDeleteThroughContext);
		}
		internal static Exception CannotMaterializeEntityType(object p0)
		{
			return new NotSupportedException(Strings.CannotMaterializeEntityType(p0));
		}
		internal static Exception ArgumentNull(string paramName)
		{
			return new ArgumentNullException(paramName);
		}
		internal static Exception ArgumentOutOfRange(string paramName)
		{
			return new ArgumentOutOfRangeException(paramName);
		}
		internal static Exception NotImplemented()
		{
			return new NotImplementedException();
		}
		internal static Exception NotSupported()
		{
			return new NotSupportedException();
		}
		internal static Exception ExpressionNotSupportedForSqlServerVersion(Collection<string> reasons)
		{
			StringBuilder stringBuilder = new StringBuilder(Strings.CannotTranslateExpressionToSql);
			foreach (string current in reasons)
			{
				stringBuilder.AppendLine(current);
			}
			return new NotSupportedException(stringBuilder.ToString());
		}

		internal static Exception CannotMaterializeList(Type type)
		{
			throw new InvalidOperationException(Strings.CannotMaterializeList(type));
		}

		internal static Exception CouldNotConvert(Type argType, Type eType)
		{
			throw new InvalidOperationException(Strings.CouldNotConvert(argType, eType));
		}

		internal static Exception IQueryableCannotReturnSelfReferencingConstantExpression()
		{
			throw new InvalidOperationException(Strings.IQueryableCannotReturnSelfReferencingConstantExpression);
		}
	}
}
