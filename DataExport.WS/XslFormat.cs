using System;
using System.Data;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Security.Principal;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.Xsl;
using Common.Logging;
using CtcApi;

namespace DataExport.WS.Config
{
	[XmlType("xsl")]
	public class XslFormat : ExportFormatStrategy
	{
		protected const string TEMPLATE_FOLDER = "Templates";
		private ILog _log = LogManager.GetCurrentClassLogger();
	  private string _contextUser;

	  public override ApplicationContext Context {get;set;}

		#region .config properties
		/// <summary>
		/// 
		/// </summary>
		[XmlAttribute("template")]
		public string Template { get; set; }

		/// <summary>
		/// 
		/// </summary>
    [XmlAttribute("templateFile")]
		public string TemplateFile { get; set; }

		#endregion

		public XslFormat()
		{
			// set default values
			Template = string.Empty;
			TemplateFile = string.Empty;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ds"></param>
		/// <returns></returns>
		public override string Serialize(DataSet ds)
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
			string xsltFilename = Path.IsPathRooted(TemplateFile) ? TemplateFile : Path.Combine(Context.BaseDirectory, TEMPLATE_FOLDER, TemplateFile);

		  try
		  {
		    // open template file for reading while sharing it with other processes.
		    using (FileStream fs = new FileStream(xsltFilename, FileMode.Open, FileAccess.Read, FileShare.Read))
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
		  catch (Exception exception)
		  {
		    _log.Error(m => m("User context '{0}' is unable to access template file '{1}'.", ContextUser, xsltFilename));
		    throw;
		  }
		}

    /// <summary>
    /// 
    /// </summary>
    /// TODO: Move ContextUser functionality into <see cref="CtcApi.ApplicationContext"/>.
	  protected string ContextUser
	  {
	    get
	    {
        if (string.IsNullOrWhiteSpace(_contextUser))
        {
          WindowsIdentity wic = WindowsIdentity.GetCurrent();
          if (wic != null)
          {
            _contextUser = wic.Name;
          }
          else
          {
            UserPrincipal principal = UserPrincipal.Current;

            if (!string.IsNullOrWhiteSpace(principal.UserPrincipalName))
            {
              _contextUser = principal.UserPrincipalName;
            }
            else
            {
              _contextUser = "*UNKNOWN*";
            }

          }
        }

	      return _contextUser;
	    }
	  }
	}
}