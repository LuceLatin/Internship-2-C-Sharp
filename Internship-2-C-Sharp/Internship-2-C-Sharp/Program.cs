using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.Intrinsics.X86;

var tablica = new Dictionary<string, (int points, int zabijeniGolovi, int primljeniGolovi)>()
{
    {"Hrvatska", (0, 0, 0)},
    {"Maroko", (0, 0, 0)},
    {"Belgija", (0, 0, 0)},
    {"Kanada", (0, 0, 0)},
};


var popisIgracaDict = new Dictionary<string, (string position, int rating)>()
{
    {"Luka Modric", ("MF", 88)},
    {"Marcelo Brozovic", ("MF", 86)},
    {"Mateo Kovacic", ("MF", 84)},
    {"Ivan Perisic", ("FW", 84)},
    {"Andrej Kramaric", ("FW", 82)},
    {"Josip Stanisic", ("DF", 72)},
    {"Josko Gvardiol", ("DF", 81)},
    {"Mario Pasalic", ("MF", 81)},
    {"Lovro Majer", ("MF", 80)},
    {"Dominik Livakovic", ("GK", 80)},
    {"Josip Sutalo", ("DF", 75)},
    {"Luka Sucic", ("MF", 73)},
    {"Borna Sosa", ("DF", 78) },
    {"Nikola Vlasic", ("MF", 78)},
    {"Bruno Petkovic", ("FW", 75)},
    {"Dejan Lovren", ("DF", 78)},
    {"Mislav Orsic", ("FW", 77)},
    {"Marko Livaja", ("FW", 77)},
    {"Domagoj Vida", ("DF", 76)},
    {"Ante Budimir", ("FW", 76)},
    {"Kristijan Jakic", ("MF", 76)},
    {"Ivica Ivusic", ("GK", 72)}, 

};


var rezultatiUtakmica = new Dictionary<int, (string team1, string team2, int goals1, int goals2)>();
var strijelci = new Dictionary<int, (string country, string name, int goals)>();

MainMenu(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);


void MainMenu(Dictionary<string, (string position, int rating)> popisIgracaDict, Dictionary<string, (int points, int zabijeniGolovi, int primljeniGolovi)> tablica,
    Dictionary<int, (string team1, string team2, int goals1, int goals2)> rezultatiUtakmica, ref Dictionary<int, (string country, string name, int goals)> strijelci)
{
    Console.Clear();
    Console.WriteLine("GLAVNI IZBORNIK: ");
    Console.WriteLine("1 - Odradi trening");
    Console.WriteLine("2 - Odigraj utakmicu");
    Console.WriteLine("3 - Statistika");
    Console.WriteLine("4 - Kontrola igraca");
    Console.WriteLine("0 - Izlaz iz aplikacije");

    int izbornik;
    do
    {
        Console.WriteLine("Izaberite jednu mogucnost 1/2/3/4/0:");
        izbornik = int.Parse(Console.ReadLine());
    }
    while (izbornik != 0 && izbornik != 1 && izbornik != 2 && izbornik != 3 && izbornik != 4);

    Console.Clear();

    switch (izbornik)
    {
        case 0:
            Console.WriteLine("izlaz iz aplikacije"); 
            break;
        case 1:
            Console.WriteLine("ODRADI TRENING");
            DoTraining(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);
            break;
        case 2:
            Console.WriteLine("ODRADI UTAKMICU");
            PlayAMatch(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);
            break;
        case 3:
            Console.Clear();
            Console.WriteLine("STATISTIKA");
            StatisticMenu(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);
            break;
        case 4:
            Console.WriteLine("KONTROLA IGRACA");
            IzbornikKontroleIgraca(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);
            break;
        default:
            Console.WriteLine("Netocno unesen broj!");
            break;
    }
    Console.WriteLine("The end");

}

void DoTraining(Dictionary<string, (string position, int rating)> popisIgracaDict, Dictionary<string, (int points, int zabijeniGolovi, int primljeniGolovi)> tablica,
    Dictionary<int, (string team1, string team2, int goals1, int goals2)> rezultatiUtakmica, ref Dictionary<int, (string country, string name, int goals)> strijelci) 
{
    Random rnd = new Random();
    foreach (var player in popisIgracaDict)
    {
        var num = (rnd.Next(-5, 6)); // returns random integers >= -5 and < 6
        var newRating = player.Value.rating + (player.Value.rating * (float)num / 100);
        Console.WriteLine($"Igrac: {player.Key}, Pozicija: {player.Value.position}, Stari rating: {player.Value.rating}, Novi rating: {(int)Math.Round(newRating)}.");
        popisIgracaDict[player.Key]=(player.Value.position, (int)Math.Round(newRating)); 
    }
    Console.WriteLine();
    if (PovratakNaGlavniIzbornik())
        MainMenu(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);
}


int RandomNumber()
{
    Random rnd = new Random();
    var num = (rnd.Next(0,7));
    return num;
}


