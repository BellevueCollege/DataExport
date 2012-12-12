using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataExport.WS.Controllers;

namespace DataExport.Web
{
	public class DataExportConfig
	{
		public ICollection<IExporter> Exporters {get;set;}
	}
}