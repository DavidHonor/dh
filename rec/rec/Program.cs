using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Diagnostics;

namespace rec
{
    class Program
    {
        static bool[,] kep_terkep;
        static bool[,] KepTerkep;

        static Bitmap EredetiKep;

        public static Bitmap kontrasztos(Bitmap eredeti, bool kepmentes, byte kontraszt)
        {
            Bitmap atalakitott = new Bitmap(eredeti.Width, eredeti.Height);
            for (int i = 0; i < eredeti.Width; i++)
            {
                for (int j = 0; j < eredeti.Height; j++)
                {
                    if ((eredeti.GetPixel(i, j).R + eredeti.GetPixel(i, j).G + eredeti.GetPixel(i, j).B) / 3 < kontraszt)
                    {
                        atalakitott.SetPixel(i, j, Color.White);
                    }
                    else atalakitott.SetPixel(i, j, Color.Black);
                }
            }
            if (kepmentes == true)
            {
                Image a = atalakitott;
                a.Save("feketefeher_kep.jpg");
            }
            return atalakitott;
        }
        static void UjKepBetoltese(Bitmap Kep, bool fektefeherkepmentese, byte kontraszt)
        {
            EredetiKep = Kep;
            Console.WriteLine("Feldolgozás..");
            if (File.Exists("kep.jpg"))
            {
                Bitmap akep = kontrasztos(Kep, fektefeherkepmentese, kontraszt);
                kep_terkep = new bool[akep.Width, akep.Height];
                for (int i = 0; i < akep.Width; i++)
                {
                    for (int j = 0; j < akep.Height; j++)
                    {
                        if (akep.GetPixel(i, j).R == 255 && akep.GetPixel(i, j).G == 255 && akep.GetPixel(i, j).B == 255)
                        {
                            kep_terkep[i, j] = true;
                        }
                        else kep_terkep[i, j] = false;
                    }
                }
            }
            else Console.WriteLine("Fájl nem található!");
        }
        static void Kiiras()
        {
            for (int i = 0; i < kep_terkep.GetLength(1); i++)
            {
                for (int j = 0; j < kep_terkep.GetLength(0); j++)
                {
                    if (kep_terkep[j, i] == true) { Console.BackgroundColor = ConsoleColor.DarkRed; Console.Write("X"); }
                    else { Console.BackgroundColor = ConsoleColor.Black; Console.Write("O"); }
                }
                Console.WriteLine();
            }
            Console.ReadLine();
        }
        public static string Táblák(string tábla_azonosítója)
        {
            switch (tábla_azonosítója)
            {
                case "stop_tabla": return "STOP tábla";
                case "elsobbsegadas_tabla": return "Elsőbbségadás tábla";
                case "behajtanitilos_tabla": return "Behajtani tilos tábla";
                case "korforgalom_tabla": return "Körforgalom tábla";
                case "30as_tabla": return "30 km/h óra tábla";
                case "60as_tabla": return "60 km/h óra tábla";
                case "a": return "A betű";
                case "b": return "B betű";
                case "c": return "C betű";
                case "d": return "D betű";
                case "e": return "E betű";
                case "f": return "F betű";
                default: return "Nincs elnevezve!";

            }
        }
        static void Meretezes()
        {
            bool[,] atmeretezo = kep_terkep;
            int elorollevagni = atmeretezo.GetLength(0);   //elöl
            for (int i = 0; i < atmeretezo.GetLength(1); i++)
            {
                for (int j = 0; j < atmeretezo.GetLength(0); j++)
                {
                    if (atmeretezo[j, i])
                    {
                        if (elorollevagni > j)
                        {
                            elorollevagni = j;
                        }
                    }
                }
            }
            int haturrollevagni = 0;  //hátul
            for (int i = 0; i < atmeretezo.GetLength(1); i++)
            {
                for (int j = 0; j < atmeretezo.GetLength(0); j++)
                {
                    if (atmeretezo[j, i])
                    {
                        if (haturrollevagni < j)
                        {
                            haturrollevagni = j;
                        }
                    }
                }
            }
            int felulrollevagni = atmeretezo.GetLength(1);  //felül
            for (int i = 0; i < atmeretezo.GetLength(0); i++)
            {
                for (int j = 0; j < atmeretezo.GetLength(1); j++)
                {
                    if (atmeretezo[i, j])
                    {
                        if (felulrollevagni > j)
                        {
                            felulrollevagni = j;
                        }
                    }
                }
            }
            int alulrollevagni = 0;  //alul
            for (int i = 0; i < atmeretezo.GetLength(0); i++)
            {
                for (int j = 0; j < atmeretezo.GetLength(1); j++)
                {
                    if (atmeretezo[i, j])
                    {
                        if (alulrollevagni < j)
                        {
                            alulrollevagni = j;
                        }
                    }
                }
            }
            bool[,] ujmeret = new bool[haturrollevagni - elorollevagni, alulrollevagni - felulrollevagni];
            for (int i = elorollevagni; i < haturrollevagni; i++)
            {
                for (int j = felulrollevagni; j < alulrollevagni; j++)
                {
                    if (atmeretezo[i, j])
                    {
                        ujmeret[i-elorollevagni, j-felulrollevagni] = true;
                    }
                }
            }
            KepTerkep = ujmeret;
            Bitmap atalakitott2 = new Bitmap(ujmeret.GetLength(0), ujmeret.GetLength(1));
            for (int i = elorollevagni; i < haturrollevagni; i++)
            {
                for (int j = felulrollevagni; j < alulrollevagni; j++)
                {
                    if ((EredetiKep.GetPixel(i, j).R + EredetiKep.GetPixel(i, j).G + EredetiKep.GetPixel(i, j).B) / 3 < 200)
                    {
                        atalakitott2.SetPixel(i- elorollevagni, j- felulrollevagni, Color.White);
                    }
                    else atalakitott2.SetPixel(i - elorollevagni, j - felulrollevagni, Color.Black);
                }
            }

            Image a = atalakitott2;
            a.Save("atmeretezes_kep.jpg");
         
        }
        static public double SzínArány()
        {
            double fekete = 0;
            double fehér = 0;
            for (int i = 0; i < kep_terkep.GetLength(0); i++)
            {
                for (int j = 0; j < kep_terkep.GetLength(1); j++)
                {
                    if (kep_terkep[i, j] == true)
                    {
                        fehér++;
                    }
                    else fekete++;
                }
            }
            double ért = fekete / fehér;
            return ért;
        }
        static void Mentés(bool adatokkiirasa, string elmentett_tabla_neve)
        {
            double tabla2_elsoharmad = 0;
            int minHelyh1 = KepTerkep.GetLength(1);
            int maxHelyh1 = 0;
            for (int i = 0; i < KepTerkep.GetLength(0) / 3; i++)   //Első harmad magasság
            {
                for (int j = 0; j < KepTerkep.GetLength(1); j++)
                {
                    if (KepTerkep[i, j])
                    {
                        if (minHelyh1 > j)
                        {
                            minHelyh1 = j;
                        }
                        if (maxHelyh1 < j)
                        {
                            maxHelyh1 = j;
                        }
                    }
                }
            }
            tabla2_elsoharmad = maxHelyh1 - minHelyh1;

            double tabla2_masodikharmad = 0;
            int minHelyh2 = KepTerkep.GetLength(1);
            int maxHelyh2 = 0;
            for (int i = KepTerkep.GetLength(0) / 3; i < (KepTerkep.GetLength(0) / 3) * 2; i++)   //Masodik harmad magasság
            {
                for (int j = 0; j < KepTerkep.GetLength(1); j++)
                {
                    if (KepTerkep[i, j])
                    {
                        if (minHelyh2 > j)
                        {
                            minHelyh2 = j;
                        }
                        if (maxHelyh2 < j)
                        {
                            maxHelyh2 = j;
                        }
                    }
                }
            }
            tabla2_masodikharmad = maxHelyh2 - minHelyh2;

            double tabla2_harmadikharmad = 0;
            int minHelyh3 = KepTerkep.GetLength(1);
            int maxHelyh3 = 0;
            for (int i = (KepTerkep.GetLength(0) / 3) * 2; i < KepTerkep.GetLength(0); i++)   //Harmadik harmad magasság
            {
                for (int j = 0; j < KepTerkep.GetLength(1); j++)
                {
                    if (KepTerkep[i, j])
                    {
                        if (minHelyh3 > j)
                        {
                            minHelyh3 = j;
                        }
                        if (maxHelyh3 < j)
                        {
                            maxHelyh3 = j;
                        }
                    }
                }
            }
            tabla2_harmadikharmad = maxHelyh3 - minHelyh3;

            double tabla_elsoharmad = 0;
            int minHely = KepTerkep.GetLength(0);
            int maxHely = 0;
            for (int i = 0; i < KepTerkep.GetLength(1) / 3; i++)   //Első harmad
            {
                for (int j = 0; j < KepTerkep.GetLength(0); j++)
                {
                    if (KepTerkep[j, i])
                    {
                        if (minHely > j)
                        {
                            minHely = j;
                        }
                        if (maxHely < j)
                        {
                            maxHely = j;
                        }
                    }
                }
            }
            tabla_elsoharmad = maxHely - minHely;

            double tabla_masodikharmad = 0;
            int minHely2 = KepTerkep.GetLength(0);
            int maxHely2 = 0;
            for (int i = KepTerkep.GetLength(1) / 3; i < (KepTerkep.GetLength(1) / 3) * 2; i++)   //Masodik harmad
            {
                for (int j = 0; j < KepTerkep.GetLength(0); j++)
                {
                    if (KepTerkep[j, i])
                    {
                        if (minHely2 > j)
                        {
                            minHely2 = j;
                        }
                        if (maxHely2 < j)
                        {
                            maxHely2 = j;
                        }
                    }
                }
            }
            tabla_masodikharmad = maxHely2 - minHely2;

            double tabla_harmadikharmad = 0;
            int minHely3 = KepTerkep.GetLength(0);
            int maxHely3 = 0;
            for (int i = (KepTerkep.GetLength(1) / 3) * 2; i < KepTerkep.GetLength(1); i++)   //Harmadik harmad
            {
                for (int j = 0; j < KepTerkep.GetLength(0); j++)
                {
                    if (KepTerkep[j, i])
                    {
                        if (minHely3 > j)
                        {
                            minHely3 = j;
                        }
                        if (maxHely3 < j)
                        {
                            maxHely3 = j;
                        }
                    }
                }
            }
            tabla_harmadikharmad = maxHely3 - minHely3;

            double szelessegArany1 = 0;
            double szelessegArany2 = 0;
            double szelessegArany3 = 0;

            double magassagArany1 = 0;
            double magassagArany2 = 0;
            double magassagArany3 = 0;

            if (tabla_elsoharmad != 0 && tabla_masodikharmad != 0) szelessegArany1 = tabla_elsoharmad / tabla_masodikharmad;
            if (tabla_masodikharmad != 0 && tabla_harmadikharmad != 0) szelessegArany2 = tabla_masodikharmad / tabla_harmadikharmad;
            if (tabla_harmadikharmad != 0 && tabla_elsoharmad != 0) szelessegArany3 = tabla_harmadikharmad / tabla_elsoharmad;

            if (tabla2_elsoharmad != 0 && tabla2_masodikharmad != 0) magassagArany1 = tabla2_elsoharmad / tabla2_masodikharmad;
            if (tabla2_elsoharmad != 0 && tabla2_masodikharmad != 0) magassagArany2 = tabla2_masodikharmad / tabla2_harmadikharmad;
            if (tabla2_elsoharmad != 0 && tabla2_masodikharmad != 0) magassagArany3 = tabla2_harmadikharmad / tabla2_elsoharmad;

            if (adatokkiirasa == true)
            {
                Console.WriteLine("Szélességi:");
                Console.WriteLine("Első harmad --> min: {0} max: {1} szélesség: {2} arány: {3}", minHely, maxHely, tabla_elsoharmad, szelessegArany1);
                Console.WriteLine("Második harmad --> min: {0} max: {1} szélesség: {2} arány: {3}", minHely2, maxHely2, tabla_masodikharmad, szelessegArany1);
                Console.WriteLine("Harmadik harmad --> min: {0} max: {1} szélesség: {2} arány: {3}", minHely3, maxHely3, tabla_harmadikharmad, szelessegArany1);
                Console.WriteLine("Magassági:");
                Console.WriteLine("Első harmad --> min: {0} max: {1} magasság: {2} arány: {3}", minHelyh1, maxHelyh1, tabla2_elsoharmad, szelessegArany1);
                Console.WriteLine("Második harmad --> min: {0} max: {1} magasság: {2} arány: {3}", minHelyh2, maxHelyh2, tabla2_masodikharmad, szelessegArany2);
                Console.WriteLine("Harmadik harmad --> min: {0} max: {1} magasság: {2} arány: {3}", minHelyh3, maxHelyh3, tabla2_harmadikharmad, szelessegArany3);
                Console.WriteLine("Színarány: " + SzínArány());
            }
            bool továbblépés = true;
            if (File.Exists("elmentett_adatok.txt"))
            {
                List<Adatok> létezők = new List<Adatok>();
                StreamReader rr = new StreamReader("elmentett_adatok.txt");
                while (!rr.EndOfStream)
                {
                    létezők.Add(new Adatok(rr.ReadLine()));
                }
                for (int i = 0; i < létezők.Count; i++)
                {
                    if (létezők[i].Tablanev == elmentett_tabla_neve)
                    {
                        Console.WriteLine("Ilyen nevű tábla már létezik!");
                        továbblépés = false;
                    }
                    if (létezők[i].Szelessegarany1 == szelessegArany1 && létezők[i].Szelessegarany2 == szelessegArany2 && létezők[i].Szelessegarany3 == szelessegArany3 && létezők[i].Magassagarany1 == magassagArany1 && létezők[i].Magassagarany2 == magassagArany2 && létezők[i].Magassagarany3 == magassagArany3)
                    {
                        Console.WriteLine("Ilyen adatokkal már mentettél!");
                        továbblépés = false;
                    }
                }
                rr.Close();
            }

            if (továbblépés == true)
            {
                StreamWriter sr = new StreamWriter("elmentett_adatok.txt", true);
                sr.WriteLine(elmentett_tabla_neve + ";" + szelessegArany1 + ";" + szelessegArany2 + ";" + szelessegArany3 + ";" + SzínArány() + ";" + magassagArany1 + ";" + magassagArany2 + ";" + magassagArany3, true);
                sr.Close();
                Console.WriteLine("Adatok elmentve! {0}", elmentett_tabla_neve);
            }
            else
            {
                Console.WriteLine("Nem tudsz továbblépni, kérlek változtass!");
            }

        }
        static void Összehasonlítás(double tűréshatár, double szin_tureshatar) //-----------------------------------ÖSSZEHASONLÍTÁS---------------------------------------
        {
            double tabla2_elsoharmad = 0;
            int minHelyh1 = KepTerkep.GetLength(1);
            int maxHelyh1 = 0;
            for (int i = 0; i < KepTerkep.GetLength(0) / 3; i++)   //Első harmad magasság
            {
                for (int j = 0; j < KepTerkep.GetLength(1); j++)
                {
                    if (KepTerkep[i, j])
                    {
                        if (minHelyh1 > j)
                        {
                            minHelyh1 = j;
                        }
                        if (maxHelyh1 < j)
                        {
                            maxHelyh1 = j;
                        }
                    }
                }
            }
            tabla2_elsoharmad = maxHelyh1 - minHelyh1;

            double tabla2_masodikharmad = 0;
            int minHelyh2 = KepTerkep.GetLength(1);
            int maxHelyh2 = 0;
            for (int i = KepTerkep.GetLength(0) / 3; i < (KepTerkep.GetLength(0) / 3) * 2; i++)   //Masodik harmad magasság
            {
                for (int j = 0; j < KepTerkep.GetLength(1); j++)
                {
                    if (KepTerkep[i, j])
                    {
                        if (minHelyh2 > j)
                        {
                            minHelyh2 = j;
                        }
                        if (maxHelyh2 < j)
                        {
                            maxHelyh2 = j;
                        }
                    }
                }
            }
            tabla2_masodikharmad = maxHelyh2 - minHelyh2;

            double tabla2_harmadikharmad = 0;
            int minHelyh3 = KepTerkep.GetLength(1);
            int maxHelyh3 = 0;
            for (int i = (KepTerkep.GetLength(0) / 3) * 2; i < KepTerkep.GetLength(0); i++)   //Harmadik harmad magasság
            {
                for (int j = 0; j < KepTerkep.GetLength(1); j++)
                {
                    if (KepTerkep[i, j])
                    {
                        if (minHelyh3 > j)
                        {
                            minHelyh3 = j;
                        }
                        if (maxHelyh3 < j)
                        {
                            maxHelyh3 = j;
                        }
                    }
                }
            }
            tabla2_harmadikharmad = maxHelyh3 - minHelyh3;

            double tabla_elsoharmad = 0;
            int minHely = KepTerkep.GetLength(0);
            int maxHely = 0;
            for (int i = 0; i < KepTerkep.GetLength(1) / 3; i++)   //Első harmad
            {
                for (int j = 0; j < KepTerkep.GetLength(0); j++)
                {
                    if (KepTerkep[j, i])
                    {
                        if (minHely > j)
                        {
                            minHely = j;
                        }
                        if (maxHely < j)
                        {
                            maxHely = j;
                        }
                    }
                }
            }
            tabla_elsoharmad = maxHely - minHely;

            double tabla_masodikharmad = 0;
            int minHely2 = KepTerkep.GetLength(0);
            int maxHely2 = 0;
            for (int i = KepTerkep.GetLength(1) / 3; i < (KepTerkep.GetLength(1) / 3) * 2; i++)   //Masodik harmad
            {
                for (int j = 0; j < KepTerkep.GetLength(0); j++)
                {
                    if (KepTerkep[j, i])
                    {
                        if (minHely2 > j)
                        {
                            minHely2 = j;
                        }
                        if (maxHely2 < j)
                        {
                            maxHely2 = j;
                        }
                    }
                }
            }
            tabla_masodikharmad = maxHely2 - minHely2;

            double tabla_harmadikharmad = 0;
            int minHely3 = KepTerkep.GetLength(0);
            int maxHely3 = 0;
            for (int i = (KepTerkep.GetLength(1) / 3) * 2; i < KepTerkep.GetLength(1); i++)   //Harmadik harmad
            {
                for (int j = 0; j < KepTerkep.GetLength(0); j++)
                {
                    if (KepTerkep[j, i])
                    {
                        if (minHely3 > j)
                        {
                            minHely3 = j;
                        }
                        if (maxHely3 < j)
                        {
                            maxHely3 = j;
                        }
                    }
                }
            }
            tabla_harmadikharmad = maxHely3 - minHely3;

            double szelessegArany1 = 0;
            double szelessegArany2 = 0;
            double szelessegArany3 = 0;

            double magassagArany1 = 0;
            double magassagArany2 = 0;
            double magassagArany3 = 0;

            if (tabla_elsoharmad != 0 && tabla_masodikharmad != 0) szelessegArany1 = tabla_elsoharmad / tabla_masodikharmad;
            if (tabla_masodikharmad != 0 && tabla_harmadikharmad != 0) szelessegArany2 = tabla_masodikharmad / tabla_harmadikharmad;
            if (tabla_harmadikharmad != 0 && tabla_elsoharmad != 0) szelessegArany3 = tabla_harmadikharmad / tabla_elsoharmad;

            if (tabla2_elsoharmad != 0 && tabla2_masodikharmad != 0) magassagArany1 = tabla2_elsoharmad / tabla2_masodikharmad;
            if (tabla2_elsoharmad != 0 && tabla2_masodikharmad != 0) magassagArany2 = tabla2_masodikharmad / tabla2_harmadikharmad;
            if (tabla2_elsoharmad != 0 && tabla2_masodikharmad != 0) magassagArany3 = tabla2_harmadikharmad / tabla2_elsoharmad;

            if (File.Exists("elmentett_adatok.txt"))
            {
                StreamReader sr = new StreamReader("elmentett_adatok.txt");
                List<Adatok> letezok = new List<Adatok>();
                while (!sr.EndOfStream)
                {
                    letezok.Add(new Adatok(sr.ReadLine()));
                }

                double mközelség1 = 0;
                double mközelség2 = 0;
                double mközelség3 = 0;
                double mösszeg = 0;

                double közelség1 = 0;
                double közelség2 = 0;
                double közelség3 = 0;
                double összeg = 0;
                double szinarany_kulonbseg = 0;

                int index = 0;

                for (int i = 0; i < letezok.Count; i++)
                {
                    mközelség1 = Math.Abs(letezok[i].Magassagarany1 - magassagArany1);
                    mközelség2 = Math.Abs(letezok[i].Magassagarany2 - magassagArany2);
                    mközelség3 = Math.Abs(letezok[i].Magassagarany3 - magassagArany3);
                    közelség1 = Math.Abs(letezok[i].Szelessegarany1 - szelessegArany1);
                    közelség2 = Math.Abs(letezok[i].Szelessegarany2 - szelessegArany2);
                    közelség3 = Math.Abs(letezok[i].Szelessegarany3 - szelessegArany3);
                    if (i == 0) { összeg = Math.Abs(közelség1 + közelség2 + közelség3); mösszeg = Math.Abs(mközelség1 + mközelség2 + mközelség3); }
                    if (összeg >= Math.Abs(közelség1 + közelség2 + közelség3) && mösszeg >= Math.Abs(mközelség1 + mközelség2 + mközelség3))
                    {
                        szinarany_kulonbseg = Math.Abs(SzínArány() - letezok[i].Szinarany);
                        összeg = Math.Abs(közelség1 + közelség2 + közelség3);
                        mösszeg = Math.Abs(mközelség1 + mközelség2 + mközelség3);
                        index = i;
                    }
                }
                vége = DateTime.UtcNow;

                double eltelt = (vége - kezdet).TotalSeconds;
                Console.WriteLine();
                Console.WriteLine("Végrehajtási idő: " + eltelt + " másodperc");
                Console.WriteLine();
                if (összeg < tűréshatár && szinarany_kulonbseg < szin_tureshatar && mösszeg < tűréshatár)
                {
                    Console.WriteLine("Jelenlegi tűréshatár: {0}  >  érték: {1}", tűréshatár, összeg);
                    Console.WriteLine();
                    Console.Write("Legközelebbi egyezés:   ");
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(Táblák(letezok[index].Tablanev));
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine();
                    Console.WriteLine("Szín egyezes -- > eredeti: " + letezok[index].Szinarany + " mostani: " + SzínArány() + " eltérés: " + szinarany_kulonbseg + " tűráshatár: " + szin_tureshatar);
                    Console.WriteLine();
                    Console.WriteLine("Tárolt adatok:  szelesseg1: {0} szelesseg2: {1} szelesseg3: {2}", letezok[index].Szelessegarany1, letezok[index].Szelessegarany2, letezok[index].Szelessegarany3);
                    Console.WriteLine("Mostani adatok: szelesseg1: {0} szelesseg2: {1} szelesseg3: {2}", szelessegArany1, szelessegArany2, szelessegArany3);
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Különbség:      szelesseg1: {0} szelesseg2: {1} szelesseg3: {2}", letezok[index].Szelessegarany1 - szelessegArany1, letezok[index].Szelessegarany2 - szelessegArany2, letezok[index].Szelessegarany3 - szelessegArany3);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine();
                    Console.WriteLine("Tárolt adatok:  magassag1: {0} magassag2: {1} magassag3: {2}", letezok[index].Magassagarany1, letezok[index].Magassagarany2, letezok[index].Magassagarany3);
                    Console.WriteLine("Mostani adatok: magassag1: {0} magassag2: {1} magassag3: {2}", magassagArany1, magassagArany2, magassagArany3);
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Különbség:      magassag1: {0} magassag2: {1} magassag3: {2}", letezok[index].Magassagarany1 - magassagArany1, letezok[index].Magassagarany2 - magassagArany2, letezok[index].Magassagarany3 - magassagArany3);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Process.Start("kep.jpg");
                    Process.Start("feketefeher_kep.jpg");
                }
                else
                {
                    Console.WriteLine("Tűréshatáron kívül volt, nem lehet felismerni! tűréshatár: {0}  <  érték: {1}", tűréshatár, összeg);
                    Console.WriteLine();
                    Console.WriteLine("Szín egyezes -- > eredeti: " + letezok[index].Szinarany + " mostani: " + SzínArány() + " eltérés: " + szinarany_kulonbseg + " tűráshatár: " + szin_tureshatar);
                    Console.WriteLine();
                    Console.WriteLine("Tárolt adatok:  szelesseg1: {0} szelesseg2: {1} szelesseg3: {2}", letezok[index].Szelessegarany1, letezok[index].Szelessegarany2, letezok[index].Szelessegarany3);
                    Console.WriteLine("Mostani adatok: szelesseg1: {0} szelesseg2: {1} szelesseg3: {2}", szelessegArany1, szelessegArany2, szelessegArany3);
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Különbség:      szelesseg1: {0} szelesseg2: {1} szelesseg3: {2}", letezok[index].Szelessegarany1 - szelessegArany1, letezok[index].Szelessegarany2 - szelessegArany2, letezok[index].Szelessegarany3 - szelessegArany3);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine();
                    Console.WriteLine("Tárolt adatok:  magassag1: {0} magassag2: {1} magassag3: {2}", letezok[index].Magassagarany1, letezok[index].Magassagarany2, letezok[index].Magassagarany3);
                    Console.WriteLine("Mostani adatok: magassag1: {0} magassag2: {1} magassag3: {2}", magassagArany1, magassagArany2, magassagArany3);
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Különbség:      magassag1: {0} magassag2: {1} magassag3: {2}", letezok[index].Magassagarany1 - magassagArany1, letezok[index].Magassagarany2 - magassagArany2, letezok[index].Magassagarany3 - magassagArany3);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Process.Start("kep.jpg");
                    Process.Start("feketefeher_kep.jpg");
                }
            }
            else
            {
                Console.WriteLine("Nincs elmetett fajl");
            }

        } //--------------------------------- EDDIG----------------------------------------------------------
        static DateTime kezdet;
        static DateTime vége;