void RatingAfterMatch(ref Dictionary<string, (string position, int rating)> popisIgracaDict, Dictionary<string, (string position, int rating)> bestEleven)
{
    foreach (var item in popisIgracaDict)
    {
        if (bestEleven.ContainsKey(item.Key))
            popisIgracaDict[item.Key]=(item.Value.position, bestEleven[item.Key].rating);
    }
    
}


void PlayAMatch (Dictionary<string, (string position, int rating)> popisIgracaDict, Dictionary<string, (int points, int zabijeniGolovi, int primljeniGolovi)> tablica,
    Dictionary<int, (string team1, string team2, int goals1, int goals2)> rezultatiUtakmica, ref Dictionary<int, (string country, string name, int goals)> strijelci)
{
    //Tablica 
    float newRating;
    Dictionary<string, (string position, int rating)> bestEleven = ReturnBestElevenByRating(popisIgracaDict);
    //
    Random rnd = new Random();
    var brojac = 0;
    var keyPraviStrijelci = 0;
    var potencijalniStrijelci = new Dictionary<int, (string country, string name, int goals)>();
    
    foreach (var player in bestEleven)
    {
        if (player.Value.position == "FW")
        {
            potencijalniStrijelci.Add(brojac, ("", player.Key, 0));
            brojac++;
        }
    }
    
    if (bestEleven.Count < 11)
    {
        Console.WriteLine("Nije moguce odigrati utakmicu jer nema 11 igraca!");
    }
    else
    {

        var iBodovi = 0;
        var jBodovi = 0;
        var cnt = 0; //ukljucena dva brojaca da napravim kombinaciju bez ponavljanja 
        var count = 0;
        var key = 0; //key za dict rezultatiUtakmica
        foreach (var i in tablica)
        {
            count++;
            foreach (var j in tablica)
            {
                if (cnt != 0)
                {
                    cnt--;
                    continue;
                }
                if (i.Key != j.Key)
                {
                    var randomZabijeniGolovi = RandomNumber();
                    var randomPrimljeniGolovi = RandomNumber();
                    if (randomZabijeniGolovi > randomPrimljeniGolovi)
                    {
                        iBodovi = 3;
                        jBodovi = 0;
                    }
                    else if (randomZabijeniGolovi < randomPrimljeniGolovi)
                    {
                        iBodovi = 0;
                        jBodovi = 3;
                    }
                    else
                    {
                        iBodovi = 1;
                        jBodovi = 3;
                    }

                    tablica[i.Key] = (i.Value.points + iBodovi, i.Value.zabijeniGolovi + randomZabijeniGolovi, i.Value.primljeniGolovi + randomPrimljeniGolovi);
                    tablica[j.Key] = (j.Value.points + jBodovi, j.Value.zabijeniGolovi + randomPrimljeniGolovi, i.Value.primljeniGolovi + randomZabijeniGolovi);

                    //rezultati utakmica

                    rezultatiUtakmica.Add(key, (i.Key, j.Key, randomZabijeniGolovi, randomPrimljeniGolovi));
                    key++;
                    Console.WriteLine($"\nUtakmica: {i.Key} : {j.Key} završila je {randomZabijeniGolovi} : {randomPrimljeniGolovi}\n");
                    Console.WriteLine("Trenutna tablica:");
                    foreach (var item in tablica)
                    {
                        Console.WriteLine($"{item.Key}, points: {item.Value.points}, gol razlika {item.Value.zabijeniGolovi} : {item.Value.primljeniGolovi}");
                    }

                    if (i.Key == "Hrvatska")
                    {
                        for (var golovi = 0; golovi < randomZabijeniGolovi; golovi++)
                        {
                            var randomStrijelac = (rnd.Next(0, potencijalniStrijelci.Count));
                            var zapamtiKey = 0;
                            foreach (var player in potencijalniStrijelci)
                            {
                                if(player.Key==randomStrijelac)
                                {
                                    var k = 0;
                                    foreach (var item in strijelci)
                                    {
                                        if (player.Value.name==item.Value.name && j.Key == item.Value.country)
                                        {
                                            k++;// ako igrac vec nije u listi strijelaca
                                            zapamtiKey = item.Key;
                                        }
                                    }
                                    if (k == 0)
                                    {
                                        strijelci.Add(keyPraviStrijelci, (j.Key, player.Value.name, 1));
                                        keyPraviStrijelci++;
                                    }
                                    else
                                        strijelci[zapamtiKey] =  (j.Key, player.Value.name, strijelci[zapamtiKey].goals + 1);
                                 }
                            }
                        }
                        if (iBodovi == 3)
                        {
                            foreach (var player in bestEleven)
                            {
                                if (player.Value.position == "FW")
                                    newRating = player.Value.rating + (player.Value.rating * (float)5 / 100);
                                else
                                    newRating = player.Value.rating + (player.Value.rating * (float)2 / 100);
                                bestEleven[player.Key] = (bestEleven[player.Key].position, (int)newRating);
                            }
                        }
                        else if (iBodovi == 1)
                        {
                            foreach (var player in bestEleven)
                            {
                                if (player.Value.position == "FW")
                                    newRating = player.Value.rating + (player.Value.rating * (float)-5 / 100);
                                else
                                    newRating = player.Value.rating + (player.Value.rating * (float)-2 / 100);

                                bestEleven[player.Key] = (bestEleven[player.Key].position, (int)newRating);
                            }
                        }
                        
                    }
                }
            }
            cnt = count;
        }
        /*
        foreach (var player in bestEleven)
        {
            Console.WriteLine(player);

        }
        */
        RatingAfterMatch(ref popisIgracaDict, bestEleven);
        if (PovratakNaGlavniIzbornik())
            MainMenu(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);
    }
}


