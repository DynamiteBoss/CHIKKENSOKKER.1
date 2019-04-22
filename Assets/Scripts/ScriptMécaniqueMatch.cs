using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine.Networking;

public class ScriptMécaniqueMatch : NetworkBehaviour
{
    string[] tags = new string[] { "Player", "AI", "Gardien" };

    const string CheminAccesPartielOpts = "/Resources/Options/Options.txt";

    GameObject Balle { get; set; }
    GameObject PnlFin { get; set; }
    Text TxtFin { get; set; }
    [SerializeField]
    const float DuréeMatch = 10f;
    const float DuréeMatchVrai = 180f;

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
    float FrequenceObjet = 600f; //10 secondes

    float probabilitéOrage;
    float probabilitéPluie;

    [SerializeField]
    const int NbFramesUpdate = 10;

    const float DimTerrainX = 42f;
    const float DimTerrainZ = 20f;

    Text TxtTimer { get; set; }
    [SyncVar(hook = "OnChronomètreChange")] public string chronomètre;
    GameObject PnlNuit { get; set; }
    Light LumierePrincipale { get; set; }

    [SyncVar (hook = "OnMatchEnCoursChange")]public bool matchEnCours = true;
    [SyncVar(hook = "OnPauseChange")] public bool enPause;


    [SyncVar(hook = "OnTimerChange")] public float timer;
    [SyncVar(hook = "OnCompteurChange")] public int compteur = 0;
    [SyncVar(hook = "OnCompteur2Change")] public int compteur2 = 0;
    [SyncVar(hook = "OnCompteur3Change")] public int compteur3 = 0;
    [SyncVar(hook = "OnCompteurSpawnChange")] public int compteurSpawn = 0;


    List<GameObject> Joueur { get; set; }
    public int nbOeufs = 0;
    int NbOeufMax = 3;

    bool ajusteLumiere = false;
    bool modeNuitLocal;

    void Start()
    {
        //lis les valeurs dans le fichier texte OPTIONS
        using (StreamReader streamReader = new StreamReader(Application.dataPath + CheminAccesPartielOpts))
        {
            streamReader.ReadLine();
            float.TryParse(streamReader.ReadLine().ToString(), out FrequenceObjet);
            streamReader.ReadLine();
            int.TryParse(streamReader.ReadLine().ToString(), out NbOeufMax);
            streamReader.ReadLine();
            float.TryParse(streamReader.ReadLine().ToString(), out probabilitéPluie);
            streamReader.ReadLine();
            float.TryParse(streamReader.ReadLine().ToString(), out probabilitéOrage);

            streamReader.Close();
        }



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
        enPause = false;
        PnlFin = GameObject.Find("Interface").transform.Find("PnlPrincipal").transform.Find("PnlFin").gameObject;
        compteurSpawn = 0;
        Balle = GameObject.FindGameObjectWithTag("Balle");

        GameObject[] liste = new GameObject[10];
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
            x.transform.position = GameObject.Find("SpawnPoint" + compteurSpawn).transform.position + Vector3.up;
            compteurSpawn++;
            //TxtFin.text = compteurSpawn.ToString();
        }
        
        foreach(GameObject x in listeB)
        {
            x.transform.position = GameObject.Find("SpawnPoint" + compteurSpawn).transform.position + Vector3.up;
            compteurSpawn++;
        }
        
        


        PnlFin.SetActive(false);
        timer = DuréeMatchVrai;
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
        enPause = true;
        GameObject[] liste = new GameObject[10];
        List<GameObject> listeCommune = new List<GameObject>();



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
        Balle.transform.parent = null;
        Balle.GetComponent<Rigidbody>().isKinematic = false;
        Balle.GetComponent<SphereCollider>().enabled = true;
        Balle.GetComponent<PlacerBalle>().estPlacer = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (true/*GameObject.FindGameObjectsWithTag("AI").Length > 3*/)   //TEMPORAIRE
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
        {
            AttendreDébutMatch();
        }
            

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
    [Command]
    public void CmdPartirModePluie()
    {
        List<GameObject> listeJoueursLocale = GameObject.FindGameObjectsWithTag("Player").ToList();
        listeJoueursLocale.AddRange(GameObject.FindGameObjectsWithTag("AI").ToList());              // inclus les AI
        foreach (GameObject g in listeJoueursLocale)
        {
            g.GetComponentInChildren<MouvementPlayer>().modeGlace = true;
        }
    }

    private void FaireProgresserMatchUnPas()
    {
        TxtTimer = GameObject.Find("Interface").transform.Find("PnlPrincipal").transform.Find("PnlScore").transform.Find("Temps").gameObject.GetComponentInChildren<Text>();
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
    void OnMatchEnCoursChange(bool changemment)
    {
        matchEnCours = changemment;
    }
    void OnPauseChange(bool changemment)
    {
        enPause = changemment;
    }
}
