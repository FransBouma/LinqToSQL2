using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace System.Data.Linq.Provider.Common
{
	/// <summary>
	/// Hashable MetaDataToken+Assembly. This type uniquely describes a metadata element
	/// like a MemberInfo. MetaDataToken by itself is not sufficient because its only
	/// unique within a single assembly.
	/// </summary>
	internal struct MetaPosition : IEqualityComparer<MetaPosition>, IEqualityComparer
	{
		private int metadataToken;
		private Assembly assembly;
		internal MetaPosition(MemberInfo mi)
			: this(mi.DeclaringType.Assembly, mi.MetadataToken)
		{
		}
		private MetaPosition(Assembly assembly, int metadataToken)
		{
			this.assembly = assembly;
			this.metadataToken = metadataToken;
		}

		// Equality is implemented here according to the advice in
		// CLR via C# 2ed, J. Richter, p 146. In particular, ValueType.Equals
		// should not be called for perf reasons.

		public override bool Equals(object obj)
		{
			if(obj == null)
			{
				return false;
			}

			if(obj.GetType() != this.GetType())
			{
				return false;
			}

			return AreEqual(this, (MetaPosition)obj);
		}

		public override int GetHashCode()
		{
			return metadataToken;
		}

		#region IEqualityComparer<MetaPosition> Members
		public bool Equals(MetaPosition x, MetaPosition y)
		{
			return AreEqual(x, y);
		}

		public int GetHashCode(MetaPosition obj)
		{
			return obj.metadataToken;
		}
		#endregion

		#region IEqualityComparer Members
		bool IEqualityComparer.Equals(object x, object y)
		{
			return this.Equals((MetaPosition)x, (MetaPosition)y);
		}
		int IEqualityComparer.GetHashCode(object obj)
		{
			return this.GetHashCode((MetaPosition)obj);
		}
		#endregion

		private static bool AreEqual(MetaPosition x, MetaPosition y)
		{
			return (x.metadataToken == y.metadataToken)
				   && (x.assembly == y.assembly);
		}

		// Since MetaPositions are immutable, we overload the equality operator
		// to test for value equality, rather than reference equality
		public static bool operator ==(MetaPosition x, MetaPosition y)
		{
			return AreEqual(x, y);
		}
		public static bool operator !=(MetaPosition x, MetaPosition y)
		{
			return !AreEqual(x, y);
		}

		internal static bool AreSameMember(MemberInfo x, MemberInfo y)
		{
			return x.MetadataToken == y.MetadataToken && x.DeclaringType.Assembly == y.DeclaringType.Assembly;
		}
	}
}