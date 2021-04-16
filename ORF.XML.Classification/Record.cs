using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ORF.XML.Classification
{
    internal class Record
    {
        [Name("Úroveň 1")]
        public string Level1 { get; set; }
        [Name("Úroveň 2")]
        public string Level2 { get; set; }
        [Name("Úroveň 3")]
        public string Level3 { get; set; }
        [Name("Název (ČJ)")]
        public string Name { get; set; }
        [Name("Vysvětlení (ČJ)")]
        public string Description { get; set; }
    }
}
