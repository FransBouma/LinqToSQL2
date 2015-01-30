using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Data.Linq.Provider.Common;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	internal class SqlLink : SqlSimpleTypeExpression {
		private MetaType rowType;
		private SqlExpression expression;
		private MetaDataMember member;
		private List<SqlExpression> keyExpressions;
		private SqlExpression expansion;
		private object id;

		internal SqlLink(object id, MetaType rowType, Type clrType, ProviderType sqlType, SqlExpression expression, MetaDataMember member, IEnumerable<SqlExpression> keyExpressions, SqlExpression expansion, Expression sourceExpression)
			: base(SqlNodeType.Link, clrType, sqlType, sourceExpression) {
			this.id = id;
			this.rowType = rowType;
			this.expansion = expansion;
			this.expression = expression;
			this.member = member;
			this.keyExpressions = new List<SqlExpression>();
			if (keyExpressions != null)
				this.keyExpressions.AddRange(keyExpressions);
			}

		internal MetaType RowType {
			get { return this.rowType; }
		}

		internal SqlExpression Expansion {
			get { return this.expansion; }
			set { this.expansion = value; }
		}


		internal SqlExpression Expression {
			get { return this.expression; }
			set { this.expression = value; }
		}

		internal MetaDataMember Member {
			get { return this.member; }
		}

		internal List<SqlExpression> KeyExpressions {
			get { return this.keyExpressions; }
		}

		internal object Id {
			get { return this.id; }
		}
	}
}