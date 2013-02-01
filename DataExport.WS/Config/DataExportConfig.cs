using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using DataExport.WS.Controllers;

namespace DataExport.WS.Config
{
	[XmlType("dataExport")]
    public class DataExportConfig : CtcApi.Config.CtcConfigBase<DataExportConfig>
	{
		[XmlArrayAttribute("exporters")]
		public List<Exporter> Exporters {get;set;}
	}
}