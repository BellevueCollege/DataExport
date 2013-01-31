using DataExport.WS;
using DataExport.WS.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using System.Data;

namespace Test.DataExport.WS
{


	[TestClass()]
	public class TestSqlDataInput
	{


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

		#region Additional test attributes
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


		[TestMethod()]
		public void Import_MockData()
		{
			SqlDataInput target = new SqlDataInput();
			target.CmdText = "SELECT 'column1 data' AS Column1, 'column2 data' AS Column2, 10 as Column3";
			DataSet actual = target.Import();

			Assert.IsNotNull(actual);
			Assert.IsTrue(actual.Tables.Count > 0, "DataSet contains no Tables");
			Assert.IsNotNull(actual.Tables[0], "A NULL Table was returned");
			
			int columnCount = actual.Tables[0].Columns.Count;
			Assert.IsTrue(columnCount == 3, "Expected 3 Columns, found {0}", columnCount);
		}
	}
}
