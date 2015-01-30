using System;
namespace System.Data.Linq
{
	internal static class Strings
	{
		internal static string OwningTeam
		{
			get
			{
				return SR.GetString("OwningTeam");
			}
		}
		internal static string CannotAddChangeConflicts
		{
			get
			{
				return SR.GetString("CannotAddChangeConflicts");
			}
		}
		internal static string CannotRemoveChangeConflicts
		{
			get
			{
				return SR.GetString("CannotRemoveChangeConflicts");
			}
		}
		internal static string UnableToDetermineDataContext
		{
			get
			{
				return SR.GetString("UnableToDetermineDataContext");
			}
		}
		internal static string CannotRemoveUnattachedEntity
		{
			get
			{
				return SR.GetString("CannotRemoveUnattachedEntity");
			}
		}
		internal static string CouldNotAttach
		{
			get
			{
				return SR.GetString("CouldNotAttach");
			}
		}
		internal static string EntitySetAlreadyLoaded
		{
			get
			{
				return SR.GetString("EntitySetAlreadyLoaded");
			}
		}
		internal static string EntitySetModifiedDuringEnumeration
		{
			get
			{
				return SR.GetString("EntitySetModifiedDuringEnumeration");
			}
		}
		internal static string ExpectedUpdateDeleteOrChange
		{
			get
			{
				return SR.GetString("ExpectedUpdateDeleteOrChange");
			}
		}
		internal static string ModifyDuringAddOrRemove
		{
			get
			{
				return SR.GetString("ModifyDuringAddOrRemove");
			}
		}
		internal static string ProviderTypeNull
		{
			get
			{
				return SR.GetString("ProviderTypeNull");
			}
		}
		internal static string UnrecognizedRefreshObject
		{
			get
			{
				return SR.GetString("UnrecognizedRefreshObject");
			}
		}
		internal static string ObjectTrackingRequired
		{
			get
			{
				return SR.GetString("ObjectTrackingRequired");
			}
		}
		internal static string OptionsCannotBeModifiedAfterQuery
		{
			get
			{
				return SR.GetString("OptionsCannotBeModifiedAfterQuery");
			}
		}
		internal static string DeferredLoadingRequiresObjectTracking
		{
			get
			{
				return SR.GetString("DeferredLoadingRequiresObjectTracking");
			}
		}
		internal static string SubqueryNotAllowedAfterFreeze
		{
			get
			{
				return SR.GetString("SubqueryNotAllowedAfterFreeze");
			}
		}
		internal static string IncludeNotAllowedAfterFreeze
		{
			get
			{
				return SR.GetString("IncludeNotAllowedAfterFreeze");
			}
		}
		internal static string LoadOptionsChangeNotAllowedAfterQuery
		{
			get
			{
				return SR.GetString("LoadOptionsChangeNotAllowedAfterQuery");
			}
		}
		internal static string IncludeCycleNotAllowed
		{
			get
			{
				return SR.GetString("IncludeCycleNotAllowed");
			}
		}
		internal static string SubqueryMustBeSequence
		{
			get
			{
				return SR.GetString("SubqueryMustBeSequence");
			}
		}
		internal static string RefreshOfDeletedObject
		{
			get
			{
				return SR.GetString("RefreshOfDeletedObject");
			}
		}
		internal static string DataContextCannotBeUsedAfterDispose
		{
			get
			{
				return SR.GetString("DataContextCannotBeUsedAfterDispose");
			}
		}
		internal static string InsertCallbackComment
		{
			get
			{
				return SR.GetString("InsertCallbackComment");
			}
		}
		internal static string UpdateCallbackComment
		{
			get
			{
				return SR.GetString("UpdateCallbackComment");
			}
		}
		internal static string DeleteCallbackComment
		{
			get
			{
				return SR.GetString("DeleteCallbackComment");
			}
		}
		internal static string RowNotFoundOrChanged
		{
			get
			{
				return SR.GetString("RowNotFoundOrChanged");
			}
		}
		internal static string CycleDetected
		{
			get
			{
				return SR.GetString("CycleDetected");
			}
		}
		internal static string CantAddAlreadyExistingItem
		{
			get
			{
				return SR.GetString("CantAddAlreadyExistingItem");
			}
		}
		internal static string CantAddAlreadyExistingKey
		{
			get
			{
				return SR.GetString("CantAddAlreadyExistingKey");
			}
		}
		internal static string DatabaseGeneratedAlreadyExistingKey
		{
			get
			{
				return SR.GetString("DatabaseGeneratedAlreadyExistingKey");
			}
		}
		internal static string InsertAutoSyncFailure
		{
			get
			{
				return SR.GetString("InsertAutoSyncFailure");
			}
		}
		internal static string InvalidLoadOptionsLoadMemberSpecification
		{
			get
			{
				return SR.GetString("InvalidLoadOptionsLoadMemberSpecification");
			}
		}
		internal static string EntityIsTheWrongType
		{
			get
			{
				return SR.GetString("EntityIsTheWrongType");
			}
		}
		internal static string OriginalEntityIsWrongType
		{
			get
			{
				return SR.GetString("OriginalEntityIsWrongType");
			}
		}
		internal static string CannotAttachAlreadyExistingEntity
		{
			get
			{
				return SR.GetString("CannotAttachAlreadyExistingEntity");
			}
		}
		internal static string CannotAttachAsModifiedWithoutOriginalState
		{
			get
			{
				return SR.GetString("CannotAttachAsModifiedWithoutOriginalState");
			}
		}
		internal static string CannotPerformOperationDuringSubmitChanges
		{
			get
			{
				return SR.GetString("CannotPerformOperationDuringSubmitChanges");
			}
		}
		internal static string CannotPerformOperationOutsideSubmitChanges
		{
			get
			{
				return SR.GetString("CannotPerformOperationOutsideSubmitChanges");
			}
		}
		internal static string CannotPerformOperationForUntrackedObject
		{
			get
			{
				return SR.GetString("CannotPerformOperationForUntrackedObject");
			}
		}
		internal static string CannotAttachAddNonNewEntities
		{
			get
			{
				return SR.GetString("CannotAttachAddNonNewEntities");
			}
		}
		internal static string InconsistentAssociationAndKeyChange(object p0, object p1)
		{
			return SR.GetString("InconsistentAssociationAndKeyChange", new object[]
			{
				p0,
				p1
			});
		}
		internal static string ArgumentTypeHasNoIdentityKey(object p0)
		{
			return SR.GetString("ArgumentTypeHasNoIdentityKey", new object[]
			{
				p0
			});
		}
		internal static string CouldNotConvert(object p0, object p1)
		{
			return SR.GetString("CouldNotConvert", new object[]
			{
				p0,
				p1
			});
		}
		internal static string ColumnMappedMoreThanOnce(object p0)
		{
			return SR.GetString("ColumnMappedMoreThanOnce", new object[]
			{
				p0
			});
		}
		internal static string CouldNotGetTableForSubtype(object p0, object p1)
		{
			return SR.GetString("CouldNotGetTableForSubtype", new object[]
			{
				p0,
				p1
			});
		}
		internal static string CouldNotRemoveRelationshipBecauseOneSideCannotBeNull(object p0, object p1, object p2)
		{
			return SR.GetString("CouldNotRemoveRelationshipBecauseOneSideCannotBeNull", new object[]
			{
				p0,
				p1,
				p2
			});
		}
		internal static string ExpectedQueryableArgument(object p0, object p1)
		{
			return SR.GetString("ExpectedQueryableArgument", new object[]
			{
				p0,
				p1
			});
		}
		internal static string KeyIsWrongSize(object p0, object p1)
		{
			return SR.GetString("KeyIsWrongSize", new object[]
			{
				p0,
				p1
			});
		}
		internal static string KeyValueIsWrongType(object p0, object p1)
		{
			return SR.GetString("KeyValueIsWrongType", new object[]
			{
				p0,
				p1
			});
		}
		internal static string IdentityChangeNotAllowed(object p0, object p1)
		{
			return SR.GetString("IdentityChangeNotAllowed", new object[]
			{
				p0,
				p1
			});
		}
		internal static string DbGeneratedChangeNotAllowed(object p0, object p1)
		{
			return SR.GetString("DbGeneratedChangeNotAllowed", new object[]
			{
				p0,
				p1
			});
		}
		internal static string ProviderDoesNotImplementRequiredInterface(object p0, object p1)
		{
			return SR.GetString("ProviderDoesNotImplementRequiredInterface", new object[]
			{
				p0,
				p1
			});
		}
		internal static string TypeCouldNotBeAdded(object p0)
		{
			return SR.GetString("TypeCouldNotBeAdded", new object[]
			{
				p0
			});
		}
		internal static string TypeCouldNotBeRemoved(object p0)
		{
			return SR.GetString("TypeCouldNotBeRemoved", new object[]
			{
				p0
			});
		}
		internal static string TypeCouldNotBeTracked(object p0)
		{
			return SR.GetString("TypeCouldNotBeTracked", new object[]
			{
				p0
			});
		}
		internal static string TypeIsNotEntity(object p0)
		{
			return SR.GetString("TypeIsNotEntity", new object[]
			{
				p0
			});
		}
		internal static string UnhandledExpressionType(object p0)
		{
			return SR.GetString("UnhandledExpressionType", new object[]
			{
				p0
			});
		}
		internal static string UnhandledBindingType(object p0)
		{
			return SR.GetString("UnhandledBindingType", new object[]
			{
				p0
			});
		}
		internal static string SubqueryDoesNotSupportOperator(object p0)
		{
			return SR.GetString("SubqueryDoesNotSupportOperator", new object[]
			{
				p0
			});
		}
		internal static string SubqueryNotSupportedOn(object p0)
		{
			return SR.GetString("SubqueryNotSupportedOn", new object[]
			{
				p0
			});
		}
		internal static string SubqueryNotSupportedOnType(object p0, object p1)
		{
			return SR.GetString("SubqueryNotSupportedOnType", new object[]
			{
				p0,
				p1
			});
		}
		internal static string CannotChangeInheritanceType(object p0, object p1, object p2, object p3)
		{
			return SR.GetString("CannotChangeInheritanceType", new object[]
			{
				p0,
				p1,
				p2,
				p3
			});
		}
		internal static string TypeIsNotMarkedAsTable(object p0)
		{
			return SR.GetString("TypeIsNotMarkedAsTable", new object[]
			{
				p0
			});
		}
		internal static string NoMethodInTypeMatchingArguments(object p0)
		{
			return SR.GetString("NoMethodInTypeMatchingArguments", new object[]
			{
				p0
			});
		}
		internal static string NonEntityAssociationMapping(object p0, object p1, object p2)
		{
			return SR.GetString("NonEntityAssociationMapping", new object[]
			{
				p0,
				p1,
				p2
			});
		}
		internal static string CannotPerformCUDOnReadOnlyTable(object p0)
		{
			return SR.GetString("CannotPerformCUDOnReadOnlyTable", new object[]
			{
				p0
			});
		}
		internal static string UpdatesFailedMessage(object p0, object p1)
		{
			return SR.GetString("UpdatesFailedMessage", new object[]
			{
				p0,
				p1
			});
		}
		internal static string EntitySetDataBindingWithAbstractBaseClass(object p0)
		{
			return SR.GetString("EntitySetDataBindingWithAbstractBaseClass", new object[]
			{
				p0
			});
		}
		internal static string EntitySetDataBindingWithNonPublicDefaultConstructor(object p0)
		{
			return SR.GetString("EntitySetDataBindingWithNonPublicDefaultConstructor", new object[]
			{
				p0
			});
		}