void IzbornikKontroleIgraca(Dictionary<string, (string position, int rating)> popisIgracaDict, Dictionary<string, 
    (int points, int zabijeniGolovi, int primljeniGolovi)> tablica, Dictionary<int, (string team1, string team2, int goals1, int goals2)> rezultatiUtakmica,
    ref Dictionary<int, (string country, string name, int goals)> strijelci)
{
    //Console.Clear();
    Console.WriteLine("IZBORNIK KONTROLE IGRACA");
    Console.WriteLine("1 - Unos novog igraca");
    Console.WriteLine("2 - Brisanje igraca");
    Console.WriteLine("3 - Uređivanje igraca");
    Console.WriteLine("0 - Izlaz iz aplikacije"); //povrat na glavni izbornik
    Console.WriteLine("Unesite broj 0/1/2/3:");
    int choice;
    do
    {
        choice = int.Parse(Console.ReadLine());
    }
    while (choice != 0 && choice != 1 && choice != 2 && choice != 3);
    switch(choice)
    {
        case 0:
            MainMenu(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);
            break;
        case 1:
            Console.Clear();
            Console.WriteLine("UNOS NOVOG IGRACA"); //pass
            UnosNovogIgraca(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);
            break;
        case 2:
            Console.Clear();
            Console.WriteLine("BRISANJE IGRACA");
            DeletePlayerMenu(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);
            break;
        case 3:
            Console.Clear();
            Console.WriteLine("UREDIVANJE IGRACA");
            EditingPlayerMenu(popisIgracaDict, tablica , rezultatiUtakmica, ref strijelci);
            break;
    }
}

//KONTROLA IGRACA
void UnosNovogIgraca(Dictionary <string, (string position, int rating)> popisIgracaDict, Dictionary<string, (int points, int zabijeniGolovi, int primljeniGolovi)> 
    tablica, Dictionary<int, (string team1, string team2, int goals1, int goals2)> rezultatiUtakmica, ref Dictionary<int, (string country, string name, int goals)> strijelci)
{
    if (popisIgracaDict.Count == 26)
    {
        Console.WriteLine("Uneseni broj igraca je vec dosegao maksimum od 26 igraca! Nije moguce unijeti dodatnog igraca!");
        if (PovratakNaGlavniIzbornik())
            MainMenu(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);
    }
    else
    {
        Console.WriteLine("Unesite podatke o igracu kojeg zelite dodati:");
        var playerName = EnterPlayerName1(popisIgracaDict);
        var positionOfPlayer = EnterPlayerPosition(popisIgracaDict);
        var ratingOfPlayer=EnterPlayerRating(popisIgracaDict);

        if (Confirmation())
        {
            popisIgracaDict.Add(playerName, (positionOfPlayer, ratingOfPlayer));
            Console.WriteLine("Promjene su napravljene. Igrac je dodan.");
        }
        else
            Console.WriteLine("Promjene se nisu dogodile. Igrac nije dodan.");
        
    }
    if (PovratakNaGlavniIzbornik())
        MainMenu(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);

}

string EnterPlayerName1(Dictionary<string, (string position, int rating)> popisIgracaDict)
{
    var playerName = "";
    while (true)
    {
        Console.WriteLine("Unesite ime i prezime igraca:");
        playerName = Console.ReadLine(); 
        if (playerName == null || playerName == String.Empty)
        {
            Console.WriteLine("Nije uneseno ime!"); 
            continue;
        }
        else if (popisIgracaDict.ContainsKey(playerName))
        {
            Console.WriteLine("Taj igrac vec postoji. Nije moguce imati dva igraca s istim imenom.");
            continue;
        }
        else 
            break;
    }
    return playerName;
}


int EnterPlayerRating(Dictionary<string, (string position, int rating)> popisIgracaDict)
{
    int ratingOfPlayer;
    while (true)
    {
        Console.WriteLine("Unesi rating igraca (broj od 0 do 100):");
        ratingOfPlayer = int.Parse(Console.ReadLine());
        if (ratingOfPlayer > 100 || ratingOfPlayer < 0) 
        {
            Console.WriteLine("Unesen krivi broj! Rating moze biti izmedju 0 i 100!"); 
            continue;
        }
        else
            break;
    }
    return ratingOfPlayer;
}

