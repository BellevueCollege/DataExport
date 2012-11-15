using System.Data;

namespace DataExport.WS.Controllers
{
	public interface IDataInput
	{
		DataSet Import();
	}
}