using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ScriptOeufMystère : NetworkBehaviour
{
    [SyncVar(hook = "OnIndiceChange")] public int indice;

    [SerializeField]
    GameObject JoueurContact { get; set; }

    int compteur = 0;

    [SyncVar(hook = "OnPositionChange")] Vector3 positionSprite;

    public const int IndiceMax = 8;

    public string équipeLive;

    void Update()
    {
        // pour que l'oeuf touren sur lui-même (FAIT PAR ALEXIS)
        this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x + 3.1f*Mathf.Sin(180-(compteur++%360)/5.7f), this.transform.rotation.eulerAngles.y + 1, this.transform.rotation.eulerAngles.z + 2.3f*Mathf.Cos(180 - (compteur++ % 360) / 15.3f));
    }

    //ontrigger mettre un indice aléatoire dans une certaine postion d'un inventaire, selon la position dans l'inventaire et l'équipe
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Tête" && other.transform.parent.parent.tag == "Player")
        {
            Destroy(this.transform.gameObject);
            GameObject.Find("Main Camera").GetComponent<ScriptMécaniqueMatch>().nbOeufs -= 1;
            indice = GameObject.FindGameObjectWithTag("Balle").GetComponent<ScriptBut>().random;
            CmdAttribuerObjetJoueur(other.transform.parent.parent.gameObject, indice);    
        }
        else if ((other.name == "ZoneContrôle" || other.name == "ZonePlacage" || other.name == "Corps") && other.transform.parent.tag == "Player")
        {
            Destroy(this.transform.gameObject);
            GameObject.Find("Main Camera").GetComponent<ScriptMécaniqueMatch>().nbOeufs -= 1;
            indice = GameObject.FindGameObjectWithTag("Balle").GetComponent<ScriptBut>().random;
            CmdAttribuerObjetJoueur(other.transform.parent.gameObject, indice);
        }
    }

    //attribuer l'item (donc l'indice) à l'emplacement de l'item dans l'inventaire
    [Command]
    private void CmdAttribuerObjetJoueur(GameObject joueur, int indice)
    {
        if (joueur.GetComponent<TypeÉquipe>().estÉquipeA)
        {
            équipeLive = "A";
            if (Inventaire.itemA2 == Inventaire.ITEMNUL && Inventaire.itemA1 != Inventaire.ITEMNUL)
            {
                Inventaire.itemA2 = indice;
                AfficherInventaire('A', 2);
                //METTRE ITEM A2
            }
            else if (Inventaire.itemA2 == Inventaire.ITEMNUL && Inventaire.itemA1 == Inventaire.ITEMNUL)
            {
                Inventaire.itemA1 = indice;
                AfficherInventaire('A', 1);
                //METTRE ITEM A1
            }
        }
        else
        {
            équipeLive = "B";
            if (Inventaire.itemB2 == Inventaire.ITEMNUL && Inventaire.itemB1 != Inventaire.ITEMNUL)
            {
                Inventaire.itemB2 = indice;
                AfficherInventaire('B', 2);
                //METTRE ITEM B2
            }
            else if (Inventaire.itemB2 == Inventaire.ITEMNUL && Inventaire.itemB1 == Inventaire.ITEMNUL)
            {
                Inventaire.itemB1 = indice;
                AfficherInventaire('B', 1);
                //METTRE ITEM B1
            }
        }
    }

    public void AfficherInventaire(char équipe, int position)
    {
        int valeurItemTemp = 8;

        //trouver la position des sprites à spawner selon la position de l'item (inventaire 1 ou 2) et l'équipe que tu es en ce  moment
        if (équipe == 'A')
        {
            if (position == 1)
            {
                valeurItemTemp = Inventaire.itemA1;
                positionSprite = new Vector3(-130, -104, -244);
            }
            else
            {
                valeurItemTemp = Inventaire.itemA2;
                positionSprite = new Vector3(-205, -104, -244);
            }
        }
        else if (équipe == 'B')
        {
            if (position == 1)
            {
                valeurItemTemp = Inventaire.itemB1;
                positionSprite = new Vector3(134, -104, -244);
            }
            else
            {
                valeurItemTemp = Inventaire.itemB2;
                positionSprite = new Vector3(210, -104, -244);
            }
        }

        //quel item (selon l'indice) est affiché sur les sprites et les spawner donc
        switch (valeurItemTemp)
        {
            case 0:
                GameObject Crotte3Sprite = (GameObject)Instantiate(Item.RetournerItemListe(8).ItemPhysique, positionSprite, Quaternion.Euler(41, 0, 0), GameObject.Find("PnlPrincipal").transform);
                NetworkServer.Spawn(Crotte3Sprite);
                if (équipeLive == "A")
                {
                    RpcSynchroParent(Crotte3Sprite);
                }
                else if (équipeLive == "B")
                {
                    CmdSynchroParent(Crotte3Sprite);
                }
                SynchroSprite(Crotte3Sprite);
                return;
            case 1:
                GameObject Crotte5Sprite = (GameObject)Instantiate(Item.RetournerItemListe(9).ItemPhysique, positionSprite, Quaternion.Euler(41, 0, 0), GameObject.Find("PnlPrincipal").transform);
                NetworkServer.Spawn(Crotte5Sprite);
                if (équipeLive == "A")
                {
                    RpcSynchroParent(Crotte5Sprite);
                }
                else if (équipeLive == "B")
                {
                    CmdSynchroParent(Crotte5Sprite);
                }
                SynchroSprite(Crotte5Sprite);
                return;
            case 2:
                GameObject OeufBlancXLSprite = (GameObject)Instantiate(Item.RetournerItemListe(11).ItemPhysique, positionSprite, Quaternion.Euler(41, 0, 0), GameObject.Find("PnlPrincipal").transform);
                NetworkServer.Spawn(OeufBlancXLSprite);
                if (équipeLive == "A")
                {
                    RpcSynchroParent(OeufBlancXLSprite);
                }
                else if (équipeLive == "B")
                {
                    CmdSynchroParent(OeufBlancXLSprite);
                }
                SynchroSprite(OeufBlancXLSprite);
                return;
            case 3:
                GameObject OeufBlanc3Sprite = (GameObject)Instantiate(Item.RetournerItemListe(10).ItemPhysique, positionSprite, Quaternion.Euler(41, 0, 0), GameObject.Find("PnlPrincipal").transform);
                NetworkServer.Spawn(OeufBlanc3Sprite);
                if (équipeLive == "A")
                {
                    RpcSynchroParent(OeufBlanc3Sprite);
                }
                else if (équipeLive == "B")
                {
                    CmdSynchroParent(OeufBlanc3Sprite);
                }
                SynchroSprite(OeufBlanc3Sprite);
                return;
            case 4:
                GameObject OeufBrunSprite = (GameObject)Instantiate(Item.RetournerItemListe(12).ItemPhysique, positionSprite, Quaternion.Euler(41, 0, 0), GameObject.Find("PnlPrincipal").transform);
                NetworkServer.Spawn(OeufBrunSprite);
                if (équipeLive == "A")
                {
                    RpcSynchroParent(OeufBrunSprite);
                }
                else if (équipeLive == "B")
                {
                    CmdSynchroParent(OeufBrunSprite);
                }
                SynchroSprite(OeufBrunSprite);
                return;
            case 5:
                GameObject OeufBombeSprite = (GameObject)Instantiate(Item.RetournerItemListe(13).ItemPhysique, positionSprite, Quaternion.Euler(41, 0, 0), GameObject.Find("PnlPrincipal").transform);
                NetworkServer.Spawn(OeufBombeSprite);
                if (équipeLive == "A")
                {
                    RpcSynchroParent(OeufBombeSprite);
                }
                else if (équipeLive == "B")
                {
                    CmdSynchroParent(OeufBombeSprite);
                }
                SynchroSprite(OeufBombeSprite);
                return;
            case 6:
                GameObject OeufBrouilléSprite = (GameObject)Instantiate(Item.RetournerItemListe(14).ItemPhysique, positionSprite, Quaternion.Euler(41, 0, 0), GameObject.Find("PnlPrincipal").transform);
                NetworkServer.Spawn(OeufBrouilléSprite);
                if (équipeLive == "A")
                {
                    RpcSynchroParent(OeufBrouilléSprite);
                }
                else if (équipeLive == "B")
                {
                    CmdSynchroParent(OeufBrouilléSprite);
                }
                SynchroSprite(OeufBrouilléSprite);
                return;
            case 7:
                GameObject VersDeTerreSprite = (GameObject)Instantiate(Item.RetournerItemListe(15).ItemPhysique, positionSprite, Quaternion.Euler(41, 0, 0), GameObject.Find("PnlPrincipal").transform);
                NetworkServer.Spawn(VersDeTerreSprite);
                if (équipeLive == "A")
                {
                    RpcSynchroParent(VersDeTerreSprite);
                }
                else if (équipeLive == "B")
                {
                    CmdSynchroParent(VersDeTerreSprite);
                }
                SynchroSprite(VersDeTerreSprite);
                return;
            default:
                return;
        }
    }

    //synchroniser les parents des sprites spawnés
    [Command]
    void CmdSynchroParent(GameObject item)
    {
        item.transform.SetParent(GameObject.Find("PnlPrincipal").transform);
        RpcSynchroParent(item);
    }
    [ClientRpc]
    void RpcSynchroParent(GameObject item)
    {
        item.transform.SetParent(GameObject.Find("PnlPrincipal").transform);  
    }

    //synchronser les sprites (ce script ne marche pas pour le client, mais marche pour le host. Si un host prend un item, ca va afficher OK pour le host ET le client
    //et si un client prend un item, ç va afficher OK pour le host mais PAS pour le client.
    void SynchroSprite(GameObject item)
    {
        CmdSynchroSprite(item);
    }
    [Command]
    void CmdSynchroSprite(GameObject item)
    {
        item.transform.localPosition = positionSprite;
        item.transform.localScale = new Vector3(9, 9, 9);
        RpcSynchroSprite(item, positionSprite);
    }
    [ClientRpc]
    void RpcSynchroSprite(GameObject item, Vector3 positionVrai)
    {
        item.transform.localPosition = positionVrai; //  NE MARCHE PAS
        item.transform.localScale = new Vector3(9,9,9);  //  MARCHE PI LA ROTATION EST OK AUSSI
    }

    // pas sur ça sert à grand chose
    void OnIndiceChange(int changement)
    {
        indice = changement;
    }
    // pas sur ça sert à grand chose
    void OnPositionChange(Vector3 changement)
    {
        positionSprite = changement;
    }
}
