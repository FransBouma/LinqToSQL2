namespace System.Data.Linq.Provider.NodeTypes
{
	/// <summary>
	/// Represents one choice of object instantiation type in a type case.
	/// When 'match' is the same as type case Discriminator then the corresponding
	/// type binding is the one used for instantiation.
	/// </summary>
	internal class SqlTypeCaseWhen {
		private SqlExpression match;
		private SqlExpression @new;

		internal SqlTypeCaseWhen(SqlExpression match, SqlExpression typeBinding) {
			this.Match = match;
			this.TypeBinding = typeBinding;
		}
		internal SqlExpression Match {
			get { return this.match; }
			set {
				if (this.match != null && value != null && this.match.ClrType != value.ClrType)
					throw Error.ArgumentWrongType("value", this.match.ClrType, value.ClrType);
				this.match = value;
			}
		}
		internal SqlExpression TypeBinding {
			get { return this.@new; }
			set { this.@new = value; }
		}
	}
}