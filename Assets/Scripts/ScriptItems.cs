using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptItems : MonoBehaviour
{
    int framesDélai = 0;
    void Start()
    {}
    void Update()
    {
        framesDélai++;
        if (Input.GetKeyDown("1") && framesDélai > 60)
        {
            InstancierItem(0, this.transform.position);  // ITEM 0 TEMPORAIRE
            framesDélai = 0;
        }
        if (Input.GetKeyDown("2") && framesDélai > 60)
        {
            InstancierItem(1, this.transform.position);  // ITEM 0 TEMPORAIRE
            framesDélai = 0;
        }
        if (Input.GetKeyDown("3") && framesDélai > 60)
        {
            InstancierItem(2, this.transform.position);  // ITEM 0 TEMPORAIRE
            framesDélai = 0;
        }
        if (Input.GetKeyDown("4") && framesDélai > 60)
        {
            InstancierItem(3, this.transform.position);  // ITEM 0 TEMPORAIRE
            framesDélai = 0;
        }
        if (Input.GetKeyDown("5") && framesDélai > 60)
        {
            InstancierItem(4, this.transform.position);  // ITEM 0 TEMPORAIRE
            framesDélai = 0;
        }
        if (Input.GetKeyDown("6") && framesDélai > 60)
        {
            InstancierItem(5, this.transform.position);  // ITEM 0 TEMPORAIRE
            framesDélai = 0;
        }
        if (Input.GetKeyDown("7") && framesDélai > 60)
        {
            InstancierItem(6, this.transform.position);  // ITEM 0 TEMPORAIRE
            framesDélai = 0;
        }
        if (Input.GetKeyDown("8") && framesDélai > 60)
        {
            InstancierItem(7, this.transform.position);  // ITEM 0 TEMPORAIRE
            framesDélai = 0;
        }
    }

    public void InstancierItem(int indiceItem, Vector3 position)
    {
        switch (indiceItem)
        {
            // RESTE À INSTANCIER LES ITEMS A LA POSITION DU JOUEUR AVEC UNE FORCE SI BESOIN

            case 0:
                {
                    GameObject Crotte1 = Instantiate(Item.RetournerItemListe(0).ItemPhysique, position + transform.up * 2 + transform.right * -2 + transform.forward * 2.5f, Quaternion.Euler(90, 0, -(this.transform.rotation.eulerAngles.y)));
                    GameObject Crotte2 = Instantiate(Item.RetournerItemListe(0).ItemPhysique, position + transform.up * 2 + transform.forward * 3.5f, Quaternion.Euler(90, 0, -(this.transform.rotation.eulerAngles.y)));   
                    GameObject Crotte3 = Instantiate(Item.RetournerItemListe(0).ItemPhysique, position + transform.up * 2 + transform.right * 2 + transform.forward * 2.5f, Quaternion.Euler(90, 0, -(this.transform.rotation.eulerAngles.y)));
                    GameObject[] listCrotte = { Crotte1, Crotte2, Crotte3 };
                    foreach (GameObject item in listCrotte)
                    {
                        Destroy(item, 6);
                    }
                    ItemCrotte.FaireEffetItem(listCrotte); //DONNER UNE VITESSE OU MOUVEMENT DANS LE SCRIPT SPÉCIALISÉ
                    break;
                }
            case 1:
                {
                    GameObject Crotte1 = Instantiate(Item.RetournerItemListe(1).ItemPhysique, position + transform.up * 2 + transform.right * -4 + transform.forward * 2.5f, Quaternion.Euler(90, 0, -(this.transform.rotation.eulerAngles.y)));
                    GameObject Crotte2 = Instantiate(Item.RetournerItemListe(1).ItemPhysique, position + transform.up * 2 + transform.right * -2 + transform.forward * 3.5f, Quaternion.Euler(90, 0, -(this.transform.rotation.eulerAngles.y)));
                    GameObject Crotte3 = Instantiate(Item.RetournerItemListe(1).ItemPhysique, position + transform.up * 2 + transform.forward * 4.5f, Quaternion.Euler(90, 0, -(this.transform.rotation.eulerAngles.y)));
                    GameObject Crotte4 = Instantiate(Item.RetournerItemListe(1).ItemPhysique, position + transform.up * 2 + transform.right * 2 + transform.forward * 3.5f, Quaternion.Euler(90, 0, -(this.transform.rotation.eulerAngles.y)));
                    GameObject Crotte5 = Instantiate(Item.RetournerItemListe(1).ItemPhysique, position + transform.up * 2 + transform.right * 4 + transform.forward * 2.5f, Quaternion.Euler(90, 0, -(this.transform.rotation.eulerAngles.y)));
                    GameObject[] listCrotte = { Crotte1, Crotte2, Crotte3, Crotte4, Crotte5 };
                    foreach (GameObject item in listCrotte)
                    {
                        Destroy(item, 6);
                    }
                    ItemCrotte.FaireEffetItem(listCrotte); //DONNER UNE VITESSE OU MOUVEMENT DANS LE SCRIPT SPÉCIALISÉ
                    break;
                }
            case 2:
                {
                    GameObject OeufBlancGros = Instantiate(Item.RetournerItemListe(2).ItemPhysique, position + transform.up * 2 + transform.forward * 3.5f, Quaternion.Euler(90, 0, -(this.transform.rotation.eulerAngles.y)));
                    GameObject[] listOeux = { OeufBlancGros };
                    foreach (GameObject item in listOeux)
                    {
                        Destroy(item, 5);
                    }
                    ItemOeufBlanc.FaireEffetItem(0, listOeux); //DONNER UNE VITESSE OU MOUVEMENT DANS LE SCRIPT SPÉCIALISÉ
                    break;
                }
            case 3:
                {
                    GameObject OeufBlanc1 = Instantiate(Item.RetournerItemListe(3).ItemPhysique, position + transform.up * 2 + transform.right * -2 + transform.forward * 2.5f, Quaternion.Euler(90, 0, -(this.transform.rotation.eulerAngles.y)));
                    GameObject OeufBlanc2 = Instantiate(Item.RetournerItemListe(3).ItemPhysique, position + transform.up * 2 + transform.forward * 3.5f, Quaternion.Euler(90, 0, -(this.transform.rotation.eulerAngles.y)));
                    GameObject OeufBlanc3 = Instantiate(Item.RetournerItemListe(3).ItemPhysique, position + transform.up * 2 + transform.right * 2 + transform.forward * 2.5f, Quaternion.Euler(90, 0, -(this.transform.rotation.eulerAngles.y)));
                    GameObject[] listOeux = { OeufBlanc1, OeufBlanc2, OeufBlanc3 };
                    foreach (GameObject item in listOeux)
                    {
                        Destroy(item,5);
                    }
                    ItemOeufBlanc.FaireEffetItem(1, listOeux); //DONNER UNE VITESSE OU MOUVEMENT DANS LE SCRIPT SPÉCIALISÉ

                    break;
                }
            case 4:
                {
                    GameObject OeufBrun = Instantiate(Item.RetournerItemListe(4).ItemPhysique, position + transform.up * 2 + transform.forward * 3.5f, Quaternion.Euler(90, 0, -(this.transform.rotation.eulerAngles.y)));
                    ItemOeufBrun.FaireEffetItem(OeufBrun); //DONNER UNE VITESSE OU MOUVEMENT DANS LE SCRIPT SPÉCIALISÉ
                    Destroy(OeufBrun, 5);
                    break;
                }
            case 5:
                {
                    GameObject OeufMortier = Instantiate(Item.RetournerItemListe(5).ItemPhysique, position + transform.up * 2 + transform.forward * 3.5f, Quaternion.Euler(90, 0, -(this.transform.rotation.eulerAngles.y)));
                    ItemBombe.FaireEffetItem(OeufMortier); //DONNER UNE VITESSE OU MOUVEMENT DANS LE SCRIPT SPÉCIALISÉ
                    break;
                }
            case 6:
                {
                    GameObject oeufBrouillé = Instantiate(Item.RetournerItemListe(6).ItemPhysique, position, Quaternion.identity);   //PA BESOIN DE VISUEL
                    ItemBrouillé.FaireEffetItem(this.transform.gameObject/* RESTE JUSTE A METTRE UN JOUEUR ICI PI CA VA SE METTRE */);
                    Destroy(oeufBrouillé, 0.1f);
                    break;
                }
            case 7:
                {
                    GameObject versDeTerre = Instantiate(Item.RetournerItemListe(7).ItemPhysique, position, Quaternion.identity);   //PA BESOIN DE VISUEL
                    ItemVers.FaireEffetItem(this.transform.gameObject/* RESTE JUSTE A METTRE UN JOUEUR ICI PI CA VA SE METTRE */);
                    Destroy(versDeTerre, 0.1f);
                    break;
                }
            default:
                {
                    return;
                }
        }
    }
    static IEnumerator AttendrePourDestroy(float durée, GameObject[] items)
    {
        yield return new WaitForSeconds(durée);
        foreach(GameObject item in items)
        {
            Destroy(item);
        }
    }
    static IEnumerator AttendrePourDestroy(float durée, GameObject item)
    {
        yield return new WaitForSeconds(durée);
        Destroy(item);
    }
}
