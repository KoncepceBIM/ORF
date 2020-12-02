using ORF.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Xbim.Common;
using Xbim.Ifc4.Interfaces;

namespace ORF.Docs
{
    class Program
    {
        private static HashSet<Type> included;
        private static Assembly assembly;
        private static HashSet<Type> processed;
        private static readonly Assembly ifc4 = typeof(Xbim.Ifc4.EntityFactoryIfc4).Assembly;

        static void Main(string[] args)
        {
            included = new HashSet<Type>(new[] { typeof(IIfcPerson), typeof(IIfcOrganization), typeof(IfcArithmeticOperatorEnum), typeof(IIfcAddress), typeof(IIfcOwnerHistory) });
            assembly = typeof(CostModel).Assembly;
            processed = new HashSet<Type>();

            var toProcess = new Stack<Type>(new[] { typeof(Project), typeof(Classification) });
            using var w = File.CreateText("types.txt");
            while (toProcess.Count > 0)
            {
                var type = toProcess.Pop();
                if (!processed.Add(type))
                    continue;

                var typeName = GetName(type);
                var typeLink = GetLink(type);

                if (type.IsClass || type.IsInterface)
                {
                    var entityType = GetEntityType(type);
                    w.WriteLine($"Třída: {typeName}\t{entityType}\t{typeLink ?? ""}");

                    // only get properties with get + set
                    var properties = type.GetProperties().Where(PropertyFilter);
                    foreach (var prop in Order(properties))
                    {
                        var pType = UnwrapNullable(prop.PropertyType);
                        var isCollection = typeof(IEnumerable).IsAssignableFrom(pType) && !pType.IsValueType && !(pType == typeof(string));
                        if (isCollection)
                            pType = GetCollectionType(pType);

                        var pTypeName = GetName(pType);
                        var link = GetLink(pTypeName);
                        if (isCollection)
                            w.WriteLine($"{prop.Name}\tCollection<{pTypeName}>\t{link ?? ""}");
                        else
                            w.WriteLine($"{prop.Name}\t{pTypeName}\t{link ?? ""}");

                        if (IsForProcessing(pType))
                            toProcess.Push(pType);
                    }

                    w.WriteLine();
                    continue;
                }

                if (type.IsEnum)
                {
                    w.WriteLine($"Enumerace: {typeName}");
                    var members = type.GetEnumNames();
                    foreach (var item in members)
                        w.WriteLine(item);

                    w.WriteLine();
                    continue;
                }
            }
        }

        private static string GetEntityType(Type type)
        {
            if (type.Assembly == assembly)
            {
                var entityProp = type.GetProperty("Entity");
                if (entityProp != null)
                    return GetName(entityProp.PropertyType);
            }
            return "";
        }

        private static List<PropertyInfo> Order(IEnumerable<PropertyInfo> properties)
        {
            var result = properties.OrderBy(p => p.Name).ToList(); ;
            var id = result.FirstOrDefault(p => string.Equals("id", p.Name, StringComparison.OrdinalIgnoreCase));
            if (id != null)
            {
                result.Remove(id);
                result.Insert(0, id);
            }
            var name = result.FirstOrDefault(p => string.Equals("name", p.Name, StringComparison.OrdinalIgnoreCase));
            if (name != null)
            {
                var idx = id != null ? 1 : 0;
                result.Remove(name);
                result.Insert(idx, name);
            }
            var description = result.FirstOrDefault(p => string.Equals("description", p.Name, StringComparison.OrdinalIgnoreCase));
            if (description != null)
            {
                var idx = new[] { id, name }.Where(p => p != null).Count();
                result.Remove(description);
                result.Insert(idx, description);
            }
            return result;
        }

        private static bool PropertyFilter(PropertyInfo property)
        {
            if (property.GetIndexParameters().Length > 0)
                return false;

            if (property.SetMethod != null)
                return true;

            var type = property.PropertyType;

            // inverse property
            if (property.DeclaringType.Assembly == ifc4 && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                return false;

            if (typeof(IEnumerable).IsAssignableFrom(type))
                return true;

            return false;
        }

        private static Type GetCollectionType(Type collection)
        {
            if (collection.IsGenericType)
                return collection.GetGenericArguments()[0];
            return collection.GetInterfaces().Where(i => i.IsGenericType).FirstOrDefault().GetGenericArguments()[0];
        }

        private static Type UnwrapNullable(Type type)
        {
            var inner = Nullable.GetUnderlyingType(type);
            if (inner != null)
                return inner;
            return type;
        }

        private static bool IsForProcessing(Type type)
        {
            if (processed.Contains(type))
                return false;
            if (!type.IsClass && !type.IsInterface && !type.IsEnum)
                return false;
            if (type.Assembly != assembly && !included.Contains(type))
                return false;
            return true;
        }

        private static string GetName(Type type)
        {
            var typeName = type.Name;
            //if (typeName.StartsWith("ifc", StringComparison.OrdinalIgnoreCase))
            //    return typeName.Substring(3);
            if (typeName.StartsWith("iifc", StringComparison.OrdinalIgnoreCase))
                return typeName.Substring(1);

            if (string.Equals("IfcLabel", typeName, StringComparison.OrdinalIgnoreCase))
                return "String";
            if (string.Equals("IfcIdentifier", typeName, StringComparison.OrdinalIgnoreCase))
                return "String";
            if (string.Equals("IfcText", typeName, StringComparison.OrdinalIgnoreCase))
                return "String";

            return typeName;
        }

        private static string GetLink(Type type)
        {
            if (type.Assembly == assembly)
            {
                var entityProp = type.GetProperty("Entity");
                if (entityProp != null)
                    return GetLink(GetName(entityProp.PropertyType));
            }

            return GetLink(GetName(type));
        }

        private static string GetLink(string typeName)
        {
            if (typeName.StartsWith("ifc", StringComparison.OrdinalIgnoreCase))
                return $"https://standards.buildingsmart.org/IFC/RELEASE/IFC4/ADD2_TC1/HTML/link/{typeName.ToLowerInvariant()}.htm";
            return null;
        }

    }
}
