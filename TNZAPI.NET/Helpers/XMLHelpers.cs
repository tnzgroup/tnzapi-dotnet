using System.Xml;
using System.Xml.Serialization;
using TNZAPI.NET.Api.Messaging.Common.Components;
using static TNZAPI.NET.Core.Enums;

namespace TNZAPI.NET.Helpers
{
    public class XMLHelpers
    {
        #region XmlSerialize

        public static T Deserialize<T>(string input) where T : class
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(input))
            {
                return (T)ser.Deserialize(sr);
            }
        }

        internal static string Serialize<T>(T ObjectToSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(ObjectToSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, ObjectToSerialize);
                return textWriter.ToString();
            }
        }
        #endregion XMLSerialize

        internal static XmlNode addChildNode(XmlDocument xmlDoc, string name, string val)
        {
            XmlNode workingNode = xmlDoc.CreateElement(name);

            workingNode.InnerText = val;

            return workingNode;
        }

        internal static XmlNode addChildNodeCDATA(XmlDocument xmlDoc, string name, string val)
        {
            XmlNode workingNode = xmlDoc.CreateElement(name);

            workingNode.InnerXml = @"<![CDATA[" + val + "]]>";

            return workingNode;
        }


        #region Destinations
        public static XmlNode BuildXmlDestinationsNode(XmlDocument xmlDoc, ICollection<Recipient> recipients, string dest_type)
        {
            if (recipients.Count > 0)
            {
                var destNode = xmlDoc.CreateElement("Destinations");

                foreach (var recipient in recipients)
                {
                    destNode.AppendChild(BuildXmlDestinationNode(xmlDoc, recipient, dest_type));
                }

                return destNode;
            }

            return null;
        }

        public static XmlNode BuildXmlDestinationNode(XmlDocument xmlDoc, Recipient recipient, string dest_type)
        {
            var workingNode = xmlDoc.CreateElement("Destination");

            if (dest_type.ToUpper().Equals("SMS"))
                workingNode.AppendChild(addChildNode(xmlDoc, "Recipient", recipient.MobileNumber));
            if (dest_type.ToUpper().Equals("EMAIL"))
                workingNode.AppendChild(addChildNode(xmlDoc, "Recipient", recipient.EmailAddress));
            if (dest_type.ToUpper().Equals("FAX"))
                workingNode.AppendChild(addChildNode(xmlDoc, "Recipient", recipient.FaxNumber));
            if (dest_type.ToUpper().Equals("VOICE") || dest_type.ToUpper().Equals("TTS"))
                workingNode.AppendChild(addChildNode(xmlDoc, "Recipient", recipient.PhoneNumber));

            if (recipient.Attention != null)
                workingNode.AppendChild(addChildNode(xmlDoc, "Attention", recipient.Attention));
            if (recipient.CompanyName != null)
                workingNode.AppendChild(addChildNode(xmlDoc, "Company", recipient.CompanyName));
            if (recipient.FirstName != null)
                workingNode.AppendChild(addChildNode(xmlDoc, "FirstName", recipient.FirstName));
            if (recipient.LastName != null)
                workingNode.AppendChild(addChildNode(xmlDoc, "LastName", recipient.LastName));
            if (recipient.Custom1 != null)
                workingNode.AppendChild(addChildNode(xmlDoc, "Custom1", recipient.Custom1));
            if (recipient.Custom2 != null)
                workingNode.AppendChild(addChildNode(xmlDoc, "Custom2", recipient.Custom2));
            if (recipient.Custom3 != null)
                workingNode.AppendChild(addChildNode(xmlDoc, "Custom3", recipient.Custom3));
            if (recipient.Custom4 != null)
                workingNode.AppendChild(addChildNode(xmlDoc, "Custom4", recipient.Custom4));
            if (recipient.Custom5 != null)
                workingNode.AppendChild(addChildNode(xmlDoc, "Custom5", recipient.Custom5));

            return workingNode;
        }
        #endregion

        #region Files
        public static XmlNode BuildXmlFilesNode(XmlDocument xmlDoc, ICollection<Attachment> attachments)
        {
            if (attachments.Count > 0)
            {
                var filesNode = xmlDoc.CreateElement("Files");

                foreach (var attachment in attachments)
                {
                    filesNode.AppendChild(BuildXmlFileNode(xmlDoc, attachment));
                }

                return filesNode;
            }

            return null;
        }

        public static XmlNode BuildXmlFileNode(XmlDocument xmlDoc, Attachment attachment)
        {
            var workingNode = xmlDoc.CreateElement("File");
            workingNode.AppendChild(addChildNode(xmlDoc, "Name", attachment.FileName));
            workingNode.AppendChild(addChildNode(xmlDoc, "Data", attachment.FileContent));

            return workingNode;
        }
        #endregion

        #region Keypads
        public static XmlNode BuildXmlKeypadsNode(XmlDocument xmlDoc, ICollection<Keypad> keypads)
        {
            if (keypads.Count > 0)
            {
                var keypadsNode = xmlDoc.CreateElement("Keypads");

                foreach (var keypad in keypads)
                {
                    keypadsNode.AppendChild(BuildXmlKeypadNode(xmlDoc, keypad));
                }

                return keypadsNode;
            }

            return null;
        }

        public static XmlNode BuildXmlKeypadNode(XmlDocument xmlDoc, Keypad keypad)
        {
            var workingNode = xmlDoc.CreateElement("Keypad");
            workingNode.AppendChild(addChildNode(xmlDoc, "Tone", keypad.Tone.ToString()));
            if (keypad.RouteNumber != null && !keypad.RouteNumber.Equals(""))
            {
                workingNode.AppendChild(addChildNode(xmlDoc, "RouteNumber", keypad.RouteNumber));
            }
            if (keypad.Play != null && !keypad.Play.Equals(""))
            {
                workingNode.AppendChild(addChildNode(xmlDoc, "Play", keypad.Play));
            }
            //if (keypad.PlayFile != null && !keypad.PlayFile.Equals(""))
            //{
            //    workingNode.AppendChild(addChildNode(xmlDoc, "Play", FileHandlers.GetFileContents(keypad.PlayFile)));
            //}
            if (keypad.PlaySection != KeypadPlaySection.None )
            {
                workingNode.AppendChild(addChildNode(xmlDoc, "PlaySection", keypad.PlaySection.ToString()));
            }
            if (keypad.PlayFileData != null)
            {
                workingNode.AppendChild(addChildNode(xmlDoc, "PlayFile", keypad.PlayFileData.FileContent));
            }

            return workingNode;
        }
        #endregion
    }
}
