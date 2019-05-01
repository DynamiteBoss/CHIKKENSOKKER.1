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
