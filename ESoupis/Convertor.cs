using ORF;
using ORF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.MeasureResource;

namespace ESoupis
{
    public static class Convertor
    {
        public static CostModel Convert(TeSoupis soupis)
        {
            unitCache.Clear();

            // information about the application, person and organisation
            // who created the file
            var credentials = new XbimEditorCredentials
            {
                ApplicationDevelopersName = "Martin Cerny",
                ApplicationFullName = soupis.Zdroj.ToString(),
                ApplicationIdentifier = soupis.Zdroj.ToString(),
                ApplicationVersion = "1.0",
                //your user
                EditorsFamilyName = Environment.UserName,
                EditorsGivenName = "",
                EditorsOrganisationName = Environment.UserDomainName
            };

            var stavba = soupis.STAVBA.FirstOrDefault();
            var model = new CostModel(credentials, stavba.Nazev);
            using (var txn = model.BeginTransaction())
            {
                // currency
                var costUnit = model.Create.MonetaryUnit(u => u.Currency = soupis.Mena.ToString());
                model.Project.Units.Add(costUnit);

                // project information
                model.Project.LongName = stavba.SPOPIS;
                model.Project.Address = model.Create.PostalAddress(a => a.AddressLines.Add(stavba.Misto));

                // process all sites
                foreach (var s in soupis.STAVBA)
                {
                    // root element
                    var schedule = new CostSchedule(model, s.Cislo);

                    // subjects (client, supplier)
                    ProcessActors(model, schedule, s.SUBJEKT);

                    // schedule items and classification
                    ProcessObjects(model, schedule, s.OBJEKT);
                }

                // commit changes
                txn.Commit();
                return model;
            }
        }

        private static void ProcessObjects(CostModel model, CostSchedule schedule, IEnumerable<TObjekt> objekty)
        {
            if (objekty == null || !objekty.Any())
                return;

            foreach (var obj in objekty)
            {
                // root element of the cost schedule
                var rootItem = new CostItem(model)
                {
                    Name = obj.Nazev,
                    Description = obj.OPOPIS,
                    Identifier = obj.Cislo
                };

                // additional properties can be stored in custom property sets
                rootItem["CZ_CostItem"] = new PropertySet(model);
                rootItem["CZ_CostItem"]["CisloJKSO"] = new IfcIdentifier(obj.CisloJKSO);
                rootItem["CZ_CostItem"]["NazevJKSO"] = new IfcIdentifier(obj.NazevJKSO);
                rootItem["CZ_CostItem"]["Charakteristika"] = new IfcText(obj.Charakteristika);
                rootItem["CZ_CostItem"]["DruhStavebniAkce"] = new IfcText(obj.DruhStavebniAkce);
                schedule.CostItemRoots.Add(rootItem);

                foreach (var soup in obj.SOUPIS)
                {
                    var item = new CostItem(model)
                    {
                        Name = soup.Nazev,
                        Identifier = soup.Cislo,
                        Description = soup.RPOPIS
                    };
                    rootItem.Children.Add(item);

                    // classification hierarchy in IFC. Cost Items will be related to Classification Items
                    var classificationMap = new Dictionary<string, ClassificationItem>();
                    if (soup.ZATRIDENI != null && soup.ZATRIDENI.Any())
                    {
                        var rootClassificationName = GetClassificationName(soup);
                        var rootClassification = new Classification(model, rootClassificationName);
                        classificationMap = ConvertClassification(model, soup.ZATRIDENI, rootClassification.Children);
                    }

                    // next level of the cost breakdown structure
                    ProcessParts(model, soup.DIL, item, classificationMap);
                }
            }
        }

        private static string GetClassificationName(TSoupis soupis)
        {
            if (soupis.DIL == null || !soupis.DIL.Any())
                return "Unknown";

            var ids = new HashSet<string>();
            var level = soupis.ZATRIDENI ?? new List<TZatrideni>();
            while (level.Any())
            {
                foreach (var item in level)
                    ids.Add(item.PolozkaZatrideniUID);
                level = level.SelectMany(l => l.ZATRIDENI ?? new List<TZatrideni>()).ToList();
            }

            return soupis.DIL
                .SelectMany(d => d.POLOZKA ?? new List<TPolozka>())
                .Where(p => !string.IsNullOrEmpty(p.PolozkaZatrideniUID) && !string.IsNullOrWhiteSpace(p.CenovaSoustava))
                .FirstOrDefault(p => ids.Contains(p.PolozkaZatrideniUID))?
                .CenovaSoustava ?? "Unknown";
        }

