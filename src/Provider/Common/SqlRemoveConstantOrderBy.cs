using System.Data.Linq.Provider.NodeTypes;
using System.Data.Linq.Provider.Visitors;

namespace System.Data.Linq.Provider.Common
{
	/// <summary>
	/// SQL doesn't allow constants in ORDER BY.
	/// 
	/// Worse, an integer constant greater than 0 is treated as ORDER BY ProjectionColumn[i] so the results
	/// can be unexpected.
	/// 
	/// The LINQ semantic for OrderBy(o=>constant) is for it to have no effect on the ordering. We enforce
	/// that semantic here by removing all constant columns from OrderBy.
	/// </summary>
	internal class SqlRemoveConstantOrderBy
	{
		/// <summary>
		/// Remove relative constants from OrderBy.
		/// </summary>
		internal static SqlNode Remove(SqlNode node)
		{
			return new ConstantInOrderByRemover().Visit(node);
		}
	}
}
