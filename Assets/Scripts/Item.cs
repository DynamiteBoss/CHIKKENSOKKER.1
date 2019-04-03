using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Item
{
    public string NomItem { get; private set; }
    public GameObject ItemPhysique { get; private set; }

    public static Item RetournerItemListe(int indice)
    {
            switch (indice)
            {
            case 0:
                return new Item("Crottes3", (GameObject)Resources.Load("Prefab/Crotte"));
            case 1:
                return new Item("Crottes5", (GameObject)Resources.Load("Prefab/Crotte"));
            case 2:
                return new Item("OeufBlancGros", (GameObject)Resources.Load("Prefab/OeufBlanc"));
            case 3:
                return new Item("OeufBlanc3", (GameObject)Resources.Load("Prefab/OeufBlanc"));
            case 4:
                return new Item("OeufBrun", (GameObject)Resources.Load("Prefab/OeufBrun"));
            case 5:
                return new Item("OeufMortier", (GameObject)Resources.Load("Prefab/OeufBombe"));
            case 6:
                return new Item("OeufBrouillé", (GameObject)Resources.Load("Prefab/OeufBrouillé"));  //AUCUN VISUEL
            case 7:
                return new Item("VersDeTerre", (GameObject)Resources.Load("Prefab/VersDeTerre"));   //AUCUN VISUEL
            case 8:
                return new Item("ZoneExplosion", (GameObject)Resources.Load("Prefab/ZoneExplosion"));
            default:
                return null;
            }
    }
    public Item(string nomItem, GameObject itemPhysique)
    {
        NomItem = nomItem;
        ItemPhysique = itemPhysique;
    }
}
