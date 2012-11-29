using System.IO;
using DataExport.WS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using System.Data;

namespace Test.DataExport.WS
{


	[TestClass()]
	public class TestXslFormat
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


		/// <summary>
		/// 
		///</summary>
		[TestMethod]
		public void SerializeToMaxientStudentSchedules()
		{
			XslFormat formatter = new XslFormat();
			// The path to the templates folder will not evaluate properly because we're executing in
			// the context of the unit test engine - so set this as an absolute value.
			string templatesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\DataExport.WS\Templates");
			formatter.TemplateFile = Path.Combine(templatesFolder, "Maxient2.StudentSchedule.xslt");

			DataTable table = new DataTable();
			table.Columns.Add("SID", typeof(string));
			table.Columns.Add("CourseID", typeof(string));
			table.Columns.Add("Section", typeof(string));
			table.Columns.Add("Room", typeof(string));
			table.Columns.Add("Instructor", typeof(string));

			table.Rows.Add("954999991", "ART	101", "A", "R104", "Instructor, Anne B.");
			table.Rows.Add("954999991", "ENGL&101", "OAS", "B230", "Teacher, Carol D");
			table.Rows.Add("954999991", "BUS&	201", "A", "N204", "Frank");
			
			table.Rows.Add("954999992", "CS&	100", "B", "C220", "Professor, Eric");
			table.Rows.Add("954999992", "ENGL&101", "OAS", "B230", "Teacher, Carol D");

			string actual = string.Empty;
			using (DataSet ds = new DataSet())
			{
				ds.Tables.Add(table);

				actual = formatter.Serialize(ds);
			}

			Assert.IsFalse(string.IsNullOrWhiteSpace(actual), "Formatter returned a NULL or empty string.");

			string expected =@"954999991
ART	101 A R104 Instructor, Anne B.
ENGL&101 OAS B230 Teacher, Carol D
BUS&	201 A N204 Frank
**********
954999992
CS&	100 B C220 Professor, Eric
ENGL&101 OAS B230 Teacher, Carol D
**********
";
			Assert.AreEqual(expected, actual);
		}

	}
}
