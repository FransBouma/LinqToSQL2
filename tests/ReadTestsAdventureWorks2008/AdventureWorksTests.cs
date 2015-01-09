using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

using System.Data.Linq;
using ReadTestsAdventureWorks2008;
using ReadTestsAdventureWorks2008.EntityClasses;
using ReadTestsAdventureWorks2008.TypedViewClasses;
using System.Data.Linq.Mapping;

using SD.Tools.OrmProfiler.Interceptor;
using System.Data.Common;
using System.Configuration;

namespace ReadTestsAdventureWorks2008
{
	/// <summary>
	/// Selection of tests ported from the LLBLGen Pro runtime framework Linq test library. As Linq to Sql doesn't support TPE inheritance, some tests aren't
	/// that useful, but we've left them here as they can't hurt. 
	/// </summary>
	[TestFixture]
	public class AdventureWorksTests
	{
		#region Members
		private MappingSource _mappingSourceFromXmlFile;
		#endregion


		[TestFixtureSetUp]
		public void SetupTests()
		{
			InterceptorCore.Initialize("AdventureWorksTests");
		}


		[Test]
		public void AnyOnSubtypeWithContainsAndWhere()
		{
			using(var ctx = GetContext())
			{
				var ids = new[] { 43659, 43660, 43661};
				var q = (from c in ctx.Customers
						 from soh in c.SalesOrderHeaders
						 where ids.Contains(soh.SalesOrderId)
						 select c).Where(x => x.SalesTerritory.CountryRegionCode == "US").Any();

				Assert.IsTrue(q);
			}
		}

		[Test]
		public void AnyOnSubtypeWithContainsAndWhere2()
		{
			using(var ctx = GetContext())
			{
				var ids = new[] { 43659, 43660, 43661 };
				var q = (from c in ctx.Customers
						 from soh in c.SalesOrderHeaders
						 where ids.Contains(soh.SalesOrderId)
						 select c);
				var q2 = (from x in q
						  join st in ctx.SalesTerritories on x.TerritoryId equals st.TerritoryId into jst
						  from y in jst.DefaultIfEmpty()
						  where y.CountryRegionCode == "US"
						  select x).Any();

				var v = q2;

				Assert.IsTrue(v);
			}
		}


		[Test]
		public void GroupByWithHopTest()
		{
			using(var ctx = GetContext())
			{
				var q = ctx.SalesOrderDetails
							.Where(srr => srr.SalesOrderHeader.CustomerId == 14120
									&& srr.SalesOrderHeader.ShipDate != null)
							.GroupBy(srr => new { srr.SalesOrderId, srr.SalesOrderHeader.ShipMethodId, srr.SalesOrderHeader.ShipDate })
							.Select(x => new
							{
								Date = x.Key.ShipDate,
								ShipMethodId = x.Key.ShipMethodId,
								Value = x.Average(z => (decimal)z.OrderQty)
							});

				var results = q.ToList();
				Assert.AreEqual(2, results.Count);
				foreach(var v in results)
				{
					Assert.AreEqual(1.0, v.Value);
					Assert.AreEqual(1, v.ShipMethodId);
				}
			}
		}


		[Test]
		public void AnyOverWhere()
		{
			using(var ctx = GetContext())
			{
				bool exists = ctx.Customers.Where(c => c.CustomerId == 15758).Any();
				Assert.IsTrue(exists);
			}
		}

		[Test]
		[Ignore("Spatial isn't supported, see #11")]
		public void FetchAddressWithGeographyField()
		{
			using(var ctx = GetContext())
			{
				var q = from a in ctx.Addresses
						where a.Id == 3
						select a;

				Assert.AreEqual(1, q.Count());
			}
		}


		[Test]
		[Ignore("Spatial isn't supported, see #11")]
		public void DoubleAndDecimalMathCalculationsWithLet()
		{
			// Requires function mappings of atan/sqrt/sin/cos. These have to be supplied separately.

			//using(var ctx = GetContext())
			//{
			//	const double EarthRadius = 3963.1;
			//	var q = from address in ctx.Addresses
			//			let lat1 = (double)address.SpatialLocation.Lat.Value * Math.PI / 180
			//			let lon1 = (double)address.SpatialLocation.Long.Value * Math.PI / 180
			//			//let x2 = AdventureWorksFunctions.Sin(lat1) *
			//			//AdventureWorksFunctions.Cos(lat1)
			//			//let d = EarthRadius * (-1 * AdventureWorksFunctions.Atan(x2 / AdventureWorksFunctions.Sqrt(1 - x2 * x2)) + Math.PI / 2)
			//			//where address.Latitude.HasValue && address.Longitude.HasValue
			//			select d;
			//	foreach(var d in q)
			//	{
			//		Assert.AreEqual(5524.3839485018443, d);
			//	}
			//}
		}