        private static void ProcessActors(CostModel model, CostSchedule schedule, IEnumerable<TSubjekt> subjekty)
        {
            if (subjekty == null || !subjekty.Any())
                return;

            foreach (var subjekt in subjekty)
            {
                // actor will be assigned to schedule through IfcRelAssignsToActor
                var actor = new Actor(model, subjekt.Nazev);
                var role = GetRole(subjekt.Typ);
                if (role != IfcRoleEnum.USERDEFINED)
                    // prefered way is to use one of the predefined roles
                    schedule.Actors.Add(actor, role);
                else
                    // extensibility allows to use any string as a role
                    schedule.Actors.Add(actor, subjekt.Typ.ToString());

                // actor can have person
                var person = model.Create.Person(p =>
                {
                    p.GivenName = subjekt.Kontakt;
                });
                // and organization
                var organization = model.Create.Organization(o =>
                {
                    o.Identification = subjekt.ICO;
                    o.Name = subjekt.Nazev;
                    o.Addresses.Add(model.Create.PostalAddress(a =>
                    {
                        a.AddressLines.Add(subjekt.Adresa);
                        a.Town = subjekt.Misto;
                        a.PostalCode = subjekt.PSC;
                        a.Country = subjekt.Stat;
                    }));
                    o.Addresses.Add(model.Create.TelecomAddress(a =>
                    {
                        a.ElectronicMailAddresses.Add(subjekt.Email);
                        a.TelephoneNumbers.Add(subjekt.Telefon);
                    }));
                });
                actor.Entity.TheActor = model.Create.PersonAndOrganization(po =>
                {
                    po.ThePerson = person;
                    po.TheOrganization = organization;
                });
                // additional information can be stored using custom property set(s)
                actor["CZ_Actor"] = new PropertySet(model);
                actor["CZ_Actor"]["DIC"] = new IfcIdentifier(subjekt.DIC);
            }
        }

        private static void ProcessItems(CostModel model, List<TPolozka> polozky, CostItem parent, Dictionary<string, ClassificationItem> map)
        {
            if (polozky == null)
                return;
            foreach (var polozka in polozky)
            {
                var item = new CostItem(model)
                {
                    Name = polozka.Nazev,
                    Identifier = polozka.Cislo,
                    Description = polozka.PPOPIS
                };

                // unit cost. There might be more than one or it can be modeled as a hierarchy with operators
                var unitCost = new CostValue(model) {  Value = System.Convert.ToDouble(polozka.JCena) };
                // in the base case there is just one
                item.UnitValues.Add(unitCost);

                // Quantities have measure type and unit, if it is not cost
                AddQuantity(model, item, polozka.MJ, System.Convert.ToDouble(polozka.Mnozstvi));

                // additional properties can be stored in custom property sets
                item["CZ_CostItem"] = new PropertySet(model);
                item["CZ_CostItem"]["UID"] = new IfcIdentifier(polozka.UID);
                item["CZ_CostItem"]["Typ"] = new IfcIdentifier(polozka.Typ.ToString());
                item["CZ_CostItem"]["JHmotnost"] = new IfcMassMeasure(System.Convert.ToDouble(polozka.JHmotnost));
                item["CZ_CostItem"]["JDemontazniHmotnost"] = new IfcMassMeasure(System.Convert.ToDouble(polozka.JDemontazniHmotnost));
                item["CZ_CostItem"]["SazbaDPH"] = new IfcRatioMeasure(System.Convert.ToDouble(polozka.SazbaDPH) / 100);
                item["CZ_CostItem"]["ObchNazev"] = new IfcIdentifier(polozka.ObchNazev);
                item["CZ_CostItem"]["ObchNazevAN"] = new IfcBoolean(polozka.ObchNazevAN);
                item["CZ_CostItem"]["PoradoveCislo"] = new IfcInteger(polozka.PoradoveCislo);
                parent.Children.Add(item);

                // assign classification item if defined
                if (
                    !string.IsNullOrWhiteSpace(polozka.PolozkaZatrideniUID) &&
                    map.TryGetValue(polozka.PolozkaZatrideniUID, out ClassificationItem ci))
                {
                    item.ClassificationItems.Add(ci);
                }
            }
        }

