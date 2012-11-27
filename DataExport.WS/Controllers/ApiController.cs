using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Common.Logging;
using CtcApi.Extensions;
using CtcApi.Web.Mvc;

namespace DataExport.WS.Controllers
{
	/// <summary>
	/// 
	/// </summary>
  public class ApiController : BaseController
  {
		private ILog _log = LogManager.GetCurrentClassLogger();
		// (ExporterConfig)ConfigurationManager.GetSection(ExporterConfig.GetSectionName())
		// TODO: replace the following initializations with loading from config settings (above)
		private IList<IExporter> _exporters = new List<IExporter>
		                                      	{
		                                      			new Exporter
		                                      				{
		                                      						Name = "maxient",
		                                      						Data = (new SqlDataInput()),
																											Format = new CsvFormat {Separator = "|"}
		                                      				}
		                                      	};

		#region Initialization
		/// <summary>
		/// 
		/// </summary>
		/// <remarks>
		/// This constructor initializes the <see cref="BaseController"/>, which provides common
		/// functionality like populating <i>ViewBag.Version</i> with the <see cref="Version"/>
		/// of the current MVC application.
		/// </remarks>
		public ApiController() : base(Assembly.GetExecutingAssembly())
		{
		}
		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
    public ActionResult Index()
    {
			_log.Trace(m => m("=> ApiController::Index()"));

      return View();
    }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ActionResult Export(string id)
		{
			IEnumerable<IExporter> e =  _exporters.Where(x => x.Name.ToUpper() == id.ToUpper());

			if (e.Count() > 0)
			{
				IExporter exporter = _exporters.Take(1).Single();

				string csv = exporter.Export();
				_log.Trace(m => m("Export result:\n{0}", csv));

				// TODO: add config settings for saving file
				csv.ToFile("output.txt");

				// TODO: implement uploading via SSH

				if (string.IsNullOrWhiteSpace(csv))
				{
					_log.Warn(m => m("Exporter '{0}' returned an empty string.", id));
				}
				else
				{
					return View("ExportResult", true);
				}
			}
			else
			{
				_log.Warn(m => m("No exporter was found for '{0}'", id));
			}
			return View("ExportResult", false);
		}
  }
}
