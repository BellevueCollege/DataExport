using System.Configuration;
using System.Text.RegularExpressions;
using CtcApi;
using System.IO;
using DataExport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
using Renci.SshNet.Common;

namespace Test.DataExport.WS
{


	[TestClass()]
	public class TestSftpDelivery
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

    protected string Keyfile
    {
      get {return "SupportFiles/testkey.rsa"; }
    }

    #region Tests against real SFTP servers
		[TestMethod()]
		[Ignore()]	// performs real upload to actual SSH server
		public void TestUploadToLANServer()
		{
		  byte[] data = DeliveryStrategy.ConvertToSource("blah");

			SftpDelivery sftp = new SftpDelivery()
			                    	{
																Source = data,
			                    			Hostname = ConfigurationManager.AppSettings["SftpHostname"],
			                    			Destination = string.Concat(ConfigurationManager.AppSettings["SftpDestinationPath"], "TestFile.csv"),
			                    			Username = ConfigurationManager.AppSettings["SftpLogin"],
			                    			/* The key below was generated just for testing */
			                    			KeyFile = Keyfile,
			                    			Password = string.Empty,
			                    	};

			bool result = sftp.Put();
			Assert.IsTrue(result, "File upload failed.");
		}

	  [TestMethod()]
		[Ignore()]	// performs real upload to actual SSH server
		public void TestUploadToMaxientServer()
		{
			string keyfile = ConfigurationManager.AppSettings["MaxientSftpKeyFile"];
			byte[] data = DeliveryStrategy.ConvertToSource("blah");

			string keyfile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testkey.rsa");
			SftpDelivery sftp = new SftpDelivery()
			                    	{
																Source = data,
			                    			Hostname = ConfigurationManager.AppSettings["MaxientSftpHostname"],
			                    			Destination = ConfigurationManager.AppSettings["MaxientSftpDestinationPathAndFile"],
			                    			Username = ConfigurationManager.AppSettings["MaxientSftpLogin"],
			                    			/* The key below was generated just for testing */
			                    			KeyFile = keyfile,
			                    			Password = string.Empty,
			                    	};

			bool result = sftp.Put();
			Assert.IsTrue(result, "File upload failed.");
		}

		#endregion

    #region Configuration tests
    [TestMethod]
    public void Config_WriteMode_Default()
    {
      Mock<ISftpClient> sftpMock = GetFakeSftpClient();

      SftpDelivery sftp = InitializeDelivery(sftpMock);
      Assert.AreEqual(DeliveryWriteMode.Exception, sftp.WriteMode);
    }

    #endregion
    
    [TestMethod]
		[ExpectedException(typeof(SftpPermissionDeniedException))]
		public void Upload_KeyAuth_ExceptionIfExists()
		{
			Mock<ISftpClient> sftpMock = GetFakeSftpClient();
			SetFileExists(sftpMock, true);
			SetFileOverwrite(sftpMock, false);

			SftpDelivery sftp = InitializeDelivery(sftpMock);
      SetKeyFile(sftpMock, sftp);
			bool result = sftp.Put();

			// Verify mock members ran
			sftpMock.Verify();
			sftpMock.Verify(m => m.UploadFile(It.IsAny<Stream>(), It.IsAny<string>(), false), Times.Never());

			Assert.IsFalse(result, "File upload reported successful when should have been aborted.");
		}

		[TestMethod]
		public void Upload_KeyAuth_OverwriteWhenExists()
		{
			Mock<ISftpClient> sftpMock = GetFakeSftpClient();
			SetFileExists(sftpMock, true);
			SetFileOverwrite(sftpMock, true);

			SftpDelivery sftp = InitializeDelivery(sftpMock);
      SetKeyFile(sftpMock, sftp);
      bool result = sftp.Put(DeliveryWriteMode.Overwrite);

			// Verify mock members ran
			sftpMock.Verify();
			Assert.IsTrue(result, "Delivery of file failed.");
		}

