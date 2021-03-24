using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ORF.XML.Examples
{
    internal static class UnitsExample
    {
        public static void Run()
        {


            // Reinforcement grade(t/ m³)
            var reinforcementGrade = new TDerivedUnit
            {
                Component = new TDerivedUnitComponent[] {
                    new TDerivedUnitComponent {
                        Exponent = 1,
                        Unit = new TConversionUnit {
                            Name = "ton",
                            Symbol = "t",
                            Offset = 0,
                            Scale = 1000,
                            BaseUnit = new TSIUnit
                            {
                                Name = SIUnitName.gram,
                                Prefix = SIUnitPrefix.kilo,
                                PrefixSpecified = true
                            }
                        }
                    },
                    new TDerivedUnitComponent {
                        Exponent = -3,
                        Unit = new TSIUnit
                        {
                            Name = SIUnitName.metre,
                            PrefixSpecified = false
                        }
                    }
                }
            };

            // Unit costs(€/ m²)
            var unitCost = new TDerivedUnit
            {
                Component = new TDerivedUnitComponent[]
                {
                    new TDerivedUnitComponent
                    {
                        Exponent = 1,
                        Unit = new TMonetaryUnit
                        {
                            Currency = "€"
                        }
                    },
                    new TDerivedUnitComponent
                    {
                        Exponent = -2,
                        Unit = new TSIUnit
                        {
                            Name = SIUnitName.metre,
                            PrefixSpecified = false
                        }
                    }
                }
            };

            // Heat transfer coefficient W/ (m²K)
            var hestTransferCoefficient = new TDerivedUnit
            {
                Component = new TDerivedUnitComponent[]
                {
                    new TDerivedUnitComponent
                    {
                        Exponent = 1,
                        Unit = new TSIUnit
                        {
                            Name = SIUnitName.watt,
                            PrefixSpecified = false
                        }
                    },
                    new TDerivedUnitComponent
                    {
                        Exponent = -2,
                        Unit = new TSIUnit
                        {
                            Name = SIUnitName.metre,
                            PrefixSpecified = false
                        }
                    },
                    new TDerivedUnitComponent
                    {
                        Exponent = -1,
                        Unit = new TSIUnit
                        {
                            Name = SIUnitName.kelvin,
                            PrefixSpecified = false
                        }
                    }
                }
            };

            // Fire resistance(minutes)
            var fireResistance = new TConversionUnit
            {
                Name = "minute",
                Offset = 0,
                Scale = 60,
                Symbol = "min",
                BaseUnit = new TSIUnit
                {
                    Name = SIUnitName.second,
                    PrefixSpecified = false
                }
            };

            // Passenger flow(people/ minute)
            var passengerFlow = new TDerivedUnit
            {
                Component = new TDerivedUnitComponent[]
                {
                    new TDerivedUnitComponent
                    {
                        Exponent = 1,
                        Unit = new TContextDependentUnit
                        {
                            Name = "people",
                            Symbol = "people"
                        }
                    },
                    new TDerivedUnitComponent
                    {
                        Exponent = -1,
                        Unit = new TConversionUnit
                        {
                            Name = "minute",
                            Offset = 0,
                            Scale = 60,
                            Symbol = "min",
                            BaseUnit = new TSIUnit
                            {
                                Name = SIUnitName.second,
                                PrefixSpecified = false
                            }
                        }
                    }
                }
            };

            var units = new TUnitList
            {
                Unit = new TUnit[] {
                reinforcementGrade ,
                unitCost,
                hestTransferCoefficient,
                fireResistance,
                passengerFlow
            }
            };
            var serializer = new XmlSerializer(typeof(TUnitList));

            using var output = File.Create("units.example.xml");
            using var xml = XmlWriter.Create(output, new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  "
            });
            serializer.Serialize(xml, units);
        }
    }
}
