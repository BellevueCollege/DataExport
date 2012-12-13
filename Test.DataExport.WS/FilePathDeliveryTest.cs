using System.IO;
using System.Text;
using Common.Logging;
using DataExport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;

namespace Test.DataExport.WS
{


	[TestClass()]
	public class TestFilePathDelivery
	{
		private ILog _log = LogManager.GetCurrentClassLogger();
		readonly private static string _outputFolder = AppDomain.CurrentDomain.BaseDirectory;

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

		#region Tests

		[TestMethod()]
		[ExpectedException(typeof(NullReferenceException))]
		public void Put_DestinationNull()
		{
			string fileContents = string.Format("heading1,date,heading3\ncol1,{0:d},column3\n", DateTime.Now);
			byte[] data = DeliveryBase.ConvertToSource(fileContents);

			IDeliveryStrategy target = new FilePathDelivery
			                           	{
			                           			Source = data
			                           	};

			target.Put();
			Assert.Fail("A NULL Destination did not throw the expected Exception");
		}

		[TestMethod()]
		[ExpectedException(typeof(NullReferenceException))]
		public void Put_DestinationEmpty()
		{
			string fileContents = string.Format("heading1,date,heading3\ncol1,{0:d},column3\n", DateTime.Now);
			byte[] data = DeliveryBase.ConvertToSource(fileContents);

			IDeliveryStrategy target = new FilePathDelivery
			                           	{
			                           			Source = data,
																			Destination = string.Empty
			                           	};

			target.Put();
			Assert.Fail("An empty Destination did not throw the expected Exception");
		}

		[TestMethod()]
		[ExpectedException(typeof(DirectoryNotFoundException))]
		public void Put_InvalidDestinationPath()
		{
			string fileContents = string.Format("heading1,date,heading3\ncol1,{0:d},column3\n", DateTime.Now);
			byte[] data = DeliveryBase.ConvertToSource(fileContents);

			IDeliveryStrategy target = new FilePathDelivery
			                           	{
			                           			Source = data,
																			Destination = @"t:\destination\does\not\exist"
			                           	};

			target.Put();
			Assert.Fail("An invalid Destination did not throw the expected Exception");
		}

		[TestMethod()]
		public void Put_Overwrite()
		{
			string fileContents = string.Format("heading1,date,heading3\ncol1,{0:d},column3\n", DateTime.Now);
			byte[] data = DeliveryBase.ConvertToSource(fileContents);

			string outputFile = Path.Combine(_outputFolder, "TestPut.csv");
			
			IDeliveryStrategy target = new FilePathDelivery
			                           	{
			                           			Source = data,
			                           			Destination = outputFile
			                           	};

			bool result = target.Put(DeliveryWriteMode.Overwrite);
			Assert.IsTrue(result, "Saving file failed: '{0}'", outputFile);

			string actual = ReadFromFile(outputFile);
			Assert.AreEqual(fileContents, actual);
		}

		[TestMethod()]
		public void Put_Ignore()
		{
			string fileContents = string.Format("heading1,date,heading3\ncol1,{0:d},column3\n", DateTime.Now);
			byte[] data = DeliveryBase.ConvertToSource(fileContents);

			string outputFile = Path.Combine(_outputFolder, "TestPut.csv");
			
			IDeliveryStrategy target = new FilePathDelivery
			                           	{
			                           			Source = data,
			                           			Destination = outputFile
			                           	};

			CreateFile(outputFile, "Created by unit test.");
			bool result = target.Put(DeliveryWriteMode.Ignore);
			Assert.IsFalse(result, "Did not skip writing when file exists: '{0}'", outputFile);

			string actual = ReadFromFile(outputFile);
			Assert.AreEqual(fileContents, actual);
		}

		[TestMethod()]
		[ExpectedException(typeof(NotSupportedException))]
		public void Put_Exception()
		{
			string fileContents = string.Format("heading1,date,heading3\ncol1,{0:d},column3\n", DateTime.Now);
			byte[] data = DeliveryBase.ConvertToSource(fileContents);

			string outputFile = Path.Combine(_outputFolder, "TestPut.csv");
			
			IDeliveryStrategy target = new FilePathDelivery
			                           	{
			                           			Source = data,
			                           			Destination = outputFile
			                           	};

			CreateFile(outputFile, "Created by unit test.");
			target.Put(DeliveryWriteMode.Exception);
			Assert.Fail("Did not throw expected Exception");
		}

		#endregion

		#region helper methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		private string ReadFromFile(string filename)
		{
			try
			{
				if (!File.Exists(filename))
				{
					return "(not found)";
				}
				using (StreamReader fs = File.OpenText(filename))
				{
					string buffer = fs.ReadToEnd();
					fs.Close();
					return buffer;
				}
			}
			catch (Exception ex)
			{
				_log.Error(m => m("Unable to read from file '{0}'...\n{1}", filename, ex));
			}

			return "(empty)";
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="data"></param>
		private void CreateFile(string filename, string data)
		{
			CreateFile(filename, Encoding.UTF8.GetBytes(data));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="data"></param>
		private void CreateFile(string filename, byte[] data)
		{
			if (!File.Exists(filename))
			{
				int length = data.Length;
				using (FileStream fs = File.Create(filename, length, FileOptions.None))
				{
					fs.Write(data, 0, length);
					fs.Flush();
					fs.Close();
				}
			}
		}

		#endregion

	}
}
