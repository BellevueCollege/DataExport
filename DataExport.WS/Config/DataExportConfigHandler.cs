using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace DataExport.WS.Config
{
    internal class DataExportConfigHandler : IConfigurationSectionHandler
    {
        /// <summary>
        /// Initializes a new <see cref="DataExportConfig"/> object from the .config file.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="configContext"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public object Create(object parent, object configContext, XmlNode section)
        {
            DataExportConfig result = null;
            if (section == null)
                return result;
            XmlSerializer ser = new XmlSerializer(typeof(DataExportConfig));

            using (XmlNodeReader reader = new XmlNodeReader(section))
            {
                result = (DataExportConfig)ser.Deserialize(reader);

                return result;
            }
        }
    }
}