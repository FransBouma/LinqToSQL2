using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Reflection;
using System.Linq;
using System.Diagnostics;

namespace System.Data.Linq
{
	using System.Data.Linq.Mapping;
	using System.Data.Linq.Provider;
	using System.Diagnostics.CodeAnalysis;

	internal sealed class ChangeConflictSession
	{
		private DataContext context;
		private DataContext refreshContext;

		internal ChangeConflictSession(DataContext context)
		{
			this.context = context;
		}

		internal DataContext Context
		{
			get { return this.context; }
		}

		internal DataContext RefreshContext
		{
			get
			{
				if(this.refreshContext == null)
				{
					this.refreshContext = this.context.CreateRefreshContext();
				}
				return this.refreshContext;
			}
		}
	}
}

