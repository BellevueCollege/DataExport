using CtcApi;
using DataExport.WS.Controllers;

namespace DataExport.WS
{
	public interface IExporter
	{
		IExportFormat Format { get;set; }
		string Name { get; set; }
		IDataInput Data { get;set; }
		ApplicationContext Context{get;set;}
		IDeliveryStrategy Deliver{get;set;}
		string Export();
	}
}