        static void Main()
        {
            kezdet = DateTime.UtcNow;
            UjKepBetoltese(new Bitmap("kep.jpg"), true, 100);
            Meretezes();
           // Mentés(true, "30_astabla");
            Összehasonlítás(0.22, 10000.0);
            //Kiiras();

            Console.ReadLine();
        }
    }
    internal class Adatok
    {
        string tablanev;
        double szelessegarany1;
        double szelessegarany2;
        double szelessegarany3;
        double szinarany;
        double magassagarany1;
        double magassagarany2;
        double magassagarany3;

        public string Tablanev
        {
            get
            {
                return tablanev;
            }

            set
            {
                tablanev = value;
            }
        }

        public double Szelessegarany1
        {
            get
            {
                return szelessegarany1;
            }

            set
            {
                szelessegarany1 = value;
            }
        }

        public double Szelessegarany2
        {
            get
            {
                return szelessegarany2;
            }

            set
            {
                szelessegarany2 = value;
            }
        }

        public double Szelessegarany3
        {
            get
            {
                return szelessegarany3;
            }

            set
            {
                szelessegarany3 = value;
            }
        }

        public double Szinarany
        {
            get
            {
                return szinarany;
            }

            set
            {
                szinarany = value;
            }
        }

        public double Magassagarany1
        {
            get
            {
                return magassagarany1;
            }

            set
            {
                magassagarany1 = value;
            }
        }

        public double Magassagarany2
        {
            get
            {
                return magassagarany2;
            }

            set
            {
                magassagarany2 = value;
            }
        }

        public double Magassagarany3
        {
            get
            {
                return magassagarany3;
            }

            set
            {
                magassagarany3 = value;
            }
        }

        public Adatok(string sor)
        {
            string[] tordeles = sor.Split(';');
            tablanev = tordeles[0];
            szelessegarany1 = Convert.ToDouble(tordeles[1]);
            szelessegarany2 = Convert.ToDouble(tordeles[2]);
            szelessegarany3 = Convert.ToDouble(tordeles[3]);
            szinarany = Convert.ToDouble(tordeles[4]);
            magassagarany1 = Convert.ToDouble(tordeles[5]);
            magassagarany2 = Convert.ToDouble(tordeles[6]);
            magassagarany3 = Convert.ToDouble(tordeles[7]);
        }
    }

}
