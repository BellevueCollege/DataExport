using CtcApi;

namespace DataExport.WS.Controllers
{
	public interface IExporter
	{
		IExportFormat Format { get;set; }
		string Name { get; set; }
		IDataInput Data { get;set; }
		ApplicationContext Context{get;set;}
		string Export();
	}
}