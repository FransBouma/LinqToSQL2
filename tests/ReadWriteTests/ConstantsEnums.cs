using System;

namespace ReadWriteTests.SqlServer
{
	/// <summary>
	/// ConstantsEnums. Constants and enums for the test classes to use.
	/// </summary>
	public class ConstantsEnums
	{
		// Address
		public const string StreetName1 = "Keizerstraat";
		public const int HouseNumber1 = 12 ;
		public const string ZipCode1 = "2583 RK";
		public const string City1 = "Den Haag";
		public const string Country1 = "Nederland";

		public const string StreetName2 = "OplaPlein";
		public const int HouseNumber2 = 3 ;
		public const string ZipCode2 = "1111 AA";
		public const string City2 = "Ubbingawierum";
		public const string Country2 = "Nederland";
		
		public const string StreetName3 = "Hekstraat";
		public const int HouseNumber3 = 12 ;
		public const string ZipCode3 = "2513 AA";
		public const string City3 = "Den Haag";
		public const string Country3 = "Nederland";
		
		public const string StreetName4 = "Huppellaan";
		public const int HouseNumber4 = 5 ;
		public const string ZipCode4 = "3323 ZZ";
		public const string City4 = "Sopsum";
		public const string Country4 = "Nederland";

		// Customer
		public const string Customer1CompanyName = "Foo Inc";
		public const string Customer1ContactPerson = "John Foo";
		public const string Customer1CompanyEmailAddress = "foo@example.com";
		public const string Customer2CompanyName = "Dude Inc.";
		public const string Customer2ContactPerson = "Jack Dude";
		public const string Customer2CompanyEmailAddress = "dude@example.com";

		// Employee
		public const string Employee1Name = "Sjaak TestUser";
		public const string Employee2Name = "Sjibbe Oebelema";

		// Order

		// OrderRow
		public const int Quantity = 25;

		// Product
		public const string Product1ShortDescrption = "Intruder 2000";
		public const string Product2ShortDescrption = "Dabading Pro";
		public const string Product1FullDescription = "This is the description of Intruder 2000. This is the description of Intruder 2000. This is the description of Intruder 2000. This is the description of Intruder 2000. This is the description of Intruder 2000. This is the description of Intruder 2000. This is the description of Intruder 2000. This is the description of Intruder 2000. This is the description of Intruder 2000. This is the description of Intruder 2000. This is the description of Intruder 2000. This is the description of Intruder 2000. This is the description of Intruder 2000. This is the description of Intruder 2000. This is the description of Intruder 2000. This is the description of Intruder 2000. This is the description of Intruder 2000. This is the description of Intruder 2000. This is the description of Intruder 2000. This is the description of Intruder 2000. This is the description of Intruder 2000. This is the description of Intruder 2000. This is the description of Intruder 2000. This is the description of Intruder 2000. This is the description of Intruder 2000. This is the description of Intruder 2000. ";
		public const string Product2FullDescription = "This is the description of Dabading Pro. This is the description of Dabading Pro. This is the description of Dabading Pro. This is the description of Dabading Pro. This is the description of Dabading Pro. This is the description of Dabading Pro. This is the description of Dabading Pro. This is the description of Dabading Pro. This is the description of Dabading Pro. This is the description of Dabading Pro. This is the description of Dabading Pro. This is the description of Dabading Pro. This is the description of Dabading Pro. This is the description of Dabading Pro. This is the description of Dabading Pro. This is the description of Dabading Pro. This is the description of Dabading Pro. This is the description of Dabading Pro. This is the description of Dabading Pro. This is the description of Dabading Pro. This is the description of Dabading Pro. This is the description of Dabading Pro. This is the description of Dabading Pro. This is the description of Dabading Pro. This is the description of Dabading Pro. This is the description of Dabading Pro. ";
		public const decimal Product1Price = 99.95M;
		public const decimal Product2Price = 199.99M;
		
		// SpecialCustomer
		public const float Customer2Discount = 10.0f;
	}
}
