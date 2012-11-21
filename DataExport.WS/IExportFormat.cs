using System.Data;

namespace DataExport.WS
{
	public interface IExportFormat
	{
		string Serialize(DataSet ds);
	}
}