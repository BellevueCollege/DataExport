using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using DataExport.WS.Controllers;

namespace DataExport.WS.Config
{
	[XmlType("dataExport")]
	public class DataExportConfig
	{
		[XmlArrayAttribute("exporters")]
		public ICollection<IExporter> Exporters {get;set;}
	}
}