// Cette partie du code à été inspiréé de la vidéo suivante : 
// https://www.youtube.com/watch?v=9w2kwGDZ6wM
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Linq;


// FAIRE UN CUSTOM PLAYER SPAWN
public class NetworkManagerPerso : NetworkManager
{
    public GameObject playerPre { get; set; }
    public GameObject aIPre { get; set; }
    public GameObject gardienPre { get; set; }
    public List<GameObject> prefabs = new List<GameObject>();

    Button Btn1v1 { get; set; }
    Button Btn2v2 { get; set; }
    Canvas CnvConnexion { get; set; }
    Canvas CnvNbJoueur { get; set; }

    public bool est1v1 = false;

    public Équipe ÉquipeA { get; set; }
    public Équipe ÉquipeB { get; set; }


    public ÉquipeV2 ÉquipeAV2 { get; set; }
    public ÉquipeV2 ÉquipeBV2 { get; set; }

    short compteurId = 0;

    int compteurA = 0;
    int compteurB = 0;
    GameObject spawnPoint1;
    

    public void JoindrePartie()
    {
        InstancierAddresseIP();
        InstancierPort();
        NetworkManager.singleton.StartClient();
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        ClientScene.Ready(conn);
        ClientScene.AddPlayer(0/*(short)NetworkServer.connections.Count*/);
    }
    public void CreateHost1()
    {
        est1v1 = true;
        InstancierAddresseIP();
        InstancierPort();
        NetworkManager.singleton.StartHost();
        CréerÉquipes();
      //  ÉquipeA[0].JoueurPhysique = GameObject.Find()

        //ÉquipeA[]
    }
    public void CreateHost2()
    {
        est1v1 = false;
        InstancierAddresseIP();
        InstancierPort();
        NetworkManager.singleton.StartHost();
        CréerÉquipes();
        //  ÉquipeA[0].JoueurPhysique = GameObject.Find()

        //ÉquipeA[]
    }
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        compteurId++;
        if(compteurA == 0)
        {
            for (int i = 0; i < ÉquipeV2.GRANDEUR; i++)
            {
                NetworkServer.AddPlayerForConnection(conn, ÉquipeAV2.ListeJoueur[i].gameObject, playerControllerId);
            }
        }
        else
        {
            for (int i = 0; i < ÉquipeV2.GRANDEUR; i++)
            {
                NetworkServer.AddPlayerForConnection(conn, ÉquipeBV2.ListeJoueur[i].gameObject, playerControllerId);
            }
        }
        compteurA++;

        
       
        //GameObject joueur = (GameObject)Instantiate(playerPrefab);
        //Debug.Log(compteurId);
        //joueur.transform.name = string.Format("Player ({0})", compteurId);
        //compteurId++;
        //joueur.transform.position = GameObject.Find("SpawnPoint"+ compteurId).transform.position;
        //NetworkServer.AddPlayerForConnection(conn, joueur, playerControllerId);
        //if(compteurA==0)
        //{
        //    ÉquipeA[compteurA++].JoueurPhysique = joueur;
        //}
        //else
        //{
        //    ÉquipeB[compteurB++].JoueurPhysique = joueur;
        //}
    }
    void CréerÉquipes()
    {
        // ÉquipeA = new Équipe('A');
        // ÉquipeB = new Équipe('B');
        if(est1v1)
        {
            ÉquipeAV2 = new ÉquipeV2(CréerÉquipe1v1("A", prefabs));
            ÉquipeBV2 = new ÉquipeV2(CréerÉquipe1v1("B", prefabs));
        }
        else
        {
            ÉquipeAV2 = new ÉquipeV2(CréerÉquipe2v2("A", prefabs));
            ÉquipeBV2 = new ÉquipeV2(CréerÉquipe2v2("B", prefabs));
        }

    }
    static List<JoueurV2> CréerÉquipe2v2(string équipe,List<GameObject> prefab)
    {
        List<JoueurV2> liste = new List<JoueurV2>();
        liste.Add(new Player("Joueur1", équipe,prefab[0]));
        liste.Add(new Player("Joueur2", équipe,prefab[0]));
        liste.Add(new AI("Joueur3", équipe,prefab[1]));
        liste.Add(new AI("Joueur4", équipe,prefab[1]));
        liste.Add(new Gardien("Joueur5", équipe,prefab[2]));
        return liste;
    }
    static List<JoueurV2> CréerÉquipe1v1(string équipe, List<GameObject> prefab)
    {
        List<JoueurV2> liste = new List<JoueurV2>();
        liste.Add(new Player("Joueur1", équipe, prefab[0]));
        liste.Add(new AI("Joueur2", équipe, prefab[1]));
        liste.Add(new AI("Joueur3", équipe, prefab[1]));
        liste.Add(new AI("Joueur4", équipe, prefab[1]));
        liste.Add(new Gardien("Joueur5", équipe, prefab[2]));
        return liste;
    }

    void InstancierAddresseIP()
    {
        string AddresseIP = GameObject.Find("InputIpServeurÀJoindre").GetComponentInChildren<Text>().text;
        NetworkManager.singleton.networkAddress = AddresseIP;
    }

    void InstancierPort()
    {
        NetworkManager.singleton.networkPort = 7777;
    }


    private void OnLevelWasLoaded(int level)
    {
        if (level == 0)
        {
            GérerBoutonsMenu();
        }

        else if(level == 1)
        {
            GérerBoutonsJeu();
        }
    }

    void GérerBoutonsMenu()
    {
        GameObject.Find("BtnHost").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("BtnHost").GetComponent<Button>().onClick.AddListener(() => GérerGrandeurÉquipe());
        GameObject.Find("BtnJoin").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("BtnJoin").GetComponent<Button>().onClick.AddListener(() => JoindrePartie());
    }
    
    void GérerBoutonsJeu()
    {
        GameObject.Find("BtnDisconnect").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("BtnDisconnect").GetComponent<Button>().onClick.AddListener(() => NetworkManager.singleton.StopClient());
    }
    public void GérerGrandeurÉquipe()
    {
        CnvConnexion.enabled = false;
        CnvNbJoueur.enabled = true;

        Btn1v1.onClick.RemoveAllListeners();
        Btn1v1.onClick.AddListener(() => CreateHost1());
        Btn2v2.onClick.RemoveAllListeners();
        Btn2v2.onClick.AddListener(() => CreateHost2());
    }
    private void Start()
    {
        Btn1v1 = GameObject.Find("Btn1v1").GetComponent<Button>();
        Btn2v2 = GameObject.Find("Btn2v2").GetComponent<Button>();
        CnvConnexion = GameObject.Find("CnvConnexion").GetComponent<Canvas>();
        CnvNbJoueur = GameObject.Find("CnvNbJoueur").GetComponent<Canvas>();

        CnvNbJoueur.enabled = false;

        playerPre = Resources.Load<GameObject>("Prefab/Player");
        aIPre = Resources.Load<GameObject>("Prefab/AI");
        gardienPre = Resources.Load<GameObject>("Prefab/gardien");

        prefabs.Add(playerPre);
        prefabs.Add(aIPre);
        prefabs.Add(gardienPre);
    }
}
