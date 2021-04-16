using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace ORF.XML.Classification
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine($"No arguments. Set working directory.");
                return;
            }

            var path = args[0];
            if (!Directory.Exists(path))
            {
                Console.WriteLine($"'{path}' is not a valid path.");
                return;
            }

            foreach (var file in Directory.EnumerateFiles(path, "*.csv", SearchOption.TopDirectoryOnly))
            {
                var name = Path.GetFileNameWithoutExtension(file);
                using (var stream = File.OpenText(file))
                using (var csv = new CsvHelper.CsvReader(stream, new CsvConfiguration(CultureInfo.CurrentCulture) { 
                    MissingFieldFound = null,  
                    HeaderValidated = (args) => {
                        if (args.InvalidHeaders == null)
                            return;
                        foreach (var header in args.InvalidHeaders)
                        {
                            Console.WriteLine($"Invalid header {string.Join(", ", header.Names)}");
                        }
                    } 
                }))
                {
                    var classification = new TKlasifikace() { Nazev = name };
                    var level2 = new List<TTrida>();
                    foreach (var record in csv.GetRecords<Record>())
                    {
                        var cls = new TTrida {
                            Nazev = record.Name,
                            Popis = record.Description
                        };

                        if (!string.IsNullOrWhiteSpace(record.Level1))
                        {
                            cls.Kod = record.Level1.TrimEnd('?');
                            Add(classification, cls);
                            level2.Clear();
                        }

                        else if (!string.IsNullOrWhiteSpace(record.Level2))
                        {
                            cls.Kod = record.Level2.TrimEnd('?');

                            var parent = classification.Trida.LastOrDefault();
                            if (parent == null)
                                throw new Exception($"Orphan {cls.Nazev}");

                            Add(parent, cls);
                            level2.Add(cls);
                        }

                        else if (!string.IsNullOrWhiteSpace(record.Level3))
                        {
                            cls.Kod = record.Level3.TrimEnd('?');

                            var parent = level2.LastOrDefault();
                            if (parent == null)
                                parent = classification.Trida.LastOrDefault();
                            if (parent == null)
                                throw new Exception($"Orphan {cls.Nazev}");
                         
                            Add(parent, cls);
                        }
                    }

                    using var output = File.Create(Path.Combine(path, $"{name}.xml"));
                    using var xml = XmlWriter.Create(output, new XmlWriterSettings
                    {
                        Indent = true,
                        IndentChars = "  "
                    });
                    var serializer = new XmlSerializer(typeof(TKlasifikace));
                    serializer.Serialize(xml, classification);
                }
            }
        }

        private static void Add(TKlasifikace parent, TTrida child)
        {
            if (parent.Trida == null)
                parent.Trida = new[] { child };
            else
            {
                var children = parent.Trida.ToList();
                children.Add(child);
                parent.Trida = children.ToArray();
            }
        }

        private static void Add(TTrida parent, TTrida child)
        {
            if (parent.Trida == null)
                parent.Trida = new[] { child };
            else
            {
                var children = parent.Trida.ToList();
                children.Add(child);
                parent.Trida = children.ToArray();
            }
        }
    }
}