string EnterPlayerPosition(Dictionary<string, (string position, int rating)> popisIgracaDict)
{
    var positionOfPlayer = "";
    while (true)
    {
        Console.WriteLine("Unesite poziciju igraca GK/DF/MF/FW:");
        positionOfPlayer = Console.ReadLine().ToUpper();
        if (positionOfPlayer == null || positionOfPlayer == String.Empty)
        {
            Console.WriteLine("Nije unesena pozicija"); 
            continue;
        }
        else if (positionOfPlayer != "GK" && positionOfPlayer != "DF" && positionOfPlayer != "MF" && positionOfPlayer != "FW") 
        {
            Console.WriteLine("Netocno unesena pozicija igraca!");
            continue;
        }
        else
            break;
    }
    return positionOfPlayer;
}

//ispis igraca po redosljedu kako su uneseni
void IspisIgraca(Dictionary<string, (string position, int rating)> popisIgracaDict, Dictionary<string,
    (int points, int zabijeniGolovi, int primljeniGolovi)> tablica, Dictionary<int, (string team1, string team2, int goals1, int goals2)> rezultatiUtakmica,
    ref Dictionary<int, (string country, string name, int goals)> strijelci)
{
    foreach (var item in popisIgracaDict)
        Console.WriteLine($" Player: {item.Key}, Position: {item.Value.position}, Rating: {item.Value.rating}");
    if (PovratakNaGlavniIzbornik())
        MainMenu(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);

}


void DeletePlayerMenu(Dictionary<string, (string position, int rating)> popisIgracaDict, Dictionary<string, (int points, int zabijeniGolovi, int primljeniGolovi)>
    tablica, Dictionary<int, (string team1, string team2, int goals1, int goals2)> rezultatiUtakmica, ref Dictionary<int, (string country, string name, int goals)> strijelci)
{
    Console.Clear();
    Console.WriteLine("0 - Povratak na prethodni izbornik");
    Console.WriteLine("1 - Brisanje igrača unosom imena i prezimena");
    Console.WriteLine("Unesi broj 0 ili 1");
    int num;
    while(true)
    {
        num = int.Parse(Console.ReadLine());
        if (num == 0)
        {
            IzbornikKontroleIgraca(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);
            continue;
        }
        else if (num == 1)
        {
            DeletePlayer(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);
            continue;
        }
        else
            break;
    }
    
}

void DeletePlayer(Dictionary<string, (string position, int rating)> popisIgracaDict, Dictionary<string,
    (int points, int zabijeniGolovi, int primljeniGolovi)> tablica, Dictionary<int, (string team1, string team2, int goals1, int goals2)> rezultatiUtakmica,
    ref Dictionary<int, (string country, string name, int goals)> strijelci)
{
    Console.WriteLine("Unesite ime i prezime igraca kojeg želite izbrisati:");
    var player=Console.ReadLine();
    popisIgracaDict.Remove(player);

    if (Confirmation() == true)
    {
        popisIgracaDict.Remove(player);
        Console.WriteLine("Promjene su napravljene.");
        Console.WriteLine("Igrac je izbrisan.");

    }
    else
        Console.WriteLine("Promjene se nisu dogodile.");
    if (PovratakNaGlavniIzbornik())
        MainMenu(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);

}

void EditingPlayerMenu(Dictionary<string, (string position, int rating)> popisIgracaDict, Dictionary<string, (int points, int zabijeniGolovi, int primljeniGolovi)>
    tablica, Dictionary<int, (string team1, string team2, int goals1, int goals2)> rezultatiUtakmica, ref Dictionary<int, (string country, string name, int goals)> strijelci)
{
    Console.Clear();
    Console.WriteLine("1 - Uredi ime i prezime igrača");
    Console.WriteLine("2 - Uredi poziciju igrača (GK, DF, MF ili FW)");
    Console.WriteLine("3 - Uredi rating igrača (od 1 do 100)");
    Console.WriteLine("0 - Izlaz na prethodni izbornik");
    var choice=int.Parse(Console.ReadLine());
    switch (choice)
    {
        case 0:
            Console.WriteLine("0 - Izlaz na glavni izbornik");
            MainMenu(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);
            break;
        case 1:
            Console.Clear();
            Console.WriteLine("1 - Uredi ime i prezime igrača");
            EditPlayerName(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);
            break;
        case 2:
            Console.Clear();
            Console.WriteLine("2 - Uredi poziciju igrača (GK, DF, MF ili FW)");
            EditPlayerPosition(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);
            break;
        case 3:
            Console.Clear();
            Console.WriteLine("3 - Uredi rating igrača (od 1 do 100)");
            EditPlayerRating(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);
            break;

    }
}

