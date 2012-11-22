using System;
using System.Data;

namespace DataExport.WS.Controllers
{
	public class Exporter : IExporter
	{
		public IExportFormat Format {get;set;}

		public string Name {get;set;}

		public IDataInput Data {get;set;}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string Export()
		{
			DataSet ds = Data.Import();
			return Format.Serialize(ds);
		}
	}
}