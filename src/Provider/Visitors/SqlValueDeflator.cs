using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Visitors
{
	/// <summary>
	/// Removes references to literal values
	/// </summary>
	internal class SqlValueDeflator : SqlVisitor
	{
		#region Member Declarations
		private SelectionDeflator _sDeflator;
		private bool _isTopLevel = true;
		#endregion

		internal SqlValueDeflator()
		{
			_sDeflator = new SelectionDeflator();
		}

		internal override SqlSelect VisitSelect(SqlSelect select)
		{
			if(_isTopLevel)
			{
				@select.Selection = _sDeflator.VisitExpression(@select.Selection);
			}
			return @select;
		}

		internal override SqlExpression VisitSubSelect(SqlSubSelect ss)
		{
			bool saveIsTopLevel = _isTopLevel;
			try
			{
				return base.VisitSubSelect(ss);
			}
			finally
			{
				_isTopLevel = saveIsTopLevel;
			}
		}

		class SelectionDeflator : SqlVisitor
		{
			internal override SqlExpression VisitColumnRef(SqlColumnRef cref)
			{
				SqlExpression literal = this.GetLiteralValue(cref);
				if(literal != null)
				{
					return literal;
				}
				return cref;
			}

			private SqlValue GetLiteralValue(SqlExpression expr)
			{
				while(expr != null && expr.NodeType == SqlNodeType.ColumnRef)
				{
					expr = ((SqlColumnRef)expr).Column.Expression;
				}
				return expr as SqlValue;
			}
		}
	}
}