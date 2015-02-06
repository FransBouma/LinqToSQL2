using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Visitors
{
	/// <summary>
	/// A validator which enforces that no more SqlMethodCall nodes exist in the tree.
	/// </summary>
	internal class ExpectNoMethodCalls : SqlVisitor
	{

		internal override SqlExpression VisitMethodCall(SqlMethodCall mc)
		{
			// eventually we may support this type of stuff given the SQL CLR, but for now it is illegal
			throw Error.MethodHasNoSupportConversionToSql(mc.Method.Name);
		}

		// check everything except selection expressions (which will be client or ignored)
		internal override SqlSelect VisitSelect(SqlSelect select)
		{
			return this.VisitSelectCore(select);
		}
	}
}