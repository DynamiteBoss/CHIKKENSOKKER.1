using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class Inventaire
{
    public const int ITEMNUL = 8;

    [SyncVar(hook = "OnItemA1")] public static int itemA1 = 8;
    [SyncVar(hook = "OnItemA2")] public static int itemA2 = 8;
    [SyncVar(hook = "OnItemB1")] public static int itemB1 = 8;
    [SyncVar(hook = "OnItemB2")] public static int itemB2 = 8;

    static void OnItemA1(int changement)
    {
        itemA1 = changement;
    }
    static void OnItemA2(int changement)
    {
        itemA2 = changement;
    }
    static void OnItemB1(int changement)
    {
        itemB1 = changement;
    }
    static void OnItemB2(int changement)
    {
        itemB2 = changement;
    }
    public static void AfficherInventaire(char équipe, int position)
    {
        int valeurItemTemp = 8;

        if (équipe == 'A') { if (position == 1) { valeurItemTemp = itemA1; } else { valeurItemTemp = itemA2; } }
        if (équipe == 'B') { if (position == 1) { valeurItemTemp = itemB1; } else { valeurItemTemp = itemB2; } }

        switch (valeurItemTemp)
        {
            case 0:
                AfficherSprite(position, équipe, "crotte3");
                return;
            case 1:
                AfficherSprite(position, équipe, "crotte5");
                return;
            case 2:
                AfficherSprite(position, équipe, "oeufXL");
                return;
            case 3:
                AfficherSprite(position, équipe, "oeuf3");
                return;
            case 4:
                AfficherSprite(position, équipe, "torpille");
                return;
            case 5:
                AfficherSprite(position, équipe, "bombe");
                return;
            case 6:
                AfficherSprite(position, équipe, "brouillé");
                return;
            case 7:
                AfficherSprite(position, équipe, "versdeterre");
                return;
            default:
                return;
        }
    }
    private static void AfficherSprite(int position, char équipe, string nomSprite)
    {
        GameObject.Find("Objet" + position + équipe).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Image/" + nomSprite);
    }

    //A FAIRE SEULEMENT QUAND UN NOUVEL ITEM EST MIS OU ENLEVER D'UN INVENTAIRE

    //AfficherInventaire('A', 1);
    //AfficherInventaire('B', 1);    //temporaire en attendant parce que anyway on va pa pouvoir utiliser d'item si ya juste un player sur le jeu (quan Joueur1B existe pas ca fok)
    //AfficherInventaire('A', 2);
    //AfficherInventaire('B', 2);     //temporaire en attendant parce que anyway on va pa pouvoir utiliser d'item si ya juste un player sur le jeu (quan Joueur1B existe pas ca fok)
}
