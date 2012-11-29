using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Xsl;
using Common.Logging;

namespace DataExport.WS
{
	public class XslFormat : XslFormatConfig, IExportFormat
	{
		private ILog _log = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ds"></param>
		/// <returns></returns>
		public string Serialize(DataSet ds)
		{
			XmlDocument xml = new XmlDocument();

			try
			{
				xml.LoadXml(ds.GetXml());
			}
			catch (Exception ex)
			{
				_log.Error(m => m("Failed to convert DataSet to XML.\n{0}", ex));
			}
			
			string formattedString = ApplyTransform(xml);
			return formattedString;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="xmlDoc"></param>
		/// <returns></returns>
		private string ApplyTransform(XmlDocument xmlDoc)
		{
			try
			{
				XslCompiledTransform xslt = LoadXslTempate();

				using (StringWriter output = new StringWriter())
				{
					xslt.Transform(xmlDoc.CreateNavigator(), null, output);
					return output.ToString();
				}
			}
			catch (XmlSchemaValidationException ex)
			{
				_log.Error(m => m("{0}: XSLT is not valid.\n{1}", TemplateFile, ex));
			}
			catch (XsltException ex)
			{
				_log.Error(m => m("{0}: XSLT is not valid.\n{1}", TemplateFile, ex));
			}
			catch (XmlException ex)
			{
				_log.Error(m => m("{0}: XML is not valid.\n{1}", TemplateFile, ex));
			}
			catch (XmlSchemaException ex)
			{
				_log.Error(m => m("{0}: Schema is not valid.\n{1}", TemplateFile, ex));
			}
			catch (ArgumentNullException ex)
			{
				_log.Error(m => m("{0}: XSLT is blank.\n{1}", TemplateFile, ex));
			}

			_log.Warn(m => m("XML transform returning an empty string."));
			return string.Empty;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private XslCompiledTransform LoadXslTempate()
		{
			string xsltFilename = Path.IsPathRooted(TemplateFile) ? TemplateFile : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TEMPLATE_FOLDER, TemplateFile);

			using (FileStream fs = new FileStream(xsltFilename, FileMode.Open, FileAccess.Read))
			{
				XslCompiledTransform xslt = new XslCompiledTransform();
				XmlReaderSettings xmlsettings = new XmlReaderSettings
				                                	{
				                                			ConformanceLevel = ConformanceLevel.Document
				                                	};

				XmlReader reader = XmlReader.Create(fs, xmlsettings);
				xslt.Load(reader);
			
				return xslt;
			}
		}
	}
}