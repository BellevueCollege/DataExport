using DataExport.WS;
using DataExport.WS.Config;
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
			target.FieldSeparator = "|";

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

		[TestMethod()]
		public void VerifyTrailingCharsRemoved()
		{
			string actual = string.Empty;
			string[] record = {"954999991", "_teststu01  ", "Student01,"};

			CsvFormat target = new CsvFormat();
			target.FieldSeparator = "|";
			char[] trimCharsEnd = new[] {',', ' '};
			target.FieldTrimEndChars = trimCharsEnd;

			DataTable table = new DataTable();
			table.Columns.Add("SID", typeof(string));
			table.Columns.Add("Username", typeof(string));
			table.Columns.Add("Last Name", typeof(string));

			table.Rows.Add(record);

			using (DataSet ds = new DataSet())
			{
				ds.Tables.Add(table);

				actual = target.Serialize(ds);
			}

			string expected = string.Empty;
			foreach (string s in record)
			{
				if (string.IsNullOrWhiteSpace(expected))
				{
					expected = s.TrimEnd(trimCharsEnd);
				}
				else
				{
					expected = string.Concat(expected, "|", s.TrimEnd(trimCharsEnd));
				}
			}
			Assert.AreEqual(string.Concat(expected, Environment.NewLine), actual);
		}

		[TestMethod()]
		public void VerifyLeadingCharsRemoved()
		{
			string actual = string.Empty;
			string[] record = {",954999991", "  _teststu01", "Student01"};

			CsvFormat target = new CsvFormat();
			target.FieldSeparator = "|";
			char[] trimStartChars = new[] {',', ' '};
			target.FieldTrimLeadingChars = trimStartChars;

			DataTable table = new DataTable();
			table.Columns.Add("SID", typeof(string));
			table.Columns.Add("Username", typeof(string));
			table.Columns.Add("Last Name", typeof(string));

			table.Rows.Add(record);

			using (DataSet ds = new DataSet())
			{
				ds.Tables.Add(table);

				actual = target.Serialize(ds);
			}

			string expected = string.Empty;
			foreach (string s in record)
			{
				if (string.IsNullOrWhiteSpace(expected))
				{
					expected = s.TrimStart(trimStartChars);
				}
				else
				{
					expected = string.Concat(expected, "|", s.TrimStart(trimStartChars));
				}
			}
			Assert.AreEqual(string.Concat(expected, Environment.NewLine), actual);
		}
	}
}
