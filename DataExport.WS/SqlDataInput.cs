using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using DataExport.WS.Controllers;

namespace DataExport.WS
{
	public class SqlDataInput : SqlInputConfig, IDataInput
	{
		/// <summary>
		/// 
		/// </summary>
		public SqlDataInput()
		{
			// TODO: replace these initialization values with .config properties
			Connection = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public DataSet Import()
		{
			DataSet ds = new DataSet();

			// TODO: make factory type configurable
			// - either pull from connection strings, or specify explicitly. Fall back on System.Data.SqlClient.
			DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
			
			using (DbConnection conn = factory.CreateConnection())
			{
			  if (conn != null)
			  {
			    conn.ConnectionString = Connection;

			    using (DbCommand cmd = conn.CreateCommand())
			    {
			      cmd.CommandText = CmdText;

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