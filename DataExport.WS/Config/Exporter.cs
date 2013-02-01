using System;
using System.Data;
using System.Xml.Serialization;
using Common.Logging;
using CtcApi;
using DataExport.WS.Controllers;

namespace DataExport.WS.Config
{
	[XmlType("exporter")]
	public class Exporter
	{
		private ILog _log = LogManager.GetCurrentClassLogger();

		public ApplicationContext Context {get;set;}

		[XmlAttribute("name")]
		public string Name {get;set;}

        [XmlElement("csvFormat", typeof(CsvFormat))]
        [XmlElement("xslFormat", typeof(XslFormat))]
		public ExportFormatStrategy Format {get;set;}

        [XmlElement("sqlInput", typeof(SqlDataInput))]
		public DataInputStrategy Data {get;set;}

        [XmlElement("fileDelivery", typeof(FilePathDelivery))]
        [XmlElement("sftpDelivery", typeof(SftpDelivery))]
		public DeliveryStrategy Deliver {get;set;}

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
			Deliver.Source = DeliveryStrategy.ConvertToSource(text);
			_log.Info(m => m("Transmitting exported data via [{0}]...", Deliver));
			Deliver.Put();

			// return the text to the caller for troubleshooting, etc.
			return text;
		}
	}
}