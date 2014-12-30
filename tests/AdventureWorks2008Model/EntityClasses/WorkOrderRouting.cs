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
	/// <summary>Class which represents the entity 'WorkOrderRouting', mapped on table 'AdventureWorks.Production.WorkOrderRouting'.</summary>
	public partial class WorkOrderRouting : INotifyPropertyChanging, INotifyPropertyChanged
	{
		#region Events
		/// <summary>Event which is raised when a property value is changing.</summary>
		public event PropertyChangingEventHandler PropertyChanging;
		/// <summary>Event which is raised when a property value changes.</summary>
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion
		
		#region Class Member Declarations
		private Nullable<System.Decimal>	_actualCost;
		private Nullable<System.DateTime>	_actualEndDate;
		private Nullable<System.Decimal>	_actualResourceHrs;
		private Nullable<System.DateTime>	_actualStartDate;
		private System.Int16	_locationId;
		private System.DateTime	_modifiedDate;
		private System.Int16	_operationSequence;
		private System.Decimal	_plannedCost;
		private System.Int32	_productId;
		private System.DateTime	_scheduledEndDate;
		private System.DateTime	_scheduledStartDate;
		private System.Int32	_workOrderId;
		private EntityRef <Location> _location;
		private EntityRef <WorkOrder> _workOrder;
		#endregion
		
		#region Extensibility Method Definitions
		partial void OnLoaded();
		partial void OnValidate(System.Data.Linq.ChangeAction action);
		partial void OnCreated();
		partial void OnActualCostChanging(Nullable<System.Decimal> value);
		partial void OnActualCostChanged();
		partial void OnActualEndDateChanging(Nullable<System.DateTime> value);
		partial void OnActualEndDateChanged();
		partial void OnActualResourceHrsChanging(Nullable<System.Decimal> value);
		partial void OnActualResourceHrsChanged();
		partial void OnActualStartDateChanging(Nullable<System.DateTime> value);
		partial void OnActualStartDateChanged();
		partial void OnLocationIdChanging(System.Int16 value);
		partial void OnLocationIdChanged();
		partial void OnModifiedDateChanging(System.DateTime value);
		partial void OnModifiedDateChanged();
		partial void OnOperationSequenceChanging(System.Int16 value);
		partial void OnOperationSequenceChanged();
		partial void OnPlannedCostChanging(System.Decimal value);
		partial void OnPlannedCostChanged();
		partial void OnProductIdChanging(System.Int32 value);
		partial void OnProductIdChanged();
		partial void OnScheduledEndDateChanging(System.DateTime value);
		partial void OnScheduledEndDateChanged();
		partial void OnScheduledStartDateChanging(System.DateTime value);
		partial void OnScheduledStartDateChanged();
		partial void OnWorkOrderIdChanging(System.Int32 value);
		partial void OnWorkOrderIdChanged();
		#endregion
		
		/// <summary>Initializes a new instance of the <see cref="WorkOrderRouting"/> class.</summary>
		public WorkOrderRouting()
		{
			_location = default(EntityRef<Location>);
			_workOrder = default(EntityRef<WorkOrder>);
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
		/// <summary>Gets or sets the ActualCost field. Mapped on target field 'ActualCost'. </summary>
		public Nullable<System.Decimal> ActualCost
		{
			get	{ return _actualCost; }
			set
			{
				if((_actualCost != value))
				{
					OnActualCostChanging(value);
					SendPropertyChanging("ActualCost");
					_actualCost = value;
					SendPropertyChanged("ActualCost");
					OnActualCostChanged();
				}
			}
		}

		/// <summary>Gets or sets the ActualEndDate field. Mapped on target field 'ActualEndDate'. </summary>
		public Nullable<System.DateTime> ActualEndDate
		{
			get	{ return _actualEndDate; }
			set
			{
				if((_actualEndDate != value))
				{
					OnActualEndDateChanging(value);
					SendPropertyChanging("ActualEndDate");
					_actualEndDate = value;
					SendPropertyChanged("ActualEndDate");
					OnActualEndDateChanged();
				}
			}
		}

		/// <summary>Gets or sets the ActualResourceHrs field. Mapped on target field 'ActualResourceHrs'. </summary>
		public Nullable<System.Decimal> ActualResourceHrs
		{
			get	{ return _actualResourceHrs; }
			set
			{
				if((_actualResourceHrs != value))
				{
					OnActualResourceHrsChanging(value);
					SendPropertyChanging("ActualResourceHrs");
					_actualResourceHrs = value;
					SendPropertyChanged("ActualResourceHrs");
					OnActualResourceHrsChanged();
				}
			}
		}

		/// <summary>Gets or sets the ActualStartDate field. Mapped on target field 'ActualStartDate'. </summary>
		public Nullable<System.DateTime> ActualStartDate
		{
			get	{ return _actualStartDate; }
			set
			{
				if((_actualStartDate != value))
				{
					OnActualStartDateChanging(value);
					SendPropertyChanging("ActualStartDate");
					_actualStartDate = value;
					SendPropertyChanged("ActualStartDate");
					OnActualStartDateChanged();
				}
			}
		}

		/// <summary>Gets or sets the LocationId field. Mapped on target field 'LocationID'. </summary>
		public System.Int16 LocationId
		{
			get	{ return _locationId; }
			set
			{
				if((_locationId != value))
				{
					if(_location.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					OnLocationIdChanging(value);
					SendPropertyChanging("LocationId");
					_locationId = value;
					SendPropertyChanged("LocationId");
					OnLocationIdChanged();
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

		/// <summary>Gets or sets the OperationSequence field. Mapped on target field 'OperationSequence'. </summary>
		public System.Int16 OperationSequence
		{
			get	{ return _operationSequence; }
			set
			{
				if((_operationSequence != value))
				{
					OnOperationSequenceChanging(value);
					SendPropertyChanging("OperationSequence");
					_operationSequence = value;
					SendPropertyChanged("OperationSequence");
					OnOperationSequenceChanged();
				}
			}
		}

		/// <summary>Gets or sets the PlannedCost field. Mapped on target field 'PlannedCost'. </summary>
		public System.Decimal PlannedCost
		{
			get	{ return _plannedCost; }
			set
			{
				if((_plannedCost != value))
				{
					OnPlannedCostChanging(value);
					SendPropertyChanging("PlannedCost");
					_plannedCost = value;
					SendPropertyChanged("PlannedCost");
					OnPlannedCostChanged();
				}
			}
		}

		/// <summary>Gets or sets the ProductId field. Mapped on target field 'ProductID'. </summary>
		public System.Int32 ProductId
		{
			get	{ return _productId; }
			set
			{
				if((_productId != value))
				{
					OnProductIdChanging(value);
					SendPropertyChanging("ProductId");
					_productId = value;
					SendPropertyChanged("ProductId");
					OnProductIdChanged();
				}
			}
		}

		/// <summary>Gets or sets the ScheduledEndDate field. Mapped on target field 'ScheduledEndDate'. </summary>
		public System.DateTime ScheduledEndDate
		{
			get	{ return _scheduledEndDate; }
			set
			{
				if((_scheduledEndDate != value))
				{
					OnScheduledEndDateChanging(value);
					SendPropertyChanging("ScheduledEndDate");
					_scheduledEndDate = value;
					SendPropertyChanged("ScheduledEndDate");
					OnScheduledEndDateChanged();
				}
			}
		}

		/// <summary>Gets or sets the ScheduledStartDate field. Mapped on target field 'ScheduledStartDate'. </summary>
		public System.DateTime ScheduledStartDate
		{
			get	{ return _scheduledStartDate; }
			set
			{
				if((_scheduledStartDate != value))
				{
					OnScheduledStartDateChanging(value);
					SendPropertyChanging("ScheduledStartDate");
					_scheduledStartDate = value;
					SendPropertyChanged("ScheduledStartDate");
					OnScheduledStartDateChanged();
				}
			}
		}

		/// <summary>Gets or sets the WorkOrderId field. Mapped on target field 'WorkOrderID'. </summary>
		public System.Int32 WorkOrderId
		{
			get	{ return _workOrderId; }
			set
			{
				if((_workOrderId != value))
				{
					if(_workOrder.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					OnWorkOrderIdChanging(value);
					SendPropertyChanging("WorkOrderId");
					_workOrderId = value;
					SendPropertyChanged("WorkOrderId");
					OnWorkOrderIdChanged();
				}
			}
		}

		/// <summary>Represents the navigator which is mapped onto the association 'WorkOrderRouting.Location - Location.WorkOrderRoutings (m:1)'</summary>
		public Location Location
		{
			get { return _location.Entity; }
			set
			{
				Location previousValue = _location.Entity;
				if((previousValue != value) || (_location.HasLoadedOrAssignedValue == false))
				{
					this.SendPropertyChanging("Location");
					if(previousValue != null)
					{
						_location.Entity = null;
						previousValue.WorkOrderRoutings.Remove(this);
					}
					_location.Entity = value;
					if(value == null)
					{
						_locationId = default(System.Int16);
					}
					else
					{
						value.WorkOrderRoutings.Add(this);
						_locationId = value.LocationId;
					}
					this.SendPropertyChanged("Location");
				}
			}
		}
		
		/// <summary>Represents the navigator which is mapped onto the association 'WorkOrderRouting.WorkOrder - WorkOrder.WorkOrderRoutings (m:1)'</summary>
		public WorkOrder WorkOrder
		{
			get { return _workOrder.Entity; }
			set
			{
				WorkOrder previousValue = _workOrder.Entity;
				if((previousValue != value) || (_workOrder.HasLoadedOrAssignedValue == false))
				{
					this.SendPropertyChanging("WorkOrder");
					if(previousValue != null)
					{
						_workOrder.Entity = null;
						previousValue.WorkOrderRoutings.Remove(this);
					}
					_workOrder.Entity = value;
					if(value == null)
					{
						_workOrderId = default(System.Int32);
					}
					else
					{
						value.WorkOrderRoutings.Add(this);
						_workOrderId = value.WorkOrderId;
					}
					this.SendPropertyChanged("WorkOrder");
				}
			}
		}
		
		#endregion
	}
}
#pragma warning restore 0649