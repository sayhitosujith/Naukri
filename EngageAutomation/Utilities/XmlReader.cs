using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engage.Automation.Utilities
{
    class XmlReader
    {
        public static DataSet ReadXml(string AppName)
        {
            var dsSet = new DataSet();
            try
            {
                string xmlString = ConfigurationManager.AppSettings["xml_location"];

                if (xmlString.Equals(string.Empty))
                    xmlString = File.ReadAllText(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\TestData\\" + AppName + "_TestData.xml");
                else
                    xmlString = File.ReadAllText(xmlString + "\\" + AppName + "_TestData.xml");

                var stringReader = new StringReader(xmlString);

                dsSet.ReadXml(stringReader);
            }
            catch (Exception)
            {
                throw;
            }
            return dsSet;
        }
    }
}

