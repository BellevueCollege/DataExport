using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DataExport.Web.Properties;

namespace DataExport.WS
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : HttpApplication
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
					"Default", // Route name
					"{controller}/{action}/{id}", // URL with parameters
					new { controller = "Api", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);

		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);
		}
	}

	/// <summary>
	/// Provides common application methods, properties, etc.
	/// </summary>
	internal static class Global
	{
		/// <summary>
		/// Gets the current <see cref="DateTime"/>, or the override value configured in <see cref="Settings"/>
		/// </summary>
		/// <returns></returns>
		static public DateTime CurrentDateTime()
		{
			DateTime today;
			if (!DateTime.TryParse(Settings.Default.OverrideCurrentDate, out today))
			{
				today = DateTime.Now;
			}
			return today;
		}
	}
}