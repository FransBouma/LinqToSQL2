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
	/// A MetaParameter represents the mapping between a method parameter and a database function parameter.
	/// </summary>
	public abstract class MetaParameter
	{
		/// <summary>
		/// The underlying method parameter.
		/// </summary>
		public abstract ParameterInfo Parameter { get; }
		/// <summary>
		/// The name of the parameter (same as the ParameterInfo's name).
		/// </summary>
		public abstract string Name { get; }
		/// <summary>
		/// The name of the database function's parameter.
		/// </summary>
		public abstract string MappedName { get; }
		/// <summary>
		/// The CLR type of the parameter.
		/// </summary>
		public abstract Type ParameterType { get; }
		/// <summary>
		/// The database type of the parameter.
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Db", Justification = "Conforms to legacy spelling.")]
		public abstract string DbType { get; }
	}
}

