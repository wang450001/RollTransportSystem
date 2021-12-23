using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Spiderweb.Common.Conf
{
    public class XmlAppConfig : CAppConfig
    {
        const string ROOT_NAME = "profile";
        XmlElement root;

        public XmlAppConfig(string path)
            : base(path)
        {
            XmlDocument doc = new XmlDocument();

            doc.Load(Path);

            root = doc.DocumentElement;
        }

        public override string Read(string section, string key, string strDefault = "")
        {
            XmlNode entryNode = root.SelectSingleNode(GetSectionsPath(section) + "/" + GetEntryPath(key));

            return entryNode.InnerText;
        }

        public override int Read(string section, string key, int intDefault = 0)
        {
            int intTemp = 0;

            if (!int.TryParse(Read(section, key, "0"), out intTemp))
                intTemp = intDefault;

            return intTemp;
        }

        public override bool Write(string section, string key, string strValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Retrieves the XPath string used for retrieving a section from the XML file. </summary>
        /// <returns>
        ///   An XPath string. </returns>
        /// <seealso cref="GetEntryPath" />
        private string GetSectionsPath(string section)
        {
            return "section[@name=\"" + section + "\"]";
        }

        /// <summary>
        ///   Retrieves the XPath string used for retrieving an entry from the XML file. </summary>
        /// <returns>
        ///   An XPath string. </returns>
        /// <seealso cref="GetSectionsPath" />
        private string GetEntryPath(string entry)
        {
            return "entry[@name=\"" + entry + "\"]";
        }

        private bool SetValueInXML(string section, string entry, object value)
        {
            if (value == null) return false;

            try
            {
                string valueString = value.ToString();

                // If the file does not exist, use the writer to quickly create it
                if (!File.Exists(Path))
                {
                    XmlTextWriter writer = new XmlTextWriter(Path, Encoding.UTF8);

                    writer.Formatting = Formatting.Indented;

                    writer.WriteStartDocument();

                    writer.WriteStartElement(ROOT_NAME);

                    writer.WriteStartElement("section");
                    writer.WriteAttributeString("name", null, section);

                    writer.WriteStartElement("entry");
                    writer.WriteAttributeString("name", null, entry);
                    writer.WriteString(valueString);

                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    writer.WriteEndElement();

                    writer.Flush();
                    writer.BaseStream.Position = 0;
                    writer.Close();
                    writer.Dispose();
                }

                // The file exists, edit it

                XmlDocument doc = new XmlDocument();
                doc.Load(Path);


                XmlElement root = doc.DocumentElement;

                // Get the section element and add it if it's not there
                XmlNode sectionNode = root.SelectSingleNode(GetSectionsPath(section));
                if (sectionNode == null)
                {
                    XmlElement element = doc.CreateElement("section");
                    XmlAttribute attribute = doc.CreateAttribute("name");
                    attribute.Value = section;
                    element.Attributes.Append(attribute);
                    sectionNode = root.AppendChild(element);
                }

                // Get the entry element and add it if it's not there
                XmlNode entryNode = sectionNode.SelectSingleNode(GetEntryPath(entry));
                if (entryNode == null)
                {
                    XmlElement element = doc.CreateElement("entry");
                    XmlAttribute attribute = doc.CreateAttribute("name");
                    attribute.Value = entry;
                    element.Attributes.Append(attribute);
                    entryNode = sectionNode.AppendChild(element);
                }

                // Add the value and save the file
                entryNode.InnerText = valueString;
                doc.Save(Path);
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception("XML SetValue", ex);
            }
        }
    }
}
