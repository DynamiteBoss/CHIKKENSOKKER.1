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

    void Start()
    {    
    }

    void Update()
    {
        this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x + 3.1f*Mathf.Sin(180-(compteur++%360)/5.7f), this.transform.rotation.eulerAngles.y + 1, this.transform.rotation.eulerAngles.z + 2.3f*Mathf.Cos(180 - (compteur++ % 360) / 15.3f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Tête" && other.transform.parent.parent.tag == "Player")
        {

            Destroy(this.transform.gameObject);
            GameObject.Find("Main Camera").GetComponent<ScriptMécaniqueMatch>().nbOeufs -= 1;
            //int random = UnityEngine.Random.Range(0, IndiceMax);
            indice = GameObject.FindGameObjectWithTag("Balle").GetComponent<ScriptBut>().random;
            //indice = UnityEngine.Random.Range(0, IndiceMax);
            CmdAttribuerObjetJoueur(other.transform.parent.parent.gameObject, indice);    
        }
        else if ((other.name == "ZoneContrôle" || other.name == "ZonePlacage" || other.name == "Corps") && other.transform.parent.tag == "Player")
        {
            Destroy(this.transform.gameObject);
            GameObject.Find("Main Camera").GetComponent<ScriptMécaniqueMatch>().nbOeufs -= 1;
            //int random = UnityEngine.Random.Range(0, IndiceMax);
            indice = GameObject.FindGameObjectWithTag("Balle").GetComponent<ScriptBut>().random;
            //ndice = UnityEngine.Random.Range(0, IndiceMax);
            CmdAttribuerObjetJoueur(other.transform.parent.gameObject, indice);
        }
    }
    [Command]
    private void CmdAttribuerObjetJoueur(GameObject joueur, int indice)
    {
        Debug.Log(indice);
        if (joueur.GetComponent<TypeÉquipe>().estÉquipeA)
        {
            équipeLive = "A";
            if (Inventaire.itemA2 == Inventaire.ITEMNUL && Inventaire.itemA1 != Inventaire.ITEMNUL)
            {
                Inventaire.itemA2 = indice;
                AfficherInventaire('A', 2);
                //METTRE ITEM A2
                Debug.Log("L'item A2 a été changé en " + Inventaire.EnTexte(indice));

            }
            else if (Inventaire.itemA2 == Inventaire.ITEMNUL && Inventaire.itemA1 == Inventaire.ITEMNUL)
            {
                Inventaire.itemA1 = indice;
                Debug.Log(Inventaire.itemA1);
                AfficherInventaire('A', 1);
                Debug.Log(Inventaire.itemA1);
                //METTRE ITEM A1
                Debug.Log("L'item A1 a été changé en " + Inventaire.EnTexte(indice));
            }
        }
        else
        {
            équipeLive = "B";
            if (Inventaire.itemB2 == Inventaire.ITEMNUL && Inventaire.itemB1 != Inventaire.ITEMNUL)
            {
                Inventaire.itemB2 = indice;
                AfficherInventaire('B', 2);
                Debug.Log("L'item B2 a été changé en " + Inventaire.EnTexte(indice));
                //METTRE ITEM B2
            }
            else if (Inventaire.itemB2 == Inventaire.ITEMNUL && Inventaire.itemB1 == Inventaire.ITEMNUL)
            {
                Inventaire.itemB1 = indice;
                AfficherInventaire('B', 1);
                //METTRE ITEM B1
                Debug.Log("L'item B1 a été changé en " + Inventaire.EnTexte(indice));
            }
        }
    }

    public void AfficherInventaire(char équipe, int position)
    {
        int valeurItemTemp = 8;

        if (équipe == 'A')
        {
            if (position == 1)
            {
                valeurItemTemp = Inventaire.itemA1;
                positionSprite = new Vector3(-130, -104, -244);
                Debug.Log("la position est " + positionSprite.ToString());
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

        switch (valeurItemTemp)
        {
            case 0:
                GameObject Crotte3Sprite = (GameObject)Instantiate(Item.RetournerItemListe(8).ItemPhysique, positionSprite, Quaternion.Euler(41, 0, 0), GameObject.Find("PnlPrincipal").transform);
                Debug.Log("la position est " + positionSprite.ToString());

                NetworkServer.Spawn(Crotte3Sprite);
                Debug.Log("la position est " + positionSprite.ToString());

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
                Debug.Log("la position est " + positionSprite.ToString());

                NetworkServer.Spawn(Crotte5Sprite);
                Debug.Log("la position est " + positionSprite.ToString());

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
                Debug.Log("la position est " + positionSprite.ToString());

                NetworkServer.Spawn(OeufBlancXLSprite);
                Debug.Log("la position est " + positionSprite.ToString());

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
                Debug.Log("la position est " + positionSprite.ToString());


                NetworkServer.Spawn(OeufBlanc3Sprite);
                Debug.Log("la position est " + positionSprite.ToString());

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
                Debug.Log("la position est " + positionSprite.ToString());

                NetworkServer.Spawn(OeufBrunSprite);
                Debug.Log("la position est " + positionSprite.ToString());

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
                Debug.Log("la position est " + positionSprite.ToString());

                NetworkServer.Spawn(OeufBombeSprite);
                Debug.Log("la position est " + positionSprite.ToString());

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
                Debug.Log("la position est " + positionSprite.ToString());

                NetworkServer.Spawn(OeufBrouilléSprite);
                Debug.Log("la position est " + positionSprite.ToString());

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
                Debug.Log("la position est " + positionSprite.ToString());

                NetworkServer.Spawn(VersDeTerreSprite);
                Debug.Log("la position est " + positionSprite.ToString());

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
    [Command]
    void CmdSynchroParent(GameObject item)
    {
        item.transform.SetParent(GameObject.Find("PnlPrincipal").transform);
        RpcSynchroParent(item);
    }
    [ClientRpc]
    void RpcSynchroParent(GameObject item)
    {
        item.transform.SetParent(GameObject.Find("PnlPrincipal").transform);  //CA TROUVE PAS LE PANEL
    }

    void SynchroSprite(GameObject item)
    {
        CmdSynchroSprite(item);
    }
    [Command]
    void CmdSynchroSprite(GameObject item)
    {
        Debug.Log(positionSprite.ToString() + "INITIAL");
        item.transform.localPosition = positionSprite;
        item.transform.localScale = new Vector3(8, 8, 8);
        RpcSynchroSprite(item, positionSprite);
        Debug.Log(positionSprite.ToString() + "FINAL");  
    }
    [ClientRpc]
    void RpcSynchroSprite(GameObject item, Vector3 positionVrai)
    {
        Debug.Log(positionVrai.ToString() + "DANS LE RPC");  //CA MARQUE 0,0,0
        item.transform.localPosition = positionVrai; //  NE MARCHE PAS
        item.transform.localScale = new Vector3(8,8,8);  //  MARCHE PI LA ROTATION EST OK AUSSI
    }

    void OnIndiceChange(int changement)
    {
        indice = changement;
    }
    void OnPositionChange(Vector3 changement)
    {
        positionSprite = changement;
    }
}
