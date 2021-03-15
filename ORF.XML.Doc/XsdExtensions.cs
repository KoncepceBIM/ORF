using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Schema;

namespace ORF.XML.Doc
{
    internal static class XsdExtensions
    {
        public static IEnumerable<XmlMemberInfo> GetMembers(this XmlSchemaComplexType type)
        {
            var extension = type?.ContentModel?.Content as XmlSchemaComplexContentExtension;
            if (extension != null)
            {
                foreach (var attr in extension.Attributes.OfType<XmlSchemaAttribute>())
                {
                    yield return new XmlMemberInfo(attr);
                }
                var extSequence = (extension.Particle as XmlSchemaSequence)?.Items.OfType<XmlSchemaElement>() ??
                    Enumerable.Empty<XmlSchemaElement>();
                foreach (var element in extSequence)
                {
                    yield return new XmlMemberInfo(element);
                }
                yield break;
            }

            var attributes = type.Attributes.OfType<XmlSchemaAttribute>();
            foreach (var attr in attributes)
            {
                yield return new XmlMemberInfo(attr);                     
            }
            var sequence = (type.Particle as XmlSchemaSequence)?.Items.OfType<XmlSchemaElement>() ?? 
                Enumerable.Empty<XmlSchemaElement>();
            foreach (var element in sequence)
            {
                yield return new XmlMemberInfo(element);
            }
        }

        public static IEnumerable<XmlMemberInfo> GetAllMembers(this XmlSchemaComplexType type)
        {
            var current = type;
            while (current != null)
            {
                foreach (var item in current.GetMembers())
                    yield return item;

                current = current.GetBase();
            }
        }

        public static XmlSchemaComplexType GetBase(this XmlSchemaComplexType type)
        {
            var extension = type?.ContentModel?.Content as XmlSchemaComplexContentExtension;
            if (extension == null)
                return null;

            var baseName = extension.BaseTypeName;
            var schema = type.GetSchema();
            return schema.Items
                .OfType<XmlSchemaComplexType>()
                .Where(t => t.Name == baseName.Name)
                .FirstOrDefault();
        }

        public static XmlSchema GetSchema(this XmlSchemaComplexType type)
        {
            XmlSchemaObject current = type;
            while (current.Parent != null)
                current = current.Parent;
            return current as XmlSchema;
        }

        public static string Annotation(this XmlSchemaAnnotated schema)
        {
            var doc = schema?.Annotation?.Items[0] as XmlSchemaDocumentation;
            if (doc == null)
                return "";
            return string.Join("<br />", doc?.Markup.Select(m => m.InnerText));
        }

        public static bool IsEnum(this XmlSchemaSimpleType type)
        {
            if (!(type.Content is XmlSchemaSimpleTypeRestriction restriction))
                return false;

            return restriction.Facets.OfType<XmlSchemaEnumerationFacet>().Any();
        }

        public static IEnumerable<XmlSchemaEnumerationFacet> GetEnum(this XmlSchemaSimpleType type)
        {
            if (!(type.Content is XmlSchemaSimpleTypeRestriction restriction))
                return Enumerable.Empty<XmlSchemaEnumerationFacet>();

            return restriction.Facets.OfType<XmlSchemaEnumerationFacet>();
        }
    }
}