		[Test]
		public void FetchEntityWithRenamedPkField()
		{
			using(var ctx = GetContext())
			{
				var q = from a in ctx.AddressTypes
						where a.Id == 3
						select a;

				Assert.AreEqual(1, q.Count());
			}
		}


		[Test]
		public void NullExceptionWithMissingOrderByTest()
		{
			using(var ctx = GetContext())
			{
				var q = (from soh in ctx.SalesOrderHeaders
				         where soh.SalesOrderId < 43690
				         from sod in soh.SalesOrderDetails
				         orderby soh.SalesOrderId
				         select new {
				                    	soh.SalesOrderId,
				                    	ctx.Products.First(p => p.ProductId == sod.ProductId).Name,
										sod.SalesOrderDetailId,
						 });
				var results = q.ToList();
				Assert.AreEqual(289, results.Count);
			}
		}
        


		/// <summary>
		/// Test should produce data obtained with 2 left joins, where 1 is a leftjoin forced by the left join of the relation it is joined to (instead of an
		/// inner join)
		/// </summary>
		[Test]
		public void FetchProjectionWithRelatedDataOverTwoManyToOneHops()
		{
			using(var ctx = GetContext())
			{
				var q = from p in ctx.Products
						orderby p.ProductId ascending
						select new
						{
							p.ProductId,
							p.Name,
							CategoryName = p.ProductSubcategory.ProductCategory.Name
						};

				int count = 0;
				foreach(var v in q)
				{
					count++;
				}

				Assert.AreEqual(504, count);
			}
		}


		[Test]
		public void DefaultIfEmptyOnRelatedEntity()
		{
			using(var ctx = GetContext())
			{
				var q = from customer in ctx.Customers
							from soh in customer.SalesOrderHeaders.DefaultIfEmpty()
							where soh.SalesOrderId == null
							select customer;
				int count = 0;
				foreach(var v in q)
				{
					count++;
				}
				Assert.AreEqual(701, count);
			}
		}

  
		[Test]
		public void DefaultIfEmptyOnRelatedEntityWithMultipleHops()
		{
			using(var ctx = GetContext())
			{
				var q = from customer in ctx.Customers
						from soh in customer.SalesOrderHeaders.DefaultIfEmpty()
						where soh.SalesOrderId == null
						select customer;

				int count = 0;
				foreach(var v in q)
				{
					count++;
				}
				Assert.AreEqual(701, count);
			}
		}

		
		/// <summary>
		/// Tests if a misplaced where still leads to a proper query.
		/// </summary>
		[Test]
		public void DefaultIfEmptyOnRelatedEntityWithMultipleHops4()
		{
			using(var ctx = GetContext())
			{
				var q = from customer in ctx.Customers
						from soh in customer.SalesOrderHeaders
						where soh.Comment != null
						from sod in soh.SalesOrderDetails
						where sod.CarrierTrackingNumber!=null
						select customer;

				int count = 0;
				foreach(var v in q)
				{
					count++;
				}
				Assert.AreEqual(0, count);
			}
		}


		[Test]
		public void WhereInNestedFromCombinedWithDefaultIfEmpty()
		{
			using(var ctx = GetContext())
			{
				var q = from customer in ctx.Customers
						where (customer.CustomerId > 10000 && customer.CustomerId < 12000)
						from soh in customer.SalesOrderHeaders.DefaultIfEmpty()
						select new { customer.CustomerId, soh.SalesOrderId };

				int count = 0;
				foreach(var v in q)
				{
					count++;
				}
				Assert.AreEqual(2975, count);
			}
		}

		[Test]
		public void CustomProjectionIntoAnonymousTypeOnSelectManyList()
		{
			using(var ctx = GetContext())
			{
				var q1 = from soh in ctx.SalesOrderHeaders
						from sod in soh.SalesOrderDetails
						select soh;
				var q = (from soh in q1
						select new { soh.SalesOrderId }).Distinct();

				int count = 0;
				foreach(var v in q)
				{
					count++;
					Assert.IsTrue(v.SalesOrderId > 0);
				}
				Assert.AreEqual(31465, count);
			}
		}


		[Test]
		public void CustomProjectionIntoAnonymousTypeUsingMultipleQueries()
		{
			using(var ctx = GetContext())
			{
				var q1 = from customer in ctx.Customers
							where customer.CustomerId<100
							from soh in customer.SalesOrderHeaders.DefaultIfEmpty()
							select new { customer.CustomerId, SalesOrderId = (int?)soh.SalesOrderId };
				var z = q1.ToList();

				int count = 0;
				foreach(var v in z)
				{
					count++;
				}
				Assert.AreEqual(99, count);
			}
		}

