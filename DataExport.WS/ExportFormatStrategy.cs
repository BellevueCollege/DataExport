using System.Data;
using CtcApi;

namespace DataExport.WS
{
    public abstract class ExportFormatStrategy
    {
        public abstract string Serialize(DataSet ds);
        public abstract ApplicationContext Context { get; set; }
    }
}