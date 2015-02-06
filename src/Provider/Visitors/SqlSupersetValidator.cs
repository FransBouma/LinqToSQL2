using System.Collections.Generic;
using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Visitors
{
	/// <summary>
	/// Validates the integrity of super-SQL trees.
	/// </summary>
	internal class SqlSupersetValidator
	{

		List<SqlVisitor> validators = new List<SqlVisitor>();

		/// <summary>
		/// Add a validator to the collection of validators to run.
		/// </summary>
		internal void AddValidator(SqlVisitor validator)
		{
			this.validators.Add(validator);
		}

		/// <summary>
		/// Execute each current validator.
		/// </summary>
		internal void Validate(SqlNode node)
		{
			foreach(SqlVisitor validator in this.validators)
			{
				validator.Visit(node);
			}
		}
	}
}