using System;
using System.IO;
using System.Linq;
using System.Xml.Schema;

namespace ORF.XML.Doc
{
    class Program
    {
        static void Main(string[] args)
        {
            using var file = File.OpenRead("Schema\\ORF_v0.1.xsd");
            using var doc = File.CreateText("doc.html");

            doc.Write(@"<!doctype html>
<html>
  <head>
    <meta charset='utf-8'>
  </head>
  <body> 
");

            var schema = XmlSchema.Read(file, (s, e) =>
            {
                Console.WriteLine(e.Message);
            });

            foreach (var type in schema.Items.OfType<XmlSchemaComplexType>())
            {
                doc.H3(type.Name);
                doc.P(type.Annotation());

                var members = type.GetAllMembers().OrderBy(m => m.Name);
                doc.TableOpen();
                    doc.TR("Název", "Popis", "Povinný", "Typ");
                foreach (var member in members)
                {
                    var memberType = member.IsList ? $"{member.Type}[]" : member.Type;
                    doc.TR(member.Name, member.Description, member.Required ? "Ano" : "Ne", memberType);
                }
                doc.TableClose();
            }

            foreach (var type in schema.Items.OfType<XmlSchemaSimpleType>())
            {
                doc.H3(type.Name);
                doc.P(type.Annotation());

                if (type.IsEnum())
                {

                    doc.P("Hodnoty enumerace:");
                    doc.TableOpen();
                    foreach (var e in type.GetEnum())
                    {
                        doc.TR(e.Value, e.Annotation());
                    }
                    doc.TableClose();
                }
            }

            doc.Write(@"
  </body>
</html>
");
        }
    }


}
