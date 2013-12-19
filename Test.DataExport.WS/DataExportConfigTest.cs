using DataExport.WS.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using DataExport.WS;
using System.Collections.Generic;

namespace Test.DataExport.WS
{
    
    
    /// <summary>
    ///This is a test class for DataExportConfigTest and is intended
    ///to contain all DataExportConfigTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DataExportConfigTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
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
        ///A test for Exporters
        ///</summary>
        [TestMethod()]
        public void DataExportConfig_Loaded()
        {
            DataExportConfig target = DataExportConfig.Load();

            Assert.IsNotNull(target, "DataExportConfig is NULL");

            Assert.IsNotNull(target.Exporters, "Exporters collection is NULL");
            Assert.IsTrue(target.Exporters.Count > 0, "No Exporters loaded");

            Exporter exporter = target.Exporters[0];
            
            Assert.IsNotNull(exporter.Data, "Exporter.Data is NULL");
            Assert.IsNotNull(exporter.Format, "Exporter.Format is NULL");
            Assert.IsNotNull(exporter.Deliver, "Exporter.Deliver is NULL");
        }
    }
}
