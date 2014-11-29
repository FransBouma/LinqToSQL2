//////////////////////////////////
//
//  This is a generated file and for now it's copied from the .NET 3.5 assembly as the original resx file isn't available 
//  in the reference source.
//
//////////////////////////////////

using System;
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
	}
}
