using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Common.Logging;
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
		private IList<IExporter> _exporters = new List<IExporter>()
		                                      	{
		                                      			new Exporter()
		                                      				{
		                                      						Name = "maxient",
		                                      						Data = (new SqlDataInput()),
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
			IEnumerable<IExporter> e =  _exporters.Where(x => x.Name == id);

			if (e.Count() > 0)
			{
				IExporter exporter = _exporters.Take(1).Single();

				string data = exporter.Export();

				if (string.IsNullOrWhiteSpace(data))
				{
					_log.Warn(m => m("Exporter '{0}' returned an empty string.", id));
				}
				else
				{
					// TODO: display an appropriate success response
					return View("ExportResult", model: id);
				}
			}
			else
			{
				_log.Warn(m => m("No exporter was found for '{0}'", id));
			}
			// TODO: display an appropriate failure response
			return View("ExportResult", model: string.Empty);
		}
  }
}
