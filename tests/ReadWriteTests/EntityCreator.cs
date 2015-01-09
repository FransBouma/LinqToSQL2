using System;
using WriteTests.EntityClasses;

namespace ReadWriteTests.SqlServer
{
	/// <summary>
	/// EntityCreator. Creates new entity objects for the tests to use.
	/// Populates the objects with the same data every time. The caller has to modify the data if required.
	/// Uses the constants where possible.
	/// </summary>
	public class EntityCreator
	{
		/// <summary>
		/// Creates a new address object
		/// </summary>
		/// <param name="number">can be 1 or 2. If 1, the first address constants are used otherwise the second</param>
		/// <returns></returns>
		public static Address CreateNewAddress(int number)
		{
			Address toReturn = new Address();
			switch(number)
			{
				case 1:
					toReturn.StreetName = ConstantsEnums.StreetName1;
					toReturn.HouseNumber = ConstantsEnums.HouseNumber1;
					toReturn.Zipcode = ConstantsEnums.ZipCode1;
					toReturn.City = ConstantsEnums.City1;
					toReturn.Country = ConstantsEnums.Country1;
					break;
				case 2:
					toReturn.StreetName = ConstantsEnums.StreetName2;
					toReturn.HouseNumber = ConstantsEnums.HouseNumber2;
					toReturn.Zipcode = ConstantsEnums.ZipCode2;
					toReturn.City = ConstantsEnums.City2;
					toReturn.Country = ConstantsEnums.Country2;
					break;
				case 3:
					toReturn.StreetName = ConstantsEnums.StreetName3;
					toReturn.HouseNumber = ConstantsEnums.HouseNumber3;
					toReturn.Zipcode = ConstantsEnums.ZipCode3;
					toReturn.City = ConstantsEnums.City3;
					toReturn.Country = ConstantsEnums.Country3;
					break;
				case 4:
					toReturn.StreetName = ConstantsEnums.StreetName4;
					toReturn.HouseNumber = ConstantsEnums.HouseNumber4;
					toReturn.Zipcode = ConstantsEnums.ZipCode4;
					toReturn.City = ConstantsEnums.City4;
					toReturn.Country = ConstantsEnums.Country4;
					break;
				default:
					throw new ArgumentException("number should be 1 - 4", "number");
			}

			return toReturn;
		}


		public static Customer CreateNewCustomer(int number)
		{
			var toReturn = new Customer();

			switch(number)
			{
				case 1:
					toReturn.CompanyName = ConstantsEnums.Customer1CompanyName;
					toReturn.CustomerSince = new DateTime(2000, 1, 1);
					toReturn.ContactPerson = ConstantsEnums.Customer1ContactPerson;
					toReturn.CompanyEmailAddress = ConstantsEnums.Customer1CompanyEmailAddress;
					break;
				case 2:
					toReturn.CompanyName = ConstantsEnums.Customer2CompanyName;
					toReturn.CustomerSince = new DateTime(1999, 12, 13);
					toReturn.ContactPerson = ConstantsEnums.Customer2ContactPerson;
					toReturn.CompanyEmailAddress = ConstantsEnums.Customer2CompanyEmailAddress;
					break;
				default:
					throw new ArgumentException("number should be 1 or 2", "number");
			}

			return toReturn;
		}

		public static Product CreateNewProduct(int number)
		{
			var toReturn = new Product();

			switch(number)
			{
				case 1:
					toReturn.ProductId = Guid.NewGuid();
					toReturn.ShortDescription = "New product 1";
					toReturn.FullDescription = "New product 1 created for unittests. You don't need this product";
					toReturn.Price = 10.0M;
					break;
				case 2:
					toReturn.ProductId = Guid.NewGuid();
					toReturn.ShortDescription = "New product 2";
					toReturn.FullDescription = "New product 2 created for unittests. You don't need this product";
					toReturn.Price = 20.99M;
					break;
				case 3:
					toReturn.ProductId = Guid.NewGuid();
					toReturn.ShortDescription = "New product 3";
					toReturn.FullDescription = "New product 3 created for unittests. You don't need this product";
					toReturn.Price = 20.99M;
					break;
				case 4:
					toReturn.ProductId = Guid.NewGuid();
					toReturn.ShortDescription = "New product 4";
					toReturn.FullDescription = "New product 4 created for unittests. You don't need this product";
					toReturn.Price = 20.99M;
					break;
				case 5:
					toReturn.ProductId = Guid.NewGuid();
					toReturn.ShortDescription = "New product 5";
					toReturn.FullDescription = "New product 5 created for unittests. You don't need this product";
					toReturn.Price = 20.99M;
					break;
				case 6:
					toReturn.ProductId = Guid.NewGuid();
					toReturn.ShortDescription = "New product 6";
					toReturn.FullDescription = "New product 6 created for unittests. You don't need this product";
					toReturn.Price = 20.99M;
					break;
				case 7:
					toReturn.ProductId = Guid.NewGuid();
					toReturn.ShortDescription = "New product 7";
					toReturn.FullDescription = "New product 7 created for unittests. No price, picks default";
					break;
				default:
					throw new ArgumentException("number should be 1 - 6", "number");
			}

			return toReturn;
		}
	}
}
