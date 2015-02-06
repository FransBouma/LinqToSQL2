namespace System.Data.Linq.Provider.Common
{
	internal struct NamedColumn
	{
		string name;
		bool isRequired;
		internal NamedColumn(string name, bool isRequired)
		{
			this.name = name;
			this.isRequired = isRequired;
		}
		internal string Name
		{
			get { return this.name; }
		}
		internal bool IsRequired
		{
			get { return this.isRequired; }
		}
	}
}