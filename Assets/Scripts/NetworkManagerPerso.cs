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
    int cpt = 0;
    public GameObject playerPre { get; set; }
    public GameObject aIPre { get; set; }
    public GameObject gardienPre { get; set; }
    public List<GameObject> prefabs = new List<GameObject>();
    Button Btn1v1 { get; set; }
    Button Btn2v2 { get; set; }
    Canvas CnvConnexion { get; set; }
    Canvas CnvNbJoueur { get; set; }

    public bool est1v1 = false;

 


    public ÉquipeV2 ÉquipeAV2 { get; set; }
    public ÉquipeV2 ÉquipeBV2 { get; set; }

    public int compteurId = 0;

    public int compteurA = 0;

    public int compteurB = 0;


    public void JoindrePartie()
    {
       
        InstancierAddresseIP();
        InstancierPort();
        NetworkManager.singleton.StartClient();
    }
    public override void OnClientConnect(NetworkConnection conn)
    {
        CréerÉquipe();
        ClientScene.Ready(conn);
        for(int i = 0; i < ÉquipeV2.GRANDEUR;i++)
        {
            ClientScene.AddPlayer((short)i/*(short)NetworkServer.connections.Count*/);
        }
        
        compteurA++;
    }
    public void CreateHost(bool estSeul)
    {
        compteurB = 0;
       
        est1v1 = estSeul;
        InstancierAddresseIP();
        InstancierPort();
        NetworkServer.Reset();
        NetworkManager.singleton.StartHost();
    }
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {


       


        JoueurV2 joueur = ÉquipeAV2.ListeJoueur[playerControllerId];
        GameObject prefab = (GameObject)Instantiate(joueur.Prefab);
        prefab.transform.position = GameObject.Find("SpawnPoint" + compteurB).transform.position + Vector3.up;
        compteurB++;
        string message;
        if (compteurB < 6)
        {
            prefab.GetComponent<TypeÉquipe>().estÉquipeA = true;
            prefab.GetComponent<NetworkIdentity>().AssignClientAuthority(conn);
            message = joueur.NomJoueur + "A";
        }
        else
        {
            prefab.GetComponent<TypeÉquipe>().estÉquipeA = false;
            prefab.GetComponent<NetworkIdentity>().AssignClientAuthority(conn);
            message = joueur.NomJoueur + "B";
        }

        NetworkServer.AddPlayerForConnection(conn, prefab, playerControllerId);
        prefab.name = message;
            //NetworkServer.Spawn(prefab);

        

        
        

        
    }
  


    void AjouterJoueur(NetworkConnection conn, GameObject joueur, short id)
    {
        GameObject Joueur = (GameObject)Instantiate(joueur);
        Joueur.transform.name = string.Format("Player ({0})", compteurId);

        compteurId++;
        Joueur.transform.position = GameObject.Find("SpawnPoint" + compteurId).transform.position;
        NetworkServer.AddPlayerForConnection(conn, Joueur, id);
    }


    void CréerÉquipe()
    {
      
        if(est1v1)
        {
            if(compteurA == 0)
            {
                ÉquipeAV2 = new ÉquipeV2(CréerÉquipe1v1("A", prefabs));
            }
            else
            {
                ÉquipeBV2 = new ÉquipeV2(CréerÉquipe1v1("B", prefabs));
            }
            
        }
        else
        {
            if (compteurA == 0)
            {
                ÉquipeAV2 = new ÉquipeV2(CréerÉquipe2v2("A", prefabs));
            }
            else
            {
                ÉquipeBV2 = new ÉquipeV2(CréerÉquipe2v2("B", prefabs));
            }
        }

    }
    static List<JoueurV2> CréerÉquipe2v2(string équipe,List<GameObject> prefab)
    {
        List<JoueurV2> liste = new List<JoueurV2>();
        liste.Add(new Player("Joueur1", équipe,prefab[0]));
        liste.Add(new Player("Joueur2", équipe,prefab[0]));
        liste.Add(new AI("AI1", équipe,prefab[1]));
        liste.Add(new AI("AI2", équipe,prefab[1]));
        liste.Add(new Gardien("Gardien1", équipe,prefab[2]));
        return liste;
    }
    static List<JoueurV2> CréerÉquipe1v1(string équipe, List<GameObject> prefab)
    {
        List<JoueurV2> liste = new List<JoueurV2>();
        liste.Add(new Player("Joueur1", équipe, prefab[0]));
        liste.Add(new AI("AI1", équipe, prefab[1]));
        liste.Add(new AI("AI2", équipe, prefab[1]));
        liste.Add(new AI("AI3", équipe, prefab[1]));
        liste.Add(new Gardien("Gardien1", équipe, prefab[2]));
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
            if(cpt != 0)
            {
                InstancierRéférences();
            }
        }

        else if(level == 1)
        {
            cpt = 1;
            GérerBoutonsJeu();
        }
    }

    void GérerBoutonsMenu()
    {
        GameObject.Find("BtnHost").GetComponent<Button>().onClick.AddListener(() => GérerGrandeurÉquipe());
       
        GameObject.Find("BtnJoin").GetComponent<Button>().onClick.AddListener(() => JoindrePartie());
        

    }

    public void RevenirMenu()
    {
        CnvConnexion.enabled = true;
        CnvNbJoueur.enabled = false;
       
    }
    void GérerBoutonsJeu()
    {
       
        GameObject.Find("BtnDisconnect").GetComponent<Button>().onClick.AddListener(() => Quitter());
    }

    public void Quitter()
    {
        NetworkManager.singleton.StopHost();
        NetworkManager.singleton.StopServer();
    }
    public override void OnStopHost()
    {

    }

    public void GérerGrandeurÉquipe()
    {
        CnvConnexion.enabled = false;
        CnvNbJoueur.enabled = true;
        GameObject.Find("BtnRetour").GetComponent<Button>().onClick.AddListener(() => RevenirMenu());
       
        Btn1v1.onClick.AddListener(() => CreateHost(true));
   
        Btn2v2.onClick.AddListener(() => CreateHost(false));
    }
    private void Start()
    {
        InstancierRéférences();
    }
    void InstancierRéférences()
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

