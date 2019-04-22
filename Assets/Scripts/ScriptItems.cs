﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ScriptItems : NetworkBehaviour
{
    int framesDélai = 0;

    [SerializeField]
    string équipe;

    void Start()
    {
        if (this.transform.gameObject.name.EndsWith("A")) { équipe = "A"; }
        else { équipe = "B"; }
    }
    void Update()
    {
        framesDélai++;
        if (isLocalPlayer && tag == "Player")
        {
            if (Input.GetKeyDown("1") && framesDélai > 60)
            {
                CmdInstancierItem(0, this.transform.position);  // ITEM 0 TEMPORAIRE
                framesDélai = 0;
            }
            if (Input.GetKeyDown("2") && framesDélai > 60)
            {
                CmdInstancierItem(1, this.transform.position);  // ITEM 0 TEMPORAIRE
                framesDélai = 0;
            }
            if (Input.GetKeyDown("3") && framesDélai > 60)
            {
                CmdInstancierItem(2, this.transform.position);  // ITEM 0 TEMPORAIRE
                framesDélai = 0;
            }
            if (Input.GetKeyDown("4") && framesDélai > 60)
            {
                CmdInstancierItem(3, this.transform.position);  // ITEM 0 TEMPORAIRE
                framesDélai = 0;
            }
            if (Input.GetKeyDown("5") && framesDélai > 60)
            {
                CmdInstancierItem(4, this.transform.position);  // ITEM 0 TEMPORAIRE
                framesDélai = 0;
            }
            if (Input.GetKeyDown("6") && framesDélai > 60)
            {
                CmdInstancierItem(5, this.transform.position);  // ITEM 0 TEMPORAIRE
                framesDélai = 0;
            }
            if (Input.GetKeyDown("7") && framesDélai > 60)
            {
                CmdInstancierItem(6, this.transform.position);  // ITEM 0 TEMPORAIRE
                framesDélai = 0;
            }
            if (Input.GetKeyDown("8") && framesDélai > 60)
            {
                CmdInstancierItem(7, this.transform.position);  // ITEM 0 TEMPORAIRE
                framesDélai = 0;
            }

            // vRAI COMMANDE AVEC LE VRAI BOUTON POUR LE JOUEUR 1
            if (Input.GetKeyDown("r") && framesDélai > 60 && this.transform.gameObject.name.Contains("1"))
            {
                if (équipe == "A" && Inventaire.itemA1 < Inventaire.ITEMNUL && Inventaire.itemA1 >= 0)
                {
                    CmdInstancierItem(Inventaire.itemA1, this.transform.position);

                    if (Inventaire.itemA2 < Inventaire.ITEMNUL && Inventaire.itemA2 >= 0)
                    {
                        int temp = Inventaire.itemA2;
                        Inventaire.itemA2 = Inventaire.ITEMNUL;
                        Inventaire.itemA1 = temp;
                        CmdModifierSprite(1, 'A');
                        CmdModifierSprite(2, 'A');
                    }
                    else
                    {
                        Inventaire.itemA1 = Inventaire.ITEMNUL;
                        CmdModifierSprite(1, 'A');
                    }
                    framesDélai = 0;
                }
                if (équipe == "B" && Inventaire.itemB1 < Inventaire.ITEMNUL && Inventaire.itemB1 >= 0)
                {
                    CmdInstancierItem(Inventaire.itemB1, this.transform.position);

                    if (Inventaire.itemB2 < Inventaire.ITEMNUL && Inventaire.itemB2 >= 0)
                    {
                        int temp = Inventaire.itemB2;
                        Inventaire.itemB2 = Inventaire.ITEMNUL;
                        Inventaire.itemB1 = temp;
                        CmdModifierSprite(1, 'B');
                        CmdModifierSprite(2, 'B');
                    }
                    else
                    {
                        Inventaire.itemB1 = Inventaire.ITEMNUL;
                        CmdModifierSprite(1, 'B');
                    }
                    framesDélai = 0;
                }
            }
            // VRAI COMMANDE AVEC LE VRAI BOUTON POUR LE JOUEUR 2
            if (Input.GetKeyDown(KeyCode.Keypad1) && framesDélai > 60 && this.transform.gameObject.name.Contains("2"))
            {
                if (équipe == "A" && Inventaire.itemA1 < Inventaire.ITEMNUL && Inventaire.itemA1 >= 0)
                {
                    CmdInstancierItem(Inventaire.itemA1, this.transform.position);

                    if (Inventaire.itemA2 < Inventaire.ITEMNUL && Inventaire.itemA2 >= 0)
                    {
                        int temp = Inventaire.itemA2;
                        Inventaire.itemA2 = Inventaire.ITEMNUL;
                        Inventaire.itemA1 = temp;
                        CmdModifierSprite(1, 'A');
                        CmdModifierSprite(2, 'A');
                    }
                    else
                    {
                        Inventaire.itemA1 = Inventaire.ITEMNUL;
                        CmdModifierSprite(1, 'A');
                    }
                    framesDélai = 0;
                }
                if (équipe == "B" && Inventaire.itemB1 < Inventaire.ITEMNUL && Inventaire.itemB1 >= 0)
                {
                    CmdInstancierItem(Inventaire.itemB1, this.transform.position);

                    if (Inventaire.itemB2 < Inventaire.ITEMNUL && Inventaire.itemB2 >= 0)
                    {
                        int temp = Inventaire.itemB2;
                        Inventaire.itemB2 = Inventaire.ITEMNUL;
                        Inventaire.itemB1 = temp;
                        CmdModifierSprite(1, 'B');
                        CmdModifierSprite(2, 'B');
                    }
                    else
                    {
                        Inventaire.itemB1 = Inventaire.ITEMNUL;
                        CmdModifierSprite(1, 'B');
                    }
                    framesDélai = 0;
                }
            }
        }
    }
    [Command]
    private void CmdModifierSprite(int position, char équipe)
    {
        if (position == 2)
        {
            GameObject.Find("Objet" + (position - 1) + équipe).GetComponent<SpriteRenderer>().sprite = GameObject.Find("Objet" + position + équipe).GetComponent<SpriteRenderer>().sprite;
            GameObject.Find("Objet" + position + équipe).GetComponent<SpriteRenderer>().sprite = null;
        }
        else
        {
            GameObject.Find("Objet" + position + équipe).GetComponent<SpriteRenderer>().sprite = null;
        }
    }
    [Command]
    public void CmdInstancierItem(int indiceItem, Vector3 position)
    {
        RpcInstancierItem(indiceItem, position);
    }
    [ClientRpc]
    private void RpcInstancierItem(int indiceItem, Vector3 position)
    {
        switch (indiceItem)
        {

            case 0:
                {
                    GameObject Crotte1 = (GameObject)Instantiate(Item.RetournerItemListe(0).ItemPhysique, position + transform.up * 2 + transform.right * -2 + transform.forward * 2.5f, Quaternion.Euler(90, 0, -(this.transform.rotation.eulerAngles.y)));
                    GameObject Crotte2 = (GameObject)Instantiate(Item.RetournerItemListe(0).ItemPhysique, position + transform.up * 2 + transform.forward * 3.5f, Quaternion.Euler(90, 0, -(this.transform.rotation.eulerAngles.y)));
                    GameObject Crotte3 = (GameObject)Instantiate(Item.RetournerItemListe(0).ItemPhysique, position + transform.up * 2 + transform.right * 2 + transform.forward * 2.5f, Quaternion.Euler(90, 0, -(this.transform.rotation.eulerAngles.y)));
                    GameObject[] listCrotte = { Crotte1, Crotte2, Crotte3 };
                    foreach (GameObject item in listCrotte)
                    {
                        NetworkServer.Spawn(item);
                        Destroy(item, 6);
                    }
                    ItemCrotte.FaireEffetItem(listCrotte); //DONNER UNE VITESSE OU MOUVEMENT DANS LE SCRIPT SPÉCIALISÉ
                    break;
                }
            case 1:
                {
                    GameObject Crotte1 = (GameObject)Instantiate(Item.RetournerItemListe(1).ItemPhysique, position + transform.up * 2 + transform.right * -4 + transform.forward * 2.5f, Quaternion.Euler(90, 0, -(this.transform.rotation.eulerAngles.y)));
                    GameObject Crotte2 = (GameObject)Instantiate(Item.RetournerItemListe(1).ItemPhysique, position + transform.up * 2 + transform.right * -2 + transform.forward * 3.5f, Quaternion.Euler(90, 0, -(this.transform.rotation.eulerAngles.y)));
                    GameObject Crotte3 = (GameObject)Instantiate(Item.RetournerItemListe(1).ItemPhysique, position + transform.up * 2 + transform.forward * 4.5f, Quaternion.Euler(90, 0, -(this.transform.rotation.eulerAngles.y)));
                    GameObject Crotte4 = (GameObject)Instantiate(Item.RetournerItemListe(1).ItemPhysique, position + transform.up * 2 + transform.right * 2 + transform.forward * 3.5f, Quaternion.Euler(90, 0, -(this.transform.rotation.eulerAngles.y)));
                    GameObject Crotte5 = (GameObject)Instantiate(Item.RetournerItemListe(1).ItemPhysique, position + transform.up * 2 + transform.right * 4 + transform.forward * 2.5f, Quaternion.Euler(90, 0, -(this.transform.rotation.eulerAngles.y)));
                    GameObject[] listCrotte = { Crotte1, Crotte2, Crotte3, Crotte4, Crotte5 };
                    foreach (GameObject item in listCrotte)
                    {
                        NetworkServer.Spawn(item);
                        Destroy(item, 6);
                    }
                    ItemCrotte.FaireEffetItem(listCrotte); //DONNER UNE VITESSE OU MOUVEMENT DANS LE SCRIPT SPÉCIALISÉ
                    break;
                }
            case 2:
                {
                    GameObject OeufBlancGros = (GameObject)Instantiate(Item.RetournerItemListe(2).ItemPhysique, position + transform.up * 2 + transform.forward * 4.5f, Quaternion.Euler(90, 0, -(this.transform.rotation.eulerAngles.y)));
                    GameObject[] listOeux = { OeufBlancGros };
                    foreach (GameObject item in listOeux)
                    {
                        NetworkServer.Spawn(item);
                        Destroy(item, 5);
                    }
                    ItemOeufBlanc.FaireEffetItem(0, listOeux); //DONNER UNE VITESSE OU MOUVEMENT DANS LE SCRIPT SPÉCIALISÉ
                    break;
                }
            case 3:
                {
                    GameObject OeufBlanc1 = (GameObject)Instantiate(Item.RetournerItemListe(3).ItemPhysique, position + transform.up * 2 + transform.right * -2 + transform.forward * 2.5f, Quaternion.Euler(90, 0, -(this.transform.rotation.eulerAngles.y)));
                    GameObject OeufBlanc2 = (GameObject)Instantiate(Item.RetournerItemListe(3).ItemPhysique, position + transform.up * 2 + transform.forward * 3.5f, Quaternion.Euler(90, 0, -(this.transform.rotation.eulerAngles.y)));
                    GameObject OeufBlanc3 = (GameObject)Instantiate(Item.RetournerItemListe(3).ItemPhysique, position + transform.up * 2 + transform.right * 2 + transform.forward * 2.5f, Quaternion.Euler(90, 0, -(this.transform.rotation.eulerAngles.y)));
                    GameObject[] listOeux = { OeufBlanc1, OeufBlanc2, OeufBlanc3 };
                    foreach (GameObject item in listOeux)
                    {
                        NetworkServer.Spawn(item);
                        Destroy(item, 5);
                    }
                    ItemOeufBlanc.FaireEffetItem(1, listOeux); //DONNER UNE VITESSE OU MOUVEMENT DANS LE SCRIPT SPÉCIALISÉ

                    break;
                }
            case 4:
                {
                    GameObject OeufBrun = (GameObject)Instantiate(Item.RetournerItemListe(4).ItemPhysique, position + transform.up * 2 + transform.forward * 3.5f, Quaternion.Euler(90, 0, -(this.transform.rotation.eulerAngles.y)));
                    ItemOeufBrun.FaireEffetItem(OeufBrun); //DONNER UNE VITESSE OU MOUVEMENT DANS LE SCRIPT SPÉCIALISÉ
                    NetworkServer.Spawn(OeufBrun);
                    Destroy(OeufBrun, 5);
                    break;
                }
            case 5:
                {
                    GameObject OeufMortier = (GameObject)Instantiate(Item.RetournerItemListe(5).ItemPhysique, position + transform.up * 3 + transform.forward * 3.5f, Quaternion.Euler(90, 0, -(this.transform.rotation.eulerAngles.y)));
                    ItemBombe.FaireEffetItem(OeufMortier); //DONNER UNE VITESSE OU MOUVEMENT DANS LE SCRIPT SPÉCIALISÉ
                    NetworkServer.Spawn(OeufMortier);
                    break;
                }
            case 6:
                {
                    GameObject oeufBrouillé = (GameObject)Instantiate(Item.RetournerItemListe(6).ItemPhysique, position, Quaternion.identity);   //PA BESOIN DE VISUEL
                    ItemBrouillé.FaireEffetItem(this.transform.gameObject/* RESTE JUSTE A METTRE UN JOUEUR ICI PI CA VA SE METTRE */);
                    NetworkServer.Spawn(oeufBrouillé);
                    Destroy(oeufBrouillé, 0.1f);
                    break;
                }
            case 7:
                {
                    GameObject versDeTerre = (GameObject)Instantiate(Item.RetournerItemListe(7).ItemPhysique, position, Quaternion.identity);   //PA BESOIN DE VISUEL
                    ItemVers.FaireEffetItem(this.transform.gameObject/* RESTE JUSTE A METTRE UN JOUEUR ICI PI CA VA SE METTRE */);
                    NetworkServer.Spawn(versDeTerre);
                    Destroy(versDeTerre, 0.1f);
                    break;
                }
            default:
                {
                    return;
                }
        }
    }
}
