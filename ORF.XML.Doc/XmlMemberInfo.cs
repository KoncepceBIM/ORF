using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;

namespace ORF.XML.Doc
{
    class XmlMemberInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public bool Required { get; set; }
        public bool IsList { get; set; }

        public XmlMemberInfo(XmlSchemaAttribute attribute)
        {
            Name = attribute.Name;
            Description = attribute.Annotation();
            Type = attribute.SchemaTypeName.Name;
            Required = attribute.Use == XmlSchemaUse.Required;
            IsList = false;
        }

        public XmlMemberInfo(XmlSchemaElement element)
        {
            Name = element.Name;
            Description = element.Annotation();
            Type = element.SchemaTypeName.Name;
            Required = element.MinOccurs > 0;
            IsList = element.MaxOccurs > 1;
        }
    }
}
