using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Text;
using System.Xml;
using System.IO;



    public sealed class StringWriterWithEncoding : StringWriter
    {
        private readonly Encoding encoding;

        public StringWriterWithEncoding(Encoding encoding)
        {
            this.encoding = encoding;
        }

        public override Encoding Encoding
        {
            get { return encoding; }
        }
    }

    public partial class StoredProcedures
    {
        [Microsoft.SqlServer.Server.SqlProcedure]
        public static void XMLExport(SqlXml InputXml, SqlString OutputFile)
        {
            try
            {
                if (!InputXml.IsNull && !OutputFile.IsNull)
                {

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(InputXml.Value);

                    StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.GetEncoding("ISO-8859-1"));
                    XmlWriterSettings settings = new XmlWriterSettings
                    {
                        Indent = true,
                        IndentChars = "  ",
                        NewLineChars = "\r\n",
                        NewLineHandling = NewLineHandling.Replace,
                        Encoding = System.Text.Encoding.GetEncoding("ISO-8859-1")
                    };

                    using (XmlWriter writer = XmlWriter.Create(sw, settings))
                    {
                        doc.Save(writer);
                    }


                    System.IO.File.WriteAllText(OutputFile.ToString(), sw.ToString(), System.Text.Encoding.GetEncoding("ISO-8859-1"));
                }
                else
                {
                    throw new Exception("Parameters must be set");
                }
            }
            catch
            {
                throw;
            }
        }
    }
