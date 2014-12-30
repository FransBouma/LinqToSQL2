﻿#pragma warning disable 0649
//------------------------------------------------------------------------------
// <auto-generated>This code was generated by LLBLGen Pro v4.2.</auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;

namespace ReadTestsAdventureWorks2008.EntityClasses
{
	/// <summary>Class which represents the entity 'CreditCard', mapped on table 'AdventureWorks.Sales.CreditCard'.</summary>
	public partial class CreditCard : INotifyPropertyChanging, INotifyPropertyChanged
	{
		#region Events
		/// <summary>Event which is raised when a property value is changing.</summary>
		public event PropertyChangingEventHandler PropertyChanging;
		/// <summary>Event which is raised when a property value changes.</summary>
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion
		
		#region Class Member Declarations
		private System.String	_cardNumber;
		private System.String	_cardType;
		private System.Int32	_creditCardId;
		private System.Byte	_expMonth;
		private System.Int16	_expYear;
		private System.DateTime	_modifiedDate;
		private EntitySet <PersonCreditCard> _personCreditCards;
		private EntitySet <SalesOrderHeader> _salesOrderHeaders;
		#endregion
		
		#region Extensibility Method Definitions
		partial void OnLoaded();
		partial void OnValidate(System.Data.Linq.ChangeAction action);
		partial void OnCreated();
		partial void OnCardNumberChanging(System.String value);
		partial void OnCardNumberChanged();
		partial void OnCardTypeChanging(System.String value);
		partial void OnCardTypeChanged();
		partial void OnCreditCardIdChanging(System.Int32 value);
		partial void OnCreditCardIdChanged();
		partial void OnExpMonthChanging(System.Byte value);
		partial void OnExpMonthChanged();
		partial void OnExpYearChanging(System.Int16 value);
		partial void OnExpYearChanged();
		partial void OnModifiedDateChanging(System.DateTime value);
		partial void OnModifiedDateChanged();
		#endregion
		
		/// <summary>Initializes a new instance of the <see cref="CreditCard"/> class.</summary>
		public CreditCard()
		{
			_personCreditCards = new EntitySet<PersonCreditCard>(new Action<PersonCreditCard>(this.Attach_PersonCreditCards), new Action<PersonCreditCard>(this.Detach_PersonCreditCards) );
			_salesOrderHeaders = new EntitySet<SalesOrderHeader>(new Action<SalesOrderHeader>(this.Attach_SalesOrderHeaders), new Action<SalesOrderHeader>(this.Detach_SalesOrderHeaders) );
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
		
		/// <summary>Attaches this instance to the entity specified as an associated entity</summary>
		/// <param name="entity">The related entity to attach to</param>
		private void Attach_PersonCreditCards(PersonCreditCard entity)
		{
			this.SendPropertyChanging("PersonCreditCards");
			entity.CreditCard = this;
		}
		
		/// <summary>Detaches this instance from the entity specified so it's no longer an associated entity</summary>
		/// <param name="entity">The related entity to detach from</param>
		private void Detach_PersonCreditCards(PersonCreditCard entity)
		{
			this.SendPropertyChanging("PersonCreditCards");
			entity.CreditCard = null;
		}

		/// <summary>Attaches this instance to the entity specified as an associated entity</summary>
		/// <param name="entity">The related entity to attach to</param>
		private void Attach_SalesOrderHeaders(SalesOrderHeader entity)
		{
			this.SendPropertyChanging("SalesOrderHeaders");
			entity.CreditCard = this;
		}
		
		/// <summary>Detaches this instance from the entity specified so it's no longer an associated entity</summary>
		/// <param name="entity">The related entity to detach from</param>
		private void Detach_SalesOrderHeaders(SalesOrderHeader entity)
		{
			this.SendPropertyChanging("SalesOrderHeaders");
			entity.CreditCard = null;
		}


		#region Class Property Declarations
		/// <summary>Gets or sets the CardNumber field. Mapped on target field 'CardNumber'. </summary>
		public System.String CardNumber
		{
			get	{ return _cardNumber; }
			set
			{
				if((_cardNumber != value))
				{
					OnCardNumberChanging(value);
					SendPropertyChanging("CardNumber");
					_cardNumber = value;
					SendPropertyChanged("CardNumber");
					OnCardNumberChanged();
				}
			}
		}

		/// <summary>Gets or sets the CardType field. Mapped on target field 'CardType'. </summary>
		public System.String CardType
		{
			get	{ return _cardType; }
			set
			{
				if((_cardType != value))
				{
					OnCardTypeChanging(value);
					SendPropertyChanging("CardType");
					_cardType = value;
					SendPropertyChanged("CardType");
					OnCardTypeChanged();
				}
			}
		}

		/// <summary>Gets or sets the CreditCardId field. Mapped on target field 'CreditCardID'. </summary>
		public System.Int32 CreditCardId
		{
			get	{ return _creditCardId; }
			set
			{
				if((_creditCardId != value))
				{
					OnCreditCardIdChanging(value);
					SendPropertyChanging("CreditCardId");
					_creditCardId = value;
					SendPropertyChanged("CreditCardId");
					OnCreditCardIdChanged();
				}
			}
		}

		/// <summary>Gets or sets the ExpMonth field. Mapped on target field 'ExpMonth'. </summary>
		public System.Byte ExpMonth
		{
			get	{ return _expMonth; }
			set
			{
				if((_expMonth != value))
				{
					OnExpMonthChanging(value);
					SendPropertyChanging("ExpMonth");
					_expMonth = value;
					SendPropertyChanged("ExpMonth");
					OnExpMonthChanged();
				}
			}
		}

		/// <summary>Gets or sets the ExpYear field. Mapped on target field 'ExpYear'. </summary>
		public System.Int16 ExpYear
		{
			get	{ return _expYear; }
			set
			{
				if((_expYear != value))
				{
					OnExpYearChanging(value);
					SendPropertyChanging("ExpYear");
					_expYear = value;
					SendPropertyChanged("ExpYear");
					OnExpYearChanged();
				}
			}
		}

		/// <summary>Gets or sets the ModifiedDate field. Mapped on target field 'ModifiedDate'. </summary>
		public System.DateTime ModifiedDate
		{
			get	{ return _modifiedDate; }
			set
			{
				if((_modifiedDate != value))
				{
					OnModifiedDateChanging(value);
					SendPropertyChanging("ModifiedDate");
					_modifiedDate = value;
					SendPropertyChanged("ModifiedDate");
					OnModifiedDateChanged();
				}
			}
		}

		/// <summary>Represents the navigator which is mapped onto the association 'PersonCreditCard.CreditCard - CreditCard.PersonCreditCards (m:1)'</summary>
		public EntitySet<PersonCreditCard> PersonCreditCards
		{
			get { return this._personCreditCards; }
			set { this._personCreditCards.Assign(value); }
		}
		
		/// <summary>Represents the navigator which is mapped onto the association 'SalesOrderHeader.CreditCard - CreditCard.SalesOrderHeaders (m:1)'</summary>
		public EntitySet<SalesOrderHeader> SalesOrderHeaders
		{
			get { return this._salesOrderHeaders; }
			set { this._salesOrderHeaders.Assign(value); }
		}
		
		#endregion
	}
}
#pragma warning restore 0649