using ESoupis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using Xbim.Common.ExpressValidation;

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

        [TestMethod]
        public void ConvertEsoupisTest()
        {
            using var file = File.OpenRead("Data/eSoupis.xml");
            var s = TeSoupis.Deserialize(file);
            Assert.IsNotNull(s);
                
            using var model = Convertor.Convert(s);
            model.SaveAsIfc($"eSoupis.ifc");
            Assert.IsTrue(model.IsValid(out IEnumerable<ValidationResult> errs));
        }
    }
}
