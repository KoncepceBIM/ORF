using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ORF.XML.Examples
{
    internal class PredefinedUnits
    {
        public static void Run()
        {
            var units = new List<TJednotka>();

            var mesic = new TKonverzniJednotka
            {
                Meritko = 60 * 60 * 24 * 30,
                Nazev = "Měsíc",
                Odsazeni = 0,
                Symbol = "měsíc",
                ZakladniJednotka = new TJednotkaSI
                {
                    Nazev = TNazevJednotkySI.SECOND,
                    PredponaSpecified = false
                }
            };
            var ks = new TJednotkaZavislaNaKontextu
            {
                Nazev = "Kus",
                Symbol = "ks"
            };
            var komplet = new TJednotkaZavislaNaKontextu
            {
                Nazev = "Komplet",
                Symbol = "komplet"
            };
            var den = new TKonverzniJednotka
            {
                Meritko = 60 * 60 * 24,
                Nazev = "Den",
                Odsazeni = 0,
                Symbol = "den",
                ZakladniJednotka = new TJednotkaSI
                {
                    Nazev = TNazevJednotkySI.SECOND,
                    PredponaSpecified = false
                }
            };

            units.Add(new TOdvozenaJednotka
            {
                Id = "u_01",
                Komponenta = new List<TKomponentaJednotky> {
                    new TKomponentaJednotky{
                        Exponent = 1,
                        Jednotka = ks
                    },
                    new TKomponentaJednotky
                    {
                        Exponent = -1,
                        Jednotka = mesic
                    }
                }
            });

            units.Add(new TJednotkaZavislaNaKontextu
            {
                Id = "u_02",
                Nazev = "Komplet",
                Symbol = "komplet"
            });

            units.Add(new TJednotkaSI
            {
                Id = "u_03",
                Nazev = TNazevJednotkySI.METRE
            });

            units.Add(new TOdvozenaJednotka
            {
                Id = "u_04",
                Komponenta = new List<TKomponentaJednotky> {
                    new TKomponentaJednotky{
                        Exponent = 2,
                        Jednotka = new TJednotkaSI
                        {
                            Nazev = TNazevJednotkySI.METRE
                        }
                    }
                }
            });

            units.Add(new TOdvozenaJednotka
            {
                Id = "u_05",
                Komponenta = new List<TKomponentaJednotky> {
                    new TKomponentaJednotky{
                        Exponent = 3,
                        Jednotka = new TJednotkaSI
                        {
                            Nazev = TNazevJednotkySI.METRE
                        }
                    }
                }
            });

            units.Add(new TJednotkaSI
            {
                Id = "u_06",
                Nazev = TNazevJednotkySI.METRE,
                Predpona = TPredponaSI.CENTI,
                PredponaSpecified = true
            });

            units.Add(new TOdvozenaJednotka
            {
                Id = "u_07",
                Komponenta = new List<TKomponentaJednotky> {
                    new TKomponentaJednotky{
                        Exponent = 2,
                        Jednotka = new TJednotkaSI
                        {
                            Nazev = TNazevJednotkySI.METRE,
                            Predpona = TPredponaSI.CENTI,
                            PredponaSpecified = true
                        }
                    }
                }
            });

            units.Add(new TKonverzniJednotka
            {
                Id = "u_08",
                Meritko = 100,
                Nazev = "Ar",
                Symbol = "ar",
                Odsazeni = 0,
                ZakladniJednotka = new TOdvozenaJednotka
                {
                    Komponenta = new List<TKomponentaJednotky> {
                    new TKomponentaJednotky{
                        Exponent = 2,
                        Jednotka = new TJednotkaSI
                        {
                            Nazev = TNazevJednotkySI.METRE
                        }
                    }
                }
                }
            });

            units.Add(new TJednotkaZavislaNaKontextu
            {
                Id = "u_09",
                Nazev = "Balení",
                Symbol = "balení"
            });

            units.Add(new TKonverzniJednotka
            {
                Id = "u_10",
                Meritko = 60 * 60 * 24,
                Nazev = "Den",
                Odsazeni = 0,
                Symbol = "den",
                ZakladniJednotka = new TJednotkaSI
                {
                    Nazev = TNazevJednotkySI.SECOND,
                    PredponaSpecified = false
                }
            });

            units.Add(new TKonverzniJednotka
            {
                Id = "u_11",
                Meritko = 60 * 60,
                Nazev = "Hodina",
                Odsazeni = 0,
                Symbol = "h",
                ZakladniJednotka = new TJednotkaSI
                {
                    Nazev = TNazevJednotkySI.SECOND,
                    PredponaSpecified = false
                }
            });

            units.Add(new TKonverzniJednotka
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
            });

            units.Add(new TJednotkaSI {
                Id = "u_13",
                Nazev = TNazevJednotkySI.GRAM,
                Predpona = TPredponaSI.KILO,
                PredponaSpecified = true
            });

            units.Add(new TKonverzniJednotka
            {
                Id = "u_14",
                Meritko = 60,
                Nazev = "Minuta",
                Odsazeni = 0,
                Symbol = "min",
                ZakladniJednotka = new TJednotkaSI
                {
                    Nazev = TNazevJednotkySI.SECOND,
                    PredponaSpecified = false
                }
            });

            units.Add(new TKonverzniJednotka
            {
                Id = "u_15",
                Meritko = 10000,
                Nazev = "Hektar",
                Odsazeni = 0,
                Symbol = "ha",
                ZakladniJednotka = new TOdvozenaJednotka
                {
                    Komponenta = new List<TKomponentaJednotky> {
                    new TKomponentaJednotky{
                        Exponent = 2,
                        Jednotka = new TJednotkaSI
                        {
                            Nazev = TNazevJednotkySI.METRE
                        }
                    }
                }
                }
            });

            units.Add(new TJednotkaZavislaNaKontextu
            {
                Id = "u_16",
                Nazev = "Kus",
                Symbol = "ks"
            });

            units.Add(new TKonverzniJednotka
            {
                Id = "u_17",
                Meritko = 1,
                Nazev = "Litr",
                Odsazeni = 0,
                Symbol = "l",
                ZakladniJednotka = new TOdvozenaJednotka
                {
                    Komponenta = new List<TKomponentaJednotky> {
                    new TKomponentaJednotky{
                        Exponent = 3,
                        Jednotka = new TJednotkaSI
                        {
                            Nazev = TNazevJednotkySI.METRE,
                            Predpona = TPredponaSI.DECI,
                            PredponaSpecified = true
                        }
                    }
                }
                }
            });

            units.Add(new TJednotkaSI
            {
                Id = "u_18",
                Nazev = TNazevJednotkySI.METRE,
                Predpona = TPredponaSI.KILO,
                PredponaSpecified = true
            });

            units.Add(new TJednotkaSI
            {
                Id = "u_19",
                Nazev = TNazevJednotkySI.METRE,
                Predpona = TPredponaSI.DECI,
                PredponaSpecified = true
            });

            units.Add(new TOdvozenaJednotka
            {
                Id = "u_20",
                Komponenta = new List<TKomponentaJednotky> {
                    new TKomponentaJednotky {
                        Exponent = 2,
                        Jednotka = new TJednotkaSI
                        {
                            Nazev = TNazevJednotkySI.METRE,
                            Predpona = TPredponaSI.DECI,
                            PredponaSpecified = true
                        }
                    }
                }
            });

            units.Add(new TKonverzniJednotka
            {
                Id = "u_21",
                Meritko = 1,
                Nazev = "Mililitr",
                Odsazeni = 0,
                Symbol = "ml",
                ZakladniJednotka = new TOdvozenaJednotka
                {
                    Komponenta = new List<TKomponentaJednotky> {
                    new TKomponentaJednotky{
                        Exponent = 3,
                        Jednotka = new TJednotkaSI
                        {
                            Nazev = TNazevJednotkySI.METRE,
                            Predpona = TPredponaSI.CENTI,
                            PredponaSpecified = true
                        }
                    }
                }
                }
            });

            units.Add(new TPenezniJednotka
            {
                Id = "u_22",
                Mena = TMena.CZK
            });

            units.Add(new TKonverzniJednotka {
                Id = "u_23",
                Nazev = "10 kusů",
                Symbol = "10 ks",
                Meritko = 10,
                Odsazeni = 0,
                ZakladniJednotka = new TJednotkaZavislaNaKontextu {
                    Nazev = "kus",
                    Symbol = "ks"
                }
            });

            units.Add(new TKonverzniJednotka
            {
                Id = "u_24",
                Nazev = "100 kusů",
                Symbol = "100 ks",
                Meritko = 100,
                Odsazeni = 0,
                ZakladniJednotka = new TJednotkaZavislaNaKontextu
                {
                    Nazev = "kus",
                    Symbol = "ks"
                }
            });

            units.Add(new TKonverzniJednotka
            {
                Id = "u_25",
                Nazev = "1000 kusů",
                Symbol = "1000 ks",
                Meritko = 1000,
                Odsazeni = 0,
                ZakladniJednotka = new TJednotkaZavislaNaKontextu
                {
                    Nazev = "kus",
                    Symbol = "ks"
                }
            });

            units.Add(new TOdvozenaJednotka
            {
                Id = "u_26",
                Komponenta = new List<TKomponentaJednotky> {
                    new TKomponentaJednotky {
                        Exponent = 3,
                        Jednotka = new TJednotkaSI
                        {
                            Nazev = TNazevJednotkySI.METRE,
                            Predpona = TPredponaSI.DECI,
                            PredponaSpecified = true
                        }
                    }
                }
            });

            units.Add(new TJednotkaZavislaNaKontextu
            {
                Id = "u_27",
                Nazev = "Měření",
                Symbol = "měření"
            });

            units.Add(new TKonverzniJednotka
            {
                Id = "u_28",
                Meritko = 100,
                Nazev = "100 kilogramů",
                Odsazeni = 0,
                Symbol = "100 kg",
                ZakladniJednotka = new TJednotkaSI
                {
                    Nazev = TNazevJednotkySI.GRAM,
                    Predpona = TPredponaSI.KILO,
                    PredponaSpecified = true
                }
            });

            units.Add(new TKonverzniJednotka
            {
                Id = "u_29",
                Meritko = 100,
                Nazev = "100 metrů",
                Odsazeni = 0,
                Symbol = "100 m",
                ZakladniJednotka = new TJednotkaSI { Nazev = TNazevJednotkySI.METRE }
            });

            units.Add(new TKonverzniJednotka
            {
                Id = "u_30",
                Meritko = 100,
                Nazev = "100 metrů čtvrerečních",
                Odsazeni = 0,
                Symbol = "100 m²",
                ZakladniJednotka = new TOdvozenaJednotka
                {
                    Komponenta = new List<TKomponentaJednotky> {
                        new TKomponentaJednotky{
                            Exponent = 2,
                            Jednotka = new TJednotkaSI { Nazev = TNazevJednotkySI.METRE }
                        }
                    }
                }
            });

            units.Add(new TJednotkaZavislaNaKontextu
            {
                Id = "u_31",
                Nazev = "Cyklus",
                Symbol = "cyklus"
            });

            units.Add(new TJednotkaZavislaNaKontextu
            {
                Id = "u_32",
                Nazev = "Dávka",
                Symbol = "dávka"
            });

            units.Add(new TOdvozenaJednotka
            {
                Id = "u_33",
                Komponenta = new List<TKomponentaJednotky>
                {
                    new TKomponentaJednotky {
                        Exponent = 1,
                        Jednotka = new TJednotkaSI {Nazev = TNazevJednotkySI.METRE }
                    },
                    new TKomponentaJednotky
                    {
                        Exponent = -1,
                        Jednotka = mesic
                    }
                }
            });

            units.Add(new TJednotkaZavislaNaKontextu
            {
                Id = "u_34",
                Nazev = "Pár",
                Symbol = "pár"
            });

            units.Add(new TJednotkaZavislaNaKontextu
            {
                Id = "u_35",
                Nazev = "Případ",
                Symbol = "případ"
            });

            units.Add(new TOdvozenaJednotka
            {
                Id = "u_36",
                Komponenta = new List<TKomponentaJednotky>
               {
                    new TKomponentaJednotky {
                        Exponent = 1,
                        Jednotka = new TKonverzniJednotka
                        {
                            Meritko = 1000,
                            Nazev = "tuna",
                            Symbol = "t",
                            ZakladniJednotka = new TJednotkaSI {
                                Nazev = TNazevJednotkySI.GRAM,
                                Predpona = TPredponaSI.KILO,
                                PredponaSpecified = true
                            }
                        }
                    },
                    new TKomponentaJednotky
                    {
                        Exponent = -1,
                        Jednotka = new TJednotkaSI
                        {
                            Nazev = TNazevJednotkySI.METRE,
                            Predpona = TPredponaSI.KILO,
                            PredponaSpecified = true
                        }
                    }
               }
            });

            units.Add(new TJednotkaZavislaNaKontextu
            {
                Id = "u_37",
                Nazev = "Úsek",
                Symbol = "úsek"
            });

            units.Add(new TJednotkaZavislaNaKontextu
            {
                Id = "u_38",
                Nazev = "Žíla",
                Symbol = "žíla"
            });

            units.Add(new TJednotkaZavislaNaKontextu
            {
                Id = "u_39",
                Nazev = "Fáze",
                Symbol = "fáze"
            });

            units.Add(new TKonverzniJednotka
            {
                Id = "u_40",
                Meritko = 100,
                Nazev = "Hektolitr",
                Odsazeni = 0,
                Symbol = "hl",
                ZakladniJednotka = new TOdvozenaJednotka
                {
                    Komponenta = new List<TKomponentaJednotky> {
                    new TKomponentaJednotky{
                        Exponent = 3,
                        Jednotka = new TJednotkaSI
                        {
                            Nazev = TNazevJednotkySI.METRE,
                            Predpona = TPredponaSI.DECI,
                            PredponaSpecified = true
                        }
                    }
                }
                }
            });

            units.Add(new TJednotkaZavislaNaKontextu
            {
                Id = "u_41",
                Nazev = "Kartuš",
                Symbol = "kartuš"
            });

            units.Add(new TOdvozenaJednotka
            {
                Id = "u_42",
                Komponenta = new List<TKomponentaJednotky> {
                    new TKomponentaJednotky {
                        Exponent = 1,
                        Jednotka = new TJednotkaZavislaNaKontextu
                        {
                            Nazev = "kus",
                            Symbol = "ks"
                        }
                    },
                    new TKomponentaJednotky {
                        Exponent = -1,
                        Jednotka = new TKonverzniJednotka
                        {
                            Nazev = "Týden",
                            Symbol = "týden",
                            Meritko = 60*60*27*7,
                            ZakladniJednotka = new TJednotkaSI { Nazev = TNazevJednotkySI.SECOND }
                        }
                    }
                }
            });

            units.Add(new TJednotkaZavislaNaKontextu { Id = "u_43", Symbol = "kbelík", Nazev = "Kbelík" });
            units.Add(new TJednotkaZavislaNaKontextu { Id = "u_44", Symbol = "kotouč", Nazev = "Kotouč" });
            units.Add(new TJednotkaZavislaNaKontextu { Id = "u_45", Symbol = "pb", Nazev = "Podpěrný bod" });
            units.Add(new TJednotkaZavislaNaKontextu { Id = "u_46", Symbol = "role", Nazev = "Role" });
            units.Add(new TJednotkaZavislaNaKontextu { Id = "u_47", Symbol = "set", Nazev = "Set" });
            units.Add(new TJednotkaZavislaNaKontextu { Id = "u_48", Symbol = "stanice", Nazev = "Stanice" });
            units.Add(new TJednotkaZavislaNaKontextu { Id = "u_49", Symbol = "láhev", Nazev = "Láhev" });
            units.Add(new TJednotkaZavislaNaKontextu { Id = "u_50", Symbol = "paleta", Nazev = "Paleta" });
            units.Add(new TJednotkaZavislaNaKontextu { Id = "u_51", Symbol = "pól", Nazev = "Pól" });
            units.Add(new TJednotkaZavislaNaKontextu { Id = "u_52", Symbol = "pole", Nazev = "Pole" });
            units.Add(new TJednotkaZavislaNaKontextu { Id = "u_53", Symbol = "tabule", Nazev = "Tabule" });
            units.Add(new TJednotkaZavislaNaKontextu { Id = "u_54", Symbol = "pytel", Nazev = "Pytel" });
            units.Add(new TJednotkaZavislaNaKontextu { Id = "u_55", Symbol = "okruh", Nazev = "Okruh" });
            units.Add(new TJednotkaZavislaNaKontextu { Id = "u_56", Symbol = "svod", Nazev = "Svod" });
            units.Add(new TJednotkaZavislaNaKontextu { Id = "u_57", Symbol = "systém", Nazev = "Systém" });
            units.Add(new TJednotkaZavislaNaKontextu { Id = "u_58", Symbol = "styk", Nazev = "Styk" });
            units.Add(new TJednotkaZavislaNaKontextu { Id = "u_59", Symbol = "svár", Nazev = "Svár" });
            units.Add(new TJednotkaZavislaNaKontextu { Id = "u_60", Symbol = "ukotvení", Nazev = "Ukotvení" });

            units.Add(new TKonverzniJednotka
            {
                Id = "u_61",
                Meritko = 100,
                Nazev = "Metrický cent",
                Odsazeni = 0,
                Symbol = "q",
                ZakladniJednotka = new TJednotkaSI
                {
                    Nazev = TNazevJednotkySI.GRAM,
                    Predpona = TPredponaSI.KILO,
                    PredponaSpecified = true
                }
            });

            units.Add(new TKonverzniJednotka
            {
                Id = "u_62",
                Meritko = 1,
                Nazev = "Prostorový metr",
                Odsazeni = 0,
                Symbol = "prm",
                ZakladniJednotka = new TOdvozenaJednotka
                {
                    Komponenta = new List<TKomponentaJednotky> {
                        new TKomponentaJednotky{
                            Exponent = 3,
                            Jednotka = new TJednotkaSI { Nazev = TNazevJednotkySI.METRE }
                        }
                    }
                }
            });

            units.Add(new TKonverzniJednotka
            {
                Id = "u_63",
                Meritko = 1,
                Nazev = "Plnometr",
                Odsazeni = 0,
                Symbol = "plm",
                ZakladniJednotka = new TOdvozenaJednotka
                {
                    Komponenta = new List<TKomponentaJednotky> {
                        new TKomponentaJednotky{
                            Exponent = 3,
                            Jednotka = new TJednotkaSI { Nazev = TNazevJednotkySI.METRE }
                        }
                    }
                }
            });

            units.Add(new TKonverzniJednotka
            {
                Id = "u_64",
                Meritko = 60 * 60,
                Nazev = "Strojohodina",
                Odsazeni = 0,
                Symbol = "sh",
                ZakladniJednotka = new TOdvozenaJednotka
                {
                    Komponenta = new List<TKomponentaJednotky> {
                        new TKomponentaJednotky{
                            Exponent = 3,
                            Jednotka = new TJednotkaSI { Nazev = TNazevJednotkySI.SECOND }
                        }
                    }
                }
            });

            units.Add(new TKonverzniJednotka
            {
                Id = "u_65",
                Meritko = 60 * 60,
                Nazev = "Normohodina",
                Odsazeni = 0,
                Symbol = "nh",
                ZakladniJednotka = new TOdvozenaJednotka
                {
                    Komponenta = new List<TKomponentaJednotky> {
                        new TKomponentaJednotky{
                            Exponent = 3,
                            Jednotka = new TJednotkaSI { Nazev = TNazevJednotkySI.SECOND }
                        }
                    }
                }
            });

            units.Add(new TJednotkaZavislaNaKontextu { Id = "u_66", Symbol = "bm", Nazev = "Běžný metr" });
            units.Add(new TJednotkaZavislaNaKontextu { Id = "u_67", Symbol = "celek", Nazev = "Celek" });
            units.Add(new TJednotkaZavislaNaKontextu { Id = "u_68", Symbol = "čtyřka", Nazev = "Kabel se 4 vlákny" });

            units.Add(new TJednotkaSI
            {
                Id = "u_69",
                Nazev = TNazevJednotkySI.METRE,
                Predpona = TPredponaSI.HECTO,
                PredponaSpecified = true
            });

            units.Add(new TOdvozenaJednotka
            {
                Id = "u_70",
                Komponenta = new List<TKomponentaJednotky> {
                    new TKomponentaJednotky {
                        Exponent = 1,
                        Jednotka = new TJednotkaSI
                        {
                            Nazev = TNazevJednotkySI.METRE,
                            Predpona = TPredponaSI.KILO,
                            PredponaSpecified = true
                        }
                    },
                    new TKomponentaJednotky {
                        Exponent = 1,
                        Jednotka = new TJednotkaZavislaNaKontextu { Nazev = "Kabel se 4 vlákny", Symbol = "čtyřka"}
                    }
                }
            });

            units.Add(new TOdvozenaJednotka
            {
                Id = "u_71",
                Komponenta = new List<TKomponentaJednotky> {
                    new TKomponentaJednotky {
                        Exponent = 1,
                        Jednotka = new TJednotkaSI
                        {
                            Nazev = TNazevJednotkySI.METRE,
                            Predpona = TPredponaSI.KILO,
                            PredponaSpecified = true
                        }
                    },
                    new TKomponentaJednotky {
                        Exponent = 1,
                        Jednotka = new TJednotkaZavislaNaKontextu { Nazev = "Kabel se 4 vlákny", Symbol = "čtyřka"}
                    }
                    ,
                    new TKomponentaJednotky {
                        Exponent = -1,
                        Jednotka = mesic
                    }
                }
            });

            units.Add(new TOdvozenaJednotka
            {
                Id = "u_72",
                Komponenta = new List<TKomponentaJednotky> {
                    new TKomponentaJednotky {
                        Exponent = 1,
                        Jednotka = new TJednotkaSI
                        {
                            Nazev = TNazevJednotkySI.METRE,
                            Predpona = TPredponaSI.KILO,
                            PredponaSpecified = true
                        }
                    },
                    new TKomponentaJednotky {
                        Exponent = 1,
                        Jednotka = new TJednotkaZavislaNaKontextu { Nazev = "Pár", Symbol = "pár"}
                    }
                }
            });

            units.Add(new TOdvozenaJednotka
            {
                Id = "u_73",
                Komponenta = new List<TKomponentaJednotky> {
                    new TKomponentaJednotky {
                        Exponent = 1,
                        Jednotka = new TJednotkaSI
                        {
                            Nazev = TNazevJednotkySI.METRE,
                            Predpona = TPredponaSI.KILO,
                            PredponaSpecified = true
                        }
                    },
                    new TKomponentaJednotky {
                        Exponent = 1,
                        Jednotka = new TJednotkaZavislaNaKontextu { Nazev = "Pár", Symbol = "pár"}
                    }
                    ,
                    new TKomponentaJednotky {
                        Exponent = -1,
                        Jednotka = mesic
                    }
                }
            });

            units.Add(new TOdvozenaJednotka
            {
                Id = "u_74",
                Komponenta = new List<TKomponentaJednotky> {
                    new TKomponentaJednotky {
                        Exponent = 1,
                        Jednotka = new TJednotkaSI
                        {
                            Nazev = TNazevJednotkySI.METRE,
                            Predpona = TPredponaSI.KILO,
                            PredponaSpecified = true
                        }
                    },
                    new TKomponentaJednotky {
                        Exponent = 1,
                        Jednotka = new TJednotkaZavislaNaKontextu { Nazev = "Vlákno", Symbol = "vlákno"}
                    }
                }
            });

            units.Add(new TOdvozenaJednotka
            {
                Id = "u_75",
                Komponenta = new List<TKomponentaJednotky> {
                    new TKomponentaJednotky {
                        Exponent = 1,
                        Jednotka = new TJednotkaSI
                        {
                            Nazev = TNazevJednotkySI.METRE,
                            Predpona = TPredponaSI.KILO,
                            PredponaSpecified = true
                        }
                    },
                    new TKomponentaJednotky {
                        Exponent = 1,
                        Jednotka = new TJednotkaZavislaNaKontextu { Nazev = "Vlákno", Symbol = "vlákno"}
                    }
                    ,
                    new TKomponentaJednotky {
                        Exponent = -1,
                        Jednotka = mesic
                    }
                }
            });

            units.Add(new TOdvozenaJednotka
            {
                Id = "u_76",
                Komponenta = new List<TKomponentaJednotky> {
                    new TKomponentaJednotky {
                        Exponent = 1,
                        Jednotka = new TJednotkaSI
                        {
                            Nazev = TNazevJednotkySI.METRE,
                            Predpona = TPredponaSI.KILO,
                            PredponaSpecified = true
                        }
                    },
                    new TKomponentaJednotky {
                        Exponent = 1,
                        Jednotka = new TJednotkaZavislaNaKontextu { Nazev = "Žíla", Symbol = "žíla"}
                    }
                }
            });

            units.Add(new TOdvozenaJednotka
            {
                Id = "u_77",
                Komponenta = new List<TKomponentaJednotky> {
                    new TKomponentaJednotky {
                        Exponent = 1,
                        Jednotka = new TJednotkaZavislaNaKontextu { Nazev = "Komplet", Symbol = "kpl"}
                    },
                    new TKomponentaJednotky {
                        Exponent = 1,
                        Jednotka = mesic
                    }
                }
            });

            units.Add(new TOdvozenaJednotka
            {
                Id = "u_78",
                Komponenta = new List<TKomponentaJednotky> {
                    new TKomponentaJednotky {
                        Exponent = 1,
                        Jednotka = ks
                    },
                    new TKomponentaJednotky {
                        Exponent = 1,
                        Jednotka = den
                    }
                }
            });

            units.Add(new TKonverzniJednotka
            {
                Id = "u_79",
                Meritko = 1,
                Nazev = "Metr krychlový obestavěného prostoru",
                Odsazeni = 0,
                Symbol = "m³ OP",
                ZakladniJednotka = new TOdvozenaJednotka
                {
                    Komponenta = new List<TKomponentaJednotky> {
                        new TKomponentaJednotky{
                            Exponent = 3,
                            Jednotka = new TJednotkaSI { Nazev = TNazevJednotkySI.METRE }
                        }
                    }
                }
            });

            units.Add(new TOdvozenaJednotka
            {
                Id = "u_80",
                Komponenta = new List<TKomponentaJednotky> {
                    new TKomponentaJednotky {
                        Exponent = 1,
                        Jednotka = new TOdvozenaJednotka
                        {
                            Komponenta = new List<TKomponentaJednotky> {
                                new TKomponentaJednotky{
                                    Exponent = 3,
                                    Jednotka = new TJednotkaSI { Nazev = TNazevJednotkySI.METRE }
                                }
                            }
                        }
                    },
                    new TKomponentaJednotky {
                        Exponent = 1,
                        Jednotka = new TJednotkaSI
                        {
                            Nazev = TNazevJednotkySI.METRE,
                            Predpona = TPredponaSI.KILO,
                            PredponaSpecified = true
                        }
                    }
                }
            });

            units.Add(new TOdvozenaJednotka
            {
                Id = "u_81",
                Komponenta = new List<TKomponentaJednotky> {
                    new TKomponentaJednotky {
                        Exponent = 1,
                        Jednotka = new TJednotkaSI { Nazev = TNazevJednotkySI.METRE }
                    },
                    new TKomponentaJednotky {
                        Exponent = 1,
                        Jednotka = den
                    }
                }
            });

            units.Add(new TJednotkaZavislaNaKontextu { Id = "u_82", Symbol = "vlákno", Nazev = "Vlákno" });
            units.Add(new TJednotkaZavislaNaKontextu { Id = "u_83", Symbol = "V.J.", Nazev = "Výhybková jednotka" });
            units.Add(new TJednotkaZavislaNaKontextu { Id = "u_84", Symbol = "sada", Nazev = "Sada" });
            units.Add(new TJednotkaZavislaNaKontextu { Id = "u_85", Symbol = "soubor", Nazev = "Soubor" });
            units.Add(new TJednotkaZavislaNaKontextu { Id = "u_86", Symbol = "souprava", Nazev = "Souprava" });

            var serializer = new XmlSerializer(typeof(Jednotky));

            using var output = File.Create("definovane_jednotky.xml");
            using var xml = XmlWriter.Create(output, new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  "
            });

            serializer.Serialize(xml, new Jednotky
            {
                Jednotka = units.OrderBy(u => u.Id).ToList()
            });
        }
    }
}
