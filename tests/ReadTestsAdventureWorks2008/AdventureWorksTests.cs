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

		/*
		[Test]
		public void CustomProjectionIntoAnonymousTypeOnSelectManyList()
		{
			using(var ctx = GetContext())
			{
				LinqMetaData metaData = new LinqMetaData(adapter);
				var q1 = from soh in ctx.SalesOrderHeader
						from sod in soh.SalesOrderDetailCollection
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
				LinqMetaData metaData = new LinqMetaData(adapter);
				//var customers = ctx.Customer.AsQueryable();
				//var customersDerivedTable = customers.Where(c => c.CustomerId <100);
				//var q1 = from customer in customersDerivedTable
				var q1 = from customer in ctx.Customer
							where customer.CustomerId<100
							from soh in customer.SalesOrderHeaderCollection.DefaultIfEmpty()
							select new { customer.CustomerId, soh.SalesOrderId };
				var z = q1.ToList();

				int count = 0;
				foreach(var v in z)
				{
					count++;
				}
				Assert.AreEqual(586, count);
			}
		}

		[Test]
		public void CustomProjectionIntoAnonymousTypeUsingDefaultIfEmptyOnNestedFromsWithEmbeddedWhere()
		{
			using(var ctx = GetContext())
			{
				LinqMetaData metaData = new LinqMetaData(adapter);
				var customers = ctx.Customer.AsQueryable();
				var q = from c in customers
						from soh in c.SalesOrderHeaderCollection.Where(soh => soh.SalesOrderId < 10).DefaultIfEmpty()
						select new { c.CustomerId, soh.SalesOrderId };

				foreach(var v in q)
				{
				}
			}
		}


		/// <summary>
		/// Just for query construction tests that the queries don't crash
		/// </summary>
		[Test]
		public void MiscellaneousTests1()
		{
			using(var ctx = GetContext())
			{
				LinqMetaData metaData = new LinqMetaData(adapter);
				var query = from soh in ctx.SalesOrderHeader select soh;

				var w = (from soh in query
							from sod in soh.SalesOrderDetailCollection
							select new { soh.SalesOrderId, soh.Customer.AccountNumber, soh.CreditCard.CardNumber }).ToList();

				query = from soh in ctx.SalesOrderHeader
						from sod in soh.SalesOrderDetailCollection
						select soh;
				//SQL error

				var y = (from soh in query
							select new { soh.SalesOrderId }).ToList();

				//Bad alias? error

				var sohquery = from soh in ctx.SalesOrderHeader select soh;

				var w2 = (from soh in sohquery
							from sod in soh.SalesOrderDetailCollection
							select new { soh.SalesOrderId, soh.Customer.AccountNumber, soh.CreditCard.CardNumber }).ToList();
								
				// query = query.Where(soh => soh.Customer.CustomerAddress.Any(ca => ca.Address.StateProvince.Name == state));

				query = from soh in query
						where soh.Customer.CustomerAddressCollection.Any(ca => ca.Address.StateProvince.Name == "Arizona")
						select soh;

				//query = query.Where(soh => soh.Customer.CustomerAddress.Any(ca => ca.Address.City == cityName));

				query = from soh in query
						where soh.Customer.CustomerAddressCollection.Any(ca => ca.Address.City == "Houston")
						select soh;

				List<string> countries = new List<string>() { "Argentina", "Brazil"};
				query = from soh in query
						where soh.Customer.CustomerAddressCollection.Any(ca => countries.Contains(ca.Address.StateProvince.CountryRegion.Name))
						select soh;

				var sohShipMethod = from soh in query
									select new { soh.SalesOrderId, soh.CreditCard.CardNumber };

				sohShipMethod = sohShipMethod.Take(3);

				foreach(var v in sohShipMethod)
				{
				}
			}
		}


		/// <summary>
		/// Just for query construction tests that the queries don't crash
		/// </summary>
		[Test]
		public void MiscellaneousTests2()
		{
			using(var ctx = GetContext())
			{
				LinqMetaData metaData = new LinqMetaData(adapter);
				var customers = ctx.Customer.AsQueryable();

				var customersDerivedTable = customers.Where(c => c.CustomerId > 10); //To force a derived table

				//      var q = AWHelper.ctx.Customer.SelectMany(customer => customer.CustomerAddress, (customer, ca) => new {customer, ca}).GroupJoin(AWHelper.ctx.SalesOrderHeader, @t => @t.customer.CustomerId, soh => soh.CustomerId, (@t, oc) => new {@t, oc}).SelectMany(@t => @t.oc.DefaultIfEmpty(), (@t, nullableSOH) => new {@t.@t.customer.CustomerId, @t.@t.ca.AddressId, nullableSOH.SalesOrderId});

				var q = from customer in customersDerivedTable
						from ca in customer.CustomerAddressCollection
						join soh in ctx.SalesOrderHeader on customer.CustomerId equals soh.CustomerId into oc
						from nullableSOH in oc.DefaultIfEmpty()
						select new { customer.CustomerId, ca.AddressId, nullableSOH.SalesOrderId };
				q = q.Take(10);

				var ql = q.ToList();
				Assert.AreEqual(10, ql.Count);

				var q1 = from customer in customersDerivedTable
							from soh in customer.SalesOrderHeaderCollection.DefaultIfEmpty()
							select new { customer.CustomerId, soh.SalesOrderId };
				q1 = q1.Take(10);
				var z = q1.ToList();

				Assert.AreEqual(10, z.Count);

				var q2 = from customer in customersDerivedTable
							from ca in customer.CustomerAddressCollection
							//            from soh in customer.SalesOrderHeader.DefaultIfEmpty()
							select new { customer.CustomerId, customer.SalesTerritory.Name };
				q2 = q2.Take(10);
				var w = q2.ToList();
				Assert.AreEqual(10, w.Count);

				var q3 = from customer in customersDerivedTable
							from ca in customer.CustomerAddressCollection
							from soh in customer.SalesOrderHeaderCollection
							select new { customer.CustomerId, ca.AddressId, soh.SalesOrderId };
				q3 = q3.Take(10);
				var k = q3.ToList();
				Assert.AreEqual(10, k.Count);

				var q5 = from customer in customersDerivedTable
							from ca in customer.CustomerAddressCollection
							from soh in customer.SalesOrderHeaderCollection.DefaultIfEmpty()
							select new { customer.CustomerId, ca.AddressId, soh.SalesOrderId };
				q5 = q5.Take(10);
				var f = q5.ToList();
				Assert.AreEqual(10, f.Count);

				var q4 = from customer in customers
							from soh in customer.SalesOrderHeaderCollection.Where(soh => soh.SalesOrderId < 10).DefaultIfEmpty()
							select new { customer.CustomerId, SalesOrderId = soh.SalesOrderId };
				q4 = q4.Take(10);
				var x = q4.ToList();
				foreach(var v in x)
				{
					Assert.AreEqual(0, v.SalesOrderId);
				}
				Assert.AreEqual(10, x.Count);
			}
		}


		/// <summary>
		/// Just for query construction tests that the queries don't crash
		/// </summary>
		[Test]
		public void MiscellaneousTests3()
		{
			using(var ctx = GetContext())
			{
				LinqMetaData metaData = new LinqMetaData(adapter);
				var q = from soh in ctx.SalesOrderHeader
						from sod in soh.SalesOrderDetailCollection
						select soh;

				var x = (from soh in q
							select new { soh.SalesOrderId, soh.Customer.AccountNumber }).ToList();

			}
		}


		/// <summary>
		/// Just for query construction tests that the queries don't crash
		/// </summary>
		[Test]
		public void MiscellaneousTests4()
		{
			using(var ctx = GetContext())
			{
				LinqMetaData metaData = new LinqMetaData(adapter);
				var q = from soh in ctx.SalesOrderHeader
						from sod in soh.SalesOrderDetailCollection
						select soh;

				var x = (from soh in q
							select new { soh.SalesOrderId, soh.Customer.AccountNumber, soh.CreditCard.CardNumber }).ToList();

			}
		}

		[Test]
		public void MiscellaneousTests5()
		{
			using(var ctx = GetContext())
			{
				LinqMetaData metaData = new LinqMetaData(adapter);
				var sohquery = from soh in ctx.SalesOrderHeader select soh;

				var w = (from soh in sohquery
							from sod in soh.SalesOrderDetailCollection
							select new { soh.SalesOrderId, soh.Customer.AccountNumber, soh.CreditCard.CardNumber }).ToList();

			}
		}

		[Test]
		public void MiscellaneousTests6()
		{
			using(var ctx = GetContext())
			{
				LinqMetaData metaData = new LinqMetaData(adapter);
				var customers = ctx.Customer.AsQueryable();
				var customersDerivedTable = from customer in customers select customer; //Using this to force a derived table causes a crash
				customersDerivedTable = customers.Where(c => c.CustomerId > 10); //To force a derived table

				var q = from customer in customersDerivedTable
						join soh in
							(from s in ctx.SalesOrderHeader where s.SalesPersonId > 22 select s) on customer.CustomerId equals soh.CustomerId into oc
						from nullableSOH in oc.DefaultIfEmpty()

						from ca in customer.CustomerAddressCollection.DefaultIfEmpty()
						select new { customer.CustomerId, ca.AddressId, nullableSOH.SalesOrderId, customer.SalesTerritory.Name };
				q = q.Take(10);

				var l = q.ToList();
			}
		}

		[Test]
		public void MiscellaneousTests7()
		{
			using(var ctx = GetContext())
			{
				LinqMetaData metaData = new LinqMetaData(adapter);
				var customers = ctx.Customer.AsQueryable();
				var q = from customer in customers
						join soh in
							(from s in ctx.SalesOrderHeader where s.SalesPersonId > 22 select s) on customer.CustomerId equals soh.CustomerId into join1
						join ca in 
							(from custaddr in ctx.CustomerAddress where custaddr.AddressId > 100 select custaddr) 
								on customer.CustomerId equals ca.CustomerId into join2
						from xca in join2.DefaultIfEmpty()
						from xsoh in join1.DefaultIfEmpty()
						select new { customer.CustomerId, xca.AddressId, xsoh.SalesOrderId, customer.SalesTerritory.Name };
				q = q.Take(10);

				var l = q.ToList();
			}
		}


		[Test]
		public void MiscellaneousTests8()
		{
			using(var ctx = GetContext())
			{
				LinqMetaData metaData = new LinqMetaData(adapter);
				var customers = ctx.Customer.AsQueryable();
				var q = from customer in customers
						join ca in
							(from custaddr in ctx.CustomerAddress where custaddr.AddressId > 100 select custaddr)
								on customer.CustomerId equals ca.CustomerId into join2
						from xca in join2.DefaultIfEmpty()
						join soh in
							(from s in ctx.SalesOrderHeader where s.SalesPersonId > 22 select s) on customer.CustomerId equals soh.CustomerId into join1
						from xsoh in join1.DefaultIfEmpty()
						select new { customer.CustomerId, xca.AddressId, xsoh.SalesOrderId, customer.SalesTerritory.Name };
				q = q.Take(10);

				var l = q.ToList();
			}
		}


		[Test]
		public void MiscellaneousTests9()
		{
			using(var ctx = GetContext())
			{
				LinqMetaData metaData = new LinqMetaData(adapter);
				var customers = ctx.Customer.AsQueryable().Select(c=>c);
				customers = customers.Where(c => c.CustomerId > 10);
				var q = from c in customers
						from soh in c.SalesOrderHeaderCollection
						select new
						{
							Scalar = (from a in ctx.Address
										from ca in a.CustomerAddressCollection
										where ca.CustomerId == c.CustomerId
										select a).Count()
						};

				foreach(var v in q)
				{
				}

			}
		}


		[Test]
		public void MiscellaneousTests10()
		{
			using(var ctx = GetContext())
			{
				LinqMetaData metaData = new LinqMetaData(adapter);
				var q = from ca in ctx.CustomerAddress
						where ca.Address.PostalCode == "98011"
						group ca by ca.Address.StateProvince.CountryRegion.Name into g
						select new { g.Key, Amount = g.Count() };

				foreach(var v in q)
				{
				}
			}
		}


		[Test]
		public void MiscellaneousTests11()
		{
			using(var ctx = GetContext())
			{
				LinqMetaData metaData = new LinqMetaData(adapter);
				var q = from Address in ctx.Address
						from EmployeeAddress in Address.EmployeeAddressCollection
						from Individual in EmployeeAddress.Employee.Contact.IndividualCollection
						select Address;

				foreach(var v in q)
				{ 
				}
			}
		}


		[Test]
		public void MiscellaneousTests12()
		{
			using(var ctx = GetContext())
			{
				LinqMetaData metaData = new LinqMetaData(adapter);
				var q = from Address in ctx.Address
						from EmployeeAddress in Address.EmployeeAddressCollection
						select EmployeeAddress.Employee.Contact;

				foreach(var v in q)
				{
				}
			}
		}


		[Test]
		public void MiscellaneousTests13()
		{
			using(var ctx = GetContext())
			{
				LinqMetaData metaData = new LinqMetaData(adapter);
				var q = ctx.CustomerAddress.Select(ca => ca.Address.StateProvince.CountryRegion);
				foreach(var v in q)
				{
				}
			}
		}


		[Test]
		[Ignore("Crashes with null-ref. Possibly due to the multi-hop in the on clause. Likely related to other ignored tests")]
		public void MiscellaneousTests14()
		{
			using(var ctx = GetContext())
			{
				LinqMetaData metaData = new LinqMetaData(adapter);
				var q = from address in ctx.Address
						join employeeAddress in ctx.EmployeeAddress on address.Id equals employeeAddress.AddressId
						join individual in ctx.Individual on employeeAddress.Employee.Contact.ContactId equals individual.ContactId
						select address;

				foreach(var v in q)
				{
				}
			}
		}


		[Test]
		[Ignore("Crashes with re-aliasing due to inheritance. GEM:760")]
		public void MiscellaneousTests15()
		{
			using(var ctx = GetContext())
			{
				LinqMetaData metaData = new LinqMetaData(adapter);
				var q = from c in ctx.Contact
						where c.EmployeeCollection.Any(e=>e.EmployeeAddressCollection.Count()>0)
						select c.IndividualCollection;

				foreach(var v in q)
				{
				}
			}
		}


		[Test]
		[Ignore("Crashes with nested query issue due to the wrong parent query is produced. GEM:761")]
		public void MiscellaneousTests16()
		{
			using(var ctx = GetContext())
			{
				LinqMetaData metaData = new LinqMetaData(adapter);
				var q = ctx.EmployeeAddress.Select(e => e.Employee.Contact.IndividualCollection);

				foreach(var v in q)
				{
				}
			}
		}


		[Test]
		public void MiscellaneousTests17()
		{
			using(var ctx = GetContext())
			{
				LinqMetaData metaData = new LinqMetaData(adapter);
				var q = from soh in ctx.SalesOrderHeader
						from sod in soh.SalesOrderDetailCollection
						from sod2 in soh.SalesOrderDetailCollection
						where sod.SalesOrderId > 10 && sod2.SalesOrderId > 1
						select soh;

				var w = (from soh in q
							from sod3 in soh.SalesOrderDetailCollection
							select soh).ToList(); //crash
			}
		}


		[Test]
		public void MiscellaneousTests18()
		{
			using(var ctx = GetContext())
			{
				LinqMetaData metaData = new LinqMetaData(adapter);
				var c = from Address in ctx.Address
					from EmployeeAddress in Address.EmployeeAddressCollection
					select Address;

				var q = from Address in c
						from EmployeeAddress in Address.EmployeeAddressCollection
						from SalesOrderHeader in Address.SalesOrderHeaderCollection.DefaultIfEmpty()
						select Address;

				foreach(var v in q)
				{
				}
			}
		}


		[Test]
		public void PrefetchPathTestUsingInheritanceAndSubqueries()
		{
			using(var ctx = GetContext())
			{
				LinqMetaData metaData = new LinqMetaData(adapter);
				adapter.ParameterisedPrefetchPathThreshold = 1;
				var q = (from s in ctx.SalesPerson
							select s).WithPath(
							new PathEdge<SalesTerritoryEntity>(SalesPersonEntity.PrefetchPathSalesTerritory)
											);

				int count = 0;
				foreach(var v in q)
				{
					count++;
					if(new[] { 268, 284, 288 }.Contains(v.EmployeeId))
					{
						Assert.IsNull(v.SalesTerritory);
					}
					else
					{
						Assert.IsNotNull(v.SalesTerritory);
					}
				}
				Assert.AreEqual(17, count);
			}
		}


		[Test]
		public void DoubleAndDecimalMathCalculationsWithLet()
		{
			using(var ctx = GetContext())
			{
				LinqMetaData metaData = new LinqMetaData(adapter, new AdventureWorksFunctionMappings());
				const double EarthRadius = 3963.1;
				var q = from address in ctx.CreativeAddress
						//let lat1 = (double)address.Latitude.Value * Math.PI / 180
						//let lon1 = (double)address.Longitude.Value * Math.PI / 180
						let lat1 = address.LatitudeF.Value * Math.PI / 180
						let lon1 = address.LongitudeF.Value * Math.PI / 180
						let x2 = AdventureWorksFunctions.Sin(lat1) *
									AdventureWorksFunctions.Cos(lat1)
						let d = EarthRadius * (-1 * AdventureWorksFunctions.Atan(x2 / AdventureWorksFunctions.Sqrt(1 - x2 * x2)) + Math.PI / 2)
						where address.Latitude.HasValue && address.Longitude.HasValue
						select d;

				foreach(var d in q)
				{
					Assert.AreEqual(5524.3839485018443, d);
				}
			}
		}


		[Test]
		public void ContainsWithMultipleValuePairsAndWrappedQueriesTest()
		{
			using(var ctx = GetContext())
			{
				LinqMetaData metaData = new LinqMetaData(adapter);
				var q1 = ctx.WorkOrderRouting.AsQueryable();

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
		public void ProjectionToCustomClassUsingNullableValueThroughScalarQueryTest()
		{
			using(var ctx = GetContext())
			{
				LinqMetaData metaData = new LinqMetaData(adapter);
				var q = (from p in ctx.WorkOrder
							select new BusinessObject()
							{
								ProductId = p.ProductId,
								ModifiedDate = p.WorkOrderRoutingCollection.Max(x => x.ModifiedDate)

							});

				int count = 0;
				foreach(var v in q)
				{
					if(!v.ModifiedDate.HasValue)
					{
						count++;
					}
				}

				Assert.IsTrue(count>0);
			}
		}

        
		[Test]
		public void FetchingSecondPageUsingOutOfOrderSkipTake()
		{
			using(var ctx = GetContext())
			{
				LinqMetaData metaData = new LinqMetaData(adapter);
				var q = ctx.Contact.Take(10).Skip(10);
				int count = 0;

				foreach(var v in q)
				{
					count++;
				}
				Assert.AreEqual(10, count);
			}
		}


		[Test]
		public void CompareToOnGuidTest()
		{
			using(var ctx = GetContext())
			{
				LinqMetaData metaData = new LinqMetaData(adapter, new AdventureWorksGuidFunctionMappings());
				var q = (from a in ctx.Address select a)
						.Where(a => a.Rowguid.CompareTo(new Guid("57f73b00-26b1-11df-8600-001c2311de1b")) < 0);

				int count = 0;
				foreach(var v in q)
				{
					count++;
				}
				Assert.AreEqual(6, count);
			}
		}

        
		[Test]
		public void MultipleTimesSameJoinSameAliasProblem()
		{
			using(var ctx = GetContext())
			{
				LinqMetaData metaData = new LinqMetaData(adapter);
				var pQuery = ctx.Product.AsQueryable();
				var sQuery = ctx.ProductSubcategory.AsQueryable();
				pQuery = pQuery.Where(x => x.ProductId != 0); //example code
				var q1 = from p in pQuery
							join sc in sQuery on p.ProductSubcategoryId equals sc.ProductSubcategoryId
							//let temp = sc  uncomment this to workaround the exception
							select p;

				var pList = (from sc in ctx.ProductSubcategory
								join p in q1 on sc.ProductSubcategoryId equals p.ProductSubcategoryId
								select new
								{
									Value1 = p.ProductId,
									Value2 = sc.ProductCategoryId,
								}).ToList();

				foreach(var v in q1)
				{

				}
			}
		}

		#region Failing tests
		[Test]
		[Ignore("Fails, see LLBLMAIN-1159")]
		public void AnyOnSubtypeWithContainsAndWhere3()
		{
			using(var ctx = GetContext())
			{
				var metaData = new LinqMetaData(adapter);

				var ids = new[] { 43659, 43660, 43661 };
				var q = (from c in ctx.Customer
							from soh in c.SalesOrderHeaderCollection
							where ids.Contains(soh.SalesOrderId)
							select c).Any(x => x.SalesTerritory.CountryRegionCode == "US");

				Assert.IsTrue(q);
			}
		}
		#endregion
	*/

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


	//internal class AdventureWorksGuidFunctionMappings : FunctionMappingStore
	//{
	//	public AdventureWorksGuidFunctionMappings()
	//		: base()
	//	{
	//		this.Add(new FunctionMapping(typeof(Guid), "CompareTo", 1, "CASE WHEN {0} < {1} THEN -1 WHEN {0} = {1} THEN 0 ELSE 1 END"));
	//	}
	//}


	internal class BusinessObject
	{
		public int ProductId { get; set; }
		public DateTime? ModifiedDate { get; set; }
	}
}
