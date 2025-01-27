﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ScriptItems : NetworkBehaviour
{
    int framesDélai = 0;

    void Start()
    {
    }
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
            
        framesDélai++;

        // vRAI COMMANDE AVEC LE VRAI BOUTON POUR LE JOUEUR 1
        if ((Input.GetKeyDown("r") || Input.GetButtonDown("SquareBtn1")) && framesDélai > 60 && this.transform.gameObject.name.StartsWith("Joueur1") && isLocalPlayer)
        { 
            CmdFaireOpérationJoueur();
        }
        // VRAI COMMANDE AVEC LE VRAI BOUTON POUR LE JOUEUR 2
        if ((Input.GetKeyDown(KeyCode.Keypad0) || Input.GetButtonDown("SquareBtn2")) && framesDélai > 60 && this.transform.gameObject.name.StartsWith("Joueur2") && isLocalPlayer)
        {
            CmdFaireOpérationJoueur();
        }
    }

    //utliser un item.  Enlever la valeur de l'item si item 1 et mettre la valeur de l'item 2 dans l'item 1 si c'est l'item 2 qui a été utilisé, ensuite enlever la valeur de l'item 1
    [Command]
    private void CmdFaireOpérationJoueur()
    {
        if (GetComponent<TypeÉquipe>().estÉquipeA && Inventaire.itemA1 < Inventaire.ITEMNUL && Inventaire.itemA1 >= 0)
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
        else if (Inventaire.itemB1 < Inventaire.ITEMNUL && Inventaire.itemB1 >= 0)
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

    // même chose que les items, mais avec les sprites (voir comm à la ligne 35, en haut de CmdFaireOpérationJoueur)
    [Command]
    private void CmdModifierSprite(int position, char équipe) 
    {
        RpcModifierSprite(position, équipe);
    }
    [ClientRpc]
    private void RpcModifierSprite(int position, char équipe)
    {
        GameObject[] sprites = GameObject.FindGameObjectsWithTag("Sprite");
        if (équipe == 'A')
        {
            if (position == 2)
            {
                foreach (GameObject sprite in sprites)
                {
                    if (sprite.transform.localPosition == new Vector3(-130, -104, -244))
                    {
                        Destroy(sprite);
                        return;
                    }
                }
                foreach (GameObject sprite in sprites)
                {
                    if (sprite.transform.localPosition == new Vector3(-205, -104, -244))
                    {
                        sprite.transform.localPosition = new Vector3(-130, -104, -244);
                    }
                }
            }
            else
            {
                foreach (GameObject sprite in sprites)
                {
                    if (sprite.transform.localPosition == new Vector3(-130, -104, -244))
                    {
                        Destroy(sprite);
                        return;
                    }
                }
            }
        }
        else if (équipe == 'B')
        {
            if (position == 2)
            {
                foreach (GameObject sprite in sprites)
                {
                    if (sprite.transform.localPosition == new Vector3(134, -104, -244))
                    {
                        Destroy(sprite);
                        return;
                    }
                }
                foreach (GameObject sprite in sprites)
                {
                    if (sprite.transform.localPosition == new Vector3(210, -104, -244))
                    {
                        sprite.transform.localPosition = new Vector3(134, -104, -244);
                    }
                }
            }
            else
            {
                foreach (GameObject sprite in sprites)
                {
                    if (sprite.transform.localPosition == new Vector3(134, -104, -244))
                    {
                        Destroy(sprite);
                        return;
                    }
                }
            }
        }
    }

    //instancier un certain item à une certaine position et le spawner dans le serveur
    //faire l'effet de l'item (lui donner une force, une vitesse, un effet, etc
    [Command]
    private void CmdInstancierItem(int indiceItem, Vector3 position)
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
                        Destroy(item, 8);
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
                        Destroy(item, 8);
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
                        Destroy(item, 7);
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
                        Destroy(item, 7);
                    }
                    ItemOeufBlanc.FaireEffetItem(1, listOeux); //DONNER UNE VITESSE OU MOUVEMENT DANS LE SCRIPT SPÉCIALISÉ

                    break;
                }
            case 4:
                {
                    GameObject OeufBrun = (GameObject)Instantiate(Item.RetournerItemListe(4).ItemPhysique, position + transform.up * 2 + transform.forward * 3.5f, Quaternion.Euler(90, 0, -(this.transform.rotation.eulerAngles.y)));
                    ItemOeufBrun.FaireEffetItem(OeufBrun, this.transform.gameObject); //DONNER UNE VITESSE OU MOUVEMENT DANS LE SCRIPT SPÉCIALISÉ
                    NetworkServer.Spawn(OeufBrun);
                    Destroy(OeufBrun, 6);
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
