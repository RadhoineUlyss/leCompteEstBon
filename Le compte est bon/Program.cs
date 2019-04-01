using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Le_compte_est_bon
{
    class Program
    {
        static int Niveau = 0;
        static int N = 6;     // Nombre de nombres
        struct OPERATION
        {
            public int Total;
            public int N1, N2;
            public char op;
            public bool interm;
        }
        static OPERATION[] Ops = new OPERATION[10];


        static void RetenirOpération(int Niv, int N1, char op, int N2, int TOTAL, bool interm)
        {
            Ops[Niv].N1 = N1; Ops[Niv].N2 = N2; Ops[Niv].Total = TOTAL;
            Ops[Niv].op = op;
            Ops[Niv].interm = interm;
            if (Niveau < Niv) Niveau = Niv;
        }


        static void AfficherRésultat()
        {
            // Afficher d'abord les calculs intermédiaires
            for (int i = 0; i < Niveau; i++)
                if (Ops[i].interm)
                    Console.WriteLine(Ops[i].N1 + " " + Ops[i].op + " " + Ops[i].N2 + " = " + Ops[i].Total);

            for (int i = Niveau; i >= 0; i--)
                if (Ops[i].interm == false)
                    Console.WriteLine(Ops[i].N1 + " " + Ops[i].op + " " + Ops[i].N2 + " = " + Ops[i].Total);
        }


        public static void Main()
        {
            string r;
            Console.WriteLine(" Pour jouer contre le PC Taper 1");
            Console.WriteLine("Pour que le PC joue contre vous taper 2 ");
            r = Console.ReadLine();

            if (r == "1")
            {
                Program.contrePC();
            }else if (r == "2")
            {
                Program.pcContreUser();
            }
            else
            {
                Console.Write(" Veuillez recommencer et saisir que 1 ou 2, Merci");
            }
            Console.Read();

        }

        public static void contrePC()
        {
            int[] Nombres = new int[N];
            int Total;

            // saisir les N nombres
            string s;
            for (int i = 0; i < N; ++i)
            {
                Console.Write("nombre " + i + " : ");
                s = Console.ReadLine();
                Nombres[i] = Int32.Parse(s);
            }

            // saisie du total
            Console.Write("Donnez le nombre a trouver : ");
            s = Console.ReadLine(); Total = Int32.Parse(s);
            Console.WriteLine();

            // rechercher LA solution
            bool trouvé = lcb(Nombres, Total, 0);
            if (trouvé == true)
            {
                Console.WriteLine("Le compte est bon !");
                AfficherRésultat();
                Console.Read();
                return;
            }

            // LA solution n'ayant pas été trouvée, rechercher la solution la plus proche
            int inc = 0;
            do
            {
                trouvé = lcb(Nombres, Total + inc, 0);
                if (trouvé == false) trouvé = lcb(Nombres, Total - inc, 0);
                inc++;
            } while (trouvé == false);
            Console.WriteLine("Meilleure solution : ");
            AfficherRésultat();
            Console.Read();
        }


        public static void pcContreUser()
        {

            int[] Nombres = new int[N];
            int aTrouver;

            Random aleatoire = new Random();
            Console.WriteLine("Les nombre à utiliser sont : ");

            for (int i = 0; i < 2; ++i) //pour les deux premier nombre
            {
                Nombres[i] = aleatoire.Next(24) + 2; //Génère un entier compris entre 2 et 25
                Console.Write(Nombres[i] + " ");
            }

            for (int i = 2; i < 4; ++i)
            {
                Nombres[i] = aleatoire.Next(225) + 26; //Génère un entier compris entre 26 et 250
                Console.Write(Nombres[i] + " ");
            }

            for (int i = 4; i < 6; ++i)
            {
                Nombres[i] = aleatoire.Next(850) + 251; //Génère un entier compris entre 251 et 1000
                Console.Write(Nombres[i] + " ");
            }
            Console.WriteLine(" ");
            aTrouver = aleatoire.Next(600) + 402; //Génère un nombre a trouver compris entre 400 et 1000



            // rechercher LA solution
            bool trouvé = lcb(Nombres, aTrouver, 0);

            Console.WriteLine("Veuillez patienter on vérifie que le nombre a cherchée est possible a calculée avec les nombres donné...Merci");

            while (trouvé == false)
            {
                aTrouver = aleatoire.Next(600) + 402; //Génère un nombre a trouver compris entre 400 et 1000
                trouvé = lcb(Nombres, aTrouver, 0);
            }


            Console.WriteLine("Le nombre à trouver est : " + aTrouver);
            Console.WriteLine("Bonne chance !");



            List<int> NombresList = new List<int>(); // creation de la lsite


            for (int i = 0; i < Nombres.Length; ++i) // convertion de notre tabeau en liste
            {
                NombresList.Add(Nombres[i]);
            }

            Console.WriteLine("Vous avez 4 chances pour trouver le nombre " + aTrouver);

            for (int tour = 1; tour < 5; ++tour) {

                Console.WriteLine(" Tour " + tour);


                bool ExistDansLaListe = Program.Deviner(NombresList);
                while (ExistDansLaListe == false)
                {
                    Console.WriteLine("Veuillez bien saisir deux des chiffre de la liste indiquer, Merci");

                    ExistDansLaListe = Program.Deviner(NombresList);

                }

                if(NombresList.Last() == aTrouver && tour != 4)
                {
                    Console.WriteLine("************** Bravo Vous aviez GAGNIER! ************** ");
                }
                else if(NombresList.Last() != aTrouver && tour != 4)
                {
                    int nbr = 4 - tour;

                    Console.WriteLine("il vous reste "+ nbr + " chance(s) pour trouver le nombre " + aTrouver);
                    Console.WriteLine("******************************************");
                    Console.WriteLine("Voici Vos nouveau nombre a utiliser : ");
                    for (int i = 0; i < NombresList.Count; ++i) 
                    {
                        Console.Write(NombresList[i]+" ");
                    }
                    Console.WriteLine(" ");
                }else {
                             

                    Console.WriteLine(" ************** Vous aviez PERDU !!!! , Veuillez réessayer ************** ");
                    Console.WriteLine(" 1 : ressayer ");
                    Console.WriteLine(" 2 : quitter ");

                    string choix = Console.ReadLine();
                    if (choix == "1")
                    {
                        Program.Main();
                    }else if (choix == "2")
                    {
                        Environment.Exit(0);

                    }
                    else {
                        Console.WriteLine(" Choix incorrecte ");
                    }
                }
            }

        }

        static bool Deviner(List<int> NombresList)
        {
            Console.WriteLine("Veuillez saisir deux nombre avec un operateur \" + - * \"   (pas de division) , exemple ( X-Y ) : ");

            String expression;
            expression = Console.ReadLine();

            string valeur = new DataTable().Compute(expression, null).ToString(); //permet de calculer une expression retournera 10
            int result = Int32.Parse(valeur);

            Console.WriteLine(expression + " = " + valeur);

            String pattern = @"(\d+)([-+*/])(\d+)"; // égal à A+B

            foreach (Match m in Regex.Matches(expression, pattern)) //a+b=c
            {
                int v1 = Int32.Parse(m.Groups[1].Value); // a
                string v2 = m.Groups[2].Value; // +
                int v3 = Int32.Parse(m.Groups[3].Value); // b

   
                if (NombresList.Exists(element => element == v1) && NombresList.Exists(element => element == v3))
                {

                    //Verifier que les deux nombre existe et supprimer et ajouté le resultat

                    string lines;
                    lines = v1 + " " + v2 + " " + v3 + "\n";
                    NombresList.Remove(v1);
                    NombresList.Remove(v3);
                    NombresList.Add(result);

                }
                else
                {
                    return false;
                }
                 
           

            }

            return true;

        }

    static bool EssaiOpération(int TOTAL, int NB, int[] tr, int Niv)
        {
            bool trouvé;

            if (TOTAL <= 0) return false;

            // Peut-on réaliser TOTAL = NB + ? (? désignant une combinaison des nombres restants)
            if (TOTAL > NB)
            {
                trouvé = lcb(tr, TOTAL - NB, Niv + 1);
                if (trouvé)
                {
                    RetenirOpération(Niv, NB, '+', (TOTAL - NB), TOTAL, false);
                    return true;
                }
            }

            // Peut-on réaliser TOTAL = NB - ?
            if (TOTAL < NB)
            {
                trouvé = lcb(tr, NB - TOTAL, Niv + 1);
                if (trouvé)
                {
                    RetenirOpération(Niv, NB, '-', (NB - TOTAL), TOTAL, false);
                    return true;
                }
            }

            // Peut-on réaliser TOTAL = ? - NB
            trouvé = lcb(tr, NB + TOTAL, Niv + 1);
            if (trouvé)
            {
                RetenirOpération(Niv, (TOTAL + NB), '-', NB, TOTAL, false);
                return true;
            }

            // Peut-on réaliser TOTAL = NB * ?
            if (TOTAL % NB == 0)
            {
                trouvé = lcb(tr, TOTAL / NB, Niv + 1);
                if (trouvé)
                {
                    RetenirOpération(Niv, NB, '*', (TOTAL / NB), TOTAL, false);
                    return true;
                }
            }

            // Peut-on réaliser TOTAL = NB / ?
            if (NB % TOTAL == 0)
            {
                trouvé = lcb(tr, NB / TOTAL, Niv + 1);
                if (trouvé)
                {
                    RetenirOpération(Niv, NB, '/', (NB / TOTAL), TOTAL, false);
                    return true;
                }
            }
            return false;
        }

        static bool lcb(int[] N, int TOTAL, int Niv)
        {
            bool trouvé;
            int[] tr;

            if (TOTAL <= 0 || N.Length == 0)
            {
                //    Console.WriteLine("Dans lcb avec TOTAL < 0");
                return false;
            }


            // trier le tableau des nombres restants par ordre décroissant
            Array.Sort(N); Array.Reverse(N);


            // Un seul des nombres restants ferait-il l'affaire ?
            for (int i = 0; i < N.Length; i++)
                if (N[i] == TOTAL) return true;

            // s'il ne reste plus que deux nombres ....
            if (N.Length == 2) return TrouverDansDeux(TOTAL, N[0], N[1], Niv);

            // prendre l'un des nombres restants et essayer une combinaison à partir de ce nombre
            for (int i = 0; i < N.Length; i++)
            {
                // Préparer le tableau tr des nombres restants
                tr = new int[N.Length - 1];
                for (int j = 0, k = 0; j < N.Length; j++)
                    if (i != j) { tr[k] = N[j]; k++; }

                trouvé = EssaiOpération(TOTAL, N[i], tr, Niv);
                if (trouvé == true) return true;
            }

            // essayer ensuite une opération à partir de deux nombres

            if (N.Length < 3)
            {
                Console.WriteLine("Ici avec un tableau de moins de 3 éléments !!");
                return false;
            }

            for (int i = 0; i < N.Length; i++)                      // choix du premier nombre
            {
                // prendre un deuxième nombre parmi les restants
                for (int j = i + 1; j < N.Length; j++)                   // choix du deuxième nombre
                {
                    tr = new int[N.Length - 2];
                    int n = 0;
                    for (int k = 0; k < N.Length; k++)
                        if (k != i && k != j) { tr[n] = N[k]; n++; }

                    // essayer l'addition de ces deux nombres  
                    // ajouter la somme au tableau des nombres restants
                    int[] tr2 = new int[tr.Length + 1];
                    for (int k = 0; k < tr.Length; k++) tr2[k] = tr[k]; tr2[tr.Length] = N[i] + N[j];
                    Array.Sort(tr2); Array.Reverse(tr2);
                    trouvé = lcb(tr2, TOTAL, Niv + 1);
                    if (trouvé == true)
                    {
                        RetenirOpération(Niv, N[i], '+', N[j], N[i] + N[j], true);
                        return true;
                    }

                    // essayer la soustraction de ces deux nombres   
                    // ajouter la différence au tableau des nombres restants
                    tr2 = new int[tr.Length + 1];
                    for (int k = 0; k < tr.Length; k++) tr2[k] = tr[k]; tr2[tr.Length] = N[i] - N[j];
                    Array.Sort(tr2); Array.Reverse(tr2);
                    if (N[i] != N[j])
                    {
                        trouvé = lcb(tr2, TOTAL, Niv + 1);
                        if (trouvé == true)
                        {
                            RetenirOpération(Niv, N[i], '-', N[j], N[i] - N[j], true);
                            return true;
                        }
                    }

                    // essayer la multiplication de ces deux nombres   
                    // ajouter le produit au tableau des nombres restants
                    tr2 = new int[tr.Length + 1];
                    for (int k = 0; k < tr.Length; k++) tr2[k] = tr[k]; tr2[tr.Length] = N[i] * N[j];
                    Array.Sort(tr2); Array.Reverse(tr2);
                    trouvé = lcb(tr2, TOTAL, Niv + 1);
                    if (trouvé == true)
                    {
                        RetenirOpération(Niv, N[i], '*', N[j], N[i] * N[j], true);
                        return true;
                    }

                    // essayer la division de ces deux nombres 
                    if (N[i] % N[j] == 0)
                    {
                        tr2 = new int[tr.Length + 1];
                        for (int k = 0; k < tr.Length; k++) tr2[k] = tr[k]; tr2[tr.Length] = N[i] / N[j];
                        Array.Sort(tr2); Array.Reverse(tr2);
                        trouvé = lcb(tr2, TOTAL, Niv + 1);
                        if (trouvé == true)
                        {
                            RetenirOpération(Niv, N[i], '/', N[j], N[i] / N[j], true);
                            return true;
                        }
                    }

                }
            }

            return false;
        }


        static bool TrouverDansDeux(int TOTAL, int N1, int N2, int Niv)
        {

            if (TOTAL == N1 + N2)
            {
                RetenirOpération(Niv, N1, '+', N2, TOTAL, false);
                return true;
            }

            if (TOTAL == N1 - N2)
            {
                RetenirOpération(Niv, N1, '-', N2, TOTAL, false);
                return true;
            }

            if (TOTAL == N2 - N1)
            {
                RetenirOpération(Niv, N2, '-', N1, TOTAL, false);
                return true;
            }

            if (TOTAL == N1 * N2)
            {
                RetenirOpération(Niv, N1, '*', N2, TOTAL, false);
                return true;
            }

            if (N1 > N2 && N1 % N2 == 0 && TOTAL == N1 / N2)
            {
                RetenirOpération(Niv, N1, '/', N2, TOTAL, false);
                return true;
            }

            if (N2 > N1 && N2 % N1 == 0 && TOTAL == N2 / N1)
            {
                RetenirOpération(Niv, N2, '/', N1, TOTAL, false);
                return true;
            }

            return false;
        }           // fin de la fonction TrouverDansDeux
    }            // fin de la classe LeCompteEstBon
}            // fin de l'espace de noms