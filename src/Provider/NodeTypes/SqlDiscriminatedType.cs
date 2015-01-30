using System.Data.Linq.Mapping;
using System.Data.Linq.Provider.Common;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.NodeTypes
{
	/// <summary>
	/// Represents a dynamic CLR type that is chosen based on a discriminator expression.
	/// </summary>
	internal class SqlDiscriminatedType : SqlExpression {
		private ProviderType sqlType;
		private SqlExpression discriminator;
		private MetaType targetType;
		internal SqlDiscriminatedType(ProviderType sqlType, SqlExpression discriminator, MetaType targetType, Expression sourceExpression)
			: base(SqlNodeType.DiscriminatedType,
				typeof(Type),
				sourceExpression) {
			if (discriminator == null)
				throw Error.ArgumentNull("discriminator");
			this.discriminator = discriminator;
			this.targetType = targetType;
			this.sqlType = sqlType;
				}
		internal override ProviderType SqlType {
			get { return this.sqlType; }
		}
		internal SqlExpression Discriminator {
			get { return this.discriminator; }
			set { this.discriminator = value; }
		}

		internal MetaType TargetType {
			get { return this.targetType; }
		}
	}
}