using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Runtime.CompilerServices;

namespace System.Data.Linq
{
	using System.Data.Linq.Mapping;
	using System.Data.Linq.Provider;

	internal struct RelatedItem
	{
		internal MetaType Type;
		internal object Item;
		internal RelatedItem(MetaType type, object item)
		{
			this.Type = type;
			this.Item = item;
		}
	}
}