void EditPlayerName(Dictionary<string, (string position, int rating)> popisIgracaDict, Dictionary<string,
    (int points, int zabijeniGolovi, int primljeniGolovi)> tablica, Dictionary<int, (string team1, string team2, int goals1, int goals2)> rezultatiUtakmica,
    ref Dictionary<int, (string country, string name, int goals)> strijelci)
{
    Console.WriteLine("UREDI IME IGRACA");
    Console.WriteLine("Podatci o igracu kojeg zelite izmjeniti:");
    var playerName = EnterPlayerName(popisIgracaDict);
    Console.WriteLine("Podatci o novom igracu:");
    var newPlayerName = EnterPlayerName1(popisIgracaDict); 
    
    if (Confirmation())
    {
        popisIgracaDict.Add(newPlayerName, (popisIgracaDict[playerName].position, popisIgracaDict[playerName].rating));
        popisIgracaDict.Remove(playerName);
        Console.WriteLine("Promjene su napravljene.");
    }
    else
        Console.WriteLine("Promjene se nisu dogodile.");
    if (PovratakNaGlavniIzbornik())
        MainMenu(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);

}

string EnterPlayerName(Dictionary<string, (string position, int rating)> popisIgracaDict)
{
    var playerName = "";
    while (true)
    {
        Console.WriteLine("Unesite ime i prezime igraca:");
        playerName = Console.ReadLine(); 
        if (playerName == null || playerName == String.Empty)
        {
            Console.WriteLine("Nije unesen igrac!");
            continue;
        }
        else if (!popisIgracaDict.ContainsKey(playerName))
        {
            Console.WriteLine("Taj igrac ne postoji. Nije moguce urediti igraca koji ne postoji.");
            continue;
        }
        else
            break;
    }
    return playerName;
}
void EditPlayerPosition(Dictionary<string, (string position, int rating)> popisIgracaDict, Dictionary<string,
    (int points, int zabijeniGolovi, int primljeniGolovi)> tablica, Dictionary<int, (string team1, string team2, int goals1, int goals2)> rezultatiUtakmica,
    ref Dictionary<int, (string country, string name, int goals)> strijelci)
{
    Console.WriteLine("Unesite podatke o igracu kojem želite urediti poziciju:");
    var playerName = EnterPlayerName(popisIgracaDict);
    var positionOfPlayer = EnterPlayerPosition(popisIgracaDict);
    if (Confirmation() == true)
    {
        popisIgracaDict[playerName] = (positionOfPlayer, popisIgracaDict[playerName].rating);
        Console.WriteLine("Promjene su napravljene.");
    }
    else
        Console.WriteLine("Promjene se nisu dogodile.");
    if (PovratakNaGlavniIzbornik())
        MainMenu(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);

}

void EditPlayerRating(Dictionary<string, (string position, int rating)> popisIgracaDict, Dictionary<string,
    (int points, int zabijeniGolovi, int primljeniGolovi)> tablica, Dictionary<int, (string team1, string team2, int goals1, int goals2)> rezultatiUtakmica,
    ref Dictionary<int, (string country, string name, int goals)> strijelci)
{
    Console.WriteLine("Unesite podatke o igracku kojem zelite urediti rating");
    var playerName = EnterPlayerName(popisIgracaDict);
    var ratingOfPlayer = EnterPlayerRating(popisIgracaDict);
    if (Confirmation())
    {
        popisIgracaDict[playerName] = (popisIgracaDict[playerName].position, ratingOfPlayer);
        Console.WriteLine("Promjene su napravljene.");
    }
    else
        Console.WriteLine("Promjene se nisu dogodile.");
    if (PovratakNaGlavniIzbornik())
        MainMenu(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);
}


bool Confirmation()
{
    Console.WriteLine("Jeste li sigurni da to zelite? y or n");
    var yOrN = Console.ReadLine();
    if (yOrN == "y")
        return true;
    else
        return false;
}

void StatisticMenu(Dictionary<string, (string position, int rating)> popisIgracaDict, Dictionary<string, (int points, int zabijeniGolovi, int primljeniGolovi)>
    tablica, Dictionary<int, (string team1, string team2, int goals1, int goals2)> rezultatiUtakmica, ref Dictionary<int, (string country, string name, int goals)> strijelci)
{
    Console.WriteLine("0 - Povratak na prethodni izbornik");
    Console.WriteLine("1 - Ispis svih igrača");
    Console.WriteLine("Unesi broj 0 ili 1");
    int num;
    while (true)
    {
        num = int.Parse(Console.ReadLine());
        if (num == 0)
        {
            MainMenu(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);
            continue;
        }
        else if (num == 1)
        {
            PrintAllPlayers(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);
            continue;
        }
        else
            break;
    }
}

