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
	/// <summary>Class which represents the entity 'ErrorLog', mapped on table 'AdventureWorks.dbo.ErrorLog'.</summary>
	public partial class ErrorLog : INotifyPropertyChanging, INotifyPropertyChanged
	{
		#region Events
		/// <summary>Event which is raised when a property value is changing.</summary>
		public event PropertyChangingEventHandler PropertyChanging;
		/// <summary>Event which is raised when a property value changes.</summary>
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion
		
		#region Class Member Declarations
		private Nullable<System.Int32>	_errorLine;
		private System.Int32	_errorLogId;
		private System.String	_errorMessage;
		private System.Int32	_errorNumber;
		private System.String	_errorProcedure;
		private Nullable<System.Int32>	_errorSeverity;
		private Nullable<System.Int32>	_errorState;
		private System.DateTime	_errorTime;
		private System.String	_userName;
		#endregion
		
		#region Extensibility Method Definitions
		partial void OnLoaded();
		partial void OnValidate(System.Data.Linq.ChangeAction action);
		partial void OnCreated();
		partial void OnErrorLineChanging(Nullable<System.Int32> value);
		partial void OnErrorLineChanged();
		partial void OnErrorLogIdChanging(System.Int32 value);
		partial void OnErrorLogIdChanged();
		partial void OnErrorMessageChanging(System.String value);
		partial void OnErrorMessageChanged();
		partial void OnErrorNumberChanging(System.Int32 value);
		partial void OnErrorNumberChanged();
		partial void OnErrorProcedureChanging(System.String value);
		partial void OnErrorProcedureChanged();
		partial void OnErrorSeverityChanging(Nullable<System.Int32> value);
		partial void OnErrorSeverityChanged();
		partial void OnErrorStateChanging(Nullable<System.Int32> value);
		partial void OnErrorStateChanged();
		partial void OnErrorTimeChanging(System.DateTime value);
		partial void OnErrorTimeChanged();
		partial void OnUserNameChanging(System.String value);
		partial void OnUserNameChanged();
		#endregion
		
		/// <summary>Initializes a new instance of the <see cref="ErrorLog"/> class.</summary>
		public ErrorLog()
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
		/// <summary>Gets or sets the ErrorLine field. Mapped on target field 'ErrorLine'. </summary>
		public Nullable<System.Int32> ErrorLine
		{
			get	{ return _errorLine; }
			set
			{
				if((_errorLine != value))
				{
					OnErrorLineChanging(value);
					SendPropertyChanging("ErrorLine");
					_errorLine = value;
					SendPropertyChanged("ErrorLine");
					OnErrorLineChanged();
				}
			}
		}

		/// <summary>Gets or sets the ErrorLogId field. Mapped on target field 'ErrorLogID'. </summary>
		public System.Int32 ErrorLogId
		{
			get	{ return _errorLogId; }
			set
			{
				if((_errorLogId != value))
				{
					OnErrorLogIdChanging(value);
					SendPropertyChanging("ErrorLogId");
					_errorLogId = value;
					SendPropertyChanged("ErrorLogId");
					OnErrorLogIdChanged();
				}
			}
		}

		/// <summary>Gets or sets the ErrorMessage field. Mapped on target field 'ErrorMessage'. </summary>
		public System.String ErrorMessage
		{
			get	{ return _errorMessage; }
			set
			{
				if((_errorMessage != value))
				{
					OnErrorMessageChanging(value);
					SendPropertyChanging("ErrorMessage");
					_errorMessage = value;
					SendPropertyChanged("ErrorMessage");
					OnErrorMessageChanged();
				}
			}
		}

		/// <summary>Gets or sets the ErrorNumber field. Mapped on target field 'ErrorNumber'. </summary>
		public System.Int32 ErrorNumber
		{
			get	{ return _errorNumber; }
			set
			{
				if((_errorNumber != value))
				{
					OnErrorNumberChanging(value);
					SendPropertyChanging("ErrorNumber");
					_errorNumber = value;
					SendPropertyChanged("ErrorNumber");
					OnErrorNumberChanged();
				}
			}
		}

		/// <summary>Gets or sets the ErrorProcedure field. Mapped on target field 'ErrorProcedure'. </summary>
		public System.String ErrorProcedure
		{
			get	{ return _errorProcedure; }
			set
			{
				if((_errorProcedure != value))
				{
					OnErrorProcedureChanging(value);
					SendPropertyChanging("ErrorProcedure");
					_errorProcedure = value;
					SendPropertyChanged("ErrorProcedure");
					OnErrorProcedureChanged();
				}
			}
		}

		/// <summary>Gets or sets the ErrorSeverity field. Mapped on target field 'ErrorSeverity'. </summary>
		public Nullable<System.Int32> ErrorSeverity
		{
			get	{ return _errorSeverity; }
			set
			{
				if((_errorSeverity != value))
				{
					OnErrorSeverityChanging(value);
					SendPropertyChanging("ErrorSeverity");
					_errorSeverity = value;
					SendPropertyChanged("ErrorSeverity");
					OnErrorSeverityChanged();
				}
			}
		}

		/// <summary>Gets or sets the ErrorState field. Mapped on target field 'ErrorState'. </summary>
		public Nullable<System.Int32> ErrorState
		{
			get	{ return _errorState; }
			set
			{
				if((_errorState != value))
				{
					OnErrorStateChanging(value);
					SendPropertyChanging("ErrorState");
					_errorState = value;
					SendPropertyChanged("ErrorState");
					OnErrorStateChanged();
				}
			}
		}

		/// <summary>Gets or sets the ErrorTime field. Mapped on target field 'ErrorTime'. </summary>
		public System.DateTime ErrorTime
		{
			get	{ return _errorTime; }
			set
			{
				if((_errorTime != value))
				{
					OnErrorTimeChanging(value);
					SendPropertyChanging("ErrorTime");
					_errorTime = value;
					SendPropertyChanged("ErrorTime");
					OnErrorTimeChanged();
				}
			}
		}

		/// <summary>Gets or sets the UserName field. Mapped on target field 'UserName'. </summary>
		public System.String UserName
		{
			get	{ return _userName; }
			set
			{
				if((_userName != value))
				{
					OnUserNameChanging(value);
					SendPropertyChanging("UserName");
					_userName = value;
					SendPropertyChanged("UserName");
					OnUserNameChanged();
				}
			}
		}

		#endregion
	}
}
#pragma warning restore 0649