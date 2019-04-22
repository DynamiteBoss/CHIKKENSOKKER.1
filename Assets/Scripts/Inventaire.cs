﻿using System;
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
    [Command]
    public static void CmdAfficherInventaire(char équipe, int position)
    {
        RpcAfficherInventaire(équipe, position);
    }
    [ClientRpc]
    private static void RpcAfficherInventaire(char équipe, int position)
    {
        int valeurItemTemp = 8;

        if (équipe == 'A') { if (position == 1) { valeurItemTemp = itemA1; } else { valeurItemTemp = itemA2; } }
        if (équipe == 'B') { if (position == 1) { valeurItemTemp = itemB1; } else { valeurItemTemp = itemB2; } }

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
        RpcfficherSprite(position, équipe, nomSprite);   
    }
    [ClientRpc]
    private static void RpcfficherSprite(int position, char équipe, string nomSprite)
    {
        GameObject.Find("Objet" + position + équipe).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Image/" + nomSprite);
    }
}
