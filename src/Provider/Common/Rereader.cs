using System.Collections;
using System.Data.Common;

namespace System.Data.Linq.Provider.Common
{
	internal class Rereader : DbDataReader, IDisposable
	{
		#region Member Declarations
		private bool first;
		private DbDataReader reader;
		private string[] names;
		#endregion

		internal Rereader(DbDataReader reader, bool hasCurrentRow, string[] names)
		{
			this.reader = reader;
			this.first = hasCurrentRow;
			this.names = names;
		}

		public override bool Read()
		{
			if(this.first)
			{
				this.first = false;
				return true;
			}
			return this.reader.Read();
		}

		public override string GetName(int i)
		{
			if(this.names != null)
			{
				return this.names[i];
			}
			return reader.GetName(i);
		}

		public override void Close() { }
		public override bool NextResult() { return false; }

		public override int Depth { get { return reader.Depth; } }
		public override bool IsClosed { get { return reader.IsClosed; } }
		public override int RecordsAffected { get { return reader.RecordsAffected; } }
		public override DataTable GetSchemaTable() { return reader.GetSchemaTable(); }

		public override int FieldCount { get { return reader.FieldCount; } }
		public override object this[int i] { get { return reader[i]; } }
		public override object this[string name] { get { return reader[name]; } }
		public override bool GetBoolean(int i) { return reader.GetBoolean(i); }
		public override byte GetByte(int i) { return reader.GetByte(i); }
		public override long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferOffset, int length) { return reader.GetBytes(i, fieldOffset, buffer, bufferOffset, length); }
		public override char GetChar(int i) { return reader.GetChar(i); }
		public override long GetChars(int i, long fieldOffset, char[] buffer, int bufferOffset, int length) { return reader.GetChars(i, fieldOffset, buffer, bufferOffset, length); }
		public override string GetDataTypeName(int i) { return reader.GetDataTypeName(i); }
		public override DateTime GetDateTime(int i) { return reader.GetDateTime(i); }
		public override decimal GetDecimal(int i) { return reader.GetDecimal(i); }
		public override double GetDouble(int i) { return reader.GetDouble(i); }
		public override Type GetFieldType(int i) { return reader.GetFieldType(i); }
		public override float GetFloat(int i) { return reader.GetFloat(i); }
		public override Guid GetGuid(int i) { return reader.GetGuid(i); }
		public override short GetInt16(int i) { return reader.GetInt16(i); }
		public override int GetInt32(int i) { return reader.GetInt32(i); }
		public override long GetInt64(int i) { return reader.GetInt64(i); }
		public override int GetOrdinal(string name) { return reader.GetOrdinal(name); }
		public override string GetString(int i) { return reader.GetString(i); }
		public override object GetValue(int i) { return reader.GetValue(i); }
		public override int GetValues(object[] values) { return reader.GetValues(values); }
		public override bool IsDBNull(int i) { return reader.IsDBNull(i); }

		public override IEnumerator GetEnumerator()
		{
			return this.reader.GetEnumerator();
		}
		public override bool HasRows
		{
			get { return this.first || this.reader.HasRows; }
		}
	}
}