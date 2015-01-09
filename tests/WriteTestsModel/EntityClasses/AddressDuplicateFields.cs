﻿#pragma warning disable 0649
//------------------------------------------------------------------------------
// <auto-generated>This code was generated by LLBLGen Pro v4.2.</auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;

namespace WriteTests.EntityClasses
{
	/// <summary>Class which represents the entity 'AddressDuplicateFields', mapped on table 'LLBLGenProUnitTest.dbo.Address'.</summary>
	public partial class AddressDuplicateFields : INotifyPropertyChanging, INotifyPropertyChanged
	{
		#region Events
		/// <summary>Event which is raised when a property value is changing.</summary>
		public event PropertyChangingEventHandler PropertyChanging;
		/// <summary>Event which is raised when a property value changes.</summary>
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion
		
		#region Class Member Declarations
		private System.Int32	_addressId;
		private System.String	_city;
		private System.String	_country;
		private System.Int32	_houseNumber;
		private System.String	_streetName;
		private System.String	_streetName2;
		private Nullable<System.Guid>	_testRunId;
		private System.String	_zipcode;
		#endregion
		
		#region Extensibility Method Definitions
		partial void OnLoaded();
		partial void OnValidate(System.Data.Linq.ChangeAction action);
		partial void OnCreated();
		partial void OnAddressIdChanging(System.Int32 value);
		partial void OnAddressIdChanged();
		partial void OnCityChanging(System.String value);
		partial void OnCityChanged();
		partial void OnCountryChanging(System.String value);
		partial void OnCountryChanged();
		partial void OnHouseNumberChanging(System.Int32 value);
		partial void OnHouseNumberChanged();
		partial void OnStreetNameChanging(System.String value);
		partial void OnStreetNameChanged();
		partial void OnStreetName2Changing(System.String value);
		partial void OnStreetName2Changed();
		partial void OnTestRunIdChanging(Nullable<System.Guid> value);
		partial void OnTestRunIdChanged();
		partial void OnZipcodeChanging(System.String value);
		partial void OnZipcodeChanged();
		#endregion
		
		/// <summary>Initializes a new instance of the <see cref="AddressDuplicateFields"/> class.</summary>
		public AddressDuplicateFields()
		{
			OnCreated();
		}

		/// <summary>Raises the PropertyChanging event</summary>
		/// <param name="propertyName">name of the property which is changing</param>
		protected virtual void SendPropertyChanging(string propertyName)
		{
			if((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
			}
		}
		
		/// <summary>Raises the PropertyChanged event for the property specified</summary>
		/// <param name="propertyName">name of the property which was changed</param>
		protected virtual void SendPropertyChanged(string propertyName)
		{
			if((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		

		#region Class Property Declarations
		/// <summary>Gets or sets the AddressId field. Mapped on target field 'AddressId'. </summary>
		public System.Int32 AddressId
		{
			get	{ return _addressId; }
			set
			{
				if((_addressId != value))
				{
					OnAddressIdChanging(value);
					SendPropertyChanging("AddressId");
					_addressId = value;
					SendPropertyChanged("AddressId");
					OnAddressIdChanged();
				}
			}
		}

		/// <summary>Gets or sets the City field. Mapped on target field 'City'. </summary>
		public System.String City
		{
			get	{ return _city; }
			set
			{
				if((_city != value))
				{
					OnCityChanging(value);
					SendPropertyChanging("City");
					_city = value;
					SendPropertyChanged("City");
					OnCityChanged();
				}
			}
		}

		/// <summary>Gets or sets the Country field. Mapped on target field 'Country'. </summary>
		public System.String Country
		{
			get	{ return _country; }
			set
			{
				if((_country != value))
				{
					OnCountryChanging(value);
					SendPropertyChanging("Country");
					_country = value;
					SendPropertyChanged("Country");
					OnCountryChanged();
				}
			}
		}

		/// <summary>Gets or sets the HouseNumber field. Mapped on target field 'HouseNumber'. </summary>
		public System.Int32 HouseNumber
		{
			get	{ return _houseNumber; }
			set
			{
				if((_houseNumber != value))
				{
					OnHouseNumberChanging(value);
					SendPropertyChanging("HouseNumber");
					_houseNumber = value;
					SendPropertyChanged("HouseNumber");
					OnHouseNumberChanged();
				}
			}
		}

		/// <summary>Gets or sets the StreetName field. Mapped on target field 'StreetName'. </summary>
		public System.String StreetName
		{
			get	{ return _streetName; }
			set
			{
				if((_streetName != value))
				{
					OnStreetNameChanging(value);
					SendPropertyChanging("StreetName");
					_streetName = value;
					SendPropertyChanged("StreetName");
					OnStreetNameChanged();
				}
			}
		}

		/// <summary>Gets or sets the StreetName2 field. Mapped on target field 'StreetName'. </summary>
		public System.String StreetName2
		{
			get	{ return _streetName2; }
			set
			{
				if((_streetName2 != value))
				{
					OnStreetName2Changing(value);
					SendPropertyChanging("StreetName2");
					_streetName2 = value;
					SendPropertyChanged("StreetName2");
					OnStreetName2Changed();
				}
			}
		}

		/// <summary>Gets or sets the TestRunId field. Mapped on target field 'TestRunID'. </summary>
		public Nullable<System.Guid> TestRunId
		{
			get	{ return _testRunId; }
			set
			{
				if((_testRunId != value))
				{
					OnTestRunIdChanging(value);
					SendPropertyChanging("TestRunId");
					_testRunId = value;
					SendPropertyChanged("TestRunId");
					OnTestRunIdChanged();
				}
			}
		}

		/// <summary>Gets or sets the Zipcode field. Mapped on target field 'Zipcode'. </summary>
		public System.String Zipcode
		{
			get	{ return _zipcode; }
			set
			{
				if((_zipcode != value))
				{
					OnZipcodeChanging(value);
					SendPropertyChanging("Zipcode");
					_zipcode = value;
					SendPropertyChanged("Zipcode");
					OnZipcodeChanged();
				}
			}
		}

		#endregion
	}
}
#pragma warning restore 0649