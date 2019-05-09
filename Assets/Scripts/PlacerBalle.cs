//Fait Par Alexandre Dubuc
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlacerBalle : NetworkBehaviour
{
    public Vector3 positionJouer;
    [SyncVar(hook = "OnAcienGardienChange")]
    public string AncienGardien;
    [SyncVar(hook = "OnDernierPosseseurChange")]
    public string dernierPosseseur;
    const int NOMBRE_PLAYER_MAX = 4;
    public bool estPlacer = false;
    void OnDernierPosseseurChange(string change)
    {
        dernierPosseseur = change;
    }
    void OnAcienGardienChange(string change)
    {
        AncienGardien = change;
    }
    void Start()
    {
        AncienGardien = null;
        dernierPosseseur = GameObject.Find("Joueur1A").name;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.tag == "Untagged")
        {
            return;
        }

        if (other.tag == "ZoneC")
        {
            GameObject parent = other.transform.parent.gameObject;
            if (other.transform.parent.tag == "Player")
            {
                CmdMettreBalleEnfant(parent);
            }
            else if (other.transform.parent.tag == "AI")
            {
                CmdTrouverJoueurÀChanger(other.transform.parent.gameObject);
                CmdMettreBalleEnfant(parent);
            }
            else if (other.transform.parent.tag == "Gardien")
            {             
                CmdTrouverJoueurÀChangerGardien(other.transform.parent.gameObject);
                CmdMettreBalleEnfant(parent);
            }
        }

    }
    [Command]
    void CmdMettreBalleEnfant(GameObject other)
    {
        RpcMettreBalleEnfant(other);      
    }

    [ClientRpc]
    void RpcMettreBalleEnfant(GameObject other)
    {
        estPlacer = true;
        this.transform.parent = other.transform;
        transform.localScale = Vector3.one;

        this.transform.localPosition = new Vector3(0, 1.5f, 2);
        GetComponent<SphereCollider>().enabled = false;
        transform.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void CalculerDistanceBalle()
    {
        this.transform.localPosition = new Vector3(0, 1.5f, 2);
    }

    void RéglerCopie(GameObject joueur, GameObject copie)
    {
        string aI = "AI";
        string équipe = joueur.GetComponent<TypeÉquipe>().estÉquipeA ? "A" : "B";
        List<GameObject> vraiListe = new List<GameObject>();
        GameObject[] listeAI = GameObject.FindGameObjectsWithTag("AI");
        foreach (GameObject x in listeAI)
        {
            if (x.name.StartsWith("Joueur"))
            {

            }
            else
            {
                vraiListe.Add(x);
            }
        }
        int grandeur = (vraiListe.Count / 2) + 1;

        copie.name = aI + grandeur + équipe;
        copie.tag = aI;
        copie.GetComponent<MouvementPlayer>().enabled = false;
        copie.GetComponent<ScriptMouvementAI>().enabled = true;
    }

    void RéglerCopieAI(GameObject bon, GameObject copie, GameObject[] liste)
    {
        bool est1 = false; bool est2 = false; bool est3 = false; bool est4 = false;
        string équipe = bon.GetComponent<TypeÉquipe>().estÉquipeA ? "A" : "B";
        List<GameObject> listeÉquipe = new List<GameObject>();
        foreach (GameObject x in liste)
        {
            if (bon.GetComponent<TypeÉquipe>().estÉquipeA == x.GetComponent<TypeÉquipe>().estÉquipeA)
            {
                listeÉquipe.Add(x);
            }
        }
        foreach (GameObject x in listeÉquipe)
        {
            if (x.name[x.name.Length - 2] == '1' || x.name[x.name.Length - 2] == '2')
            {
                if(x.name[x.name.Length - 2] == '1')
                {
                    est1 = true;
                }
                else
                {
                    est2 = true;
                }
                
            }
            else if (x.name[x.name.Length - 2] == '3' || x.name[x.name.Length - 2] == '4')
            {
                if(x.name[x.name.Length - 2] == '3')
                {
                    est3 = true;
                }
                else
                {
                    est4 = true;
                }
            }
        }
        if(!est1 || !est2 || !est3 || ! est4)
        {
            copie.name = ChangerNomCopie(est1, est2, est3, est4, équipe);
        }
    }
    string ChangerNomCopie(bool est1,bool est2,bool est3,bool est4,string équipe)
    {
        string copie = null;
        if (!est1 || !est2)
        {
            if (!est1)
            {
                copie = "AI1" + équipe;
            }
            else
            {
                copie = "AI2" + équipe;
            }
        }
        else if (!est3 || !est4)
        {
            if (!est3)
            {
                copie = "AI3" + équipe;
            }
            else
            {
                copie = "AI4" + équipe;
            }
        }
        return copie;
    }
    void VérifierCopie(GameObject[] listeJoueur1, GameObject[] listeAI)
    {
        foreach (GameObject x in listeJoueur1)
        {
            foreach (GameObject z in listeAI)
            {
                if (x.name == z.name)
                {

                    RéglerCopie(x, z);
                }
            }
        }
    }

    void VérifierCopieAI(GameObject[] listeAI)
    {
        int indice = 0;
        foreach (GameObject z in listeAI)
        {
            for (int i = 0; i < listeAI.Length; i++)
            {
                if (z.name == listeAI[i].name && i != indice)
                {
                    RéglerCopieAI(z, listeAI[i], listeAI);
                }
            }
            indice++;
        }
    }

    void VérifierCopieJoueur(GameObject[] listeJoueur1)
    {
        if (GameObject.FindGameObjectWithTag("Balle").transform.parent != null)
        {
            foreach (GameObject x in listeJoueur1)
            {
                if (GameObject.FindGameObjectWithTag("Balle").transform.parent.gameObject.name == x.name)
                {
                    if (x != GameObject.FindGameObjectWithTag("Balle").transform.parent.gameObject)
                    {
                        RéglerCopie(GameObject.FindGameObjectWithTag("Balle").transform.parent.gameObject, x);
                    }

                }
            }
        }
    }

    void Update()
    {
        GameObject[] listeJoueur1 = GameObject.FindGameObjectsWithTag("Player");

        GameObject[] listeAI = GameObject.FindGameObjectsWithTag("AI");

        VérifierCopie(listeJoueur1, listeAI);

        VérifierCopieAI(listeAI);

        VérifierCopieJoueur(listeJoueur1);

        if (estPlacer)
        {
            CalculerDistanceBalle();
        }
    }

    [Command]
    void CmdTrouverJoueurÀChanger(GameObject aI)
    {
        RpcTrouverJoueurÀChanger(aI);
    }

    [ClientRpc]
    void RpcTrouverJoueurÀChanger(GameObject aI)
    {
        string tampon = null;
        if (GameObject.Find(dernierPosseseur).GetComponent<TypeÉquipe>().estÉquipeA == aI.GetComponent<TypeÉquipe>().estÉquipeA)
        {
            ChangerMêmeÉquipe(aI, tampon);
        }
        else
        {
            ChangerAutreÉquipe(aI, tampon);
        }
    }
    void ChangerAutreÉquipe(GameObject aI,string tampon)
    {
        GameObject[] liste = new GameObject[NOMBRE_PLAYER_MAX];
        List<GameObject> liste2 = new List<GameObject>();
        liste = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject x in liste)
        {
            if (aI.GetComponent<TypeÉquipe>().estÉquipeA == x.GetComponent<TypeÉquipe>().estÉquipeA)
            {
                liste2.Add(x);
            }
        }
        int grandeur = liste2.Count;
        int aléatoire = UnityEngine.Random.Range(1, grandeur);
        tampon = liste2[aléatoire - 1].name;
        RpcChangerAIÀJoueur(aI, liste2[aléatoire - 1], tampon);
    }
    void ChangerMêmeÉquipe(GameObject aI,string tampon)
    {
        string équipe;
        if (GameObject.Find(dernierPosseseur).GetComponent<TypeÉquipe>().estÉquipeA)
        {
            équipe = "A";
        }
        else
        {
            équipe = "B";
        }

        if (GameObject.Find(dernierPosseseur).tag == "Player")
        {
            tampon = "Joueur" + GameObject.Find(dernierPosseseur).name[GameObject.Find(dernierPosseseur).name.Length - 2] + équipe;
        }
        RpcChangerAIÀJoueur(aI, GameObject.Find(dernierPosseseur), tampon);
    }

    [ClientRpc]
    void RpcChangerAIÀJoueur(GameObject aI, GameObject joueur, string tampon)
    {
        ChangerJoueur(joueur);
        joueur.name = aI.name;

        ChangerAI(aI, tampon);
    }

    [Command]
    void CmdTrouverJoueurÀChangerGardien(GameObject gardien)
    {
        RpcTrouverJoueurÀChangerGardien(gardien);
    }

    [ClientRpc]
    void RpcTrouverJoueurÀChangerGardien(GameObject gardien)
    {
        AncienGardien = "Gardien";
        string tampon;

        GameObject[] listeAI = new GameObject[10];
        List<GameObject> listeAIMonÉquipe = new List<GameObject>();
        listeAI = GameObject.FindGameObjectsWithTag("AI");
        string équipe;

        foreach (GameObject x in listeAI)
        {
            if (x.GetComponent<TypeÉquipe>().estÉquipeA == gardien.GetComponent<TypeÉquipe>().estÉquipeA)
            {
                listeAIMonÉquipe.Add(x);
            }

        }
        if (gardien.GetComponent<TypeÉquipe>().estÉquipeA)
        {
            équipe = "A";
        }
        else
        {
            équipe = "B";
        }
        GameObject joueur = GameObject.Find("Joueur1" + équipe);
        int grandeur = listeAIMonÉquipe.Count + 1;
        tampon = joueur.name;

        CmdChangerGardienÀJoueur(joueur, gardien, tampon, grandeur, équipe);
    }

    [Command]
    void CmdChangerGardienÀJoueur(GameObject joueur, GameObject gardien, string tampon, int grandeur, string équipe)
    {
        RpcChangerGardienÀJoueur(joueur, gardien, tampon, grandeur, équipe);
    }

    [ClientRpc]
    void RpcChangerGardienÀJoueur(GameObject joueur, GameObject gardien, string tampon, int grandeur, string équipe)
    {
        ChangerJoueur(joueur);
        joueur.name = "AI" + grandeur + équipe;

        ChangerGardien(gardien, tampon);
    }

    void ChangerJoueur(GameObject joueur)
    {
        joueur.GetComponent<MouvementPlayer>().enabled = false;
        joueur.GetComponent<ScriptMouvementAI>().enabled = true;
        joueur.tag = "AI";
        joueur.GetComponentInChildren<Rigidbody>().isKinematic = true;
    }

    void ChangerGardien(GameObject gardien, string tampon)
    {
        gardien.name = tampon;
        gardien.GetComponent<MouvementPlayer>().enabled = true;
        gardien.GetComponent<ContrôleGardien>().enabled = false;
        gardien.GetComponentInChildren<ActionPlaquageGardien>().enabled = false;
        gardien.GetComponentInChildren<GérerProbabilitéArrêt>().enabled = false;
        gardien.transform.Find("ZoneArrêt").GetComponent<BoxCollider>().enabled = false;
        gardien.tag = "Player";
    }

    void ChangerAI(GameObject aI, string tampon)
    {
        aI.name = tampon;
        aI.GetComponent<MouvementPlayer>().enabled = true;
        aI.GetComponent<ScriptMouvementAI>().enabled = false;
        aI.tag = "Player";
    }
}