        /// <summary>
        /// Ad quantity with specific measure type and unit
        /// </summary>
        /// <param name="model"></param>
        /// <param name="item"></param>
        /// <param name="unitStr"></param>
        /// <param name="value"></param>
        private static void AddQuantity(CostModel model, CostItem item, string unitStr, double value)
        {
            // units are machine readable and all basic units are expresed using fixed enumerations related to SI (ISO/IEC 80000).
            // Other units are expressed as conversion based units with exact relation to SI units
            var unit = GetUnit(model, unitStr);
            Quantity quantity;
            switch (unit?.UnitType)
            {
                case IfcUnitEnum.AREAUNIT:
                    quantity = item.Quantities.AddArea("Area");
                    break;
                case IfcUnitEnum.LENGTHUNIT:
                    quantity = item.Quantities.AddLength("Length");
                    break;
                case IfcUnitEnum.MASSUNIT:
                    quantity = item.Quantities.AddWeight("Weight");
                    break;
                case IfcUnitEnum.TIMEUNIT:
                    quantity = item.Quantities.AddTime("Time");
                    break;
                case IfcUnitEnum.VOLUMEUNIT:
                    quantity = item.Quantities.AddVolume("Volume");
                    break;
                default:
                    quantity = item.Quantities.AddCount("Count");
                    break;
            }
            quantity.Unit = unit;
            quantity.Value = value;
        }

        private static IIfcNamedUnit GetUnit(CostModel model, string name)
        {
            // minimal level of normalization
            name = name
                .ToLowerInvariant()
                .Replace(" ", "")
                .Trim('-')
                .Trim();

            if (unitCache.TryGetValue(name, out IIfcNamedUnit result))
                return result;

            // units based on known string representations
            result = CreateUnit(model, name);
            if (result != null)
                unitCache.Add(name, result);

            return result;
        }

        /// <summary>
        /// Creates units based on known string representations
        /// </summary>
        /// <param name="model"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static IIfcNamedUnit CreateUnit(CostModel model, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            var c = model.Create;
            switch (name)
            {
                case "m":
                case "mb":
                    return c.SIUnit(u =>
                    {
                        u.UnitType = IfcUnitEnum.LENGTHUNIT;
                        u.Name = IfcSIUnitName.METRE;
                    });
                case "mm":
                    return c.SIUnit(u =>
                    {
                        u.UnitType = IfcUnitEnum.LENGTHUNIT;
                        u.Name = IfcSIUnitName.METRE;
                        u.Prefix = IfcSIPrefix.MILLI;
                    });
                case "km":
                    return c.SIUnit(u =>
                    {
                        u.UnitType = IfcUnitEnum.LENGTHUNIT;
                        u.Name = IfcSIUnitName.METRE;
                        u.Prefix = IfcSIPrefix.KILO;
                    });
                case "m2":
                    return c.SIUnit(u =>
                    {
                        u.UnitType = IfcUnitEnum.AREAUNIT;
                        u.Name = IfcSIUnitName.SQUARE_METRE;
                    });
                case "m3":
                    return c.SIUnit(u =>
                    {
                        u.UnitType = IfcUnitEnum.VOLUMEUNIT;
                        u.Name = IfcSIUnitName.CUBIC_METRE;
                    });
                case "kg":
                    return c.SIUnit(u =>
                    {
                        u.UnitType = IfcUnitEnum.MASSUNIT;
                        u.Name = IfcSIUnitName.GRAM;
                        u.Prefix = IfcSIPrefix.KILO;
                    });

                case "t":
                    return c.ConversionBasedUnit(cu =>
                    {
                        cu.Name = "t";
                        cu.ConversionFactor = c.MeasureWithUnit(mu =>
                        {
                            mu.UnitComponent = c.SIUnit(u =>
                            {
                                u.UnitType = IfcUnitEnum.MASSUNIT;
                                u.Name = IfcSIUnitName.GRAM;
                                u.Prefix = IfcSIPrefix.KILO;
                            });
                            mu.ValueComponent = new IfcReal(1000);
                        });
                        cu.UnitType = IfcUnitEnum.MASSUNIT;
                        cu.Dimensions = c.DimensionalExponents(e =>
                        {
                            e.LengthExponent = 0;
                            e.MassExponent = 1;
                            e.TimeExponent = 0;
                            e.ElectricCurrentExponent = 0;
                            e.ThermodynamicTemperatureExponent = 0;
                            e.AmountOfSubstanceExponent = 0;
                            e.LuminousIntensityExponent = 0;
                        });
                    });
                default:
                    return c.ContextDependentUnit(u =>
                    {
                        u.UnitType = IfcUnitEnum.USERDEFINED;
                        u.Name = name;
                        u.Dimensions = c.DimensionalExponents(e =>
                        {
                            e.LengthExponent = 0;
                            e.MassExponent = 0;
                            e.TimeExponent = 0;
                            e.ElectricCurrentExponent = 0;
                            e.ThermodynamicTemperatureExponent = 0;
                            e.AmountOfSubstanceExponent = 0;
                            e.LuminousIntensityExponent = 0;
                        });
                    });
            }
        }

