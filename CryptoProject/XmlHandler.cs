using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Common;
using System.IO;

namespace CryptoProject
{
    public class XmlHandler
    {
        public XmlHandler()
        {

        }
        public string EntityToXml(LogEntitet le)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(LogEntitet));
            using (StringWriter textWriter = new StringWriter()){

                xmlSerializer.Serialize(textWriter, le);
                File.WriteAllText("baza.xml", textWriter.ToString());
                return textWriter.ToString();

            }
        }
    }
}
