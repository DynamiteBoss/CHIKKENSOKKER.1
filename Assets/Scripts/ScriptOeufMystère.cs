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
            //GetComponent<NetworkIdentity>().AssignClientAuthority(this.GetComponent<NetworkIdentity>().connectionToClient);
            //int random = UnityEngine.Random.Range(0, IndiceMax);
            indice = GameObject.FindGameObjectWithTag("Balle").GetComponent<ScriptBut>().random;
            //indice = UnityEngine.Random.Range(0, IndiceMax);
            CmdAttribuerObjetJoueur(other.transform.parent.parent.gameObject, indice);    
        }
        else if ((other.name == "ZoneContrôle" || other.name == "ZonePlacage" || other.name == "Corps") && other.transform.parent.tag == "Player")
        {
            Destroy(this.transform.gameObject);
            GameObject.Find("Main Camera").GetComponent<ScriptMécaniqueMatch>().nbOeufs -= 1;
            //GetComponent<NetworkIdentity>().AssignClientAuthority(this.GetComponent<NetworkIdentity>().connectionToClient);
            //int random = UnityEngine.Random.Range(0, IndiceMax);
            indice = GameObject.FindGameObjectWithTag("Balle").GetComponent<ScriptBut>().random;
            //ndice = UnityEngine.Random.Range(0, IndiceMax);
            CmdAttribuerObjetJoueur(other.transform.parent.gameObject, indice);
        }
        //GetComponent<NetworkIdentity>().RemoveClientAuthority(this.GetComponent<NetworkIdentity>().connectionToClient);
    }
    [Command]
    private void CmdAttribuerObjetJoueur(GameObject joueur, int indice)
    {
        Debug.Log(indice);
        if (joueur.GetComponent<TypeÉquipe>().estÉquipeA)
        {
            if (Inventaire.itemA2 == Inventaire.ITEMNUL && Inventaire.itemA1 != Inventaire.ITEMNUL)
            {
                Inventaire.itemA2 = indice;
                CmdAfficherInventaire('A', 2);
                //METTRE ITEM A2
                Debug.Log("L'item A2 a été changé en " + Inventaire.EnTexte(indice));

            }
            else if (Inventaire.itemA2 == Inventaire.ITEMNUL && Inventaire.itemA1 == Inventaire.ITEMNUL)
            {
                Inventaire.itemA1 = indice;
                Debug.Log(Inventaire.itemA1);
                CmdAfficherInventaire('A', 1);
                Debug.Log(Inventaire.itemA1);
                //METTRE ITEM A1
                Debug.Log("L'item A1 a été changé en " + Inventaire.EnTexte(indice));
            }
        }
        else
        {
            if (Inventaire.itemB2 == Inventaire.ITEMNUL && Inventaire.itemB1 != Inventaire.ITEMNUL)
            {
                Inventaire.itemB2 = indice;
                CmdAfficherInventaire('B', 2);
                Debug.Log("L'item B2 a été changé en " + Inventaire.EnTexte(indice));
                //METTRE ITEM B2
            }
            else if (Inventaire.itemB2 == Inventaire.ITEMNUL && Inventaire.itemB1 == Inventaire.ITEMNUL)
            {
                Inventaire.itemB1 = indice;
                CmdAfficherInventaire('B', 1);
                //METTRE ITEM B1
                Debug.Log("L'item B1 a été changé en " + Inventaire.EnTexte(indice));
            }
        }
    }

    [Command]
    public void CmdAfficherInventaire(char équipe, int position)
    {
        int valeurItemTemp = 8;

        if (équipe == 'A')
        {
            if (position == 1)
            {
                valeurItemTemp = Inventaire.itemA1;
                positionSprite = new Vector3(-134, -96, -244);
                Debug.Log("la position est " + positionSprite.ToString());
            }
            else
            {
                valeurItemTemp = Inventaire.itemA2;
                positionSprite = new Vector3(-218, -96, -244);
            }
        }
        else if (équipe == 'B')
        {
            if (position == 1)
            {
                valeurItemTemp = Inventaire.itemB1;
                positionSprite = new Vector3(132, -96, -244);
            }
            else
            {
                valeurItemTemp = Inventaire.itemB2;
                positionSprite = new Vector3(217, -96, -244);
            }
        }

        switch (valeurItemTemp)
        {
            case 0:
                //GameObject Crotte3Sprite = (GameObject)Instantiate(Item.RetournerItemListe(8).ItemPhysique, positionSprite, Quaternion.Euler(41, 0, 0));
                GameObject Crotte3Sprite = (GameObject)Instantiate(Item.RetournerItemListe(8).ItemPhysique, GameObject.Find("PnlPrincipal").transform, false);
                CmdChangerPositionSprite(Crotte3Sprite);
                NetworkServer.Spawn(Crotte3Sprite);
                //CmdAfficherSprite(position, équipe, "crotte3");
                return;
            case 1:
                //GameObject Crotte5Sprite = (GameObject)Instantiate(Item.RetournerItemListe(9).ItemPhysique, positionSprite, Quaternion.Euler(41, 0, 0));
                GameObject Crotte5Sprite = (GameObject)Instantiate(Item.RetournerItemListe(9).ItemPhysique, GameObject.Find("PnlPrincipal").transform, false);
                CmdChangerPositionSprite(Crotte5Sprite);
                NetworkServer.Spawn(Crotte5Sprite);
                //CmdAfficherSprite(position, équipe, "crotte5");
                return;
            case 2:
                //GameObject OeufBlancXLSprite = (GameObject)Instantiate(Item.RetournerItemListe(11).ItemPhysique, positionSprite, Quaternion.Euler(41, 0, 0));
                GameObject OeufBlancXLSprite = (GameObject)Instantiate(Item.RetournerItemListe(11).ItemPhysique, GameObject.Find("PnlPrincipal").transform, false);
                CmdChangerPositionSprite(OeufBlancXLSprite);
                NetworkServer.Spawn(OeufBlancXLSprite);
                //CmdAfficherSprite(position, équipe, "oeufXL");
                return;
            case 3:
                //GameObject OeufBlanc3Sprite = (GameObject)Instantiate(Item.RetournerItemListe(10).ItemPhysique, positionSprite, Quaternion.Euler(41, 0, 0));
                GameObject OeufBlanc3Sprite = (GameObject)Instantiate(Item.RetournerItemListe(10).ItemPhysique, GameObject.Find("PnlPrincipal").transform, false);
                CmdChangerPositionSprite(OeufBlanc3Sprite);
                NetworkServer.Spawn(OeufBlanc3Sprite);
                //CmdAfficherSprite(position, équipe, "oeuf3");
                return;
            case 4:
                //GameObject OeufBrunSprite = (GameObject)Instantiate(Item.RetournerItemListe(12).ItemPhysique, positionSprite, Quaternion.Euler(41, 0, 0));
                GameObject OeufBrunSprite = (GameObject)Instantiate(Item.RetournerItemListe(12).ItemPhysique, GameObject.Find("PnlPrincipal").transform, false);
                CmdChangerPositionSprite(OeufBrunSprite);
                NetworkServer.Spawn(OeufBrunSprite);
                //CmdAfficherSprite(position, équipe, "torpille");
                return;
            case 5:
                //GameObject OeufBombeSprite = (GameObject)Instantiate(Item.RetournerItemListe(13).ItemPhysique, positionSprite, Quaternion.Euler(41, 0, 0));
                GameObject OeufBombeSprite = (GameObject)Instantiate(Item.RetournerItemListe(13).ItemPhysique, GameObject.Find("PnlPrincipal").transform, false);
                CmdChangerPositionSprite(OeufBombeSprite);
                NetworkServer.Spawn(OeufBombeSprite);
                //CmdAfficherSprite(position, équipe, "bombe");
                return;
            case 6:
                //GameObject OeufBrouilléSprite = (GameObject)Instantiate(Item.RetournerItemListe(14).ItemPhysique, positionSprite, Quaternion.Euler(41, 0, 0));
                GameObject OeufBrouilléSprite = (GameObject)Instantiate(Item.RetournerItemListe(14).ItemPhysique, GameObject.Find("PnlPrincipal").transform, false);
                CmdChangerPositionSprite(OeufBrouilléSprite);
                NetworkServer.Spawn(OeufBrouilléSprite);
                //CmdAfficherSprite(position, équipe, "brouillé");
                return;
            case 7:
                //GameObject VersDeTerreSprite = (GameObject)Instantiate(Item.RetournerItemListe(15).ItemPhysique, positionSprite, Quaternion.Euler(41, 0, 0));
                GameObject VersDeTerreSprite = (GameObject)Instantiate(Item.RetournerItemListe(15).ItemPhysique, GameObject.Find("PnlPrincipal").transform, false);
                CmdChangerPositionSprite(VersDeTerreSprite);
                NetworkServer.Spawn(VersDeTerreSprite);
                //CmdAfficherSprite(position, équipe, "versdeterre");
                return;
            default:
                return;
        }
    }
    [Command]
    private void CmdChangerPositionSprite(GameObject sprite)
    {
        Debug.Log("SUIS-JE LA&&??????");
        sprite.transform.localPosition = positionSprite;
    }

    //[Command]
    //private void CmdAfficherSprite(int position, char équipe, string nomSprite)
    //{
    //    if (équipe == 'A')
    //    {
    //        if (position == 1)
    //        {
    //            Inventaire.objet1A = Resources.Load<Sprite>("Image/" + nomSprite);
    //            GameObject.Find("Objet1A").GetComponent<SpriteRenderer>().sprite = Inventaire.objet1A;
    //            Debug.Log("Le Sprite" + nomSprite + "a été affiché en 1A");
    //        }
    //        else
    //        {
    //            Inventaire.objet2A = Resources.Load<Sprite>("Image/" + nomSprite);
    //            GameObject.Find("Objet2A").GetComponent<SpriteRenderer>().sprite = Inventaire.objet2A;
    //            Debug.Log("Le Sprite" + nomSprite + "a été affiché en 2A");
    //        }
    //    }
    //    else if (équipe == 'B')
    //    {
    //        if (position == 1)
    //        {
    //            Inventaire.objet1B = Resources.Load<Sprite>("Image/" + nomSprite);
    //            GameObject.Find("Objet1B").GetComponent<SpriteRenderer>().sprite = Inventaire.objet1B;
    //            Debug.Log("Le Sprite" + nomSprite + "a été affiché en 1B");
    //        }
    //        else
    //        {
    //            Inventaire.objet2B = Resources.Load<Sprite>("Image/" + nomSprite);
    //            GameObject.Find("Objet2B").GetComponent<SpriteRenderer>().sprite = Inventaire.objet2B;
    //            Debug.Log("Le Sprite" + nomSprite + "a été affiché en 2B");

    //        }
    //    }
    //    
    //    //GameObject.Find("Objet" + position + équipe).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Image/" + nomSprite);    }    
    //}

    void OnIndiceChange(int changement)
    {
        indice = changement;
    }
    void OnPositionChange(Vector3 changement)
    {
        positionSprite = changement;
    }
}
