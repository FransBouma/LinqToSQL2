using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace System.Data.Linq.Provider.Common
{
	/// <summary>
	/// Internal interface type defining the operations dynamic materialization functions need to perform when
	/// materializing objects, without reflecting/invoking privates.
	/// <remarks>This interface is required because our anonymously hosted materialization delegates 
	/// run under partial trust and cannot access non-public members of types in the fully trusted 
	/// framework assemblies.</remarks>
	/// </summary>
	[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Materializer", Justification = "Spelling is correct.")]
	[SuppressMessage("Microsoft.Design", "CA1012:AbstractTypesShouldNotHaveConstructors", Justification = "Unknown reason.")]
	public abstract class ObjectMaterializer<TDataReader> where TDataReader : DbDataReader {
		// These are public fields rather than properties for access speed
		[SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "[....]: This is a public type that is not intended for public use.")]
		public int[] Ordinals;
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Globals", Justification = "Spelling is correct.")]
		[SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "[....]: This is a public type that is not intended for public use.")]
		public object[] Globals;
		[SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "[....]: This is a public type that is not intended for public use.")]
		public object[] Locals;
		[SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "[....]: This is a public type that is not intended for public use.")]
		public object[] Arguments;
		[SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "[....]: This is a public type that is not intended for public use.")]
		public TDataReader DataReader;
		[SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "[....]: This is a public type that is not intended for public use.")]
		public DbDataReader BufferReader;

		public ObjectMaterializer() {
			DataReader = default(TDataReader);
		}

		public abstract object InsertLookup(int globalMetaType, object instance);
		public abstract void SendEntityMaterialized(int globalMetaType, object instance);
		public abstract IEnumerable ExecuteSubQuery(int iSubQuery, object[] args);

		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "[....]: Generic parameters are required for strong-typing of the return type.")]
		public abstract IEnumerable<T> GetLinkSource<T>(int globalLink, int localFactory, object[] keyValues);

		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "[....]: Generic parameters are required for strong-typing of the return type.")]
		public abstract IEnumerable<T> GetNestedLinkSource<T>(int globalLink, int localFactory, object instance);
		public abstract bool Read();
		public abstract bool CanDeferLoad { get; }

		[SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "xiaoruda: The method has to be static because it's used in our generated code and there is no instance of the type.")]
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "[....]: Generic parameters are required for strong-typing of the return type.")]
		public static IEnumerable<TOutput> Convert<TOutput>(IEnumerable source) {
			foreach (object value in source) {
				yield return DBConvert.ChangeType<TOutput>(value);
			}
		}

		[SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "xiaoruda: The method has to be static because it's used in our generated code and there is no instance of the type.")]
		public static IGrouping<TKey, TElement> CreateGroup<TKey, TElement>(TKey key, IEnumerable<TElement> items) {
			return new ObjectReaderCompiler.Group<TKey, TElement>(key, items);
		}

		[SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "xiaoruda: The method has to be static because it's used in our generated code and there is no instance of the type.")]
		public static IOrderedEnumerable<TElement> CreateOrderedEnumerable<TElement>(IEnumerable<TElement> items) {
			return new ObjectReaderCompiler.OrderedResults<TElement>(items);
		}

		[SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "xiaoruda: The method has to be static because it's used in our generated code and there is no instance of the type.")]
		public static Exception ErrorAssignmentToNull(Type type) {
			return Error.CannotAssignNull(type);
		}
	}
}