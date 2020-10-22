using ESoupis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace ORF.Tests
{
    [TestClass]
    public class ESoupisReadingTests
    {
        [TestMethod]
        public void ReadESoupis()
        {
            using (var file = File.OpenRead("Data/eSoupis.xml"))
            {
                var s = TeSoupis.Deserialize(file);
                Assert.IsNotNull(s);
            }
        }
    }
}
