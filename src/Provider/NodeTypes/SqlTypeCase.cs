using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Data.Linq.Provider.Common;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	/// <summary>
	/// Represents the construction of an object in abstract 'super sql'.
	/// The type may be polymorphic. A discriminator field is used to determine 
	/// which type in a hierarchy should be instantiated.
	/// In the common degenerate case where the inheritance hierarchy is 1-deep 
	/// the discriminator will be a constant SqlValue and there will be one 
	/// type-case-when corresponding to that type.
	/// </summary>
	internal class SqlTypeCase : SqlExpression {
		private MetaType rowType;
		private SqlExpression discriminator;
		private List<SqlTypeCaseWhen> whens = new List<SqlTypeCaseWhen>();
		ProviderType sqlType;

		internal SqlTypeCase(Type clrType, ProviderType sqlType, MetaType rowType, SqlExpression discriminator, IEnumerable<SqlTypeCaseWhen> whens, Expression sourceExpression)
			: base(SqlNodeType.TypeCase, clrType, sourceExpression) {
			this.Discriminator = discriminator;
			if (whens == null)
				throw Error.ArgumentNull("whens");
			this.whens.AddRange(whens);
			if (this.whens.Count == 0)
				throw Error.ArgumentOutOfRange("whens");
			this.sqlType = sqlType;
			this.rowType = rowType;
			}

		internal SqlExpression Discriminator {
			get { return this.discriminator; }
			set {
				if (value == null)
					throw Error.ArgumentNull("value");
				if (this.discriminator != null && this.discriminator.ClrType != value.ClrType)
					throw Error.ArgumentWrongType("value", this.discriminator.ClrType, value.ClrType);
				this.discriminator = value;
			}
		}

		internal List<SqlTypeCaseWhen> Whens {
			get { return this.whens; }
		}

		internal override ProviderType SqlType {
			get { return sqlType; }
		}

		internal MetaType RowType {
			get { return this.rowType; }
		}
	}
}