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
using System.Xml.Schema;
namespace System.Data.Linq
{
	internal static class Error
	{
		internal static Exception CannotAddChangeConflicts()
		{
			return new NotSupportedException(Strings.CannotAddChangeConflicts);
		}
		internal static Exception CannotRemoveChangeConflicts()
		{
			return new NotSupportedException(Strings.CannotRemoveChangeConflicts);
		}
		internal static Exception InconsistentAssociationAndKeyChange(object p0, object p1)
		{
			return new InvalidOperationException(Strings.InconsistentAssociationAndKeyChange(p0, p1));
		}
		internal static Exception UnableToDetermineDataContext()
		{
			return new InvalidOperationException(Strings.UnableToDetermineDataContext);
		}
		internal static Exception ArgumentTypeHasNoIdentityKey(object p0)
		{
			return new ArgumentException(Strings.ArgumentTypeHasNoIdentityKey(p0));
		}
		internal static Exception CouldNotConvert(object p0, object p1)
		{
			return new InvalidCastException(Strings.CouldNotConvert(p0, p1));
		}
		internal static Exception CannotRemoveUnattachedEntity()
		{
			return new InvalidOperationException(Strings.CannotRemoveUnattachedEntity);
		}
		internal static Exception ColumnMappedMoreThanOnce(object p0)
		{
			return new InvalidOperationException(Strings.ColumnMappedMoreThanOnce(p0));
		}
		internal static Exception CouldNotAttach()
		{
			return new InvalidOperationException(Strings.CouldNotAttach);
		}
		internal static Exception CouldNotGetTableForSubtype(object p0, object p1)
		{
			return new InvalidOperationException(Strings.CouldNotGetTableForSubtype(p0, p1));
		}
		internal static Exception CouldNotRemoveRelationshipBecauseOneSideCannotBeNull(object p0, object p1, object p2)
		{
			return new InvalidOperationException(Strings.CouldNotRemoveRelationshipBecauseOneSideCannotBeNull(p0, p1, p2));
		}
		internal static Exception EntitySetAlreadyLoaded()
		{
			return new InvalidOperationException(Strings.EntitySetAlreadyLoaded);
		}
		internal static Exception EntitySetModifiedDuringEnumeration()
		{
			return new InvalidOperationException(Strings.EntitySetModifiedDuringEnumeration);
		}
		internal static Exception ExpectedQueryableArgument(object p0, object p1)
		{
			return new ArgumentException(Strings.ExpectedQueryableArgument(p0, p1));
		}
		internal static Exception ExpectedUpdateDeleteOrChange()
		{
			return new InvalidOperationException(Strings.ExpectedUpdateDeleteOrChange);
		}
		internal static Exception KeyIsWrongSize(object p0, object p1)
		{
			return new InvalidOperationException(Strings.KeyIsWrongSize(p0, p1));
		}
		internal static Exception KeyValueIsWrongType(object p0, object p1)
		{
			return new InvalidOperationException(Strings.KeyValueIsWrongType(p0, p1));
		}
		internal static Exception IdentityChangeNotAllowed(object p0, object p1)
		{
			return new InvalidOperationException(Strings.IdentityChangeNotAllowed(p0, p1));
		}
		internal static Exception DbGeneratedChangeNotAllowed(object p0, object p1)
		{
			return new InvalidOperationException(Strings.DbGeneratedChangeNotAllowed(p0, p1));
		}
		internal static Exception ModifyDuringAddOrRemove()
		{
			return new ArgumentException(Strings.ModifyDuringAddOrRemove);
		}
		internal static Exception ProviderDoesNotImplementRequiredInterface(object p0, object p1)
		{
			return new InvalidOperationException(Strings.ProviderDoesNotImplementRequiredInterface(p0, p1));
		}
		internal static Exception ProviderTypeNull()
		{
			return new InvalidOperationException(Strings.ProviderTypeNull);
		}
		internal static Exception TypeCouldNotBeAdded(object p0)
		{
			return new InvalidOperationException(Strings.TypeCouldNotBeAdded(p0));
		}
		internal static Exception TypeCouldNotBeRemoved(object p0)
		{
			return new InvalidOperationException(Strings.TypeCouldNotBeRemoved(p0));
		}
		internal static Exception TypeCouldNotBeTracked(object p0)
		{
			return new InvalidOperationException(Strings.TypeCouldNotBeTracked(p0));
		}
		internal static Exception TypeIsNotEntity(object p0)
		{
			return new InvalidOperationException(Strings.TypeIsNotEntity(p0));
		}
		internal static Exception UnrecognizedRefreshObject()
		{
			return new ArgumentException(Strings.UnrecognizedRefreshObject);
		}
		internal static Exception UnhandledExpressionType(object p0)
		{
			return new ArgumentException(Strings.UnhandledExpressionType(p0));
		}
		internal static Exception UnhandledBindingType(object p0)
		{
			return new ArgumentException(Strings.UnhandledBindingType(p0));
		}
		internal static Exception ObjectTrackingRequired()
		{
			return new InvalidOperationException(Strings.ObjectTrackingRequired);
		}
		internal static Exception OptionsCannotBeModifiedAfterQuery()
		{
			return new InvalidOperationException(Strings.OptionsCannotBeModifiedAfterQuery);
		}
		internal static Exception DeferredLoadingRequiresObjectTracking()
		{
			return new InvalidOperationException(Strings.DeferredLoadingRequiresObjectTracking);
		}
		internal static Exception SubqueryDoesNotSupportOperator(object p0)
		{
			return new NotSupportedException(Strings.SubqueryDoesNotSupportOperator(p0));
		}
		internal static Exception SubqueryNotSupportedOn(object p0)
		{
			return new NotSupportedException(Strings.SubqueryNotSupportedOn(p0));
		}
		internal static Exception SubqueryNotSupportedOnType(object p0, object p1)
		{
			return new NotSupportedException(Strings.SubqueryNotSupportedOnType(p0, p1));
		}
		internal static Exception SubqueryNotAllowedAfterFreeze()
		{
			return new InvalidOperationException(Strings.SubqueryNotAllowedAfterFreeze);
		}
		internal static Exception IncludeNotAllowedAfterFreeze()
		{
			return new InvalidOperationException(Strings.IncludeNotAllowedAfterFreeze);
		}
		internal static Exception LoadOptionsChangeNotAllowedAfterQuery()
		{
			return new InvalidOperationException(Strings.LoadOptionsChangeNotAllowedAfterQuery);
		}
		internal static Exception IncludeCycleNotAllowed()
		{
			return new InvalidOperationException(Strings.IncludeCycleNotAllowed);
		}
		internal static Exception SubqueryMustBeSequence()
		{
			return new InvalidOperationException(Strings.SubqueryMustBeSequence);
		}
		internal static Exception RefreshOfDeletedObject()
		{
			return new InvalidOperationException(Strings.RefreshOfDeletedObject);
		}
		internal static Exception CannotChangeInheritanceType(object p0, object p1, object p2, object p3)
		{
			return new InvalidOperationException(Strings.CannotChangeInheritanceType(p0, p1, p2, p3));
		}
		internal static Exception DataContextCannotBeUsedAfterDispose()
		{
			return new ObjectDisposedException(Strings.DataContextCannotBeUsedAfterDispose);
		}
		internal static Exception TypeIsNotMarkedAsTable(object p0)
		{
			return new InvalidOperationException(Strings.TypeIsNotMarkedAsTable(p0));
		}
		internal static Exception NoMethodInTypeMatchingArguments(object p0)
		{
			return new InvalidOperationException(Strings.NoMethodInTypeMatchingArguments(p0));
		}
		internal static Exception NonEntityAssociationMapping(object p0, object p1, object p2)
		{
			return new InvalidOperationException(Strings.NonEntityAssociationMapping(p0, p1, p2));
		}
		internal static Exception CannotPerformCUDOnReadOnlyTable(object p0)
		{
			return new InvalidOperationException(Strings.CannotPerformCUDOnReadOnlyTable(p0));
		}
		internal static Exception CycleDetected()
		{
			return new InvalidOperationException(Strings.CycleDetected);
		}
		internal static Exception CantAddAlreadyExistingItem()
		{
			return new InvalidOperationException(Strings.CantAddAlreadyExistingItem);
		}
		internal static Exception InsertAutoSyncFailure()
		{
			return new InvalidOperationException(Strings.InsertAutoSyncFailure);
		}
		internal static Exception EntitySetDataBindingWithAbstractBaseClass(object p0)
		{
			return new InvalidOperationException(Strings.EntitySetDataBindingWithAbstractBaseClass(p0));
		}
		internal static Exception EntitySetDataBindingWithNonPublicDefaultConstructor(object p0)
		{
			return new InvalidOperationException(Strings.EntitySetDataBindingWithNonPublicDefaultConstructor(p0));
		}
		internal static Exception InvalidLoadOptionsLoadMemberSpecification()
		{
			return new InvalidOperationException(Strings.InvalidLoadOptionsLoadMemberSpecification);
		}
		internal static Exception EntityIsTheWrongType()
		{
			return new InvalidOperationException(Strings.EntityIsTheWrongType);
		}
		internal static Exception OriginalEntityIsWrongType()
		{
			return new InvalidOperationException(Strings.OriginalEntityIsWrongType);
		}
		internal static Exception CannotAttachAlreadyExistingEntity()
		{
			return new InvalidOperationException(Strings.CannotAttachAlreadyExistingEntity);
		}
		internal static Exception CannotAttachAsModifiedWithoutOriginalState()
		{
			return new InvalidOperationException(Strings.CannotAttachAsModifiedWithoutOriginalState);
		}
		internal static Exception CannotPerformOperationDuringSubmitChanges()
		{
			return new InvalidOperationException(Strings.CannotPerformOperationDuringSubmitChanges);
		}
		internal static Exception CannotPerformOperationOutsideSubmitChanges()
		{
			return new InvalidOperationException(Strings.CannotPerformOperationOutsideSubmitChanges);
		}
		internal static Exception CannotPerformOperationForUntrackedObject()
		{
			return new InvalidOperationException(Strings.CannotPerformOperationForUntrackedObject);
		}
		internal static Exception CannotAttachAddNonNewEntities()
		{
			return new NotSupportedException(Strings.CannotAttachAddNonNewEntities);
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
		internal static Exception QueryWasCompiledForDifferentMappingSource()
		{
			throw new InvalidOperationException(Strings.QueryWasCompiledForDifferentMappingSource);
		}

		internal static Exception RefreshOfNewObject()
		{
			throw new InvalidOperationException(Strings.RefreshOfNewObject);
		}

		internal static Exception InvalidFieldInfo(object p0, object p1, object p2)
		{
			return new ArgumentException(Strings.InvalidFieldInfo(p0, p1, p2));
		}
		internal static Exception CouldNotCreateAccessorToProperty(object p0, object p1, object p2)
		{
			return new ArgumentException(Strings.CouldNotCreateAccessorToProperty(p0, p1, p2));
		}
		internal static Exception UnableToAssignValueToReadonlyProperty(object p0)
		{
			return new InvalidOperationException(Strings.UnableToAssignValueToReadonlyProperty(p0));
		}
		internal static Exception LinkAlreadyLoaded()
		{
			return new InvalidOperationException(Strings.LinkAlreadyLoaded);
		}
		internal static Exception EntityRefAlreadyLoaded()
		{
			return new InvalidOperationException(Strings.EntityRefAlreadyLoaded);
		}
		internal static Exception NoDiscriminatorFound(object p0)
		{
			return new InvalidOperationException(Strings.NoDiscriminatorFound(p0));
		}
		internal static Exception InheritanceTypeDoesNotDeriveFromRoot(object p0, object p1)
		{
			return new InvalidOperationException(Strings.InheritanceTypeDoesNotDeriveFromRoot(p0, p1));
		}
		internal static Exception AbstractClassAssignInheritanceDiscriminator(object p0)
		{
			return new InvalidOperationException(Strings.AbstractClassAssignInheritanceDiscriminator(p0));
		}
		internal static Exception CannotGetInheritanceDefaultFromNonInheritanceClass()
		{
			return new InvalidOperationException(Strings.CannotGetInheritanceDefaultFromNonInheritanceClass);
		}
		internal static Exception InheritanceCodeMayNotBeNull()
		{
			return new InvalidOperationException(Strings.InheritanceCodeMayNotBeNull);
		}
		internal static Exception InheritanceTypeHasMultipleDiscriminators(object p0)
		{
			return new InvalidOperationException(Strings.InheritanceTypeHasMultipleDiscriminators(p0));
		}
		internal static Exception InheritanceCodeUsedForMultipleTypes(object p0)
		{
			return new InvalidOperationException(Strings.InheritanceCodeUsedForMultipleTypes(p0));
		}
		internal static Exception InheritanceTypeHasMultipleDefaults(object p0)
		{
			return new InvalidOperationException(Strings.InheritanceTypeHasMultipleDefaults(p0));
		}
		internal static Exception InheritanceHierarchyDoesNotDefineDefault(object p0)
		{
			return new InvalidOperationException(Strings.InheritanceHierarchyDoesNotDefineDefault(p0));
		}
		internal static Exception InheritanceSubTypeIsAlsoRoot(object p0)
		{
			return new InvalidOperationException(Strings.InheritanceSubTypeIsAlsoRoot(p0));
		}
		internal static Exception NonInheritanceClassHasDiscriminator(object p0)
		{
			return new InvalidOperationException(Strings.NonInheritanceClassHasDiscriminator(p0));
		}
		internal static Exception MemberMappedMoreThanOnce(object p0)
		{
			return new InvalidOperationException(Strings.MemberMappedMoreThanOnce(p0));
		}
		internal static Exception BadStorageProperty(object p0, object p1, object p2)
		{
			return new InvalidOperationException(Strings.BadStorageProperty(p0, p1, p2));
		}
		internal static Exception IncorrectAutoSyncSpecification(object p0)
		{
			return new InvalidOperationException(Strings.IncorrectAutoSyncSpecification(p0));
		}
		internal static Exception UnhandledDeferredStorageType(object p0)
		{
			return new InvalidOperationException(Strings.UnhandledDeferredStorageType(p0));
		}
		internal static Exception BadKeyMember(object p0, object p1, object p2)
		{
			return new InvalidOperationException(Strings.BadKeyMember(p0, p1, p2));
		}
		internal static Exception ProviderTypeNotFound(object p0)
		{
			return new InvalidOperationException(Strings.ProviderTypeNotFound(p0));
		}
		internal static Exception MethodCannotBeFound(object p0)
		{
			return new InvalidOperationException(Strings.MethodCannotBeFound(p0));
		}
		internal static Exception UnableToResolveRootForType(object p0)
		{
			return new InvalidOperationException(Strings.UnableToResolveRootForType(p0));
		}
		internal static Exception MappingForTableUndefined(object p0)
		{
			return new InvalidOperationException(Strings.MappingForTableUndefined(p0));
		}
		internal static Exception CouldNotFindTypeFromMapping(object p0)
		{
			return new InvalidOperationException(Strings.CouldNotFindTypeFromMapping(p0));
		}
		internal static Exception TwoMembersMarkedAsPrimaryKeyAndDBGenerated(object p0, object p1)
		{
			return new InvalidOperationException(Strings.TwoMembersMarkedAsPrimaryKeyAndDBGenerated(p0, p1));
		}
		internal static Exception TwoMembersMarkedAsRowVersion(object p0, object p1)
		{
			return new InvalidOperationException(Strings.TwoMembersMarkedAsRowVersion(p0, p1));
		}
		internal static Exception TwoMembersMarkedAsInheritanceDiscriminator(object p0, object p1)
		{
			return new InvalidOperationException(Strings.TwoMembersMarkedAsInheritanceDiscriminator(p0, p1));
		}
		internal static Exception CouldNotFindRuntimeTypeForMapping(object p0)
		{
			return new InvalidOperationException(Strings.CouldNotFindRuntimeTypeForMapping(p0));
		}
		internal static Exception UnexpectedNull(object p0)
		{
			return new InvalidOperationException(Strings.UnexpectedNull(p0));
		}
		internal static Exception CouldNotFindElementTypeInModel(object p0)
		{
			return new InvalidOperationException(Strings.CouldNotFindElementTypeInModel(p0));
		}
		internal static Exception BadFunctionTypeInMethodMapping(object p0)
		{
			return new InvalidOperationException(Strings.BadFunctionTypeInMethodMapping(p0));
		}
		internal static Exception IncorrectNumberOfParametersMappedForMethod(object p0)
		{
			return new InvalidOperationException(Strings.IncorrectNumberOfParametersMappedForMethod(p0));
		}
		internal static Exception CouldNotFindRequiredAttribute(object p0, object p1)
		{
			return new XmlSchemaException(Strings.CouldNotFindRequiredAttribute(p0, p1));
		}
		internal static Exception InvalidDeleteOnNullSpecification(object p0)
		{
			return new InvalidOperationException(Strings.InvalidDeleteOnNullSpecification(p0));
		}
		internal static Exception MappedMemberHadNoCorrespondingMemberInType(object p0, object p1)
		{
			return new NotSupportedException(Strings.MappedMemberHadNoCorrespondingMemberInType(p0, p1));
		}
		internal static Exception UnrecognizedAttribute(object p0)
		{
			return new XmlSchemaException(Strings.UnrecognizedAttribute(p0));
		}
		internal static Exception UnrecognizedElement(object p0)
		{
			return new XmlSchemaException(Strings.UnrecognizedElement(p0));
		}
		internal static Exception TooManyResultTypesDeclaredForFunction(object p0)
		{
			return new InvalidOperationException(Strings.TooManyResultTypesDeclaredForFunction(p0));
		}
		internal static Exception NoResultTypesDeclaredForFunction(object p0)
		{
			return new InvalidOperationException(Strings.NoResultTypesDeclaredForFunction(p0));
		}
		internal static Exception UnexpectedElement(object p0, object p1)
		{
			return new XmlSchemaException(Strings.UnexpectedElement(p0, p1));
		}
		internal static Exception ExpectedEmptyElement(object p0, object p1, object p2)
		{
			return new XmlSchemaException(Strings.ExpectedEmptyElement(p0, p1, p2));
		}
		internal static Exception DatabaseNodeNotFound(object p0)
		{
			return new XmlSchemaException(Strings.DatabaseNodeNotFound(p0));
		}
		internal static Exception DiscriminatorClrTypeNotSupported(object p0, object p1, object p2)
		{
			return new NotSupportedException(Strings.DiscriminatorClrTypeNotSupported(p0, p1, p2));
		}
		internal static Exception IdentityClrTypeNotSupported(object p0, object p1, object p2)
		{
			return new NotSupportedException(Strings.IdentityClrTypeNotSupported(p0, p1, p2));
		}
		internal static Exception PrimaryKeyInSubTypeNotSupported(object p0, object p1)
		{
			return new NotSupportedException(Strings.PrimaryKeyInSubTypeNotSupported(p0, p1));
		}
		internal static Exception MismatchedThisKeyOtherKey(object p0, object p1)
		{
			return new InvalidOperationException(Strings.MismatchedThisKeyOtherKey(p0, p1));
		}
		internal static Exception InvalidUseOfGenericMethodAsMappedFunction(object p0)
		{
			return new NotSupportedException(Strings.InvalidUseOfGenericMethodAsMappedFunction(p0));
		}
		internal static Exception MappingOfInterfacesMemberIsNotSupported(object p0, object p1)
		{
			return new NotSupportedException(Strings.MappingOfInterfacesMemberIsNotSupported(p0, p1));
		}
		internal static Exception UnmappedClassMember(object p0, object p1)
		{
			return new InvalidOperationException(Strings.UnmappedClassMember(p0, p1));
		}
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
		internal static Exception ExpressionNotSupportedForSqlServerVersion(Collection<string> reasons)
		{
			StringBuilder stringBuilder = new StringBuilder(Strings.CannotTranslateExpressionToSql);
			foreach(string current in reasons)
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
