using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using LinqToSqlShared.Mapping;

namespace System.Data.Linq.Mapping
{
	using Linq;
	using System.Diagnostics.CodeAnalysis;

	/// <summary>
	/// A mapping source that uses attributes on the context to create the mapping model.
	/// </summary>
	public sealed class AttributeMappingSource : MappingSource
	{
		public AttributeMappingSource()
		{
		}

		protected override MetaModel CreateModel(Type dataContextType)
		{
			if(dataContextType == null)
			{
				throw Error.ArgumentNull("dataContextType");
			}
			return new AttributedMetaModel(this, dataContextType);
		}
	}
}

