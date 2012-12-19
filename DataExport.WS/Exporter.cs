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

		public IDeliveryStrategy Deliver {get;set;}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string Export()
		{
			DataSet ds = Data.Import();

			Format.Context = Context;
			string text = Format.Serialize(ds);

			// Transmit the result to the specified destination 
			Deliver.Context = Context;
			Deliver.Source = DeliveryBase.ConvertToSource(text);
			Deliver.Put();

			// return the text to the caller for troubleshooting, etc.
			return text;
		}
	}
}