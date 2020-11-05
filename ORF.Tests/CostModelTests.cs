using Microsoft.VisualStudio.TestTools.UnitTesting;
using ORF.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xbim.Common.ExpressValidation;
using Xbim.Ifc;

namespace ORF.Tests
{
    [TestClass]
    public class CostModelTests
    {
        [TestMethod]
        public void SimpleModelTest()
        {
            var credentials = new XbimEditorCredentials
            {
                ApplicationDevelopersName = "Martin Cerny",
                ApplicationFullName = "ORF tests",
                ApplicationIdentifier = "ORFT",
                ApplicationVersion = "4.0",
                //your user
                EditorsFamilyName = "Cerny",
                EditorsGivenName = "Martin",
                EditorsOrganisationName = "CAS"
            };

            using (var model = new CostModel(credentials, "Example cost model"))
            {
                using (var txn = model.BeginTransaction())
                {
                    var lengthUnit = model.Create.SIUnit(u => {
                        u.Name = Xbim.Ifc4.Interfaces.IfcSIUnitName.METRE;
                        u.UnitType = Xbim.Ifc4.Interfaces.IfcUnitEnum.LENGTHUNIT;
                    } );
                    var areaUnit = model.Create.SIUnit(u => {
                        u.Name = Xbim.Ifc4.Interfaces.IfcSIUnitName.SQUARE_METRE;
                        u.UnitType = Xbim.Ifc4.Interfaces.IfcUnitEnum.AREAUNIT;
                    });
                    var volumeUnit = model.Create.SIUnit(u => {
                        u.Name = Xbim.Ifc4.Interfaces.IfcSIUnitName.CUBIC_METRE;
                        u.UnitType = Xbim.Ifc4.Interfaces.IfcUnitEnum.VOLUMEUNIT;
                    });
                    var weightUnit = model.Create.SIUnit(u => {
                        u.Name = Xbim.Ifc4.Interfaces.IfcSIUnitName.GRAM;
                        u.Prefix = Xbim.Ifc4.Interfaces.IfcSIPrefix.KILO;
                        u.UnitType = Xbim.Ifc4.Interfaces.IfcUnitEnum.MASSUNIT;
                    });
                    var timeUnit = model.Create.SIUnit(u => {
                        u.Name = Xbim.Ifc4.Interfaces.IfcSIUnitName.SECOND;
                        u.UnitType = Xbim.Ifc4.Interfaces.IfcUnitEnum.TIMEUNIT;
                    });
                    var costUnit = model.Create.MonetaryUnit(u => u.Currency = "CZK");

                    // project wide units assignment
                    model.Project.Units.Add(lengthUnit);
                    model.Project.Units.Add(areaUnit);
                    model.Project.Units.Add(volumeUnit);
                    model.Project.Units.Add(weightUnit);
                    model.Project.Units.Add(timeUnit);
                    model.Project.Units.Add(costUnit);

                    var schedule = new CostSchedule(model, "Sample schedule");

                    var rootA = new CostItem(model) { 
                        Name = "Superstructure",
                        Identifier = "A.1",
                        Description = "Description of superstructure"
                    };

                    var rootB = new CostItem(model)
                    {
                        Name = "Substructure",
                        Identifier = "B.1",
                        Description = "Description of substructure"
                    };

                    schedule.CostItemRoots.Add(rootA);
                    schedule.CostItemRoots.Add(rootB);

                    var walls = new CostItem(model)
                    {
                        Name = "Walls",
                        Identifier = "A.1.1",
                    };

                    var floors = new CostItem(model)
                    {
                        Name = "Floors",
                        Identifier = "A.1.2",
                    };

                    var windows = new CostItem(model)
                    {
                        Name = "Windows",
                        Identifier = "B.1.2",
                    };

                    rootA.Children.Add(walls);
                    rootA.Children.Add(floors);
                    rootB.Children.Add(windows);

                    walls.Quantities.AddArea("Wall area").Value = 156;
                    floors.Quantities.AddArea("Floor area").Value = 466;
                    windows.Quantities.AddCount("Windows count").Value = 45;

                    txn.Commit();
                }
                Assert.IsTrue(model.IsValid(out IEnumerable<ValidationResult> errs));
                model.SaveAsIfc("orf.ifc");
            }
        }
    }
}
