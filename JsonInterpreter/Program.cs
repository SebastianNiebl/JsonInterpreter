using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace JsonInterpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0) return;    
            string json = File.ReadAllText(args[0]);
            XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode(json, "response");
            if (args.Length == 1)
            {
                Console.Out.WriteLine(Beautify(doc)); 
                return;
            }
            for (int i = 1; i < args.Length; i++)
            {
                XmlNodeList nodes = doc.DocumentElement.SelectNodes(args[i]);
                XmlNode last = nodes[nodes.Count - 1];
                foreach (XmlNode node in nodes)
                {
                    string output = node.InnerText;
                    if (i + 1 == args.Length && node == last)
                    {
                        Console.Out.Write(output);
                    }
                    else
                    {
                        Console.Out.WriteLine(output);
                    }   
                }
                if(!(i+1 == args.Length))
                Console.Out.WriteLine();
            }
        }
        static string Beautify(XmlDocument doc)
        {
            StringBuilder sb = new();
            XmlWriterSettings settings = new()
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            };
            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                doc.Save(writer);
            }
            return sb.ToString();
        }
    }
}
