using System;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using Microsoft.Win32.SafeHandles;

namespace DataExport.WS.Config
{
	public class XslFormatConfig
	{
		protected const string TEMPLATE_FOLDER = "Templates";

		/// <summary>
		/// 
		/// </summary>
		public string Template {get;set;}

		/// <summary>
		/// 
		/// </summary>
		public string TemplateFile {get;set;}
	}
}