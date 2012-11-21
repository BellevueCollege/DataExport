﻿using DataExport.WS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using System.Data;

namespace Test.DataExport.WS
{


	[TestClass()]
	public class TestCsvFormat
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
		public void SerializeToPipeSeparated()
		{
			string actual = string.Empty;
			string expected = "954999991|_teststu01|Student01";

			CsvFormat target = new CsvFormat();
			target.Separator = "|";

			DataTable table = new DataTable();
			table.Columns.Add("SID", typeof(string));
			table.Columns.Add("Username", typeof(string));
			table.Columns.Add("Last Name", typeof(string));

			table.Rows.Add(expected.Split('|'));

			using (DataSet ds = new DataSet())
			{
				ds.Tables.Add(table);

				actual = target.Serialize(ds);
			}
			Assert.AreEqual(string.Concat(expected, Environment.NewLine), actual);
		}
	}
}
