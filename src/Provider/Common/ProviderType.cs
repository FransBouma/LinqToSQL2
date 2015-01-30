using System;

namespace System.Data.Linq.Provider.Common
{
	/// <summary>
	/// An abstract type exposed by the TypeSystemProvider.
	/// </summary>
	internal abstract class ProviderType
	{
		#region Member Declarations
		private Type runtimeOnlyType;
		protected int? size;
		protected int precision;
		protected int scale;
		private int? applicationTypeIndex = null;
		#endregion

		/// <summary>
		/// True if this type is a Unicode type (eg, ntext, etc).
		/// </summary>
		internal abstract bool IsUnicodeType { get; }

		/// <summary>
		/// For a unicode type, return it's non-unicode equivalent.
		/// </summary>
		internal abstract ProviderType GetNonUnicodeEquivalent();


		/// <summary>
		/// Returns the CLR type which most closely corresponds to this provider type.
		/// </summary>
		internal abstract Type GetClosestRuntimeType();

		/// <summary>
		/// Compare implicit type coercion precedence.
		/// -1 means there is an implicit conversion from this->type.
		/// 0 means there is a two way implicit conversion from this->type
		/// 1 means there is an implicit conversion from type->this.
		/// </summary>
		internal abstract int ComparePrecedenceTo(ProviderType type);


		/// <summary>
		/// Used to indicate if the type supports comparison in provider.
		/// </summary>
		/// <returns>Returns true if type supports comparison in provider.</returns>
		internal abstract bool SupportsComparison { get; }

		/// <summary>
		/// Used to indicate if the types supports Length function (LEN in T-SQL).  
		/// </summary>
		/// <returns>Returns true if type supports use of length function on the type.</returns>
		internal abstract bool SupportsLength { get; }

		/// <summary>
		/// Returns true if the given values will be equal to eachother for this type.
		/// </summary>
		internal abstract bool AreValuesEqual(object o1, object o2);

		/// <summary>
		/// Determines whether this type is a LOB (large object) type, or an equivalent type.
		/// For example, on SqlServer, Image and VarChar(MAX) among others are considered large types.
		/// </summary>
		/// <returns></returns>
		internal abstract bool IsLargeType { get; }

		/// <summary>
		/// Convert this type into a string that can be used in a query.
		/// </summary>
		internal abstract string ToQueryString();

		/// <summary>
		/// Convert this type into a string that can be used in a query.
		/// </summary>
		internal abstract string ToQueryString(QueryFormatOptions formatOptions);

		/// <summary>
		/// Whether this type is fixed size or not.
		/// </summary>
		internal abstract bool IsFixedSize { get; }

		/// <summary>
		/// True if the type can be ordered.
		/// </summary>
		internal abstract bool IsOrderable { get; }

		/// <summary>
		/// True if the type can be grouped.
		/// </summary>
		internal abstract bool IsGroupable { get; }

		/// <summary>
		/// True if the type is a single character type.
		/// </summary>
		internal abstract bool IsChar { get; }

		/// <summary>
		/// True if the type is a multi-character type.
		/// </summary>
		internal abstract bool IsString { get; }

		/// <summary>
		/// True if the type is a number.
		/// </summary>
		internal abstract bool IsNumeric { get; }

		/// <summary>
		/// Returns true if the type uses precision and scale.  For example, returns true
		/// for SqlDBTypes Decimal, Float and Real.
		/// </summary>
		internal abstract bool HasPrecisionAndScale { get; }

		/// <summary>
		/// Determines if it is safe to suppress size specifications for
		/// the operand of a cast/convert.  For example, when casting to string,
		/// all these types have length less than the default sized used by SqlServer,
		/// so the length specification can be omitted without fear of truncation.
		/// </summary>
		internal abstract bool CanSuppressSizeForConversionToString { get; }

		internal virtual bool IsRuntimeOnlyType
		{
			get { return runtimeOnlyType != null; }
		}