void PrintAllPlayers(Dictionary<string, (string position, int rating)> popisIgracaDict, Dictionary<string, (int points, int zabijeniGolovi, int primljeniGolovi)>
    tablica, Dictionary<int, (string team1, string team2, int goals1, int goals2)> rezultatiUtakmica, ref Dictionary<int, (string country, string name, int goals)> strijelci)
{
    Console.Clear();
    Console.WriteLine("ISPIS SVIH IGRACA");

    Console.WriteLine("1 - Ispis onako kako su spremljeni");
    Console.WriteLine("2 - Ispis po rating uzlazno");
    Console.WriteLine("3 - Ispis po ratingu silazno");
    Console.WriteLine("4 - Ispis igrača po imenu i prezimenu (ispis pozicije i ratinga)");
    Console.WriteLine("5 - Ispis igrača po ratingu (ako ih je više ispisati sve)");
    Console.WriteLine("6 - Ispis igrača po poziciji (ako ih je više ispisati sve)");
    Console.WriteLine("7 - Ispis trenutnih prvih 11 igrača (na pozicijama odabrati igrače s najboljim ratingom)");
    Console.WriteLine("8 - Ispis strijelaca i koliko golova imaju");
    Console.WriteLine("9 - Ispis svih rezultata ekipe");
    Console.WriteLine("10 - Ispis rezultat svih ekipa");
    Console.WriteLine("11 - Ispis tablice grupe (mjesto na tablici, ime ekipe, broj bodova, gol razlika)");
    Console.WriteLine("0 - Povratak na glavni izbornik");


    Console.WriteLine("Unesite broj koji želite od 0 do 11");
    var choice = int.Parse(Console.ReadLine());
    switch (choice)
    {
        case 0:
            Console.WriteLine("0 - Povratak na glavni izbornik");
            MainMenu(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci); 
            break;
        case 1:
            Console.Clear();
            Console.WriteLine("1 - Ispis onako kako su spremljeni");
            IspisIgraca(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);
            break;
        case 2:
            Console.Clear();   
            Console.WriteLine("2 - Ispis po ratingu uzlazno");
            PrintByRatingAsc(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);
            break;
        case 3:
            Console.Clear();
            Console.WriteLine("3 - Ispis po ratingu silazno");
            PrintByRatingDsc(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);
            break;
        case 4:
            Console.Clear();
            Console.WriteLine("4 - Ispis igrača po imenu i prezimenu (ispis pozicije i ratinga)");
            PrintByName(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);
            break;
        case 5:
            Console.Clear();
            Console.WriteLine("5 - Ispis igrača po ratingu (ako ih je više ispisati sve)");
            PrintByRating(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);
            break;
        case 6:
            Console.Clear();
            Console.WriteLine("6 - Ispis igrača po poziciji (ako ih je više ispisati sve)");
            PrintByPosition(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);
            break; 
        case 7:
            Console.Clear();
            Console.WriteLine("7 - Ispis trenutnih prvih 11 igrača (na pozicijama odabrati igrače s najboljim ratingom)");
            PrintBestElevenByRating(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);
            break;
        case 8:
            Console.Clear();
            Console.WriteLine("8 - Ispis strijelaca i koliko golova imaju");
            PrintByGoals(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);
            break;
        case 9:
            Console.Clear();
            Console.WriteLine("9 - Ispis svih rezultata ekipe");
            PrintResultsOfTeam(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci); 
            break;
        case 10:
            Console.Clear();
            Console.WriteLine("10 - Ispis rezultat svih ekipa");
            PrintAllResults(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci); 
            break;
        case 11:
            Console.Clear();
            Console.WriteLine("11 - Ispis tablice grupe (mjesto na tablici, ime ekipe, broj bodova, gol razlika)");
            PrintTable(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci); 
            break;

    }
}

void PrintByRatingAsc(Dictionary<string, (string position, int rating)> popisIgracaDict, Dictionary<string,
    (int points, int zabijeniGolovi, int primljeniGolovi)> tablica, Dictionary<int, (string team1, string team2, int goals1, int goals2)> rezultatiUtakmica,
    ref Dictionary<int, (string country, string name, int goals)> strijelci)
{
    var sorted = (
    from keyValuePair in popisIgracaDict
    orderby keyValuePair.Value.rating ascending
    select keyValuePair
    ).ToList();
    foreach (var player in sorted)
        Console.WriteLine($"Igrac {player.Key} igra poziciju {player.Value.position} i ima rating {player.Value.rating}.");
    if (PovratakNaGlavniIzbornik())
        MainMenu(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);

}