		[Test]
		public void CustomProjectionIntoAnonymousTypeUsingDefaultIfEmptyOnNestedFromsWithEmbeddedWhere()
		{
			using(var ctx = GetContext())
			{
				var customers = ctx.Customers.AsQueryable();
				var q = from c in customers
						from soh in c.SalesOrderHeaders.Where(soh => soh.SalesOrderId < 10).DefaultIfEmpty()
						select new { c.CustomerId, SalesOrderId = (int?) soh.SalesOrderId };
				Assert.AreEqual(19820, q.ToList().Count);
			}
		}

		[Test]
		public void NavigationInProjectionOnJoinedSetTest()
		{
			using(var ctx = GetContext())
			{
				var q = from soh in ctx.SalesOrderHeaders
						from sod in soh.SalesOrderDetails
						select soh;

				var x = (from soh in q
							select new { soh.SalesOrderId, soh.Customer.AccountNumber }).ToList();
				Assert.AreEqual(121317, x.Count);
			}
		}


		[Test]
		public void AnyWithNavigationWithSetOfSetsProjection()
		{
			using(var ctx = GetContext())
			{
				var q = from p in ctx.People
						where p.Customers.Any(c=>c.SalesOrderHeaders.Count()>0)
						select p.BusinessEntityContacts;
				Assert.AreEqual(19119, q.ToList().Count);
			}
		}

		[Test]
		public void MultiNavigationInProjectionOfSetOfSets()
		{
			using(var ctx = GetContext())
			{
				var q = ctx.People.Select(p => p.BusinessEntity.Vendor.PurchaseOrderHeaders);

				Assert.AreEqual(19972, q.ToList().Count);
			}
		}


		[Test]
		public void ContainsWithMultipleValuePairsAndWrappedQueriesTest()
		{
			using(var ctx = GetContext())
			{
				var q1 = ctx.WorkOrderRoutings.AsQueryable();

				var lst = new List<Pair<int, int>>();
				lst.Add(new Pair<int, int> { Value1 = 1, Value2 = 722 });
				lst.Add(new Pair<int, int> { Value1 = 2, Value2 = 725 });
				lst.Add(new Pair<int, int> { Value1 = 3, Value2 = 726 });

				q1 = q1.Where(
					p => lst.Contains(
							new Pair<int, int>
							{
								Value1 = p.WorkOrder.WorkOrderId,
								Value2 = p.WorkOrder.ProductId
							}
							)
					);


				var q = q1.Select(
								p => new
								{
									p.ProductId,
									p.WorkOrder.WorkOrderId
								}).ToList();

				foreach(var v in q)
				{
					
				}
			}
		}


        
		[Test]
		public void FetchingSecondPageUsingSkipTake()
		{
			using(var ctx = GetContext())
			{
				var q = ctx.Customers.Skip(11).Take(10);
				int count = 0;

				foreach(var v in q)
				{
					count++;
				}
				Assert.AreEqual(10, count);
			}
		}


		[Test]
		public void MultipleTimesSameJoinSameAliasProblem()
		{
			using(var ctx = GetContext())
			{
				var pQuery = ctx.Products.AsQueryable();
				var sQuery = ctx.ProductSubcategories.AsQueryable();
				pQuery = pQuery.Where(x => x.ProductId != 0); //example code
				var q1 = from p in pQuery
							join sc in sQuery on p.ProductSubcategoryId equals sc.ProductSubcategoryId
							select p;

				var pList = (from sc in ctx.ProductSubcategories
								join p in q1 on sc.ProductSubcategoryId equals p.ProductSubcategoryId
								select new
								{
									Value1 = p.ProductId,
									Value2 = sc.ProductCategoryId,
								}).ToList();

				Assert.AreEqual(295, pList.Count);
			}
		}

		private AdventureWorks2008DataContext GetContext()
		{
			if(_mappingSourceFromXmlFile == null)
			{
				var modelAssembly = typeof(AdventureWorks2008DataContext).Assembly;
				var resourceStream = modelAssembly.GetManifestResourceStream("AdventureWorks2008.AdventureWorks2008Mappings.xml");
				_mappingSourceFromXmlFile = XmlMappingSource.FromStream(resourceStream);
			}
			// pass in sql connection to make sure the profiler gathers the right information
			var factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
			var connection = factory.CreateConnection();
			connection.ConnectionString = ConfigurationManager.ConnectionStrings["AdventureWorksConnectionString.SQL Server (SqlClient)"].ConnectionString;
			return new AdventureWorks2008DataContext(connection, _mappingSourceFromXmlFile);
		}


	}
}