        /// <summary>
        /// Cache of units
        /// </summary>
        private static readonly Dictionary<string, IIfcNamedUnit> unitCache = new Dictionary<string, IIfcNamedUnit>();

        private static void ProcessParts(CostModel model, List<TDil> dily, CostItem parent, Dictionary<string, ClassificationItem> map)
        {
            if (dily == null)
                return;
            foreach (var dil in dily)
            {
                // grouping level in the cost breakdown hierarchy
                var item = new CostItem(model)
                {
                    Name = dil.Nazev,
                    Identifier = dil.Cislo,
                    Description = dil.DPOPIS
                };

                // additional properties can be stored in custom property sets
                item["CZ_CostItem"] = new PropertySet(model);
                item["CZ_CostItem"]["Typ"] = new IfcIdentifier(dil.Typ.ToString());
                parent.Children.Add(item);

                // leafs in the cost breakdown hierarchy
                ProcessItems(model, dil.POLOZKA, item, map);
            }
        }

        private static Dictionary<string, ClassificationItem> ConvertClassification(CostModel model, IEnumerable<TZatrideni> zatrideni, ClassificationItemCollection parent)
        {
            var result = new Dictionary<string, ClassificationItem>();
            foreach (var zatr in zatrideni)
            {
                // classification items model classification hierarchy (breakdown structure)
                // there might be several classifications in the model
                var item = new ClassificationItem(model)
                {
                    Sort = zatr.Typ.ToString(),
                    Name = zatr.Nazev,
                    Identification = zatr.Cislo,
                    Description = zatr.ZPOPIS
                };

                // nested level of classification breakdown
                if (zatr.ZATRIDENI?.Any() == true)
                {
                    var children = ConvertClassification(model, zatr.ZATRIDENI, item.Children);
                    foreach (var ch in children)
                        result.Add(ch.Key, ch.Value);
                }

                // keep key in the lookup for later use
                if (!string.IsNullOrWhiteSpace(zatr.PolozkaZatrideniUID) && !result.ContainsKey(zatr.PolozkaZatrideniUID))
                    result.Add(zatr.PolozkaZatrideniUID, item);
            }
            return result;
        }

        /// <summary>
        /// Mapping roles between schemas
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static IfcRoleEnum GetRole(TTypSubjektu type)
        {
            switch (type)
            {
                case TTypSubjektu.OBJEDNATEL:
                    return IfcRoleEnum.CLIENT;
                case TTypSubjektu.DODAVATEL:
                    return IfcRoleEnum.SUPPLIER;
                default:
                    return IfcRoleEnum.USERDEFINED;
            }
        }
    }
}
