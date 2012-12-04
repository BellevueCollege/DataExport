using System.Data;
using CtcApi;

namespace DataExport.WS
{
	public interface IExportFormat
	{
		string Serialize(DataSet ds);
		ApplicationContext Context{get;set;}
	}
}