using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace foglalas_nyilvantarto
{
    class Program
    {
        static void Main(string[] args)
        {
            Beallit();
            Beolvas("foglalasok.csv");
            Menu();
        }

        static List<Foglalas> foglalasok = new List<Foglalas>();

        static void Beolvas(string fajlnev)
        {
            if (!File.Exists("foglalasok.csv"))
            {
                StreamWriter sw = new StreamWriter("foglalasok.csv");
                sw.Flush();
                sw.Close();
            }
            StreamReader sr = new StreamReader("foglalasok.csv");
            while (!sr.EndOfStream)
            {
                string beolvas = sr.ReadLine();
                int index = int.Parse(beolvas.Split(';')[0]);
                string nev = beolvas.Split(';')[1];
                string email_cim = beolvas.Split(';')[2];
                string mettol = beolvas.Split(';')[3];
                string meddig = beolvas.Split(';')[4];
                string szobaszam = beolvas.Split(';')[5];
                Foglalas foglalas = new Foglalas(index, nev, email_cim, mettol, meddig, szobaszam);
                foglalasok.Add(foglalas);
            }
            sr.Close();
        }

        static void Kiir()
        {
            Console.Clear();
            foreach (Foglalas foglalas in foglalasok)
            {
                Rajzol(true);
                KozepreIgazit(foglalas.Nev, true, false);
                KozepreIgazit(foglalas.Email_cim, true, false);
                KozepreIgazit(foglalas.Mettol, true, false);
                KozepreIgazit(foglalas.Meddig, true, false);
                KozepreIgazit(foglalas.Szobaszam, true, false);
                Rajzol(false);
            }
        }

        static void FormRajzolo(int index, string szoveg)
        {
            Console.SetCursorPosition(0, 0 + index * 8);
            Rajzol(true);
            KozepreIgazit(szoveg, true, false);
            Console.SetCursorPosition(0, 6 + index * 8);
            Rajzol(true);
        }

        static string AdatSzedo(int index)
        {
            string vendeg_adata = "";
            string temp = "";
            string pool = "öüóqwertzuiopőúasdfghjkléáűíyxcvbnmÖÜÓQWERTZUIOPŐÚASDFGHJKLÉÁŰÍYXCVBNM @.-_0123456789";
            while (temp != "\r")
            {
                temp = Console.ReadKey(true).KeyChar.ToString();
                if (pool.Contains(temp) && vendeg_adata.Length < Console.WindowWidth - 2)
                {
                    vendeg_adata += temp;
                }
                if (temp == "\b" && vendeg_adata.Length != 0)
                {
                    vendeg_adata = vendeg_adata.Remove(vendeg_adata.Length - 1, 1);
                }
                Console.SetCursorPosition(0, 5 + index * 8);
                for (int i = 0; i < Console.WindowWidth; i++)
                {
                    Console.Write(' ');
                }
                Console.SetCursorPosition(0, 5 + index * 8);
                KozepreIgazit(vendeg_adata, false, true);
            }
            return vendeg_adata;
        }

        static void Bevitel(string fajlnev)
        {
            Console.Clear();
            string[] formszovegek = { "Adja meg a vendég nevét:", "Adja meg a vendég e-mail címét:", "A vendég az alábbi időponttól foglalja le a szobát:", "A vendég az alábbi időpontig foglalja le a szobát:", "A vendég az alábbi szobát kapja meg:" };
            for (int i = 0; i < 5; i++)
            {
                FormRajzolo(i, formszovegek[i]);
            }
            Console.SetCursorPosition(0, 0);
            int index;
            if (foglalasok.Count == 0)
            {
                index = 1;
            }
            else
            {
                index = foglalasok[foglalasok.Count - 1].Index + 1;
            }
            string nev = AdatSzedo(0);
            string email_cim = AdatSzedo(1);
            string mettol = AdatSzedo(2);
            string meddig = AdatSzedo(3);
            string szobaszam = AdatSzedo(4);
            Foglalas foglalas = new Foglalas(index, nev, email_cim, mettol, meddig, szobaszam);
            foglalasok.Add(foglalas);
            StreamWriter sw = new StreamWriter(fajlnev, true);
            sw.WriteLine("{0};{1};{2};{3};{4};{5}", index, nev, email_cim, mettol, meddig, szobaszam);
            sw.Flush();
            sw.Close();
        }

        static void Menu()
        {
            while (true)
            {
                Console.Clear();
                Rajzol(true);
                KozepreIgazit("Adatbevitel", true, true);
                KozepreIgazit("Vendégek listázása", true, false);
                Rajzol(true);
                int menupont = 1;
                ConsoleKeyInfo menu;
                do
                {
                    menu = Console.ReadKey(true);
                    if ((menu.Key == ConsoleKey.UpArrow && menupont == 1) || (menu.Key == ConsoleKey.DownArrow && menupont == 1))
                    {
                        menupont = 2;
                        Console.SetCursorPosition(0, 3);
                        KozepreIgazit("Adatbevitel", false, false);
                        Console.SetCursorPosition(0, 4);
                        KozepreIgazit("Vendégek listázása", false, true);
                    }
                    else if ((menu.Key == ConsoleKey.UpArrow && menupont == 2) || (menu.Key == ConsoleKey.DownArrow && menupont == 2))
                    {
                        menupont = 1;
                        Console.SetCursorPosition(0, 4);
                        KozepreIgazit("Vendégek listázása", false, false);
                        Console.SetCursorPosition(0, 3);
                        KozepreIgazit("Adatbevitel", false, true);
                    }
                    if (menu.Key == ConsoleKey.Enter)
                    {
                        switch (menupont)
                        {
                            case 1:
                                Bevitel("foglalasok.csv");
                                ToltoKepernyo("Az adatbevitel sikeresen megtörtént!\n");
                                break;
                            case 2:
                                Kiir();
                                Console.ReadKey();
                                ToltoKepernyo("Kész!\n");
                                break;
                        }
                    }
                } while (menu.Key != ConsoleKey.Enter);
            }
        }

        static void ToltoKepernyo(string szoveg)
        {
            Console.Clear();
            Rajzol(true);
            KozepreIgazit(szoveg, true, false);
            KozepreIgazit("Menü betöltése...", true, false);
            Rajzol(true);
            Thread.Sleep(2000);
        }

        static void Rajzol(bool ujsor)
        {
            Console.WriteLine();
            for (int i = 0; i < Console.WindowWidth; i++)
            {
                Console.Write('▬');
            }
            if (ujsor)
            {
                Console.WriteLine();
            }
        }

        static void KozepreIgazit(string szo, bool ujsor, bool hatter)
        {
            for (int i = 0; i < Console.WindowWidth / 2 - szo.Length / 2; i++)
            {
                Console.Write(' ');
            }
            if (ujsor)
            {
                if (hatter)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine(szo);
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.WriteLine(szo);
                }
            }
            if (!ujsor)
            {
                if (hatter)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(szo);
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.Write(szo);
                }
            }
        }

        static void Beallit()
        {
            Console.Title = "Foglalás nyilvántartó";
            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WindowHeight = 41;
        }
    }

    class Foglalas
    {
        private int index;
        private string nev;
        private string email_cim;
        private string mettol;
        private string meddig;
        private string szobaszam;

        public Foglalas(int index, string nev, string email_cim, string mettol, string meddig, string szobaszam)
        {
            this.index = index;
            this.nev = nev;
            this.email_cim = email_cim;
            this.mettol = mettol;
            this.meddig = meddig;
            this.szobaszam = szobaszam;
        }

        public int Index
        {
            get
            {
                return index;
            }
        }

        public string Nev
        {
            get
            {
                return nev;
            }
        }

        public string Email_cim
        {
            get
            {
                return email_cim;
            }
        }

        public string Mettol
        {
            get
            {
                return mettol;
            }
        }

        public string Meddig
        {
            get
            {
                return meddig;
            }
        }

        public string Szobaszam
        {
            get
            {
                return szobaszam;
            }
        }
    }
}
