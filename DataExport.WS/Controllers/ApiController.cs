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

		#region Initialization
		/// <summary>
		/// 
		/// </summary>
		/// <remarks>
		/// This constructor initializes the <see cref="BaseController"/>, which provides common
		/// functionality like populating <i>ViewBag.Version</i> with the <see cref="Version"/>
		/// of this MVC application.
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
  }
}
