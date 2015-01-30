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
	/// A mapping source that uses an external XML mapping source to create the model.
	/// </summary>
	public sealed class XmlMappingSource : MappingSource
	{
		DatabaseMapping map;

		[ResourceExposure(ResourceScope.Assembly)] // map parameter contains type names.
		private XmlMappingSource(DatabaseMapping map)
		{
			this.map = map;
		}

		[ResourceExposure(ResourceScope.None)] // Exposure is via map instance variable.
		[ResourceConsumption(ResourceScope.Assembly | ResourceScope.Machine, ResourceScope.Assembly | ResourceScope.Machine)] // For MappedMetaModel constructor call.
		protected override MetaModel CreateModel(Type dataContextType)
		{
			if(dataContextType == null)
			{
				throw Error.ArgumentNull("dataContextType");
			}
			return new MappedMetaModel(this, dataContextType, this.map);
		}

		/// <summary>
		/// Create a mapping source from xml string.
		/// </summary>
		/// <param name="dataContextType">The type of DataContext to base the mapping on.</param>
		/// <param name="xml">A string containing XML.</param>
		/// <returns>The mapping source.</returns>
		[ResourceExposure(ResourceScope.Assembly)] // Xml contains type names.
		[ResourceConsumption(ResourceScope.Assembly)] // For FromReader method call.
		public static XmlMappingSource FromXml(string xml)
		{
			if(xml == null)
			{
				throw Error.ArgumentNull("xml");
			}
			XmlTextReader reader = new XmlTextReader(new System.IO.StringReader(xml));
			reader.DtdProcessing = DtdProcessing.Prohibit;

			return FromReader(reader);
		}

		/// <summary>
		/// Create a mapping source from xml reader.
		/// </summary>
		/// <param name="dataContextType">The type of DataContext to base the mapping on.</param>
		/// <param name="xml">An xml reader.</param>
		/// <returns>The mapping source.</returns>
		[ResourceExposure(ResourceScope.Assembly)] // reader parameter contains type names.
		[ResourceConsumption(ResourceScope.Assembly)] // XmlMappingSource constructor call.
		public static XmlMappingSource FromReader(XmlReader reader)
		{
			if(reader == null)
			{
				throw Error.ArgumentNull("reader");
			}
			reader.MoveToContent();
			DatabaseMapping db = XmlMappingReader.ReadDatabaseMapping(reader);

			if(db == null)
			{
				throw Error.DatabaseNodeNotFound(XmlMappingConstant.MappingNamespace);
			}

			return new XmlMappingSource(db);
		}

		/// <summary>
		/// Create a mapping source from xml in a stream.
		/// </summary>
		/// <param name="dataContextType">The type of DataContext to base the mapping on.</param>
		/// <param name="xml">A stream of xml.</param>
		/// <returns>The mapping source.</returns>
		[ResourceExposure(ResourceScope.Assembly)] // Stream contains type names.
		[ResourceConsumption(ResourceScope.Assembly)] // For FromReader method call.
		public static XmlMappingSource FromStream(System.IO.Stream stream)
		{
			if(stream == null)
			{
				throw Error.ArgumentNull("stream");
			}
			XmlTextReader reader = new XmlTextReader(stream);
			reader.DtdProcessing = DtdProcessing.Prohibit;

			return FromReader(reader);
		}

		/// <summary>
		/// Create a mapping source from xml loaded from a url.
		/// </summary>
		/// <param name="dataContextType">The type of DataContext to base the mapping on.</param>
		/// <param name="url">The Url pointing to the xml.</param>
		/// <returns>The mapping source.</returns>
		[SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#", Justification = "Unknown reason.")]
		[ResourceExposure(ResourceScope.Machine | ResourceScope.Assembly)] // url parameter which may contain type names.
		[ResourceConsumption(ResourceScope.Machine | ResourceScope.Assembly)] // XmlTextReader constructor & FromReader method call.
		public static XmlMappingSource FromUrl(string url)
		{
			if(url == null)
			{
				throw Error.ArgumentNull("url");
			}
			XmlTextReader reader = new XmlTextReader(url);
			reader.DtdProcessing = DtdProcessing.Prohibit;

			try
			{
				return FromReader(reader);
			}
			finally
			{
				reader.Close();
			}
		}
	}
}

