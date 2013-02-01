using System.Data;

namespace DataExport.WS.Controllers
{
    public abstract class DataInputStrategy
    {
        public abstract DataSet Import();
    }
}