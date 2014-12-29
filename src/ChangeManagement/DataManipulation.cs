using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.Common;
using System.Linq.Expressions;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;

namespace System.Data.Linq
{
	/// <summary>
	/// Class with placeholder methods which are used to encode DML statements using linq and thus are only used in Expression trees with MethodCall expressions. All
	/// methods are throwing NotImplementedExceptions as they're not supposed to be called directly. Used in the StandardChangeDirector class to create MethodCall expressions easily.
	/// </summary>
	internal static class DMLMethodPlaceholders
	{
		/// <summary>
		/// The method signature used to encode an Insert command.
		/// The method will throw a NotImplementedException if called directly.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="item"></param>
		/// <param name="resultSelector"></param>
		/// <returns></returns>
		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "item", Justification = "[....]: The method is being used to represent a method signature")]
		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "resultSelector", Justification = "[....]: The method is being used to represent a method signature")]
		public static TResult Insert<TEntity, TResult>(TEntity item, Func<TEntity, TResult> resultSelector)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// The method signature used to encode an Insert command.
		/// The method will throw a NotImplementedException if called directly.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="item"></param>
		/// <returns></returns>
		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "item", Justification = "[....]: The method is being used to represent a method signature")]
		public static int Insert<TEntity>(TEntity item)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// The method signature used to encode an Update command.
		/// The method will throw a NotImplementedException if called directly.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="item"></param>
		/// <param name="check"></param>
		/// <param name="resultSelector"></param>
		/// <returns></returns>
		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "item", Justification = "[....]: The method is being used to represent a method signature")]
		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "check", Justification = "[....]: The method is being used to represent a method signature")]
		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "resultSelector", Justification = "[....]: The method is being used to represent a method signature")]
		public static TResult Update<TEntity, TResult>(TEntity item, Func<TEntity, bool> check, Func<TEntity, TResult> resultSelector)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// The method signature used to encode an Update command.
		/// The method will throw a NotImplementedException if called directly.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="item"></param>
		/// <param name="resultSelector"></param>
		/// <returns></returns>
		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "item", Justification = "[....]: The method is being used to represent a method signature")]
		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "resultSelector", Justification = "[....]: The method is being used to represent a method signature")]
		public static TResult Update<TEntity, TResult>(TEntity item, Func<TEntity, TResult> resultSelector)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// The method signature used to encode an Update command.
		/// The method will throw a NotImplementedException if called directly.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="item"></param>
		/// <param name="check"></param>
		/// <returns></returns>
		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "item", Justification = "[....]: The method is being used to represent a method signature")]
		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "check", Justification = "[....]: The method is being used to represent a method signature")]
		public static int Update<TEntity>(TEntity item, Func<TEntity, bool> check)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// The method signature used to encode an Update command.
		/// The method will throw a NotImplementedException if called directly.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="item"></param>
		/// <returns></returns>
		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "item", Justification = "[....]: The method is being used to represent a method signature")]
		public static int Update<TEntity>(TEntity item)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// The method signature used to encode a Delete command.
		/// The method will throw a NotImplementedException if called directly.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="item"></param>
		/// <param name="check"></param>
		/// <returns></returns>
		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "item", Justification = "[....]: The method is being used to represent a method signature")]
		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "check", Justification = "[....]: The method is being used to represent a method signature")]
		public static int Delete<TEntity>(TEntity item, Func<TEntity, bool> check)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// The method signature used to encode a Delete command.
		/// The method will throw a NotImplementedException if called directly.
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="item"></param>
		/// <returns></returns>
		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "item", Justification = "[....]: The method is being used to represent a method signature")]
		public static int Delete<TEntity>(TEntity item)
		{
			throw new NotImplementedException();
		}
	}
}

