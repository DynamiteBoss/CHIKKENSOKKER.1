using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;

public class ScriptMécaniqueMatch : NetworkBehaviour
{
    string[] tags = new string[] { "Player", "AI", "Gardien" };
    GameObject Balle { get; set; }
    GameObject PnlFin { get; set; }
    Text TxtFin { get; set; }
    [SerializeField]
    const float DuréeMatch = 300f; // sECONDES

    [SerializeField]
    const float DuréePluie = 30f;

    [SerializeField]
    const float DuréeGlace = 30f;

    [SerializeField]
    const float DuréeÉclairs = 20f;

    [SerializeField]
    const float DuréeNuit = 20f;
    [SerializeField]
    bool EstEnModeNuit;
    const int OpacitéMaxPannel = 175;
    const int IntensitéMaxLumiere = 100;
    const float VitesseJourNuit = 100f;

    [SerializeField]
    const float FrequenceObjet = 600f; //10 secondes

    [SerializeField]
    const int NbFramesUpdate = 10;

    const float DimTerrainX = 42f;
    const float DimTerrainZ = 20f;
    const int NbOeufMax = 3;

    Text TxtTimer { get; set; }
    [SyncVar(hook = "OnChronomètreChange")] public string chronomètre;
    GameObject PnlNuit { get; set; }
    Light LumierePrincipale { get; set; }

    bool matchEnCours = true;

    [SyncVar(hook = "OnTimerChange")] public float timer;
    [SyncVar(hook = "OnCompteurChange")] public int compteur = 0;
    [SyncVar(hook = "OnCompteur2Change")] public int compteur2 = 0;
    [SyncVar(hook = "OnCompteur3Change")] public int compteur3 = 0;
    [SyncVar(hook = "OnCompteurSpawnChange")] public int compteurSpawn = 0;


    List<GameObject> Joueur { get; set; }
    public int nbOeufs = 0;

    bool ajusteLumiere = false;
    bool modeNuitLocal;


