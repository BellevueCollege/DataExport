using System;
using System.Data;
using CtcApi;

namespace DataExport.WS.Controllers
{
	public class Exporter : IExporter
	{
		public IExportFormat Format {get;set;}

		public string Name {get;set;}

		public IDataInput Data {get;set;}

		public ApplicationContext Context {get;set;}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string Export()
		{
			DataSet ds = Data.Import();

			Format.Context = Context;
			return Format.Serialize(ds);
		}
	}
}