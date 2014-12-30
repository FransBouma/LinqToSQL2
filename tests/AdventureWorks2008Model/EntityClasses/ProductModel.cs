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
	/// <summary>Class which represents the entity 'ProductModel', mapped on table 'AdventureWorks.Production.ProductModel'.</summary>
	public partial class ProductModel : INotifyPropertyChanging, INotifyPropertyChanged
	{
		#region Events
		/// <summary>Event which is raised when a property value is changing.</summary>
		public event PropertyChangingEventHandler PropertyChanging;
		/// <summary>Event which is raised when a property value changes.</summary>
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion
		
		#region Class Member Declarations
		private System.String	_catalogDescription;
		private System.String	_instructions;
		private System.DateTime	_modifiedDate;
		private System.String	_name;
		private System.Int32	_productModelId;
		private System.Guid	_rowguid;
		private EntitySet <Product> _products;
		private EntitySet <ProductModelIllustration> _productModelIllustrations;
		private EntitySet <ProductModelProductDescriptionCulture> _productModelProductDescriptionCultures;
		#endregion
		
		#region Extensibility Method Definitions
		partial void OnLoaded();
		partial void OnValidate(System.Data.Linq.ChangeAction action);
		partial void OnCreated();
		partial void OnCatalogDescriptionChanging(System.String value);
		partial void OnCatalogDescriptionChanged();
		partial void OnInstructionsChanging(System.String value);
		partial void OnInstructionsChanged();
		partial void OnModifiedDateChanging(System.DateTime value);
		partial void OnModifiedDateChanged();
		partial void OnNameChanging(System.String value);
		partial void OnNameChanged();
		partial void OnProductModelIdChanging(System.Int32 value);
		partial void OnProductModelIdChanged();
		partial void OnRowguidChanging(System.Guid value);
		partial void OnRowguidChanged();
		#endregion
		
		/// <summary>Initializes a new instance of the <see cref="ProductModel"/> class.</summary>
		public ProductModel()
		{
			_products = new EntitySet<Product>(new Action<Product>(this.Attach_Products), new Action<Product>(this.Detach_Products) );
			_productModelIllustrations = new EntitySet<ProductModelIllustration>(new Action<ProductModelIllustration>(this.Attach_ProductModelIllustrations), new Action<ProductModelIllustration>(this.Detach_ProductModelIllustrations) );
			_productModelProductDescriptionCultures = new EntitySet<ProductModelProductDescriptionCulture>(new Action<ProductModelProductDescriptionCulture>(this.Attach_ProductModelProductDescriptionCultures), new Action<ProductModelProductDescriptionCulture>(this.Detach_ProductModelProductDescriptionCultures) );
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
		private void Attach_Products(Product entity)
		{
			this.SendPropertyChanging("Products");
			entity.ProductModel = this;
		}
		
		/// <summary>Detaches this instance from the entity specified so it's no longer an associated entity</summary>
		/// <param name="entity">The related entity to detach from</param>
		private void Detach_Products(Product entity)
		{
			this.SendPropertyChanging("Products");
			entity.ProductModel = null;
		}

		/// <summary>Attaches this instance to the entity specified as an associated entity</summary>
		/// <param name="entity">The related entity to attach to</param>
		private void Attach_ProductModelIllustrations(ProductModelIllustration entity)
		{
			this.SendPropertyChanging("ProductModelIllustrations");
			entity.ProductModel = this;
		}
		
		/// <summary>Detaches this instance from the entity specified so it's no longer an associated entity</summary>
		/// <param name="entity">The related entity to detach from</param>
		private void Detach_ProductModelIllustrations(ProductModelIllustration entity)
		{
			this.SendPropertyChanging("ProductModelIllustrations");
			entity.ProductModel = null;
		}

		/// <summary>Attaches this instance to the entity specified as an associated entity</summary>
		/// <param name="entity">The related entity to attach to</param>
		private void Attach_ProductModelProductDescriptionCultures(ProductModelProductDescriptionCulture entity)
		{
			this.SendPropertyChanging("ProductModelProductDescriptionCultures");
			entity.ProductModel = this;
		}
		
		/// <summary>Detaches this instance from the entity specified so it's no longer an associated entity</summary>
		/// <param name="entity">The related entity to detach from</param>
		private void Detach_ProductModelProductDescriptionCultures(ProductModelProductDescriptionCulture entity)
		{
			this.SendPropertyChanging("ProductModelProductDescriptionCultures");
			entity.ProductModel = null;
		}


		#region Class Property Declarations
		/// <summary>Gets or sets the CatalogDescription field. Mapped on target field 'CatalogDescription'. </summary>
		public System.String CatalogDescription
		{
			get	{ return _catalogDescription; }
			set
			{
				if((_catalogDescription != value))
				{
					OnCatalogDescriptionChanging(value);
					SendPropertyChanging("CatalogDescription");
					_catalogDescription = value;
					SendPropertyChanged("CatalogDescription");
					OnCatalogDescriptionChanged();
				}
			}
		}

		/// <summary>Gets or sets the Instructions field. Mapped on target field 'Instructions'. </summary>
		public System.String Instructions
		{
			get	{ return _instructions; }
			set
			{
				if((_instructions != value))
				{
					OnInstructionsChanging(value);
					SendPropertyChanging("Instructions");
					_instructions = value;
					SendPropertyChanged("Instructions");
					OnInstructionsChanged();
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

		/// <summary>Gets or sets the Name field. Mapped on target field 'Name'. </summary>
		public System.String Name
		{
			get	{ return _name; }
			set
			{
				if((_name != value))
				{
					OnNameChanging(value);
					SendPropertyChanging("Name");
					_name = value;
					SendPropertyChanged("Name");
					OnNameChanged();
				}
			}
		}

		/// <summary>Gets or sets the ProductModelId field. Mapped on target field 'ProductModelID'. </summary>
		public System.Int32 ProductModelId
		{
			get	{ return _productModelId; }
			set
			{
				if((_productModelId != value))
				{
					OnProductModelIdChanging(value);
					SendPropertyChanging("ProductModelId");
					_productModelId = value;
					SendPropertyChanged("ProductModelId");
					OnProductModelIdChanged();
				}
			}
		}

		/// <summary>Gets or sets the Rowguid field. Mapped on target field 'rowguid'. </summary>
		public System.Guid Rowguid
		{
			get	{ return _rowguid; }
			set
			{
				if((_rowguid != value))
				{
					OnRowguidChanging(value);
					SendPropertyChanging("Rowguid");
					_rowguid = value;
					SendPropertyChanged("Rowguid");
					OnRowguidChanged();
				}
			}
		}

		/// <summary>Represents the navigator which is mapped onto the association 'Product.ProductModel - ProductModel.Products (m:1)'</summary>
		public EntitySet<Product> Products
		{
			get { return this._products; }
			set { this._products.Assign(value); }
		}
		
		/// <summary>Represents the navigator which is mapped onto the association 'ProductModelIllustration.ProductModel - ProductModel.ProductModelIllustrations (m:1)'</summary>
		public EntitySet<ProductModelIllustration> ProductModelIllustrations
		{
			get { return this._productModelIllustrations; }
			set { this._productModelIllustrations.Assign(value); }
		}
		
		/// <summary>Represents the navigator which is mapped onto the association 'ProductModelProductDescriptionCulture.ProductModel - ProductModel.ProductModelProductDescriptionCultures (m:1)'</summary>
		public EntitySet<ProductModelProductDescriptionCulture> ProductModelProductDescriptionCultures
		{
			get { return this._productModelProductDescriptionCultures; }
			set { this._productModelProductDescriptionCultures.Assign(value); }
		}
		
		#endregion
	}
}
#pragma warning restore 0649