    // Start is called before the first frame update
    void Start()
    {
        //TEMPORAIRE
        GameObject OeufHasard = (GameObject)Instantiate((GameObject)Resources.Load("Prefab/Item"), new Vector3(UnityEngine.Random.Range(-DimTerrainX, DimTerrainX), 1, UnityEngine.Random.Range(-DimTerrainZ, DimTerrainZ)), Quaternion.identity);
        GameObject OeufHasard2 = (GameObject)Instantiate((GameObject)Resources.Load("Prefab/Item"), new Vector3(UnityEngine.Random.Range(-DimTerrainX, DimTerrainX), 1, UnityEngine.Random.Range(-DimTerrainZ, DimTerrainZ)), Quaternion.identity);
        //TEMPORAIRE

        Balle = GameObject.FindGameObjectWithTag("Balle");

        PnlFin = GameObject.Find("Interface").transform.Find("PnlPrincipal").transform.Find("PnlFin").gameObject;
        TxtFin = PnlFin.transform.Find("TxtFin").gameObject.GetComponentInChildren<Text>();

        //timer = DuréeMatch;
        //if()
        //{
        //    timer = DuréeMatch;
        //}
        //timer = DuréeMatch;
        //if (isServer && isLocalPlayer)
        //{
        //    timer = DuréeMatch;
        //}



        TxtTimer = GameObject.Find("Interface").transform.Find("PnlPrincipal").transform.Find("PnlScore").transform.Find("Temps").gameObject.GetComponentInChildren<Text>();
        PnlNuit = GameObject.Find("Interface").transform.Find("PnlNuit").gameObject;

        LumierePrincipale = GameObject.Find("LumierePrincipale").GetComponentInChildren<Light>();
        modeNuitLocal = EstEnModeNuit;
    }
    void PartirMatch()
    {
        
        GameObject[] liste = new GameObject[10];
        GameObject[] listeAI = new GameObject[10];
        GameObject[] listeGardien = new GameObject[10];
        List<GameObject> listeCommune = new List<GameObject>();

        List<GameObject> listeA = new List<GameObject>();
        List<GameObject> listeB = new List<GameObject>();

        foreach (string x in tags)
        {
            liste = GameObject.FindGameObjectsWithTag(x);
            foreach(GameObject z in liste)
            {
                listeCommune.Add(z);
            }
        }

        foreach(GameObject x in listeCommune)
        {
            if(x.GetComponent<TypeÉquipe>().estÉquipeA)
            {
                listeA.Add(x);
            }
            else
            {
                listeB.Add(x);
            }
        }

        foreach(GameObject x in listeA)
        {
            x.transform.position = GameObject.Find("SpawnPoint" + compteurSpawn).transform.position;
            compteurSpawn++;
        }
        foreach(GameObject x in listeB)
        {
            x.transform.position = GameObject.Find("SpawnPoint" + compteurSpawn).transform.position;
            compteurSpawn++;
        }
        




        PnlFin.SetActive(false);
        timer = DuréeMatch;
        Balle.GetComponent<ScriptBut>().NbButsA = 0;
        Balle.GetComponent<ScriptBut>().NbButsB = 0;
        Balle.GetComponent<ScriptBut>().score = "0 - 0";
        Balle.transform.parent = null;
        Balle.transform.position = new Vector3(1, 0.5f, 5);
        compteur = 0;
        compteurSpawn = 0;
        matchEnCours = true;
        foreach (GameObject x in listeCommune)
        {
            if (x.tag == tags[0])
            {
                x.GetComponent<MouvementPlayer>().enabled = true;
            }
            else if (x.tag == tags[1])
            {
                x.GetComponent<ScriptMouvementAI>().enabled = true;
            }
            else
            {
                x.GetComponent<ContrôleGardien>().enabled = true;
            }
        }
    }
    void AttendreDébutMatch()
    {
        GameObject[] liste = new GameObject[10];
        List<GameObject> listeCommune = new List<GameObject>();

        GameObject[] listeAI = new GameObject[10];
        GameObject[] listeGardien = new GameObject[10];


        List<GameObject> listeA = new List<GameObject>();
        List<GameObject> listeB = new List<GameObject>();

        foreach (string x in tags)
        {
            liste = GameObject.FindGameObjectsWithTag(x);
            foreach (GameObject z in liste)
            {
                listeCommune.Add(z);
            }
        }

        foreach (GameObject x in listeCommune)
        {
            if (x.GetComponent<TypeÉquipe>().estÉquipeA)
            {
                listeA.Add(x);
            }
            else
            {
                listeB.Add(x);
            }
        }

        foreach (GameObject x in listeA)
        {

            x.transform.position = GameObject.Find("SpawnPoint" + compteurSpawn).transform.position + Vector3.up;
            compteurSpawn++;
        }
        foreach (GameObject x in listeB)
        {
            x.transform.position = GameObject.Find("SpawnPoint" + compteurSpawn).transform.position;
            compteurSpawn++;
        }
        foreach (string x in tags)
        {
            liste = GameObject.FindGameObjectsWithTag(x);
            foreach (GameObject z in liste)
            {
                listeCommune.Add(z);
            }
        }
        foreach (GameObject x in listeCommune)
        {
            if (x.tag == tags[0])
            {
                x.GetComponent<MouvementPlayer>().enabled = false;
            }
            else if (x.tag == tags[1])
            {
                x.GetComponent<ScriptMouvementAI>().enabled = false;
            }
            else
            {
                x.GetComponent<ContrôleGardien>().enabled = false;
            }
        }
        compteurSpawn = 0;
        Balle.transform.position = new Vector3(1, 0.5f, 5);
    }
    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("AI").Length > 3)
        {
            if (matchEnCours)
            {
                if (compteur3 == 0)
                {

                    PartirMatch();
                    compteur3++;
                }
                ++compteur;
                ++compteur2;
                timer -= Time.deltaTime;
                if (compteur == NbFramesUpdate + 1)
                {
                    compteur = 0;
                    if (timer > 0)
                    {
                        FaireProgresserMatchUnPas();
                    }
                    else
                    {
                        TerminerMatch();
                    }
                    if (ajusteLumiere)
                        AjusterModeNuit();

                }
                if (compteur2 % 20 == 0)
                {
                    if (EstEnModeNuit != modeNuitLocal)
                    {
                        modeNuitLocal = EstEnModeNuit;
                        ajusteLumiere = true;
                    }
                    if (compteur2 == FrequenceObjet)
                    {
                        compteur2 = 0;
                        CmdFaireApparaitreObjet();
                    }
                }
            }
        }
        else
            AttendreDébutMatch();

    }
    [Command]
    public void CmdFaireApparaitreObjet()
    {
        Vector3 positionObj = new Vector3(UnityEngine.Random.Range(-DimTerrainX, DimTerrainX), 1, UnityEngine.Random.Range(-DimTerrainZ, DimTerrainZ));
        if (nbOeufs < NbOeufMax)
        {
            GameObject OeufHasard = (GameObject)Instantiate((GameObject)Resources.Load("Prefab/Item"), positionObj, Quaternion.identity);
            NetworkServer.Spawn(OeufHasard);
            nbOeufs++;
        }
    }

    private void FaireProgresserMatchUnPas()
    {
        chronomètre = String.Format("{0:m} : {1} ", (((int)timer) / 60).ToString(), ((int)timer % 60).ToString().Length == 1 ? "0" + ((int)timer % 60).ToString() : ((int)timer % 60).ToString());
        TxtTimer.text = chronomètre;
    }

    private void TerminerMatch()
    {
        matchEnCours = false;
        string message;
        int butA = Balle.GetComponent<ScriptBut>().NbButsA;
        int butB = Balle.GetComponent<ScriptBut>().NbButsB;
        if (butA < butB)
        {
            message = "L'équipe B remporte la partie " + butB + " - " + butA;
        }
        else
        {
            if (butA > butB)
            {
                message = "L'équipe A remporte la partie " + butB + " - " + butA;
            }
            else
            {
                message = "Partie nulle " + butB + " - " + butA;
            }
        }
        PnlFin.SetActive(true);
        TxtFin.text = message;

        Invoke("AttendreDébutMatch", 4f);
        Invoke("PartirMatch", 5f);




        //GetComponent<ScriptMenuPause>().DésactiverMouvement();
    }
    void AjusterModeNuit()
    {
        List<GameObject> joueurs = GameObject.FindGameObjectsWithTag("Player")/*.OrderBy(x => int.Parse(x.name[9].ToString()))*/.ToList();

        if (!EstEnModeNuit)
        {
            for (int i = 0; i < joueurs.Count; i++)
            {
                joueurs[i].GetComponentInChildren<Light>().intensity -= 100f * Time.deltaTime * VitesseJourNuit;
            }
            LumierePrincipale.intensity += (1f) * Time.deltaTime * VitesseJourNuit;
            PnlNuit.GetComponentInChildren<Image>().color -= new Color(0, 0, 0, (.585f * Time.deltaTime) * VitesseJourNuit);
        }
        else
        {
            for (int i = 0; i < joueurs.Count; i++)
            {
                joueurs[i].GetComponentInChildren<Light>().intensity += 100f * Time.deltaTime * VitesseJourNuit;
            }
            LumierePrincipale.intensity -= (1f) * Time.deltaTime * VitesseJourNuit;
            PnlNuit.GetComponentInChildren<Image>().color += new Color(0, 0, 0, (.585f * Time.deltaTime) * VitesseJourNuit);
        }

        if (LumierePrincipale.intensity >= 1 || LumierePrincipale.intensity <= 0)
        {
            ajusteLumiere = false;
            //PnlNuit.SetActive(EstEnModeNuit);
            for (int i = 0; i < joueurs.Count; i++)
            {
                joueurs[i].GetComponentInChildren<Light>().intensity = EstEnModeNuit ? 100 : 0;
            }
        }

    }
    void OnTimerChange(float changement)
    {
        timer = changement;
    }
    void OnChronomètreChange(string changement)
    {
        chronomètre = changement;
    }
    void OnCompteurChange(int changement)
    {
        compteur = changement;
    }
    void OnCompteur2Change(int changement)
    {
        compteur2 = changement;
    }
    void OnCompteur3Change(int changement)
    {
        compteur3 = changement;
    }
    void OnCompteurSpawnChange(int changement)
    {
        compteurSpawn = changement;
    }
}
