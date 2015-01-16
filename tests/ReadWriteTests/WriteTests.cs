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
using WriteTests.EntityClasses;

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
		public void EnumSaveTest()
		{
			var et = new EnumTester();
			et.EnumField = ConsoleColor.Blue;

			using(var ctx = GetContext())
			{
				ctx.EnumTesters.InsertOnSubmit(et);
				ctx.SubmitChanges();
				Assert.IsTrue(et.Id > 0);

				ctx.EnumTesters.DeleteOnSubmit(et);
				ctx.SubmitChanges();

				var enumFetched = ctx.EnumTesters.FirstOrDefault(e => e.Id == et.Id);
				Assert.IsNull(enumFetched);
			}
		}

		
		[Test]
		public void EnumFetchAfterMultiInsertTest()
		{
			var ets = new List<EnumTester>();
			ets.Add(new EnumTester() { EnumField = ConsoleColor.Black });
			ets.Add(new EnumTester() { EnumField = ConsoleColor.Blue, NullableEnumField = ConsoleColor.Red });
			ets.Add(new EnumTester() { EnumField = ConsoleColor.Blue, NullableEnumField = ConsoleColor.Red });
			using(var ctx = GetContext())
			{
				ctx.EnumTesters.InsertAllOnSubmit(ets);
				ctx.SubmitChanges();
				foreach(var et in ets)
				{
					Assert.IsTrue(et.Id > 0);
				}

				// fetch blues back
				var blues = ctx.EnumTesters.Where(e => e.EnumField == ConsoleColor.Blue).ToList();
				Assert.AreEqual(2, blues.Count);
				foreach(var et in blues)
				{
					Assert.AreEqual(ConsoleColor.Blue, et.EnumField);
					Assert.AreEqual(ConsoleColor.Red, et.NullableEnumField.Value);
				}

				ctx.EnumTesters.DeleteAllOnSubmit(ets);
				ctx.SubmitChanges();

				var etsFetched = ctx.EnumTesters.Where(e => ets.Select(x => x.Id).ToList().Contains(e.Id)).ToList();
				Assert.AreEqual(0, etsFetched.Count);
			}
		}


		// COPY MORE TESTS HERE


		#region Failing tests due to missing features
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
		

		[Test]
		[Ignore("Fails as Linq to Sql doesn't support entity splitting. See #20 ")]
		public void SplitOffEntitySaveNoReference()
		{
			var mainData = new SplitOffNoBlobData();
			mainData.FirstName = "John";
			mainData.LastName = "Doe";

			var splitOff = new SplitOffBlobData();
			splitOff.Notes = "These are the notes";
			splitOff.Photo = new byte[] { 0, 1, 2 };

			using(var ctx = GetContext())
			{
				ctx.SplitOffNoBlobDatas.InsertOnSubmit(mainData);
				ctx.SubmitChanges();
				Assert.IsTrue(mainData.Id > 0);
				splitOff.MainDataId = mainData.Id;
				ctx.SplitOffBlobDatas.InsertOnSubmit(splitOff);
				ctx.SubmitChanges();

				// fetch both in new entities
				var mainDataFetched = ctx.SplitOffNoBlobDatas.FirstOrDefault(s => s.Id == mainData.Id);
				Assert.IsNotNull(mainDataFetched);
				Assert.AreEqual(mainData.FirstName, mainDataFetched.FirstName);
				Assert.AreEqual(mainData.LastName, mainDataFetched.LastName);
				var splitOffFetched = ctx.SplitOffBlobDatas.FirstOrDefault(s=>s.MainDataId==mainData.Id);
				Assert.IsNotNull(splitOffFetched);
				Assert.AreEqual(splitOff.Notes, splitOffFetched.Notes);
				Assert.AreEqual(3, splitOffFetched.Photo.Length);

				// just delete 1, no need to delete two as they both point to the same row in the DB.
				ctx.SplitOffNoBlobDatas.DeleteOnSubmit(mainData);
				ctx.SubmitChanges();

				mainDataFetched = ctx.SplitOffNoBlobDatas.FirstOrDefault(s => s.Id == mainData.Id);
				Assert.IsNull(mainDataFetched);
			}
		}
		// There are more entity splitting tests available, but as they all fail at the moment they're not ported yet. 

		
		[Test]
		[Ignore("Linq to Sql has no prevention to persist empty entities, so persisting an empty entity or entities will result in runtime errors. See #22")]
		public void PhantomInsertPreventionTest()
		{
			var newCustomer = new Customer();
			var ba = new Address();
			var va = new Address();
			newCustomer.VisitingAddress = va;
			newCustomer.BillingAddress = ba;

			// as everything is empty, and Address isn't saved because it's not dirty, customer shouldn't be saved as well.
			using(var ctx = GetContext())
			{
				ctx.Customers.InsertOnSubmit(newCustomer);
				ctx.SubmitChanges();
				Assert.AreEqual(0, ba.AddressId);
				Assert.AreEqual(0, va.AddressId);
			}
		}
		#endregion


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
