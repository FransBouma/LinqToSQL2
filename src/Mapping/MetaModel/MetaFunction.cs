using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace System.Data.Linq.Mapping
{
	/// <summary>
	/// A MetaFunction represents the mapping between a context method and a database function.
	/// </summary>
	public abstract class MetaFunction
	{
		/// <summary>
		/// The MetaModel containing this function.
		/// </summary>
		public abstract MetaModel Model { get; }
		/// <summary>
		/// The underlying context method.
		/// </summary>
		public abstract MethodInfo Method { get; }
		/// <summary>
		/// The name of the method (same as the MethodInfo's name).
		/// </summary>
		public abstract string Name { get; }
		/// <summary>
		/// The name of the database function or procedure.
		/// </summary>
		public abstract string MappedName { get; }
		/// <summary>
		/// True if the function can be composed within a query
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Composable", Justification = "Spelling is correct.")]
		public abstract bool IsComposable { get; }
		/// <summary>
		/// Gets an enumeration of the function parameters.
		/// </summary>
		/// <returns></returns>
		public abstract ReadOnlyCollection<MetaParameter> Parameters { get; }
		/// <summary>
		/// The return parameter
		/// </summary>
		public abstract MetaParameter ReturnParameter { get; }
		/// <summary>
		/// True if the stored procedure has multiple result types.
		/// </summary>
		public abstract bool HasMultipleResults { get; }
		/// <summary>
		/// An enumeration of all the known result row types of a stored-procedure.
		/// </summary>
		/// <returns>Enumeration of possible result row types.</returns>
		public abstract ReadOnlyCollection<MetaType> ResultRowTypes { get; }
	}
}

