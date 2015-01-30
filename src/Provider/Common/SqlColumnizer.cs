using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data.Linq.Provider.NodeTypes;
using System.Data.Linq.Provider.Visitors;
using System.Linq;
using System.Diagnostics;

namespace System.Data.Linq.Provider.Common
{
	// partions select expressions and common subexpressions into scalar and non-scalar pieces by 
	// wrapping scalar pieces floating column nodes.
	internal class SqlColumnizer
	{
		ColumnNominator nominator;
		ColumnDeclarer declarer;

		internal SqlColumnizer()
		{
			this.nominator = new ColumnNominator();
			this.declarer = new ColumnDeclarer();
		}

		internal SqlExpression ColumnizeSelection(SqlExpression selection)
		{
			return this.declarer.Declare(selection, this.nominator.Nominate(selection));
		}

		internal static bool CanBeColumn(SqlExpression expression)
		{
			return ColumnNominator.CanBeColumn(expression);
		}
	}
}
