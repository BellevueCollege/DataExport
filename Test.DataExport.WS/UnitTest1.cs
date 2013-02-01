using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.DataExport.WS
{
    [TestClass]
    [Ignore]    // Only for research/experimentation - not part of the standard test suite.
    public class ResearchConfig
    {
        [TestMethod]
        public void LoadConfig_SqlInput()
        {
//            FooConfig config = ConfigurationManager.GetSection(FooConfig.GetSectionName()) as FooConfig;
            FooConfig config = FooConfig.Load();

            Assert.IsNotNull(config);
            Assert.IsNotNull(config.Exporters);
            
            Assert.IsTrue(config.Exporters.Count > 0, "No exporters found!");

            Assert.IsNotNull(config.Exporters[0].Input);
//            Assert.IsInstanceOfType(config.Exporters[0].Input, typeof(sql));
            Assert.AreEqual("SELECT", (config.Exporters[0].Input as sql).Command);

            // ************************************************************************************************************************************************
            // TODO: Look at this: http://stackoverflow.com/questions/1799154/serializable-class-inheriting-from-an-interface-with-a-property-of-its-own-type
            // to make the above .Input inherit properly (hopefully)
            // ************************************************************************************************************************************************
        }

        [TestMethod]
        public void LoadConfig_FileInput()
        {
            FooConfig config = FooConfig.Load();

            Assert.IsNotNull(config);
            Assert.IsNotNull(config.Exporters);
            
            Assert.IsTrue(config.Exporters.Count > 0, "No exporters found!");

            Assert.IsNotNull(config.Exporters[0].Input);
            Assert.AreEqual(@"C:\path", (config.Exporters[0].Input as file).Path);
        }

        [TestMethod]
        public void DeserialzeClass()
        {
            List<exporter> w = new List<exporter>();
            w.Add(new exporter() {Input = new sql() {Command = "SELECT"}});
            FooConfig foo = new FooConfig() { Exporters = w };

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter();
            XmlSerializer xs = new XmlSerializer(foo.GetType());
            XmlSerializerNamespaces xsn = new XmlSerializerNamespaces();
            xsn.Add("", "");

            XmlTextWriter xtw = new XmlTextWriter(sw);
            xtw.Formatting = Formatting.Indented;
            xtw.WriteRaw("");

            xs.Serialize(xtw, foo, xsn);
            string s = sb.ToString();
//            s = Regex.Replace(s, "(<" + foo.GetType().Name + ") (>)", "$1 type=\"" + foo.GetType().FullName + "\"$2");

            Debug.Print(s);
        }
    }

    [XmlType("foo")]
    public class FooConfig : CtcApi.Config.CtcConfigBase<FooConfig>
    {
        [XmlArrayAttribute("exporters")]
        public List<exporter> Exporters { get; set; }
    }

    [XmlType("exporter")]
    public class exporter
    {
        [XmlElement("sqlInput", typeof(sql))]
        [XmlElement("fileInput", typeof(file))]
        public input Input { get; set; }
    }

    [XmlType("file")]
    public class file : input
    {
        [XmlAttribute("path")]
        public string Path { get; set; }
    }

//    [XmlType(AnonymousType = true)]
    public class input
    {
        public virtual bool Execute()
        {
            throw new NotImplementedException();
        }
    }

    [XmlType("sql")]
    public class sql : input
    {
        [XmlAttribute("command")]
        public string Command { get; set; }

        public override bool Execute()
        {
            throw new NotImplementedException();
        }
    }

    //[XmlType("factory")]
    //[XmlInclude(typeof(Oliver))]
    //[XmlInclude(typeof(Oompa))]
    //public class FooConfig : CtcApi.Config.CtcConfigBase<FooConfig>
    //{
    //    [XmlArrayAttribute("wonkas")]
    //    public List<Wonka> Exporters { get; set; }

    //    public Urchin Slave { get; set; }

    //    public Urchin Volunteer { get; set; }
    //}

    //[XmlType("wonka")]
    //public class Wonka
    //{
    //    [XmlAttribute("name")]
    //    public string Name { get; set; }
    //}


    //public abstract class Urchin
    //{
    //}

    //[XmlType("oliver")]
    //public class Oliver : Urchin
    //{
    //    [XmlAttribute("state")]
    //    public string State { get; set; }
    //}

    //[XmlType("oompa")]
    //public class Oompa : Urchin
    //{
    //    [XmlAttribute("height")]
    //    public int Height { get; set; }
    //}




    internal class FooConfigHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            FooConfig result = null;
            if (section == null)
                return result;
            XmlSerializer ser = new XmlSerializer(typeof(FooConfig));

            using (XmlNodeReader reader = new XmlNodeReader(section))
            {
                result = (FooConfig)ser.Deserialize(reader);

                return result;
            }
        }
    }
}