		public static string QueryWasCompiledForDifferentMappingSource 
		{
			get { return SR.GetString("QueryWasCompiledForDifferentMappingSource"); }
		}

		public static string RefreshOfNewObject
		{
			get { return SR.GetString("RefreshOfNewObject"); }
		}

		internal static string LinkAlreadyLoaded
		{
			get
			{
				return SR.GetString("LinkAlreadyLoaded");
			}
		}
		internal static string EntityRefAlreadyLoaded
		{
			get
			{
				return SR.GetString("EntityRefAlreadyLoaded");
			}
		}
		internal static string CannotGetInheritanceDefaultFromNonInheritanceClass
		{
			get
			{
				return SR.GetString("CannotGetInheritanceDefaultFromNonInheritanceClass");
			}
		}
		internal static string InheritanceCodeMayNotBeNull
		{
			get
			{
				return SR.GetString("InheritanceCodeMayNotBeNull");
			}
		}
		internal static string InvalidFieldInfo(object p0, object p1, object p2)
		{
			return SR.GetString("InvalidFieldInfo", new object[]
			{
				p0,
				p1,
				p2
			});
		}
		internal static string CouldNotCreateAccessorToProperty(object p0, object p1, object p2)
		{
			return SR.GetString("CouldNotCreateAccessorToProperty", new object[]
			{
				p0,
				p1,
				p2
			});
		}
		internal static string UnableToAssignValueToReadonlyProperty(object p0)
		{
			return SR.GetString("UnableToAssignValueToReadonlyProperty", new object[]
			{
				p0
			});
		}
		internal static string NoDiscriminatorFound(object p0)
		{
			return SR.GetString("NoDiscriminatorFound", new object[]
			{
				p0
			});
		}
		internal static string InheritanceTypeDoesNotDeriveFromRoot(object p0, object p1)
		{
			return SR.GetString("InheritanceTypeDoesNotDeriveFromRoot", new object[]
			{
				p0,
				p1
			});
		}
		internal static string AbstractClassAssignInheritanceDiscriminator(object p0)
		{
			return SR.GetString("AbstractClassAssignInheritanceDiscriminator", new object[]
			{
				p0
			});
		}
		internal static string InheritanceTypeHasMultipleDiscriminators(object p0)
		{
			return SR.GetString("InheritanceTypeHasMultipleDiscriminators", new object[]
			{
				p0
			});
		}
		internal static string InheritanceCodeUsedForMultipleTypes(object p0)
		{
			return SR.GetString("InheritanceCodeUsedForMultipleTypes", new object[]
			{
				p0
			});
		}
		internal static string InheritanceTypeHasMultipleDefaults(object p0)
		{
			return SR.GetString("InheritanceTypeHasMultipleDefaults", new object[]
			{
				p0
			});
		}
		internal static string InheritanceHierarchyDoesNotDefineDefault(object p0)
		{
			return SR.GetString("InheritanceHierarchyDoesNotDefineDefault", new object[]
			{
				p0
			});
		}
		internal static string InheritanceSubTypeIsAlsoRoot(object p0)
		{
			return SR.GetString("InheritanceSubTypeIsAlsoRoot", new object[]
			{
				p0
			});
		}
		internal static string NonInheritanceClassHasDiscriminator(object p0)
		{
			return SR.GetString("NonInheritanceClassHasDiscriminator", new object[]
			{
				p0
			});
		}
		internal static string MemberMappedMoreThanOnce(object p0)
		{
			return SR.GetString("MemberMappedMoreThanOnce", new object[]
			{
				p0
			});
		}
		internal static string BadStorageProperty(object p0, object p1, object p2)
		{
			return SR.GetString("BadStorageProperty", new object[]
			{
				p0,
				p1,
				p2
			});
		}
		internal static string IncorrectAutoSyncSpecification(object p0)
		{
			return SR.GetString("IncorrectAutoSyncSpecification", new object[]
			{
				p0
			});
		}
		internal static string UnhandledDeferredStorageType(object p0)
		{
			return SR.GetString("UnhandledDeferredStorageType", new object[]
			{
				p0
			});
		}
		internal static string BadKeyMember(object p0, object p1, object p2)
		{
			return SR.GetString("BadKeyMember", new object[]
			{
				p0,
				p1,
				p2
			});
		}
		internal static string ProviderTypeNotFound(object p0)
		{
			return SR.GetString("ProviderTypeNotFound", new object[]
			{
				p0
			});
		}
		internal static string MethodCannotBeFound(object p0)
		{
			return SR.GetString("MethodCannotBeFound", new object[]
			{
				p0
			});
		}
		internal static string UnableToResolveRootForType(object p0)
		{
			return SR.GetString("UnableToResolveRootForType", new object[]
			{
				p0
			});
		}
		internal static string MappingForTableUndefined(object p0)
		{
			return SR.GetString("MappingForTableUndefined", new object[]
			{
				p0
			});
		}
		internal static string CouldNotFindTypeFromMapping(object p0)
		{
			return SR.GetString("CouldNotFindTypeFromMapping", new object[]
			{
				p0
			});
		}
		internal static string TwoMembersMarkedAsPrimaryKeyAndDBGenerated(object p0, object p1)
		{
			return SR.GetString("TwoMembersMarkedAsPrimaryKeyAndDBGenerated", new object[]
			{
				p0,
				p1
			});
		}
		internal static string TwoMembersMarkedAsRowVersion(object p0, object p1)
		{
			return SR.GetString("TwoMembersMarkedAsRowVersion", new object[]
			{
				p0,
				p1
			});
		}
		internal static string TwoMembersMarkedAsInheritanceDiscriminator(object p0, object p1)
		{
			return SR.GetString("TwoMembersMarkedAsInheritanceDiscriminator", new object[]
			{
				p0,
				p1
			});
		}
		internal static string CouldNotFindRuntimeTypeForMapping(object p0)
		{
			return SR.GetString("CouldNotFindRuntimeTypeForMapping", new object[]
			{
				p0
			});
		}
		internal static string UnexpectedNull(object p0)
		{
			return SR.GetString("UnexpectedNull", new object[]
			{
				p0
			});
		}
		internal static string CouldNotFindElementTypeInModel(object p0)
		{
			return SR.GetString("CouldNotFindElementTypeInModel", new object[]
			{
				p0
			});
		}
		internal static string BadFunctionTypeInMethodMapping(object p0)
		{
			return SR.GetString("BadFunctionTypeInMethodMapping", new object[]
			{
				p0
			});
		}
		internal static string IncorrectNumberOfParametersMappedForMethod(object p0)
		{
			return SR.GetString("IncorrectNumberOfParametersMappedForMethod", new object[]
			{
				p0
			});
		}
		internal static string CouldNotFindRequiredAttribute(object p0, object p1)
		{
			return SR.GetString("CouldNotFindRequiredAttribute", new object[]
			{
				p0,
				p1
			});
		}
		internal static string InvalidDeleteOnNullSpecification(object p0)
		{
			return SR.GetString("InvalidDeleteOnNullSpecification", new object[]
			{
				p0
			});
		}
		internal static string MappedMemberHadNoCorrespondingMemberInType(object p0, object p1)
		{
			return SR.GetString("MappedMemberHadNoCorrespondingMemberInType", new object[]
			{
				p0,
				p1
			});
		}
		internal static string UnrecognizedAttribute(object p0)
		{
			return SR.GetString("UnrecognizedAttribute", new object[]
			{
				p0
			});
		}
		internal static string UnrecognizedElement(object p0)
		{
			return SR.GetString("UnrecognizedElement", new object[]
			{
				p0
			});
		}
		internal static string TooManyResultTypesDeclaredForFunction(object p0)
		{
			return SR.GetString("TooManyResultTypesDeclaredForFunction", new object[]
			{
				p0
			});
		}
		internal static string NoResultTypesDeclaredForFunction(object p0)
		{
			return SR.GetString("NoResultTypesDeclaredForFunction", new object[]
			{
				p0
			});
		}
		internal static string UnexpectedElement(object p0, object p1)
		{
			return SR.GetString("UnexpectedElement", new object[]
			{
				p0,
				p1
			});
		}
		internal static string ExpectedEmptyElement(object p0, object p1, object p2)
		{
			return SR.GetString("ExpectedEmptyElement", new object[]
			{
				p0,
				p1,
				p2
			});
		}
		internal static string DatabaseNodeNotFound(object p0)
		{
			return SR.GetString("DatabaseNodeNotFound", new object[]
			{
				p0
			});
		}
		internal static string DiscriminatorClrTypeNotSupported(object p0, object p1, object p2)
		{
			return SR.GetString("DiscriminatorClrTypeNotSupported", new object[]
			{
				p0,
				p1,
				p2
			});
		}
		internal static string IdentityClrTypeNotSupported(object p0, object p1, object p2)
		{
			return SR.GetString("IdentityClrTypeNotSupported", new object[]
			{
				p0,
				p1,
				p2
			});
		}
		internal static string PrimaryKeyInSubTypeNotSupported(object p0, object p1)
		{
			return SR.GetString("PrimaryKeyInSubTypeNotSupported", new object[]
			{
				p0,
				p1
			});
		}
		internal static string MismatchedThisKeyOtherKey(object p0, object p1)
		{
			return SR.GetString("MismatchedThisKeyOtherKey", new object[]
			{
				p0,
				p1
			});
		}
		internal static string InvalidUseOfGenericMethodAsMappedFunction(object p0)
		{
			return SR.GetString("InvalidUseOfGenericMethodAsMappedFunction", new object[]
			{
				p0
			});
		}
		internal static string MappingOfInterfacesMemberIsNotSupported(object p0, object p1)
		{
			return SR.GetString("MappingOfInterfacesMemberIsNotSupported", new object[]
			{
				p0,
				p1
			});
		}
		internal static string UnmappedClassMember(object p0, object p1)
		{
			return SR.GetString("UnmappedClassMember", new object[]
			{
				p0,
				p1
			});
		}

