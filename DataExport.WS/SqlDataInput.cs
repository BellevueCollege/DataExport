using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using DataExport.WS.Config;
using DataExport.WS.Controllers;

namespace DataExport.WS
{
	public class SqlDataInput : SqlInputConfig, IDataInput
	{
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public DataSet Import()
		{
			DataSet ds = new DataSet();

			DbProviderFactory factory = DbProviderFactories.GetFactory(ProviderType);
			
			using (DbConnection conn = factory.CreateConnection())
			{
			  if (conn != null)
			  {
			    conn.ConnectionString = ConnectionString;

			    using (DbCommand cmd = conn.CreateCommand())
			    {
			      cmd.CommandText = CmdText;
						// TODO: make the database time out configurable. Perhaps per-DataInput?
			    	cmd.CommandTimeout = Timeout;

			      IDbDataAdapter adapter = factory.CreateDataAdapter();
				
			      if (adapter != null)
			      {
			        adapter.SelectCommand = cmd;
			        adapter.Fill(ds);
			      }
			    }
			  }
			}
			
			return ds;
		}
	}
}