void PrintByRatingDsc(Dictionary<string, (string position, int rating)> popisIgracaDict, Dictionary<string,
    (int points, int zabijeniGolovi, int primljeniGolovi)> tablica, Dictionary<int, (string team1, string team2, int goals1, int goals2)> rezultatiUtakmica,
    ref Dictionary<int, (string country, string name, int goals)> strijelci)
{ 
    var sorted = (
    from keyValuePair in popisIgracaDict
    orderby keyValuePair.Value.rating descending
    select keyValuePair
    ).ToList();
    foreach (var player in sorted)
        Console.WriteLine($"Igrac {player.Key} igra poziciju {player.Value.position} i ima rating {player.Value.rating}.");
    if (PovratakNaGlavniIzbornik())
        MainMenu(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);

}


//4 - Ispis igrača po imenu i prezimenu (ispis pozicije i ratinga
void PrintByName(Dictionary<string, (string position, int rating)> popisIgracaDict,
    Dictionary<string, (int points, int zabijeniGolovi, int primljeniGolovi)> tablica,
    Dictionary<int, (string team1, string team2, int goals1, int goals2)> rezultatiUtakmica,
    ref Dictionary<int, (string country, string name, int goals)> strijelci)
{
    Console.WriteLine("Unesite ime igrača čije podatke želite:");
    var name=Console.ReadLine();
    foreach (var player in popisIgracaDict)
    {
        if (player.Key == name)
            Console.WriteLine($"Igrac {player.Key} igra poziciju {player.Value.position} i ima rating {player.Value.rating}.");
    }
    if (PovratakNaGlavniIzbornik())
        MainMenu(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);
}


// Ispis igrača po ratingu (ako ih je više ispisati sve)
void PrintByRating(Dictionary<string, (string position, int rating)> popisIgracaDict, Dictionary<string, (int points, int zabijeniGolovi, int primljeniGolovi)> 
    tablica, Dictionary<int, (string team1, string team2, int goals1, int goals2)> rezultatiUtakmica, ref Dictionary<int, (string country, string name, int goals)> strijelci)
{
    var cnt = 0;
    Console.WriteLine("Unesite rating:");
    var rating = int.Parse(Console.ReadLine());
    foreach (var player in popisIgracaDict)
    {
        if (popisIgracaDict[player.Key].rating == rating)
            Console.WriteLine($"Igrac {player.Key} igra poziciju {player.Value.position} i ima rating {player.Value.rating}.");
        else
            cnt++;
    }
    if(cnt == popisIgracaDict.Count)
        Console.WriteLine("Nijedan igrac nema taj rating");
    if (PovratakNaGlavniIzbornik())
        MainMenu(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);
}

//Ispis igrača po poziciji (ako ih je više ispisati sve) AKO ZELI SVE???
void PrintByPosition(Dictionary<string, (string position, int rating)> popisIgracaDict, Dictionary<string,
    (int points, int zabijeniGolovi, int primljeniGolovi)> tablica, Dictionary<int, (string team1, string team2, int goals1, int goals2)> rezultatiUtakmica,
    ref Dictionary<int, (string country, string name, int goals)> strijelci)
{
    Console.WriteLine("Unesite poziciju:");
    var position = Console.ReadLine().ToUpper();
    var cnt = 0;
    foreach (var player in popisIgracaDict)
    {
        if (popisIgracaDict[player.Key].position == position)
            Console.WriteLine($"Igrac {player.Key} igra poziciju {player.Value.position} i ima rating {player.Value.rating}.");
        else
            cnt++;
    }
    if (cnt == popisIgracaDict.Count)
        Console.WriteLine("Nema nijedan igrac s tom pozicijom!");
    if (PovratakNaGlavniIzbornik())
        MainMenu(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);
}

//Ispis trenutnih prvih 11 igrača (na pozicijama odabrati igrače s najboljim ratingom)   4-3-3 (1 GK, 4 DF, 3 MF i 3 FW)
Dictionary<string, (string, int)> ReturnBestElevenByRating(Dictionary<string, (string position, int rating)> popisIgracaDict)
{
    var bestEleven = new Dictionary<string, (string position, int rating)>();
    var sorted = (
    from keyValuePair in popisIgracaDict
    orderby keyValuePair.Value.rating descending
    select keyValuePair
    ).ToList();
    var gk = 0;
    var df = 0;
    var mf = 0;
    var fw = 0; ;

    foreach (var player in sorted)
    {
        if (player.Value.position == "GK" && gk == 0)
        {
            bestEleven.Add(player.Key, (player.Value.position, player.Value.rating));
            gk++;
        }
        else if (player.Value.position == "DF" && df < 4)
        {
            bestEleven.Add(player.Key, (player.Value.position, player.Value.rating));
            df++;
        }
        else if(player.Value.position == "MF" && mf < 3)
        {
            bestEleven.Add(player.Key, (player.Value.position, player.Value.rating));
            mf++;
        }
        else if(player.Value.position == "FW" && fw < 3)
        {
            bestEleven.Add(player.Key, (player.Value.position, player.Value.rating));
            fw++;
        }
    }
    return bestEleven;
        
}

