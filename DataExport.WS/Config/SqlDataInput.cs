using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Linq;
using Common.Logging;
using DataExport.WS.Config;
using DataExport.WS.Controllers;

namespace DataExport.WS.Config
{
	[XmlType("sql")]
	public class SqlDataInput : DataInputStrategy
	{
		private ILog _log = LogManager.GetCurrentClassLogger();
		private string _connection;

		#region .config properties
		/// <summary>
		/// 
		/// </summary>
		[XmlAttribute("connection")]
		public string Connection
		{
			get { return _connection; }
			set
			{
				_connection = value;

				try
				{
					if (ConfigurationManager.ConnectionStrings != null && ConfigurationManager.ConnectionStrings.Count > 0)
					{
						// connection strings exist in the connectionStrings section of the .config file

						if (ConfigurationManager.ConnectionStrings.Cast<ConnectionStringSettings>().Any(c => c.Name == _connection))
						{
							// found one that matches the Connection specified
							ConnectionName = _connection;
							
							ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[_connection];
							ConnectionString = settings.ConnectionString;

							if (!string.IsNullOrWhiteSpace(settings.ProviderName))
							{
								ProviderType = settings.ProviderName;
							}
						}
						else
						{
							// no connectionString found by that name - assume an actual connection string
							ConnectionString = _connection;
						}

						return;
					}
				}
				catch (Exception ex)
				{
					_log.Warn(m => m("Exception occurred while trying to look up connectionString of '{0}'\n{1}", _connection, ex));
				}

				ConnectionString = _connection;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[XmlAttribute("provider")]
		public string ProviderType { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[XmlAttribute("command")]
		public string CmdText { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[XmlAttribute("timeout")]
		public int Timeout { get; set; }

		#endregion

		/// <summary>
		/// 
		/// </summary>
		public string ConnectionName{get;set;}

		/// <summary>
		/// 
		/// </summary>
		public string ConnectionString {get;set;}

		public SqlDataInput()
		{
			// set default values
			Timeout = 60;
			ConnectionName = "Default";
			ProviderType = "System.Data.SqlClient";
			ConnectionString = ConfigurationManager.ConnectionStrings[ConnectionName].ConnectionString;
		}
	
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override DataSet Import()
		{
			DataSet ds = new DataSet();

			DbProviderFactory factory = DbProviderFactories.GetFactory(ProviderType);

		    try
		    {
		        using (DbConnection conn = factory.CreateConnection())
		        {
		            if (conn != null)
		            {
		                conn.ConnectionString = ConnectionString;

		                using (DbCommand cmd = conn.CreateCommand())
		                {
		                    cmd.CommandText = CmdText;
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
		    }
		    catch (Exception ex)
		    {
		        _log.Error(m => m("There was a problem reading from the database: {0}\n", ex));
		    }

			return ds;
		}
	}
}