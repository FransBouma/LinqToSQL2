using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Data.Linq;
using System.Threading;
using System.Runtime.Versioning;
using LinqToSqlShared.Mapping;
using System.Runtime.CompilerServices;

namespace System.Data.Linq.Mapping
{
	internal abstract class MetaAssociationImpl : MetaAssociation
	{
		private static char[] keySeparators = new char[] { ',' };

		/// <summary>
		/// Given a MetaType and a set of key fields, return the set of MetaDataMembers
		/// corresponding to the key.
		/// </summary>
		protected static ReadOnlyCollection<MetaDataMember> MakeKeys(MetaType mtype, string keyFields)
		{
			string[] names = keyFields.Split(keySeparators);
			MetaDataMember[] members = new MetaDataMember[names.Length];
			for(int i = 0; i < names.Length; i++)
			{
				names[i] = names[i].Trim();
				MemberInfo[] rmis = mtype.Type.GetMember(names[i], BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if(rmis == null || rmis.Length != 1)
				{
					throw Error.BadKeyMember(names[i], keyFields, mtype.Name);
				}
				members[i] = mtype.GetDataMember(rmis[0]);
				if(members[i] == null)
				{
					throw Error.BadKeyMember(names[i], keyFields, mtype.Name);
				}
			}
			return new List<MetaDataMember>(members).AsReadOnly();
		}

		/// <summary>
		/// Compare two sets of keys for equality.
		/// </summary>
		protected static bool AreEqual(IEnumerable<MetaDataMember> key1, IEnumerable<MetaDataMember> key2)
		{
			using(IEnumerator<MetaDataMember> e1 = key1.GetEnumerator())
			{
				using(IEnumerator<MetaDataMember> e2 = key2.GetEnumerator())
				{
					bool m1, m2;
					for(m1 = e1.MoveNext(), m2 = e2.MoveNext(); m1 && m2; m1 = e1.MoveNext(), m2 = e2.MoveNext())
					{
						if(e1.Current != e2.Current)
							return false;
					}
					if(m1 != m2)
						return false;
				}
			}
			return true;
		}

		public override string ToString()
		{
			return string.Format(Globalization.CultureInfo.InvariantCulture, "{0} ->{1} {2}", ThisMember.DeclaringType.Name, IsMany ? "*" : "", OtherType.Name);
		}
	}
}

