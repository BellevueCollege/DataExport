using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataExport.WS.Controllers;

namespace DataExport.WS.Config
{
	public class DataExportConfig
	{
		public ICollection<IExporter> Exporters {get;set;}
	}
}