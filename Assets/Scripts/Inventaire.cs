using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Inventaire 
{
    public const int ITEMNUL = 8;

    [SyncVar(hook = "OnItemA1")] public static int itemA1 = 8;
    [SyncVar(hook = "OnItemA2")] public static int itemA2 = 8;
    [SyncVar(hook = "OnItemB1")] public static int itemB1 = 8;
    [SyncVar(hook = "OnItemB2")] public static int itemB2 = 8;

    //  TEST
    [SyncVar(hook = "OnObjet1A")] public static Sprite objet1A;
    [SyncVar(hook = "OnObjet2A")] public static Sprite objet2A;
    [SyncVar(hook = "OnObjet1B")] public static Sprite objet1B;
    [SyncVar(hook = "OnObjet2B")] public static Sprite objet2B;
    //  TEST 

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
    static void OnObjet1A(Sprite changement)
    {
        objet1A = changement;
    }
    static void OnObjet2A(Sprite changement)
    {
        objet2A = changement;
    }
    static void OnObjet1B(Sprite changement)
    {
        objet1B = changement;
    }
    static void OnObjet2B(Sprite changement)
    {
        objet2B = changement;
    }
    [Command]
    public static void CmdAfficherInventaire(char équipe, int position)
    {
        RpcAfficherInventaire(équipe, position);
    }

    [ClientRpc]
    public static void RpcAfficherInventaire(char équipe, int position)
    {
        int valeurItemTemp = 8;

        if (équipe == 'A')
        { if (position == 1)
            {
                valeurItemTemp = itemA1;
            }
            else
            {
                valeurItemTemp = itemA2;
            }
        }
        else if (équipe == 'B')
        {
            if (position == 1)
            {
                valeurItemTemp = itemB1;
            }
            else
            {
                valeurItemTemp = itemB2;
            }
        }

        switch (valeurItemTemp)
        {
            case 0:
                CmdAfficherSprite(position, équipe, "crotte3");
                return;
            case 1:
                CmdAfficherSprite(position, équipe, "crotte5");
                return;
            case 2:
                CmdAfficherSprite(position, équipe, "oeufXL");
                return;
            case 3:
                CmdAfficherSprite(position, équipe, "oeuf3");
                return;
            case 4:
                CmdAfficherSprite(position, équipe, "torpille");
                return;
            case 5:
                CmdAfficherSprite(position, équipe, "bombe");
                return;
            case 6:
                CmdAfficherSprite(position, équipe, "brouillé");
                return;
            case 7:
                CmdAfficherSprite(position, équipe, "versdeterre");
                return;
            default:
                return;
        }
    }
    [Command]
    private static void CmdAfficherSprite(int position, char équipe, string nomSprite)
    {
        RpcAfficherSprite(position, équipe, nomSprite);
    }
    [ClientRpc]
    private static void RpcAfficherSprite(int position, char équipe, string nomSprite)
    {
        if (équipe == 'A')
        {
            if (position == 1)
            {
                objet1A = Resources.Load<Sprite>("Image/" + nomSprite);
                GameObject.Find("Objet1A").GetComponent<SpriteRenderer>().sprite = objet1A;
                Debug.Log("Le Sprite" + nomSprite + "a été affiché en 1A");
            }
            else
            {
                objet2A = Resources.Load<Sprite>("Image/" + nomSprite);
                GameObject.Find("Objet2A").GetComponent<SpriteRenderer>().sprite = objet2A;
                Debug.Log("Le Sprite" + nomSprite + "a été affiché en 2A");
            }
        }
        else if (équipe == 'B')
        {
            if (position == 1)
            {
                objet1B = Resources.Load<Sprite>("Image/" + nomSprite);
                GameObject.Find("Objet1B").GetComponent<SpriteRenderer>().sprite = objet1B;
                Debug.Log("Le Sprite" + nomSprite + "a été affiché en 1B");
            }
            else
            {
                objet2B = Resources.Load<Sprite>("Image/" + nomSprite);
                GameObject.Find("Objet2B").GetComponent<SpriteRenderer>().sprite = objet2B;
                Debug.Log("Le Sprite" + nomSprite + "a été affiché en 2B");

            }
        }
        //GameObject.Find("Objet" + position + équipe).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Image/" + nomSprite);    }
    }
    public static string EnTexte(int valeur)
    {
        switch (valeur)
        {
            case 0:
                return "crotte3";
            case 1:
                return "crotte5";
            case 2:
                return "oeufXL";
            case 3:
                return "oeuf3";
            case 4:
                return "torpille";
            case 5:
                return "bombe";
            case 6:
                return "brouillé";
            case 7:
                return "versdeterre";
            default:
                return "xd";
        }
    }
}