		[TestMethod]
		public void Upload_KeyAuth_FilenameWithTimestamp()
		{
			Mock<ISftpClient> sftpMock = GetFakeSftpClient();
			SetFileExists(sftpMock, true);
			SetFileOverwrite(sftpMock, true);

			SftpDelivery sftp = InitializeDelivery(sftpMock, @"TestFile-{Timestamp|yyyyMMdd-HHmmss}.csv");
      SetKeyFile(sftpMock, sftp);
      bool result = sftp.Put(DeliveryWriteMode.Overwrite);

			// Verify mock members were executed
			sftpMock.Verify();

		  string actualDest = sftpMock.Object.Destination;
		  Assert.IsTrue(Regex.IsMatch(actualDest, @".+TestFile-20[1-9][3-9](0[1-9]|1[0-2])(0[1-9]|[1-2]\d|3[0-2])-([0-1]\d|2[1-3])[0-6]\d[0-6]\d\.csv"), "Filename ({0}) is incorrect", actualDest);
			Assert.IsTrue(result, "Delivery of file failed.");
		}

	  [TestMethod]
		public void Upload_KeyAuth_IgnoreWhenExists()
		{
			Mock<ISftpClient> sftpMock = GetFakeSftpClient();
			SetFileExists(sftpMock, true);
			SetFileOverwrite(sftpMock, false);

			SftpDelivery sftp = InitializeDelivery(sftpMock);
      SetKeyFile(sftpMock, sftp);
      bool result = sftp.Put(DeliveryWriteMode.Ignore);

			// Verify mock members ran
			sftpMock.Verify();
			sftpMock.Verify(m => m.UploadFile(It.IsAny<Stream>(), It.IsAny<string>(), false), Times.Never());

			Assert.IsFalse(result, "File was delivered when should have been ignored.");
		}


	  // TODO: Write tests for additional scenarios (see FilePathDeliveryTest)

	  #region Helper methods
	  private SftpDelivery InitializeDelivery(Mock<ISftpClient> sftpMock)
	  {
	    return InitializeDelivery(sftpMock, "TestFile.csv");
	  }

	  private SftpDelivery InitializeDelivery(Mock<ISftpClient> sftpMock, string filename)
	  {
      byte[] data = FilePathDelivery.ConvertToSource("blah");

	    SftpDelivery sftp = new SftpDelivery(sftpMock.Object)
	                                        {
	                                          SaveFileCopy = true,
                                            Context = new ApplicationContext {BaseDirectory = AppDomain.CurrentDomain.BaseDirectory},
                                            Source = data,
                                            Destination = string.Concat(ConfigurationManager.AppSettings["SftpDestinationPath"], filename),
	                                        };
      
      // set the underlying destination (after template expansion has been applied)
      sftpMock.SetupProperty(m => m.Destination, sftp.Destination);
      
      return sftp;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sftpMock"></param>
    /// <param name="canOverride"></param>
	  private void SetFileOverwrite(Mock<ISftpClient> sftpMock, bool canOverride)
		{
			if (canOverride)
			{
				sftpMock.Setup(m => m.UploadFile(It.IsAny<Stream>(), It.IsAny<string>(), canOverride)).Verifiable("UploadFile() was not called");
			}
			else
			{
				sftpMock.Setup(m => m.UploadFile(It.IsAny<Stream>(), It.IsAny<string>(), canOverride));
			}
		}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sftpMock"></param>
    /// <param name="existsValue"></param>
		private void SetFileExists(Mock<ISftpClient> sftpMock, bool existsValue)
		{
			sftpMock.Setup(m => m.Exists(It.IsAny<string>())).Returns(existsValue);
		}

    private void SetKeyFile(Mock<ISftpClient> mock, SftpDelivery sftp)
    {
      sftp.KeyFile = Keyfile;
      mock.SetupProperty(m => m.KeyFiles);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private Mock<ISftpClient> GetFakeSftpClient()
		{
			Mock<ISftpClient> sftpMock = new Mock<ISftpClient>();
			sftpMock.SetupProperty(m => m.Hostname, "faux.server.foo");
			sftpMock.SetupProperty(m => m.Username, "fakeUser");
			sftpMock.SetupProperty(m => m.IsConnected, true);

			sftpMock.Setup(m => m.Connect()).Verifiable("SFTP connection code did not execute.");
			sftpMock.Setup(m => m.Disconnect()).Verifiable("SFTP disconnection code did not execute.");
			return sftpMock;
		}

		#endregion
	}
}
