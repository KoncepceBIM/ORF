﻿using ORF;
using ORF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using Xbim.Ifc;
using Xbim.Ifc2x3.FacilitiesMgmtDomain;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.MeasureResource;

namespace ESoupis
{
    public static class Convertor
    {
        public static CostModel Convert(TeSoupis soupis)
        {
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
            using (var model = new CostModel(credentials, stavba.Nazev))
            using (var txn = model.BeginTransaction())
            {
                model.Project.LongName = stavba.SPOPIS;
                model.Project.Address = model.Create.PostalAddress(a => a.AddressLines.Add(stavba.Misto));

                // currency
                var costUnit = model.Create.MonetaryUnit(u => u.Currency = soupis.Mena.ToString());
                model.Project.Units.Add(costUnit);

                var schedule = new CostSchedule(model, stavba.Cislo);

                // subjects
                foreach (var subjekt in stavba.SUBJEKT)
                {
                    var actor = new Actor(model, subjekt.Nazev);
                    schedule.Actors.Add(actor, GetRole(subjekt.Typ));

                    var person = model.Create.Person(p =>
                    {
                        p.GivenName = subjekt.Kontakt;
                    });
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
                    actor["CZ_Actor"] = new PropertySet(model);
                    actor["CZ_Actor"]["DIC"] = new IfcIdentifier(subjekt.DIC);
                }

                foreach (var obj in stavba.OBJEKT)
                {
                    var rootItem = new CostItem(model)
                    {
                        Name = obj.Nazev,
                        Description = obj.OPOPIS,
                        Identifier = obj.Cislo
                    };
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

                        var classificationMap = new Dictionary<string, ClassificationItem>();
                        if (soup.ZATRIDENI != null && soup.ZATRIDENI.Any())
                        {
                            var rootClassification = new Classification(model, soupis.Zdroj.ToString());
                            classificationMap = ConvertClassification(model, soup.ZATRIDENI, rootClassification.Children);
                        }

                        ProcessParts(model, soup.DIL, item, classificationMap);
                    }
                }


                txn.Commit();
                return model;
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

                var unitCost = item.UnitValues.Add();
                unitCost.Value = System.Convert.ToDouble(polozka.JCena);

                AddQuantity(model, item, polozka.MJ, System.Convert.ToDouble(polozka.Mnozstvi));

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

                if (
                    !string.IsNullOrWhiteSpace(polozka.PolozkaZatrideniUID) && 
                    map.TryGetValue(polozka.PolozkaZatrideniUID, out ClassificationItem ci))
                {
                    item.ClassificationItems.Add(ci);
                }
            }
        }

        private static void AddQuantity(CostModel model, CostItem item, string unitStr, double value)
        {

        }

        private static void ProcessParts(CostModel model, List<TDil> dily, CostItem parent, Dictionary<string, ClassificationItem> map)
        {
            if (dily == null)
                return;
            foreach (var dil in dily)
            {
                var item = new CostItem(model)
                {
                    Name = dil.Nazev,
                    Identifier = dil.Cislo,
                    Description = dil.DPOPIS
                };

                item["CZ_CostItem"] = new PropertySet(model);
                item["CZ_CostItem"]["Typ"] = new IfcIdentifier(dil.Typ.ToString());
                parent.Children.Add(item);

                ProcessItems(model, dil.POLOZKA, item, map);
            }
        }

        private static Dictionary<string, ClassificationItem> ConvertClassification(CostModel model, IEnumerable<TZatrideni> zatrideni, ClassificationItemCollection parent)
        {
            var result = new Dictionary<string, ClassificationItem>();
            foreach (var zatr in zatrideni)
            {
                var item = new ClassificationItem(model)
                {
                    Sort = zatr.Typ.ToString(),
                    Name = zatr.Nazev,
                    Identification = zatr.Cislo,
                    Description = zatr.ZPOPIS
                };
                if (zatr.ZATRIDENI?.Any() == true)
                {
                    var children = ConvertClassification(model, zatr.ZATRIDENI, item.Children);
                    foreach (var ch in children)
                        result.Add(ch.Key, ch.Value);
                }
                if (!string.IsNullOrWhiteSpace(zatr.PolozkaZatrideniUID) && !result.ContainsKey(zatr.PolozkaZatrideniUID))
                    result.Add(zatr.PolozkaZatrideniUID, item);
            }
            return result;
        }

        private static IfcRoleEnum GetRole(TTypSubjektu type)
        {
            switch (type)
            {
                case TTypSubjektu.OBJEDNATEL:
                    return IfcRoleEnum.CLIENT;
                case TTypSubjektu.DODAVATEL:
                    return IfcRoleEnum.SUPPLIER;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
