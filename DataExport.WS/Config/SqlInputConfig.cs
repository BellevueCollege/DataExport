using System.Configuration;

namespace DataExport.WS.Config
{
	public class SqlInputConfig
	{
		/// <summary>
		/// 
		/// </summary>
		public string ConnectionName{get;set;}

		/// <summary>
		/// 
		/// </summary>
		public string ConnectionString {get;set;}

		/// <summary>
		/// 
		/// </summary>
		public string ProviderType{get;set;}

		/// <summary>
		/// 
		/// </summary>
		public string CmdText {get;set;}

		/// <summary>
		/// 
		/// </summary>
		public int Timeout{get;set;}

		public SqlInputConfig()
		{
			Timeout = 60;
			ConnectionName = "Default";
			ProviderType = "System.Data.SqlClient";
			ConnectionString = ConfigurationManager.ConnectionStrings[ConnectionName].ConnectionString;
		}
	}
}