		internal static string VbLikeDoesNotSupportMultipleCharacterRanges
		{
			get
			{
				return SR.GetString("VbLikeDoesNotSupportMultipleCharacterRanges");
			}
		}
		internal static string VbLikeUnclosedBracket
		{
			get
			{
				return SR.GetString("VbLikeUnclosedBracket");
			}
		}
		internal static string ProviderCannotBeUsedAfterDispose
		{
			get
			{
				return SR.GetString("ProviderCannotBeUsedAfterDispose");
			}
		}
		internal static string ContextNotInitialized
		{
			get
			{
				return SR.GetString("ContextNotInitialized");
			}
		}
		internal static string CouldNotDetermineCatalogName
		{
			get
			{
				return SR.GetString("CouldNotDetermineCatalogName");
			}
		}
		internal static string DistributedTransactionsAreNotAllowed
		{
			get
			{
				return SR.GetString("DistributedTransactionsAreNotAllowed");
			}
		}
		internal static string CannotEnumerateResultsMoreThanOnce
		{
			get
			{
				return SR.GetString("CannotEnumerateResultsMoreThanOnce");
			}
		}
		internal static string ToStringOnlySupportedForPrimitiveTypes
		{
			get
			{
				return SR.GetString("ToStringOnlySupportedForPrimitiveTypes");
			}
		}
		internal static string TransactionDoesNotMatchConnection
		{
			get
			{
				return SR.GetString("TransactionDoesNotMatchConnection");
			}
		}
		internal static string UnsupportedDateTimeConstructorForm
		{
			get
			{
				return SR.GetString("UnsupportedDateTimeConstructorForm");
			}
		}
		internal static string UnsupportedDateTimeOffsetConstructorForm
		{
			get
			{
				return SR.GetString("UnsupportedDateTimeOffsetConstructorForm");
			}
		}
		internal static string UnsupportedStringConstructorForm
		{
			get
			{
				return SR.GetString("UnsupportedStringConstructorForm");
			}
		}
		internal static string UnsupportedTimeSpanConstructorForm
		{
			get
			{
				return SR.GetString("UnsupportedTimeSpanConstructorForm");
			}
		}
		internal static string MathRoundNotSupported
		{
			get
			{
				return SR.GetString("MathRoundNotSupported");
			}
		}
		internal static string NonConstantExpressionsNotSupportedForRounding
		{
			get
			{
				return SR.GetString("NonConstantExpressionsNotSupportedForRounding");
			}
		}
		internal static string CompiledQueryAgainstMultipleShapesNotSupported
		{
			get
			{
				return SR.GetString("CompiledQueryAgainstMultipleShapesNotSupported");
			}
		}
		internal static string IndexOfWithStringComparisonArgNotSupported
		{
			get
			{
				return SR.GetString("IndexOfWithStringComparisonArgNotSupported");
			}
		}
		internal static string LastIndexOfWithStringComparisonArgNotSupported
		{
			get
			{
				return SR.GetString("LastIndexOfWithStringComparisonArgNotSupported");
			}
		}
		internal static string ConvertToCharFromBoolNotSupported
		{
			get
			{
				return SR.GetString("ConvertToCharFromBoolNotSupported");
			}
		}
		internal static string ConvertToDateTimeOnlyForDateTimeOrString
		{
			get
			{
				return SR.GetString("ConvertToDateTimeOnlyForDateTimeOrString");
			}
		}
		internal static string CannotTranslateExpressionToSql
		{
			get
			{
				return SR.GetString("CannotTranslateExpressionToSql");
			}
		}
		internal static string SkipIsValidOnlyOverOrderedQueries
		{
			get
			{
				return SR.GetString("SkipIsValidOnlyOverOrderedQueries");
			}
		}
		internal static string SkipRequiresSingleTableQueryWithPKs
		{
			get
			{
				return SR.GetString("SkipRequiresSingleTableQueryWithPKs");
			}
		}
		internal static string ExpressionNotDeferredQuerySource
		{
			get
			{
				return SR.GetString("ExpressionNotDeferredQuerySource");
			}
		}
		internal static string DeferredMemberWrongType
		{
			get
			{
				return SR.GetString("DeferredMemberWrongType");
			}
		}
		internal static string BadProjectionInSelect
		{
			get
			{
				return SR.GetString("BadProjectionInSelect");
			}
		}
		internal static string WrongDataContext
		{
			get
			{
				return SR.GetString("WrongDataContext");
			}
		}
		internal static string CannotCompareItemsAssociatedWithDifferentTable
		{
			get
			{
				return SR.GetString("CannotCompareItemsAssociatedWithDifferentTable");
			}
		}
		internal static string ColumnCannotReferToItself
		{
			get
			{
				return SR.GetString("ColumnCannotReferToItself");
			}
		}
		internal static string ColumnClrTypeDoesNotAgreeWithExpressionsClrType
		{
			get
			{
				return SR.GetString("ColumnClrTypeDoesNotAgreeWithExpressionsClrType");
			}
		}
		internal static string ConstructedArraysNotSupported
		{
			get
			{
				return SR.GetString("ConstructedArraysNotSupported");
			}
		}
		internal static string ParametersCannotBeSequences
		{
			get
			{
				return SR.GetString("ParametersCannotBeSequences");
			}
		}
		internal static string CapturedValuesCannotBeSequences
		{
			get
			{
				return SR.GetString("CapturedValuesCannotBeSequences");
			}
		}
		internal static string CouldNotGetClrType
		{
			get
			{
				return SR.GetString("CouldNotGetClrType");
			}
		}
		internal static string CouldNotGetSqlType
		{
			get
			{
				return SR.GetString("CouldNotGetSqlType");
			}
		}
		internal static string DidNotExpectTypeBinding
		{
			get
			{
				return SR.GetString("DidNotExpectTypeBinding");
			}
		}
		internal static string EmptyCaseNotSupported
		{
			get
			{
				return SR.GetString("EmptyCaseNotSupported");
			}
		}
		internal static string ExpectedNoObjectType
		{
			get
			{
				return SR.GetString("ExpectedNoObjectType");
			}
		}
		internal static string ExpectedBitFoundPredicate
		{
			get
			{
				return SR.GetString("ExpectedBitFoundPredicate");
			}
		}
		internal static string ExpectedPredicateFoundBit
		{
			get
			{
				return SR.GetString("ExpectedPredicateFoundBit");
			}
		}
		internal static string InvalidGroupByExpression
		{
			get
			{
				return SR.GetString("InvalidGroupByExpression");
			}
		}
		internal static string Impossible
		{
			get
			{
				return SR.GetString("Impossible");
			}
		}
		internal static string InfiniteDescent
		{
			get
			{
				return SR.GetString("InfiniteDescent");
			}
		}
		internal static string InvalidReferenceToRemovedAliasDuringDeflation
		{
			get
			{
				return SR.GetString("InvalidReferenceToRemovedAliasDuringDeflation");
			}
		}
		internal static string ReaderUsedAfterDispose
		{
			get
			{
				return SR.GetString("ReaderUsedAfterDispose");
			}
		}
		internal static string TypeBinaryOperatorNotRecognized
		{
			get
			{
				return SR.GetString("TypeBinaryOperatorNotRecognized");
			}
		}
		internal static string UnexpectedFloatingColumn
		{
			get
			{
				return SR.GetString("UnexpectedFloatingColumn");
			}
		}
		internal static string UnexpectedSharedExpression
		{
			get
			{
				return SR.GetString("UnexpectedSharedExpression");
			}
		}
		internal static string UnexpectedSharedExpressionReference
		{
			get
			{
				return SR.GetString("UnexpectedSharedExpressionReference");
			}
		}
		internal static string UnhandledStringTypeComparison
		{
			get
			{
				return SR.GetString("UnhandledStringTypeComparison");
			}
		}
		internal static string UnionIncompatibleConstruction
		{
			get
			{
				return SR.GetString("UnionIncompatibleConstruction");
			}
		}
		internal static string UnionDifferentMembers
		{
			get
			{
				return SR.GetString("UnionDifferentMembers");
			}
		}
		internal static string UnionDifferentMemberOrder
		{
			get
			{
				return SR.GetString("UnionDifferentMemberOrder");
			}
		}
		internal static string UnionOfIncompatibleDynamicTypes
		{
			get
			{
				return SR.GetString("UnionOfIncompatibleDynamicTypes");
			}
		}
		internal static string UnionWithHierarchy
		{
			get
			{
				return SR.GetString("UnionWithHierarchy");
			}
		}
		internal static string IntersectNotSupportedForHierarchicalTypes
		{
			get
			{
				return SR.GetString("IntersectNotSupportedForHierarchicalTypes");
			}
		}
		internal static string ExceptNotSupportedForHierarchicalTypes
		{
			get
			{
				return SR.GetString("ExceptNotSupportedForHierarchicalTypes");
			}
		}
		internal static string GroupingNotSupportedAsOrderCriterion
		{
			get
			{
				return SR.GetString("GroupingNotSupportedAsOrderCriterion");
			}
		}
		internal static string SelectManyDoesNotSupportStrings
		{
			get
			{
				return SR.GetString("SelectManyDoesNotSupportStrings");
			}
		}
		internal static string SkipNotSupportedForSequenceTypes
		{
			get
			{
				return SR.GetString("SkipNotSupportedForSequenceTypes");
			}
		}
		internal static string QueryOnLocalCollectionNotSupported
		{
			get
			{
				return SR.GetString("QueryOnLocalCollectionNotSupported");
			}
		}
		internal static string TypeColumnWithUnhandledSource
		{
			get
			{
				return SR.GetString("TypeColumnWithUnhandledSource");
			}
		}
		internal static string GeneralCollectionMaterializationNotSupported
		{
			get
			{
				return SR.GetString("GeneralCollectionMaterializationNotSupported");
			}
		}
		internal static string SprocsCannotBeComposed
		{
			get
			{
				return SR.GetString("SprocsCannotBeComposed");
			}
		}
		internal static string InsertItemMustBeConstant
		{
			get
			{
				return SR.GetString("InsertItemMustBeConstant");
			}
		}
		internal static string UpdateItemMustBeConstant
		{
			get
			{
				return SR.GetString("UpdateItemMustBeConstant");
			}
		}
		internal static string DatabaseDeleteThroughContext
		{
			get
			{
				return SR.GetString("DatabaseDeleteThroughContext");
			}
		}
		internal static string UnrecognizedProviderMode(object p0)
		{
			return SR.GetString("UnrecognizedProviderMode", new object[]
			{
				p0
			});
		}
		internal static string CompiledQueryCannotReturnType(object p0)
		{
			return SR.GetString("CompiledQueryCannotReturnType", new object[]
			{
				p0
			});
		}
		internal static string ArgumentEmpty(object p0)
		{
			return SR.GetString("ArgumentEmpty", new object[]
			{
				p0
			});
		}
		internal static string ArgumentTypeMismatch(object p0)
		{
			return SR.GetString("ArgumentTypeMismatch", new object[]
			{
				p0
			});
		}
		internal static string CouldNotDetermineSqlType(object p0)
		{
			return SR.GetString("CouldNotDetermineSqlType", new object[]
			{
				p0
			});
		}
		internal static string CouldNotDetermineDbGeneratedSqlType(object p0)
		{
			return SR.GetString("CouldNotDetermineDbGeneratedSqlType", new object[]
			{
				p0
			});
		}
		internal static string CreateDatabaseFailedBecauseOfClassWithNoMembers(object p0)
		{
			return SR.GetString("CreateDatabaseFailedBecauseOfClassWithNoMembers", new object[]
			{
				p0
			});
		}
		internal static string CreateDatabaseFailedBecauseOfContextWithNoTables(object p0)
		{
			return SR.GetString("CreateDatabaseFailedBecauseOfContextWithNoTables", new object[]
			{
				p0
			});
		}
		internal static string CreateDatabaseFailedBecauseSqlCEDatabaseAlreadyExists(object p0)
		{
			return SR.GetString("CreateDatabaseFailedBecauseSqlCEDatabaseAlreadyExists", new object[]
			{
				p0
			});
		}
		internal static string InvalidConnectionArgument(object p0)
		{
			return SR.GetString("InvalidConnectionArgument", new object[]
			{
				p0
			});
		}
		internal static string IifReturnTypesMustBeEqual(object p0, object p1)
		{
			return SR.GetString("IifReturnTypesMustBeEqual", new object[]
			{
				p0,
				p1
			});
		}
		internal static string MethodNotMappedToStoredProcedure(object p0)
		{
			return SR.GetString("MethodNotMappedToStoredProcedure", new object[]
			{
				p0
			});
		}
		internal static string ResultTypeNotMappedToFunction(object p0, object p1)
		{
			return SR.GetString("ResultTypeNotMappedToFunction", new object[]
			{
				p0,
				p1
			});
		}
		internal static string UnexpectedTypeCode(object p0)
		{
			return SR.GetString("UnexpectedTypeCode", new object[]
			{
				p0
			});
		}
		internal static string UnsupportedTypeConstructorForm(object p0)
		{
			return SR.GetString("UnsupportedTypeConstructorForm", new object[]
			{
				p0
			});
		}
		internal static string WrongNumberOfValuesInCollectionArgument(object p0, object p1, object p2)
		{
			return SR.GetString("WrongNumberOfValuesInCollectionArgument", new object[]
			{
				p0,
				p1,
				p2
			});
		}
		internal static string LogGeneralInfoMessage(object p0, object p1)
		{
			return SR.GetString("LogGeneralInfoMessage", new object[]
			{
				p0,
				p1
			});
		}
		internal static string LogAttemptingToDeleteDatabase(object p0)
		{
			return SR.GetString("LogAttemptingToDeleteDatabase", new object[]
			{
				p0
			});
		}
		internal static string LogStoredProcedureExecution(object p0, object p1)
		{
			return SR.GetString("LogStoredProcedureExecution", new object[]
			{
				p0,
				p1
			});
		}
		internal static string MemberCannotBeTranslated(object p0, object p1)
		{
			return SR.GetString("MemberCannotBeTranslated", new object[]
			{
				p0,
				p1
			});
		}
		internal static string NonConstantExpressionsNotSupportedFor(object p0)
		{
			return SR.GetString("NonConstantExpressionsNotSupportedFor", new object[]
			{
				p0
			});
		}
		internal static string SqlMethodOnlyForSql(object p0)
		{
			return SR.GetString("SqlMethodOnlyForSql", new object[]
			{
				p0
			});
		}
		internal static string LenOfTextOrNTextNotSupported(object p0)
		{
			return SR.GetString("LenOfTextOrNTextNotSupported", new object[]
			{
				p0
			});
		}
		internal static string TextNTextAndImageCannotOccurInDistinct(object p0)
		{
			return SR.GetString("TextNTextAndImageCannotOccurInDistinct", new object[]
			{
				p0
			});
		}
		internal static string TextNTextAndImageCannotOccurInUnion(object p0)
		{
			return SR.GetString("TextNTextAndImageCannotOccurInUnion", new object[]
			{
				p0
			});
		}
		internal static string MaxSizeNotSupported(object p0)
		{
			return SR.GetString("MaxSizeNotSupported", new object[]
			{
				p0
			});
		}
		internal static string CannotConvertToEntityRef(object p0)
		{
			return SR.GetString("CannotConvertToEntityRef", new object[]
			{
				p0
			});
		}
		internal static string ArgumentWrongType(object p0, object p1, object p2)
		{
			return SR.GetString("ArgumentWrongType", new object[]
			{
				p0,
				p1,
				p2
			});
		}
		internal static string ArgumentWrongValue(object p0)
		{
			return SR.GetString("ArgumentWrongValue", new object[]
			{
				p0
			});
		}
		internal static string InvalidReturnFromSproc(object p0)
		{
			return SR.GetString("InvalidReturnFromSproc", new object[]
			{
				p0
			});
		}
		internal static string BinaryOperatorNotRecognized(object p0)
		{
			return SR.GetString("BinaryOperatorNotRecognized", new object[]
			{
				p0
			});
		}
		internal static string CannotAggregateType(object p0)
		{
			return SR.GetString("CannotAggregateType", new object[]
			{
				p0
			});
		}
		internal static string CannotDeleteTypesOf(object p0)
		{
			return SR.GetString("CannotDeleteTypesOf", new object[]
			{
				p0
			});
		}
		internal static string ClassLiteralsNotAllowed(object p0)
		{
			return SR.GetString("ClassLiteralsNotAllowed", new object[]
			{
				p0
			});
		}
		internal static string ClientCaseShouldNotHold(object p0)
		{
			return SR.GetString("ClientCaseShouldNotHold", new object[]
			{
				p0
			});
		}
		internal static string ClrBoolDoesNotAgreeWithSqlType(object p0)
		{
			return SR.GetString("ClrBoolDoesNotAgreeWithSqlType", new object[]
			{
				p0
			});
		}
		internal static string ColumnIsDefinedInMultiplePlaces(object p0)
		{
			return SR.GetString("ColumnIsDefinedInMultiplePlaces", new object[]
			{
				p0
			});
		}
		internal static string ColumnIsNotAccessibleThroughGroupBy(object p0)
		{
			return SR.GetString("ColumnIsNotAccessibleThroughGroupBy", new object[]
			{
				p0
			});
		}
		internal static string ColumnIsNotAccessibleThroughDistinct(object p0)
		{
			return SR.GetString("ColumnIsNotAccessibleThroughDistinct", new object[]
			{
				p0
			});
		}
		internal static string ColumnReferencedIsNotInScope(object p0)
		{
			return SR.GetString("ColumnReferencedIsNotInScope", new object[]
			{
				p0
			});
		}
		internal static string CouldNotAssignSequence(object p0, object p1)
		{
			return SR.GetString("CouldNotAssignSequence", new object[]
			{
				p0,
				p1
			});
		}
		internal static string CouldNotTranslateExpressionForReading(object p0)
		{
			return SR.GetString("CouldNotTranslateExpressionForReading", new object[]
			{
				p0
			});
		}
		internal static string CouldNotHandleAliasRef(object p0)
		{
			return SR.GetString("CouldNotHandleAliasRef", new object[]
			{
				p0
			});
		}
		internal static string DidNotExpectAs(object p0)
		{
			return SR.GetString("DidNotExpectAs", new object[]
			{
				p0
			});
		}
		internal static string DidNotExpectTypeChange(object p0, object p1)
		{
			return SR.GetString("DidNotExpectTypeChange", new object[]
			{
				p0,
				p1
			});
		}
		internal static string ExpectedClrTypesToAgree(object p0, object p1)
		{
			return SR.GetString("ExpectedClrTypesToAgree", new object[]
			{
				p0,
				p1
			});
		}
		internal static string ExpectedQueryableArgument(object p0, object p1, object p2)
		{
			return SR.GetString("ExpectedQueryableArgument", new object[]
			{
				p0,
				p1,
				p2
			});
		}
		internal static string InvalidGroupByExpressionType(object p0)
		{
			return SR.GetString("InvalidGroupByExpressionType", new object[]
			{
				p0
			});
		}
		internal static string InvalidOrderByExpression(object p0)
		{
			return SR.GetString("InvalidOrderByExpression", new object[]
			{
				p0
			});
		}
		internal static string InvalidFormatNode(object p0)
		{
			return SR.GetString("InvalidFormatNode", new object[]
			{
				p0
			});
		}
		internal static string InvalidSequenceOperatorCall(object p0)
		{
			return SR.GetString("InvalidSequenceOperatorCall", new object[]
			{
				p0
			});
		}
		internal static string ParameterNotInScope(object p0)
		{
			return SR.GetString("ParameterNotInScope", new object[]
			{
				p0
			});
		}
		internal static string MemberAccessIllegal(object p0, object p1, object p2)
		{
			return SR.GetString("MemberAccessIllegal", new object[]
			{
				p0,
				p1,
				p2
			});
		}
		internal static string MemberCouldNotBeTranslated(object p0, object p1)
		{
			return SR.GetString("MemberCouldNotBeTranslated", new object[]
			{
				p0,
				p1
			});
		}
		internal static string MemberNotPartOfProjection(object p0, object p1)
		{
			return SR.GetString("MemberNotPartOfProjection", new object[]
			{
				p0,
				p1
			});
		}
		internal static string MethodHasNoSupportConversionToSql(object p0)
		{
			return SR.GetString("MethodHasNoSupportConversionToSql", new object[]
			{
				p0
			});
		}
		internal static string MethodFormHasNoSupportConversionToSql(object p0, object p1)
		{
			return SR.GetString("MethodFormHasNoSupportConversionToSql", new object[]
			{
				p0,
				p1
			});
		}
		internal static string UnableToBindUnmappedMember(object p0, object p1, object p2)
		{
			return SR.GetString("UnableToBindUnmappedMember", new object[]
			{
				p0,
				p1,
				p2
			});
		}
		internal static string QueryOperatorNotSupported(object p0)
		{
			return SR.GetString("QueryOperatorNotSupported", new object[]
			{
				p0
			});
		}
		internal static string QueryOperatorOverloadNotSupported(object p0)
		{
			return SR.GetString("QueryOperatorOverloadNotSupported", new object[]
			{
				p0
			});
		}
		internal static string RequiredColumnDoesNotExist(object p0)
		{
			return SR.GetString("RequiredColumnDoesNotExist", new object[]
			{
				p0
			});
		}
		internal static string SimpleCaseShouldNotHold(object p0)
		{
			return SR.GetString("SimpleCaseShouldNotHold", new object[]
			{
				p0
			});
		}
		internal static string UnexpectedNode(object p0)
		{
			return SR.GetString("UnexpectedNode", new object[]
			{
				p0
			});
		}
		internal static string UnhandledMemberAccess(object p0, object p1)
		{
			return SR.GetString("UnhandledMemberAccess", new object[]
			{
				p0,
				p1
			});
		}
		internal static string UnmappedDataMember(object p0, object p1, object p2)
		{
			return SR.GetString("UnmappedDataMember", new object[]
			{
				p0,
				p1,
				p2
			});
		}
		internal static string UnrecognizedExpressionNode(object p0)
		{
			return SR.GetString("UnrecognizedExpressionNode", new object[]
			{
				p0
			});
		}
		internal static string ValueHasNoLiteralInSql(object p0)
		{
			return SR.GetString("ValueHasNoLiteralInSql", new object[]
			{
				p0
			});
		}
		internal static string NonCountAggregateFunctionsAreNotValidOnProjections(object p0)
		{
			return SR.GetString("NonCountAggregateFunctionsAreNotValidOnProjections", new object[]
			{
				p0
			});
		}
		internal static string SourceExpressionAnnotation(object p0)
		{
			return SR.GetString("SourceExpressionAnnotation", new object[]
			{
				p0
			});
		}
		internal static string SequenceOperatorsNotSupportedForType(object p0)
		{
			return SR.GetString("SequenceOperatorsNotSupportedForType", new object[]
			{
				p0
			});
		}
		internal static string ComparisonNotSupportedForType(object p0)
		{
			return SR.GetString("ComparisonNotSupportedForType", new object[]
			{
				p0
			});
		}
		internal static string UnsupportedNodeType(object p0)
		{
			return SR.GetString("UnsupportedNodeType", new object[]
			{
				p0
			});
		}
		internal static string TypeCannotBeOrdered(object p0)
		{
			return SR.GetString("TypeCannotBeOrdered", new object[]
			{
				p0
			});
		}
		internal static string InvalidMethodExecution(object p0)
		{
			return SR.GetString("InvalidMethodExecution", new object[]
			{
				p0
			});
		}
		internal static string CouldNotConvertToPropertyOrField(object p0)
		{
			return SR.GetString("CouldNotConvertToPropertyOrField", new object[]
			{
				p0
			});
		}
		internal static string BadParameterType(object p0)
		{
			return SR.GetString("BadParameterType", new object[]
			{
				p0
			});
		}
		internal static string CannotAssignToMember(object p0)
		{
			return SR.GetString("CannotAssignToMember", new object[]
			{
				p0
			});
		}
		internal static string MappedTypeMustHaveDefaultConstructor(object p0)
		{
			return SR.GetString("MappedTypeMustHaveDefaultConstructor", new object[]
			{
				p0
			});
		}
		internal static string UnsafeStringConversion(object p0, object p1)
		{
			return SR.GetString("UnsafeStringConversion", new object[]
			{
				p0,
				p1
			});
		}
		internal static string CannotAssignNull(object p0)
		{
			return SR.GetString("CannotAssignNull", new object[]
			{
				p0
			});
		}
		internal static string ProviderNotInstalled(object p0, object p1)
		{
			return SR.GetString("ProviderNotInstalled", new object[]
			{
				p0,
				p1
			});
		}
		internal static string InvalidProviderType(object p0)
		{
			return SR.GetString("InvalidProviderType", new object[]
			{
				p0
			});
		}
		internal static string InvalidDbGeneratedType(object p0)
		{
			return SR.GetString("InvalidDbGeneratedType", new object[]
			{
				p0
			});
		}
		internal static string CannotMaterializeEntityType(object p0)
		{
			return SR.GetString("CannotMaterializeEntityType", new object[]
			{
				p0
			});
		}

		internal static string CannotMaterializeList(object p0)
		{
			return SR.GetString("CannotMaterializeList", new object[] { p0 });
		}

		public static string IQueryableCannotReturnSelfReferencingConstantExpression
		{
			get { return SR.GetString("IQueryableCannotReturnSelfReferencingConstantExpression"); }
		}
	}
}
