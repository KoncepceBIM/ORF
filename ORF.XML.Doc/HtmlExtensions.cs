using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ORF.XML.Doc
{
    internal static class HtmlExtensions
    {
        public static void H2(this TextWriter w, string text)
        {
            w.WriteLine($"<h2>{text}</h2>");
        }

        public static void H3(this TextWriter w, string text)
        {
            w.WriteLine($"<h3>{text}</h3>");
        }

        public static void TableOpen(this TextWriter w)
        {
            w.WriteLine($"<table>");
        }

        public static void TableClose(this TextWriter w)
        {
            w.WriteLine($"</table>");
        }

        public static void TR(this TextWriter w, IEnumerable<string> values) => TR(w, values.ToArray());

        public static void TR(this TextWriter w, params string[] values)
        {
            w.WriteLine($"<tr>");
            foreach (var item in values)
            {
                w.WriteLine($"<td>");
                w.WriteLine(item);
                w.WriteLine($"</td>");
            }
            w.WriteLine($"</tr>");
        }

        public static void P(this TextWriter w, string text)
        {
            w.WriteLine("<p>");
            w.WriteLine(text);
            w.WriteLine("</p>");
        }
    }
}
