using System;
using System.IO;
using System.Xml;
using System.Data;
using System.Linq;

using NUnit.Framework;

using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data.Common;

using System.Collections.Generic;
using SD.Tools.OrmProfiler.Interceptor;
using System.Configuration;
using WriteTests;

namespace ReadWriteTests.SqlServer
{

	[TestFixture]
	public class WriteTests
	{
		private Guid	_testRunID;
		private bool	_eventHandlerVisited;
		private MappingSource _mappingSourceFromXmlFile;


		[TestFixtureSetUp]
		public void Init()
		{
			// give testrunID
			_testRunID = Guid.NewGuid();
			Console.WriteLine("TestRunID: {0}", _testRunID.ToString());
			InterceptorCore.Initialize("Write tests, LLBLGenProUnitTest");
		}

		[Test]
		public void SimpleInsertDeleteAddressTest()
		{
			using(var ctx = GetContext())
			{
				var toInsert = EntityCreator.CreateNewAddress(2);
				toInsert.TestRunId = _testRunID;
				ctx.Addresses.InsertOnSubmit(toInsert);
				ctx.SubmitChanges();
				var id = toInsert.AddressId;
				Assert.IsTrue(id > 0);
				ctx.Addresses.DeleteOnSubmit(toInsert);
				ctx.SubmitChanges();
				var idDeleted = toInsert.AddressId;
				var a = ctx.Addresses.FirstOrDefault(x => x.AddressId == id);
				Assert.IsNull(a);
			}
		}


		[Test]
		[Ignore("Fails on last line because Linq to Sql inserts a NULL for price, so the default constraint is bypassed. See #14")]
		public void RefetchWithDefaultInDbTest()
		{
			var toInsert = EntityCreator.CreateNewProduct(7);
			toInsert.TestRunId = _testRunID;
			Assert.IsNull(toInsert.Price);
			using(var ctx = GetContext())
			{
				ctx.Products.InsertOnSubmit(toInsert);
				ctx.SubmitChanges();
				var fetchedBack = ctx.Products.FirstOrDefault(p=>p.ProductId==toInsert.ProductId);
				Assert.IsNotNull(fetchedBack);
				ctx.Refresh(RefreshMode.OverwriteCurrentValues, toInsert);
			}
			Assert.AreEqual(1.0, toInsert.Price);
		}


// COPY MORE TESTS HERE


		/// <summary>
		/// Default event handler for change event tests.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		private void DefaultEventHandler(object sender, EventArgs e)
		{
			_eventHandlerVisited = true;
		}

		void DefaultPropertyChangedHandler(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			_eventHandlerVisited = true;
		}


		[TestFixtureTearDown]
		public void CleanUp()
		{
			// remove all data for this testrun
			GetContext().CallClearTestRunData(_testRunID);
			//GetContext().CallClearAll();
		}


		private WriteTestsDataContext GetContext()
		{
			if(_mappingSourceFromXmlFile == null)
			{
				var modelAssembly = typeof(WriteTestsDataContext).Assembly;
				var resourceStream = modelAssembly.GetManifestResourceStream("WriteTests.WriteTestsMappings.xml");
				_mappingSourceFromXmlFile = XmlMappingSource.FromStream(resourceStream);
			}
			// pass in sql connection to make sure the profiler gathers the right information
			var factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
			var connection = factory.CreateConnection();
			connection.ConnectionString = ConfigurationManager.ConnectionStrings["WriteTestsConnectionString.SQL Server (SqlClient)"].ConnectionString;
			return new WriteTestsDataContext(connection, _mappingSourceFromXmlFile);
		}
	}
}
