using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;

public class ScriptBut : NetworkBehaviour
{
    const int IndiceMax = 8;
    float compteur2 = 0;
    string[] tags = new string[] { "Player", "AI", "Gardien" };
    public int compteurSpawn = 0;
    const float TEMPS_MIN = 1f;
    string NomBut1 = "But1";
    string NomBut2 = "But2";

    [SyncVar(hook = "OnButChangeA")]
    public int NbButsA = 0;
    [SyncVar(hook = "OnButChangeB")]
    public int NbButsB = 0;

    [SyncVar(hook = "OnRandomChangeB")]
    public int random;


    [SyncVar(hook = "OnRandomChange")]
    public int randomEvent;


    [SyncVar(hook ="OnScoreChange")]
    public string score = 0 + "  -  " + 0;

    [SyncVar(hook = "OnTimeChange")]
    public float compteur = 0;

    [SyncVar(hook = "OnPauseChange")]
    public bool enPause;

    [SyncVar(hook = "OnButChange")]
    public bool butEffectuer;

    [SerializeField]
    bool estÉquipeA = true;

    
    Text InterfaceScore { get; set; }

    GameObject Ballon { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        InterfaceScore = GameObject.Find("Interface").transform.Find("PnlPrincipal").transform.Find("PnlScore").transform.Find("Score").GetComponent<Text>();
        Ballon = this.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "But" && compteur >= TEMPS_MIN)
        {
            butEffectuer = true;
            /*Ballon = this.gameObject;
            GameObject[] liste = new GameObject[10];
            List<GameObject> liste1 = new List<GameObject>();
            compteur = 0;
            Ballon.transform.position = new Vector3(1, 0.5f, 5);
            Ballon.GetComponent<Rigidbody>().isKinematic = true;
            Ballon.transform.parent = null;*/

            if (other.name == NomBut1)
                ++NbButsB;
            else if (other.name == NomBut2)
                ++NbButsA;

            score = NbButsB.ToString() + "  -  " + NbButsA.ToString();
            /*
            compteur = 0;
            enPause = true;
            PlacerJoueur(liste1,liste);
            Invoke("RéactiverMouvement", 1f);*/
        }

        // Ajouter un "Point" à l'équipe 1
    }
    
    void PlacerJoueur(List<GameObject> liste1, GameObject[] liste)
    {
        List<GameObject> listeA = new List<GameObject>();
        List<GameObject> listeB = new List<GameObject>();
        foreach (string x in tags)
        {
            liste = GameObject.FindGameObjectsWithTag(x);
            foreach (GameObject z in liste)
            {
                liste1.Add(z);
            }
        }
        foreach(GameObject x in liste1)
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
        compteurSpawn = 0;
        foreach (GameObject x in liste1)
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
    }
  
    void RéactiverMouvement()
    {
        GameObject[] liste = new GameObject[10];
        List<GameObject> liste1 = new List<GameObject>();
        foreach (string x in tags)
        {
            liste = GameObject.FindGameObjectsWithTag(x);
            foreach (GameObject z in liste)
            {
                liste1.Add(z);
            }
        }
        foreach(GameObject x in liste1)
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
        enPause = false;
        butEffectuer = false;
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "But")
            Ballon.GetComponent<Rigidbody>().isKinematic = false;
    }

    void OnButChangeA(int but)
    {
        NbButsA = but;
       // InterfaceScore.text = (NbButsB).ToString() + "  -  " + (NbButsA).ToString();
    }
    void OnButChangeB(int but)
    {
        NbButsB = but;
       // InterfaceScore.text = (NbButsB).ToString() + "  -  " + (NbButsA).ToString();
    }
    void OnRandomChangeB(int changement)
    {
        random = changement;
        
    }
    void OnRandomChange(int changement)
    {
        randomEvent = changement;

    }
    void OnScoreChange(string change)
    {
        score = change;
        InterfaceScore.text = score;
    }
    void OnTimeChange(float temps)
    {
        compteur = temps;
    }
    void OnPauseChange(bool changement)
    {
        enPause = changement;
    }
    void OnButChange(bool changement)
    {
        butEffectuer = changement;
    }
    // Update is called once per frame
    void Update()
    {
        compteur += Time.deltaTime;
        if(butEffectuer)
        {
            ButRéaliser();
        }

        compteur2 += Time.deltaTime;
        if(compteur2 >= 4f)
        {
            random = UnityEngine.Random.Range(0, IndiceMax);
            randomEvent = UnityEngine.Random.Range(0, 3);
        }

        
    }
    [Command]
    void CmdButRéaliser()
    {
        //RpcButRéaliser();
    }
  
    void ButRéaliser()
    {
        Ballon = this.gameObject;
        GameObject[] liste = new GameObject[10];
        List<GameObject> liste1 = new List<GameObject>();
        compteur = 0;
        Ballon.transform.position = new Vector3(1, 0.5f, 5);
        Ballon.GetComponent<Rigidbody>().isKinematic = true;
        Ballon.transform.parent = null;
        Ballon.GetComponent<SphereCollider>().enabled = true;

        compteur = 0;
        enPause = true;
        PlacerJoueur(liste1, liste);
        Invoke("RéactiverMouvement", 1f);
    }
}
