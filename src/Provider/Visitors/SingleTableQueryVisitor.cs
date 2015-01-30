using System.Collections.Generic;
using System.Data.Linq.Provider.NodeTypes;
using System.Linq;
using System.Reflection;

namespace System.Data.Linq.Provider.Visitors
{
	internal class SingleTableQueryVisitor : SqlVisitor
	{
		#region Member Declartions
		private bool _isValid;
		private bool _isDistinct;
		private List<MemberInfo> _identityMembers;
		#endregion


		internal SingleTableQueryVisitor()
		{
			_isValid = true;
		}

		internal override SqlNode Visit(SqlNode node)
		{
			// recurse until we know we're invalid
			if(_isValid && node != null)
			{
				return base.Visit(node);
			}

			return node;
		}

		internal override SqlTable VisitTable(SqlTable tab)
		{
			// if we're distinct, we don't care about joins
			if(_isDistinct)
			{
				return tab;
			}

			if(_identityMembers != null)
			{
				_isValid = false;
			}
			else
			{
				this.AddIdentityMembers(tab.MetaTable.RowType.IdentityMembers.Select(m => m.Member));
			}

			return tab;
		}

		internal override SqlSelect VisitSelect(SqlSelect select)
		{
			if(select.IsDistinct)
			{
				_isDistinct = true;
				// get all members from selection
				this.AddIdentityMembers(select.Selection.ClrType.GetProperties());
				return select;
			}
			// We're not distinct, but let's check our sources...
			select.From = (SqlSource)base.Visit(select.From);

			if(_identityMembers == null || _identityMembers.Count == 0)
			{
				throw Error.SkipRequiresSingleTableQueryWithPKs();
			}
			switch(select.Selection.NodeType)
			{
				case SqlNodeType.Column:
				case SqlNodeType.ColumnRef:
				case SqlNodeType.Member:
				{
					// we've got a bare member/column node, eg "select c.CustomerId"
					// find out if it refers to the table's PK, of which there must be only 1
					if(_identityMembers.Count == 1)
					{
						MemberInfo column = _identityMembers[0];
						_isValid &= IsColumnMatch(column, @select.Selection);
					}
					else
					{
						_isValid = false;
					}

					break;
				}
				case SqlNodeType.New:
				case SqlNodeType.AliasRef:
				{
					select.Selection = this.VisitExpression(@select.Selection);
					break;
				}
				case SqlNodeType.Treat:
				case SqlNodeType.TypeCase:
				{
					break;
				}
				default:
				{
					_isValid = false;
					break;
				}
			}

			return select;
		}


		internal override SqlExpression VisitNew(SqlNew sox)
		{
			// check the args for the PKs
			foreach(MemberInfo column in _identityMembers)
			{
				// assume we're invalid unless we find a matching argument which is
				// a bare column/columnRef to the PK
				bool isMatch = false;

				// find a matching arg
				foreach(SqlExpression expr in sox.Args)
				{
					isMatch = IsColumnMatch(column, expr);

					if(isMatch)
					{
						break;
					}
				}

				if(!isMatch)
				{
					foreach(SqlMemberAssign ma in sox.Members)
					{
						SqlExpression expr = ma.Expression;

						isMatch = IsColumnMatch(column, expr);

						if(isMatch)
						{
							break;
						}
					}
				}

				_isValid &= isMatch;
				if(!_isValid)
				{
					break;
				}
			}

			return sox;
		}

		internal override SqlNode VisitUnion(SqlUnion su)
		{
			// we don't want to descend inward

			// just check that it's not a UNION ALL
			if(su.All)
			{
				_isValid = false;
			}

			// UNIONs are distinct
			_isDistinct = true;

			// get all members from selection
			this.AddIdentityMembers(su.GetClrType().GetProperties());
			return su;
		}

		private static bool IsColumnMatch(MemberInfo column, SqlExpression expr)
		{
			MemberInfo memberInfo = null;

			switch(expr.NodeType)
			{
				case SqlNodeType.Column:
					{
						memberInfo = ((SqlColumn)expr).MetaMember.Member;
						break;
					}
				case SqlNodeType.ColumnRef:
					{
						memberInfo = (((SqlColumnRef)expr).Column).MetaMember.Member;
						break;
					}
				case SqlNodeType.Member:
					{
						memberInfo = ((SqlMember)expr).Member;
						break;
					}
			}

			return (memberInfo != null && memberInfo == column);
		}


		private void AddIdentityMembers(IEnumerable<MemberInfo> members)
		{
			System.Diagnostics.Debug.Assert(_identityMembers == null, "We already have a set of keys -- why are we adding more?");
			_identityMembers = new List<MemberInfo>(members);
		}

		#region Property Declarations
		internal bool IsValid
		{
			get { return _isValid; }
		}
		#endregion
	}
}