		internal virtual bool IsApplicationType
		{
			get { return applicationTypeIndex != null; }
		}

		/// <summary>
		/// Whether this is a legitimate server side type like NVARCHAR.
		/// </summary>
		protected bool IsTypeKnownByProvider
		{
			get { return !IsApplicationType && !IsRuntimeOnlyType; }
		}

		internal virtual bool HasSizeOrIsLarge
		{
			get
			{
				return this.size.HasValue || this.IsLargeType;
			}
		}

		internal virtual int? Size
		{
			get
			{
				return this.size;
			}
		}

		internal int Precision
		{
			get { return this.precision; }
		}

		internal int Scale
		{
			get { return this.scale; }
		}

		internal virtual bool CanBeColumn
		{
			get { return !this.IsApplicationType && !this.IsRuntimeOnlyType; }
		}

		internal virtual bool CanBeParameter
		{
			get { return !this.IsApplicationType && !this.IsRuntimeOnlyType; }
		}


		public static bool operator ==(ProviderType typeA, ProviderType typeB)
		{
			if((object)typeA == (object)typeB)
				return true;
			if((object)typeA != null)
				return typeA.Equals(typeB);
			return false;
		}

		public static bool operator !=(ProviderType typeA, ProviderType typeB)
		{
			if((object)typeA == (object)typeB)
				return false;
			if((object)typeA != null)
				return !typeA.Equals(typeB);
			return true;
		}
		
		public override string ToString()
		{
			return SingleValue(GetClosestRuntimeType())
				   + SingleValue(ToQueryString())
				   + KeyValue<bool>("IsApplicationType", IsApplicationType)
				   + KeyValue("IsUnicodeType", IsUnicodeType)
				   + KeyValue<bool>("IsRuntimeOnlyType", IsRuntimeOnlyType)
				   + KeyValue("SupportsComparison", SupportsComparison)
				   + KeyValue("SupportsLength", SupportsLength)
				   + KeyValue("IsLargeType", IsLargeType)
				   + KeyValue("IsFixedSize", IsFixedSize)
				   + KeyValue("IsOrderable", IsOrderable)
				   + KeyValue("IsGroupable", IsGroupable)
				   + KeyValue("IsNumeric", IsNumeric)
				   + KeyValue("IsChar", IsChar)
				   + KeyValue("IsString", IsString);
		}

		/// <summary>
		/// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// true if the specified object  is equal to the current object; otherwise, false.
		/// </returns>
		/// <param name="obj">The object to compare with the current object. </param>
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}


		/// <summary>
		/// Serves as a hash function for a particular type. 
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"/>.
		/// </returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		
		/// <summary>
		/// Helper method for building ToString functions.
		/// </summary>
		protected static string KeyValue<T>(string key, T value)
		{
			if(value != null)
			{
				return key + "=" + value.ToString() + " ";
			}
			return String.Empty;
		}


		/// <summary>
		/// Helper method for building ToString functions.
		/// </summary>
		protected static string SingleValue<T>(T value)
		{
			if(value != null)
			{
				return value.ToString() + " ";
			}
			return String.Empty;
		}



		internal virtual bool IsApplicationTypeOf(int index)
		{
			if(IsApplicationType)
			{
				return applicationTypeIndex == index;
			}
			return false;
		}


		internal virtual bool IsSameTypeFamily(ProviderType type)
		{
			if(IsApplicationType)
			{
				return false;
			}
			if(type.IsApplicationType)
			{
				return false;
			}
			return this.Category == type.Category;
		}

		#region Property Declarations
		/// <summary>
		/// Gets the type category the type represented by this instance is in.
		/// </summary>
		internal abstract TypeCategory Category { get; }


		protected Type RuntimeOnlyType
		{
			get { return runtimeOnlyType; }
			set { runtimeOnlyType = value; }
		}


		protected int? ApplicationTypeIndex
		{
			get {  return applicationTypeIndex;}
			set { applicationTypeIndex = value; }
		}
		#endregion
	}
}