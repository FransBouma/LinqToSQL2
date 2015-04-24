using System.Collections.Generic;
using System.Data.Linq.Provider.Common;
using System.Data.Linq.Provider.Interfaces;
using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Visitors
{
	internal class SubQueryCompiler : SqlVisitor
	{
		IProvider provider;
		List<ICompiledSubQuery> subQueries;

		internal SubQueryCompiler(IProvider provider)
		{
			this.provider = provider;
		}

		internal ICompiledSubQuery[] Compile(SqlNode node)
		{
			this.subQueries = new List<ICompiledSubQuery>();
			this.Visit(node);
			return this.subQueries.ToArray();
		}

		internal override SqlSelect VisitSelect(SqlSelect select)
		{
			this.Visit(@select.Selection);
			return @select;
		}

		internal override SqlExpression VisitSubSelect(SqlSubSelect ss)
		{
			return ss;
		}

		internal override SqlExpression VisitClientQuery(SqlClientQuery cq)
		{
			Type clientElementType = cq.Query.NodeType == SqlNodeType.Multiset ? TypeSystem.GetElementType(cq.ClrType) : cq.ClrType;
			ICompiledSubQuery c = this.provider.CompileSubQuery(cq.Query.Select, clientElementType, cq.Parameters.AsReadOnly());
			cq.Ordinal = this.subQueries.Count;
			this.subQueries.Add(c);
			return cq;
		}
	}
}