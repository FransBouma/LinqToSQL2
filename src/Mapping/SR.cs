using System;
using System.Globalization;
using System.Resources;
using System.Threading;
namespace System.Data.Linq.Mapping
{
	internal sealed class SR
	{
		internal const string OwningTeam = "OwningTeam";
		internal const string InvalidFieldInfo = "InvalidFieldInfo";
		internal const string CouldNotCreateAccessorToProperty = "CouldNotCreateAccessorToProperty";
		internal const string UnableToAssignValueToReadonlyProperty = "UnableToAssignValueToReadonlyProperty";
		internal const string LinkAlreadyLoaded = "LinkAlreadyLoaded";
		internal const string EntityRefAlreadyLoaded = "EntityRefAlreadyLoaded";
		internal const string NoDiscriminatorFound = "NoDiscriminatorFound";
		internal const string InheritanceTypeDoesNotDeriveFromRoot = "InheritanceTypeDoesNotDeriveFromRoot";
		internal const string AbstractClassAssignInheritanceDiscriminator = "AbstractClassAssignInheritanceDiscriminator";
		internal const string CannotGetInheritanceDefaultFromNonInheritanceClass = "CannotGetInheritanceDefaultFromNonInheritanceClass";
		internal const string InheritanceCodeMayNotBeNull = "InheritanceCodeMayNotBeNull";
		internal const string InheritanceTypeHasMultipleDiscriminators = "InheritanceTypeHasMultipleDiscriminators";
		internal const string InheritanceCodeUsedForMultipleTypes = "InheritanceCodeUsedForMultipleTypes";
		internal const string InheritanceTypeHasMultipleDefaults = "InheritanceTypeHasMultipleDefaults";
		internal const string InheritanceHierarchyDoesNotDefineDefault = "InheritanceHierarchyDoesNotDefineDefault";
		internal const string InheritanceSubTypeIsAlsoRoot = "InheritanceSubTypeIsAlsoRoot";
		internal const string NonInheritanceClassHasDiscriminator = "NonInheritanceClassHasDiscriminator";
		internal const string MemberMappedMoreThanOnce = "MemberMappedMoreThanOnce";
		internal const string BadStorageProperty = "BadStorageProperty";
		internal const string IncorrectAutoSyncSpecification = "IncorrectAutoSyncSpecification";
		internal const string UnhandledDeferredStorageType = "UnhandledDeferredStorageType";
		internal const string BadKeyMember = "BadKeyMember";
		internal const string ProviderTypeNotFound = "ProviderTypeNotFound";
		internal const string MethodCannotBeFound = "MethodCannotBeFound";
		internal const string UnableToResolveRootForType = "UnableToResolveRootForType";
		internal const string MappingForTableUndefined = "MappingForTableUndefined";
		internal const string CouldNotFindTypeFromMapping = "CouldNotFindTypeFromMapping";
		internal const string TwoMembersMarkedAsPrimaryKeyAndDBGenerated = "TwoMembersMarkedAsPrimaryKeyAndDBGenerated";
		internal const string TwoMembersMarkedAsRowVersion = "TwoMembersMarkedAsRowVersion";
		internal const string TwoMembersMarkedAsInheritanceDiscriminator = "TwoMembersMarkedAsInheritanceDiscriminator";
		internal const string CouldNotFindRuntimeTypeForMapping = "CouldNotFindRuntimeTypeForMapping";
		internal const string UnexpectedNull = "UnexpectedNull";
		internal const string CouldNotFindElementTypeInModel = "CouldNotFindElementTypeInModel";
		internal const string BadFunctionTypeInMethodMapping = "BadFunctionTypeInMethodMapping";
		internal const string IncorrectNumberOfParametersMappedForMethod = "IncorrectNumberOfParametersMappedForMethod";
		internal const string CouldNotFindRequiredAttribute = "CouldNotFindRequiredAttribute";
		internal const string InvalidDeleteOnNullSpecification = "InvalidDeleteOnNullSpecification";
		internal const string MappedMemberHadNoCorrespondingMemberInType = "MappedMemberHadNoCorrespondingMemberInType";
		internal const string UnrecognizedAttribute = "UnrecognizedAttribute";
		internal const string UnrecognizedElement = "UnrecognizedElement";
		internal const string TooManyResultTypesDeclaredForFunction = "TooManyResultTypesDeclaredForFunction";
		internal const string NoResultTypesDeclaredForFunction = "NoResultTypesDeclaredForFunction";
		internal const string UnexpectedElement = "UnexpectedElement";
		internal const string ExpectedEmptyElement = "ExpectedEmptyElement";
		internal const string DatabaseNodeNotFound = "DatabaseNodeNotFound";
		internal const string DiscriminatorClrTypeNotSupported = "DiscriminatorClrTypeNotSupported";
		internal const string IdentityClrTypeNotSupported = "IdentityClrTypeNotSupported";
		internal const string PrimaryKeyInSubTypeNotSupported = "PrimaryKeyInSubTypeNotSupported";
		internal const string MismatchedThisKeyOtherKey = "MismatchedThisKeyOtherKey";
		internal const string InvalidUseOfGenericMethodAsMappedFunction = "InvalidUseOfGenericMethodAsMappedFunction";
		internal const string MappingOfInterfacesMemberIsNotSupported = "MappingOfInterfacesMemberIsNotSupported";
		internal const string UnmappedClassMember = "UnmappedClassMember";
		private static SR loader;
		private ResourceManager resources;
		private static CultureInfo Culture
		{
			get
			{
				return null;
			}
		}
		public static ResourceManager Resources
		{
			get
			{
				return SR.GetLoader().resources;
			}
		}
		internal SR()
		{
			this.resources = new ResourceManager("System.Data.Linq.Properties.Mapping", base.GetType().Assembly);
		}
		private static SR GetLoader()
		{
			if (SR.loader == null)
			{
				SR value = new SR();
				Interlocked.CompareExchange<SR>(ref SR.loader, value, null);
			}
			return SR.loader;
		}
		public static string GetString(string name, params object[] args)
		{
			SR sR = SR.GetLoader();
			if (sR == null)
			{
				return null;
			}
			string @string = sR.resources.GetString(name, SR.Culture);
			if (args != null && args.Length > 0)
			{
				for (int i = 0; i < args.Length; i++)
				{
					string text = args[i] as string;
					if (text != null && text.Length > 1024)
					{
						args[i] = text.Substring(0, 1021) + "...";
					}
				}
				return string.Format(CultureInfo.CurrentCulture, @string, args);
			}
			return @string;
		}
		public static string GetString(string name)
		{
			SR sR = SR.GetLoader();
			if (sR == null)
			{
				return null;
			}
			return sR.resources.GetString(name, SR.Culture);
		}
		public static object GetObject(string name)
		{
			SR sR = SR.GetLoader();
			if (sR == null)
			{
				return null;
			}
			return sR.resources.GetObject(name, SR.Culture);
		}
	}
}
