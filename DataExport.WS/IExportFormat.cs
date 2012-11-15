using System.Data;

namespace DataExport.WS.Controllers
{
	public interface IExportFormat
	{
		string Export(DataSet ds);
	}
}