void PrintBestElevenByRating(Dictionary<string, (string position, int rating)> popisIgracaDict,
    Dictionary<string, (int points, int zabijeniGolovi, int primljeniGolovi)> tablica,
    Dictionary<int, (string team1, string team2, int goals1, int goals2)> rezultatiUtakmica,
    ref Dictionary<int, (string country, string name, int goals)> strijelci)
{
    Dictionary<string, (string position, int rating)> bestEleven = ReturnBestElevenByRating(popisIgracaDict);
    Console.WriteLine("Best 11 players");
    foreach (var player in bestEleven)
        Console.WriteLine($"Igrac {player.Key} igra poziciju {player.Value.position} i ima rating {player.Value.rating}.");
    if (PovratakNaGlavniIzbornik())
        MainMenu(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);

}

//Ispis strijelaca i koliko golova imaju
void PrintByGoals(Dictionary<string, (string position, int rating)> popisIgracaDict,
    Dictionary<string, (int points, int zabijeniGolovi, int primljeniGolovi)> tablica,
    Dictionary<int, (string team1, string team2, int goals1, int goals2)> rezultatiUtakmica,
    ref Dictionary<int, (string country, string name, int goals)> strijelci)
{
    if (strijelci.Count==0)
    {
        Console.WriteLine("Nije zabijen još nijedan gol.");
    }
    else
    {
        foreach (var item in strijelci)
        {
            Console.WriteLine($"Na utakmici Hrvatska-{item.Value.country} igrac {item.Value.name} je zabio {item.Value.goals} gol/a.");
        }
    }
    

    if (PovratakNaGlavniIzbornik())
        MainMenu(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);
}

//Ispis svih rezultata ekipe
void PrintResultsOfTeam(Dictionary<string, (string position, int rating)> popisIgracaDict, Dictionary<string,
    (int points, int zabijeniGolovi, int primljeniGolovi)> tablica, Dictionary<int, (string team1, string team2, int goals1, int goals2)> rezultatiUtakmica,
    ref Dictionary<int, (string country, string name, int goals)> strijelci)
{
    if (rezultatiUtakmica.Count==0)
        Console.WriteLine("Nijedna utakmica još nije odigrana!");
    else
    {
        Console.WriteLine("Upišite ime ekipe čije rezultate želite vidjeti (Hrvatska, Belgija, Maroko, Kanada):");
        var team = Console.ReadLine(); 

        foreach (var item in rezultatiUtakmica)
        {
            if (team == item.Value.team1 || team == item.Value.team2)
                Console.WriteLine($"{item.Value.team1} {item.Value.goals1} : {item.Value.goals2} {item.Value.team2}");
        }
    }
    if (PovratakNaGlavniIzbornik())
        MainMenu(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);

}

//Ispis rezultat svih ekipa
void PrintAllResults(Dictionary<string, (string position, int rating)> popisIgracaDict, Dictionary<string,
    (int points, int zabijeniGolovi, int primljeniGolovi)> tablica, Dictionary<int, (string team1, string team2, int goals1, int goals2)> rezultatiUtakmica,
    ref Dictionary<int, (string country, string name, int goals)> strijelci)
{
    if (rezultatiUtakmica.Count == 0)
        Console.WriteLine("Nijedna utakmica još nije odigrana!");
    else
    {
        Console.WriteLine("Svi rezultati svih ekipa:");
        foreach (var item in rezultatiUtakmica)
            Console.WriteLine($"{item.Value.team1} {item.Value.goals1} : {item.Value.goals2} {item.Value.team2}");
    }
    if (PovratakNaGlavniIzbornik())
        MainMenu(popisIgracaDict, tablica, rezultatiUtakmica,ref strijelci);

}

//Ispis tablice grupe (mjesto na tablici, ime ekipe, broj bodova, gol razlika)
void PrintTable(Dictionary<string, (string position, int rating)> popisIgracaDict, Dictionary<string,
    (int points, int zabijeniGolovi, int primljeniGolovi)> tablica, Dictionary<int, (string team1, string team2, int goals1, int goals2)> rezultatiUtakmica,
    ref Dictionary<int, (string country, string name, int goals)> strijelci)
{
    var sorted = (
    from keyValuePair in tablica
    orderby keyValuePair.Value.points descending
    select keyValuePair
    ).ToList();
    var mjesto = 1;
    foreach (var item in sorted)
    { 
        Console.WriteLine($"{mjesto}. {item.Key} broj bodova: {item.Value.points}, gol razlika: {item.Value.zabijeniGolovi} : {item.Value.primljeniGolovi}");
        mjesto++;
    }
    if (PovratakNaGlavniIzbornik())
        MainMenu(popisIgracaDict, tablica, rezultatiUtakmica, ref strijelci);
}

bool PovratakNaGlavniIzbornik()
{
    Console.WriteLine("Unesite 0 ako zelite povratak na glavni izbornik:");
    var izbornik = int.Parse(Console.ReadLine());
    if (izbornik == 0)
        return true;
    else
        return false;
}