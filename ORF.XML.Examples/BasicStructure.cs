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
                                                                                                    new Cn { Value = 1.8 },
                                                                                                    new Cn { Value = 3.8 }
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
                                                                                                    new Cn { Value = 2 },
                                                                                                    new Cn { Value = 1.5 },
                                                                                                    new Cn { Value = 200 }
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
                                                                                    new Ci { Type = CiType.real, TypeSpecified = true, Value = "A" }
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
                serializer.Serialize(xml, orf);
            }
        }

        private static int counter = 1000;
        private static string GetId() => $"i{counter++}";
    }
}
