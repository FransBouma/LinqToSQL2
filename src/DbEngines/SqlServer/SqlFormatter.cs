using System.Collections.Generic;
using System.Data.Linq.Provider.Common;
using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.DbEngines.SqlServer
{
	/// <summary>
	/// Class which produces SQL Server specific SQL fragments for SqlNode instances.
	/// </summary>
	internal class SqlFormatter : DbFormatter
	{
		#region Member Declarations
		private CommandTextProducer _commandTextProducer;
		#endregion

		internal SqlFormatter()
		{
			this._commandTextProducer = new CommandTextProducer();
		}

		internal override string Format(SqlNode node, bool isDebug)
		{
			return this._commandTextProducer.Format(node, isDebug);
		}

		internal string[] FormatBlock(SqlBlock block, bool isDebug)
		{
			List<string> results = new List<string>(block.Statements.Count);
			for(int i = 0, n = block.Statements.Count; i < n; i++)
			{
				SqlStatement stmt = block.Statements[i];
				SqlSelect select = stmt as SqlSelect;
				if(select != null && select.DoNotOutput)
				{
					continue;
				}
				results.Add(this.Format(stmt, isDebug));
			}
			return results.ToArray();
		}

		internal override string Format(SqlNode node)
		{
			return this._commandTextProducer.Format(node);
		}

		internal bool ParenthesizeTop
		{
			get { return this._commandTextProducer.ParenthesizeTop; }
			set { this._commandTextProducer.ParenthesizeTop = value; }
		}
	}
}
