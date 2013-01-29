using System;
using System.Data;
using Common.Logging;
using CtcApi;

namespace DataExport.WS.Controllers
{
	public class Exporter : IExporter
	{
		private ILog _log = LogManager.GetCurrentClassLogger();

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
			_log.Debug(m => m("Importing data via [{0}]...", Data));
			DataSet ds = Data.Import();

			Format.Context = Context;
			_log.Debug(m => m("Applying formatting via [{0}]...", Format));
			string text = Format.Serialize(ds);

			// Transmit the result to the specified destination 
			Deliver.Context = Context;
			Deliver.Source = DeliveryBase.ConvertToSource(text);
			_log.Info(m => m("Transmitting exported data via [{0}]...", Deliver));
			Deliver.Put();

			// return the text to the caller for troubleshooting, etc.
			return text;
		}
	}
}