using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ORF.XML.Examples
{
    internal class BasicStructure
    {
        public static void Run()
        {
            var orf = new TORF
            {
                Definice = new TDefinice
                {
                    Jednotka = new List<TJednotka>
                    {
                        new TKonverzniJednotka
                        {
                            Id = "u_12",
                            Meritko = 1000,
                            Nazev = "Tuna",
                            Odsazeni = 0,
                            Symbol = "t",
                            ZakladniJednotka = new TJednotkaSI
                            {
                                Nazev = TNazevJednotkySI.GRAM,
                                Predpona = TPredponaSI.KILO,
                                PredponaSpecified = true
                            }
                        },
                        new TJednotkaZavislaNaKontextu
                        {
                            Id = "u_16",
                            Nazev = "Kus",
                            Symbol = "ks"
                        }
                    }
                },
                Rozpocet = new TRozpocet
                {
                    Id = GetId(),
                    DatumOceneni = DateTime.Now,
                    DatumVytvoreniRozpoctu = DateTime.Now,
                    Nazev = "Test",
                    Objekt = new List<TObjekt> { 
                        // Objekt prvni urovne
                        new TObjekt {
                            Id = GetId(),
                            UzelRozpoctu = new List<TUzel> {
                                // Objekt druhe urovne
                                new TObjekt {
                                    Id = GetId(),
                                    UzelRozpoctu = new List<TUzel> {
                                        // Objekt treti urovne
                                        new TObjekt {
                                            Id = GetId(),
                                            UzelRozpoctu = new List<TUzel> {
                                                // Dil
                                                new TUzelRozpoctu
                                                {
                                                    Id = GetId(),
                                                    Typ = TTypUzlu.DIL,
                                                    Polozka = new List<TPolozkaRozpoctu> {
                                                        // Polozka
                                                        new TPolozkaRozpoctu {
                                                            Id = GetId(),
                                                            Typ = TTypPolozky.HSV,
                                                            Nazev = "HLOUBENÍ RÝH ŠÍŘ DO 2M PAŽ I NEPAŽ TŘ. I",
                                                            Popis = "položka zahrnuje: - vodorovná a svislá doprava, přemístění, přeložení, manipulace s výkopkem - kompletní provedení vykopávky nezapažené i zapažené - ošetření výkopiště po celou dobu práce v něm vč. klimatických opatření - ztížení vykopávek v blízkosti podzemního vedení, konstrukcí a objektů vč. jejich dočasného zajištění - ztížení pod vodou, v okolí výbušnin, ve stísněných prostorech a pod. - příplatek za lepivost - těžení po vrstvách, pásech a po jiných nutných částech (figurách) - čerpání vody vč. čerpacích jímek, potrubí a pohotovostní čerpací soupravy (viz ustanovení k pol. 1151,2) - potřebné snížení hladiny podzemní vody - těžení a rozpojování jednotlivých balvanů - vytahování a nošení výkopku - svahování a přesvah. svahů do konečného tvaru, výměna hornin v podloží a v pláni znehodnocené klimatickými vlivy - ruční vykopávky, odstranění kořenů a napadávek - pažení, vzepření a rozepření vč. přepažování (vyjma štětových stěn) - úpravu, ochranu a očištění dna, základové spáry, stěn a svahů - odvedení nebo obvedení vody v okolí výkopiště a ve výkopišti - třídění výkopku - veškeré pomocné konstrukce umožňující provedení vykopávky (příjezdy, sjezdy, nájezdy, lešení, podpěr. konstr., přemostění, zpevněné plochy, zakrytí a pod.) - nezahrnuje uložení zeminy (na skládku, do násypu) ani poplatky za skládku, vykazují se v položce č.0141**",
                                                            Mnozstvi = new TMnozstvi{
                                                                Jednotka = new TJednotkaRef { Ref = "u_12" },
                                                                Mnozstvi = 15.123M,
                                                                VykazVymer = new List<Mrow>
                                                                {
                                                                    new Mrow
                                                                    {
                                                                        ItemElementName = ItemChoiceType2.semantics,
                                                                        Item = new Semantics {
                                                                            Annotation = "Odkop pro zřízení zatrubnění vjezdu:",
                                                                            ItemElementName = ItemChoiceType1.apply,
                                                                            Item = new Apply {
                                                                                Item = new Csymbol(),
                                                                                ItemElementName = ItemChoiceType.eq,

                                                                                Items = new object [] {
                                                                                    new Apply
                                                                                    {
                                                                                        Item = new Csymbol(),
                                                                                        ItemElementName = ItemChoiceType.divide,

                                                                                        Items = new object []
                                                                                        {
                                                                                            new Apply
                                                                                            {
                                                                                                Item = new Csymbol(),
                                                                                                ItemElementName = ItemChoiceType.plus,

                                                                                                Items = new object[]
                                                                                                {
                                                                                                    new ValueFromModel { 
                                                                                                        Value = 1.8, 
                                                                                                        Modelpath = "ABC_456_Model.ifc",
                                                                                                        Entityid = "0ma8jNZV9AnwbYak9012AW",
                                                                                                        Propertyset = "CZ_Base_Quantities",
                                                                                                        Property = "Length"
                                                                                                    },
                                                                                                    new ValueFromModel { 
                                                                                                        Value = 3.8 ,
                                                                                                        Modelpath = "ABC_456_Model.ifc",
                                                                                                        Entityid = "1iQXL_8d1BdQHrPU$IKgI4",
                                                                                                        Propertyset = "CZ_Base_Quantities",
                                                                                                        Property = "Length"
                                                                                                    }
                                                                                                },
                                                                                                ItemsElementName = new ItemsChoiceType[] {
                                                                                                    ItemsChoiceType.cn,
                                                                                                    ItemsChoiceType.cn
                                                                                                }
                                                                                            },
                                                                                            new Apply
                                                                                            {
                                                                                                Item = new Csymbol(),
                                                                                                ItemElementName = ItemChoiceType.times,

                                                                                                Items = new object[]
                                                                                                {
                                                                                                    new ValueFromModel { 
                                                                                                        Value = 2,
                                                                                                        Modelpath = "ABC_456_Model.ifc",
                                                                                                        Entityid = "1iQXL_8d1BdQHrPU$IKgI4",
                                                                                                        Propertyset = "CZ_Base_Quantities",
                                                                                                        Property = "Height"
                                                                                                    },
                                                                                                    new ValueFromModel { 
                                                                                                        Value = 1.5,
                                                                                                        Modelpath = "ABC_456_Model.ifc",
                                                                                                        Entityid = "1iQXL_8d1BdQHrPU$IKgI4",
                                                                                                        Propertyset = "CZ_Base_Quantities",
                                                                                                        Property = "Width"
                                                                                                    },
                                                                                                    new ValueFromModel { 
                                                                                                        Value = 200,
                                                                                                        Modelpath = "ABC_456_Model.ifc",
                                                                                                        Entityid = "1iQXL_8d1BdQHrPU$IKgI4",
                                                                                                        Propertyset = "CZ_Base_Quantities",
                                                                                                        Property = "Depth"
                                                                                                    }
                                                                                                },
                                                                                                ItemsElementName = new ItemsChoiceType[] {
                                                                                                    ItemsChoiceType.cn,
                                                                                                    ItemsChoiceType.cn,
                                                                                                    ItemsChoiceType.cn
                                                                                                }
                                                                                            }
                                                                                        },
                                                                                        ItemsElementName = new ItemsChoiceType[] {
                                                                                            ItemsChoiceType.apply,
                                                                                            ItemsChoiceType.apply
                                                                                        }
                                                                                    },
                                                                                    new Ci { Type = CiType.real, Value = "A" }
                                                                                },
                                                                                ItemsElementName = new ItemsChoiceType[]{ 
                                                                                    ItemsChoiceType.apply,
                                                                                    ItemsChoiceType.ci
                                                                                }
                                                                            } 
                                                                        }
                                                                    },
                                                                    new Mrow
                                                                    {
                                                                        ItemElementName = ItemChoiceType2.semantics,
                                                                        Item = new Semantics {
                                                                            Annotation = "Odečet trub a odečet vozovky:",
                                                                            ItemElementName = ItemChoiceType1.apply,
                                                                            Item = new Apply {
                                                                                Item = new Csymbol(),
                                                                                ItemElementName = ItemChoiceType.eq,

                                                                                Items = new object [] {
                                                                                    new Apply
                                                                                    {
                                                                                        Item = new Csymbol(),
                                                                                        ItemElementName = ItemChoiceType.sum,

                                                                                        Items = new object []
                                                                                        {
                                                                                            new Cn { Value = -113.1 },
                                                                                            new Cn { Value = -226.68 }
                                                                                        },
                                                                                        ItemsElementName = new ItemsChoiceType[] {
                                                                                            ItemsChoiceType.cn,
                                                                                            ItemsChoiceType.cn
                                                                                        }
                                                                                    },
                                                                                    new Ci { Type = CiType.real, Value = "B" }
                                                                                },
                                                                                ItemsElementName = new ItemsChoiceType[]{
                                                                                    ItemsChoiceType.apply,
                                                                                    ItemsChoiceType.ci
                                                                                }
                                                                            }
                                                                        }
                                                                    },
                                                                    new Mrow
                                                                    {
                                                                        ItemElementName = ItemChoiceType2.semantics,
                                                                        Item = new Semantics {
                                                                            Annotation = "Výkop rýh pro betonové prahy:",
                                                                            ItemElementName = ItemChoiceType1.apply,
                                                                            Item = new Apply {
                                                                                Item = new Csymbol(),
                                                                                ItemElementName = ItemChoiceType.eq,

                                                                                Items = new object [] {
                                                                                    new Apply
                                                                                    {
                                                                                        Item = new Csymbol(),
                                                                                        ItemElementName = ItemChoiceType.times,

                                                                                        Items = new object []
                                                                                        {
                                                                                            new Cn { Value = 10.0 },
                                                                                            new Cn { Value = 2.0 },
                                                                                            new Cn { Value = 0.5 },
                                                                                            new Cn { Value = 0.7 },
                                                                                            new Cn { Value = 1.0 }
                                                                                        },
                                                                                        ItemsElementName = new ItemsChoiceType[] {
                                                                                            ItemsChoiceType.cn,
                                                                                            ItemsChoiceType.cn,
                                                                                            ItemsChoiceType.cn,
                                                                                            ItemsChoiceType.cn,
                                                                                            ItemsChoiceType.cn
                                                                                        }
                                                                                    },
                                                                                    new Ci { Type = CiType.real, Value = "C" }
                                                                                },
                                                                                ItemsElementName = new ItemsChoiceType[]{
                                                                                    ItemsChoiceType.apply,
                                                                                    ItemsChoiceType.ci
                                                                                }
                                                                            }
                                                                        }
                                                                    },
                                                                    new Mrow
                                                                    {
                                                                        ItemElementName = ItemChoiceType2.semantics,
                                                                        Item = new Semantics {
                                                                            Annotation = "Odečet trub a odečet vozovky:",
                                                                            ItemElementName = ItemChoiceType1.apply,
                                                                            Item = new Apply {
                                                                                Item = new Csymbol(),
                                                                                ItemElementName = ItemChoiceType.eq,

                                                                                Items = new object [] {
                                                                                    new Apply
                                                                                    {
                                                                                        Item = new Csymbol(),
                                                                                        ItemElementName = ItemChoiceType.sum,

                                                                                        Items = new object []
                                                                                        {
                                                                                            new Cn { Value = -113.1 },
                                                                                            new Cn { Value = -226.68 }
                                                                                        },
                                                                                        ItemsElementName = new ItemsChoiceType[] {
                                                                                            ItemsChoiceType.cn,
                                                                                            ItemsChoiceType.cn
                                                                                        }
                                                                                    },
                                                                                    new Ci { Type = CiType.real, Value = "B" }
                                                                                },
                                                                                ItemsElementName = new ItemsChoiceType[]{
                                                                                    ItemsChoiceType.apply,
                                                                                    ItemsChoiceType.ci
                                                                                }
                                                                            }
                                                                        }
                                                                    },
                                                                    new Mrow
                                                                    {
                                                                        ItemElementName = ItemChoiceType2.semantics,
                                                                        Item = new Semantics {
                                                                            Annotation = "Celkem:",
                                                                            ItemElementName = ItemChoiceType1.apply,
                                                                            Item = new Apply {
                                                                                Item = new Csymbol(),
                                                                                ItemElementName = ItemChoiceType.eq,

                                                                                Items = new object [] {
                                                                                    new Apply
                                                                                    {
                                                                                        Item = new Csymbol(),
                                                                                        ItemElementName = ItemChoiceType.sum,

                                                                                        Items = new object []
                                                                                        {
                                                                                            new Ci {Value = "A"},
                                                                                            new Ci {Value = "B"},
                                                                                            new Ci {Value = "C"}
                                                                                        },
                                                                                        ItemsElementName = new ItemsChoiceType[] {
                                                                                            ItemsChoiceType.ci,
                                                                                            ItemsChoiceType.ci,
                                                                                            ItemsChoiceType.ci
                                                                                        }
                                                                                    },
                                                                                    new Ci { Type = CiType.real, Value = "D" }
                                                                                },
                                                                                ItemsElementName = new ItemsChoiceType[]{
                                                                                    ItemsChoiceType.apply,
                                                                                    ItemsChoiceType.ci
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            },

                                                        }
                                                    , new TPolozkaRozpoctu {
                                                        Id = GetId(),
                                                        Typ = TTypPolozky.HSV,
                                                        TypSpecified = true,
                                                        Nazev = "Počet kusů potrubí",
                                                        Popis = "Počet kusů potrubí",
                                                        Mnozstvi = new TMnozstvi
                                                        {
                                                            Mnozstvi = 5,
                                                            Jednotka = new TJednotkaRef{ Ref = "u_16"},
                                                            VykazVymer = new List<Mrow>
                                                            {
                                                                new Mrow
                                                                {
                                                                    Item = new Apply {
                                                                        Item = new Csymbol(),
                                                                        ItemElementName = ItemChoiceType.sum,

                                                                        Items = new object[]{ 
                                                                            new ValueFromModel
                                                                            {
                                                                                Value = 1,
                                                                                Entityid =  PseudoIFCGuid(),
                                                                                Modelpath = "ABC_456_Model.ifc"
                                                                            },
                                                                            new ValueFromModel
                                                                            {
                                                                                Value = 1,
                                                                                Entityid = PseudoIFCGuid(),
                                                                                Modelpath = "ABC_456_Model.ifc"
                                                                            },
                                                                            new ValueFromModel
                                                                            {
                                                                                Value = 1,
                                                                                Entityid = PseudoIFCGuid(),
                                                                                Modelpath = "ABC_456_Model.ifc"
                                                                            },
                                                                            new ValueFromModel
                                                                            {
                                                                                Value = 1,
                                                                                Entityid = PseudoIFCGuid(),
                                                                                Modelpath = "ABC_456_Model.ifc"
                                                                            },
                                                                            new ValueFromModel
                                                                            {
                                                                                Value = 1,
                                                                                Entityid = PseudoIFCGuid(),
                                                                                Modelpath = "ABC_456_Model.ifc"
                                                                            }
                                                                        },
                                                                        ItemsElementName = new ItemsChoiceType[]{ 
                                                                            ItemsChoiceType.cn,
                                                                            ItemsChoiceType.cn,
                                                                            ItemsChoiceType.cn,
                                                                            ItemsChoiceType.cn,
                                                                            ItemsChoiceType.cn,
                                                                        }

                                                                    },
                                                                    ItemElementName = ItemChoiceType2.apply
                                                                }
                                                            }
                                                        }
                                                    }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var serializer = new XmlSerializer(typeof(TORF));
            using (var w = File.CreateText("orf_test.xml"))
            using (var xml = XmlWriter.Create(w, new XmlWriterSettings { Indent = true, IndentChars = "  "}))
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add("o", "http://www.koncepcebim.cz/ORF_v1.0.xsd");
                ns.Add("m", "http://www.w3.org/1998/Math/MathML");
                ns.Add("xs", "http://www.w3.org/2001/XMLSchema-instance");

                serializer.Serialize(xml, orf, ns);
            }
        }

        private static int counter = 1000;
        private static string GetId() => $"i{counter++}";

        private static string PseudoIFCGuid()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("/", "-").Replace("+", "_").Replace("=", "");
        }
    }
}
