using System.IO;
using CtcApi;
using DataExport.WS.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using System.Web.Mvc;

namespace Test.DataExport.WS
{
	[TestClass()]
	public class ApiControllerTests
	{

		#region Additional test attributes

		private TestContext testContextInstance;

		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion

		#region Tests
		[TestMethod()]
		public void TestIndex()
		{
			ApiController target = InitializeController();
			ViewResult actual = target.Index() as ViewResult;
			Assert.IsNotNull(actual);
		}

	  #region Maxient export tests
    // WARNING: These tests connect to a live, production SSH server
    // These tests also rely on properly-configured <exporter> blocks.

	  [TestMethod]
	  [Ignore] // This test connects to a live, production SSH server
	  public void TestExport_Maxient_Feed1_Demographics()
	  {
	    ApiController api = InitializeController();
	    ViewResult view = api.Export("maxient1") as ViewResult;
	    Assert.IsNotNull(view);

	    object model = view.Model;
	    Assert.IsNotNull(model);

	    bool result = Boolean.Parse(model.ToString());
	    Assert.IsTrue(result, "Export call reports a failure.");
	  }

	  [TestMethod]
	  [Ignore] // This test connects to a live, production SSH server
	  public void TestExport_Maxient_Feed2_Schedule()
	  {
	    ApiController api = InitializeController();
	    ViewResult view = api.Export("maxient2") as ViewResult;
	    Assert.IsNotNull(view);

	    object model = view.Model;
	    Assert.IsNotNull(model);

	    bool result = Boolean.Parse(model.ToString());
	    Assert.IsTrue(result, "Export call reports a failure.");
	  }

	  #endregion

		#endregion

		#region Helper methods
		private ApiController InitializeController()
		{
			ApplicationContext context = new ApplicationContext
			                             	{
			                             			BaseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\DataExport.WS")
			                             	};
			return new ApiController(context);
		}

		#endregion
	}
}
