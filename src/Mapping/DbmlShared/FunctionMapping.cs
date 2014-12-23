using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Globalization;

namespace LinqToSqlShared.Mapping
{
	/// <summary>
	/// DatabaseMapping and related classes represent a parsed version of the
	/// XML mapping string. This unvalidated intermediate representation is 
	/// necessary because unused mappings are intentially never validated.
	/// </summary>
	internal class FunctionMapping
	{
		string name;
		string methodName;
		bool isComposable;
		List<ParameterMapping> parameters;
		List<TypeMapping> types;
		ReturnMapping funReturn;

		internal FunctionMapping()
		{
			this.parameters = new List<ParameterMapping>();
			this.types = new List<TypeMapping>();
		}

		internal string Name
		{
			get { return this.name; }
			set { this.name = value; }
		}

		internal string MethodName
		{
			get { return this.methodName; }
			set { this.methodName = value; }
		}

		internal bool IsComposable
		{
			get { return this.isComposable; }
			set { this.isComposable = value; }
		}

		internal string XmlIsComposable
		{
			get { return this.isComposable ? XmlMappingConstant.True : null; }
			set { this.isComposable = (value != null) ? bool.Parse(value) : false; }
		}

		internal List<ParameterMapping> Parameters
		{
			get { return this.parameters; }
		}

		internal List<TypeMapping> Types
		{
			get { return this.types; }
		}

		internal ReturnMapping FunReturn
		{
			get { return this.funReturn; }
			set { this.funReturn = value; }
		}
	}
}

