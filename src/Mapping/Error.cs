//////////////////////////////////
//
//  This is a generated file and for now it's copied from the .NET 3.5 assembly as the original resx file isn't available 
//  in the reference source.
//
//////////////////////////////////

using System;
using System.Xml.Schema;
namespace System.Data.Linq.Mapping
{
	internal static class Error
	{
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
	}
}
