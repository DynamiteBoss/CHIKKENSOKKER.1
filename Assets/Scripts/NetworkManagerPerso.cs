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
    public Équipe ÉquipeA { get; set; }
    public Équipe ÉquipeB { get; set; }
    short compteurId = 0;
    int compteurA = 0;
    int compteurB = 0;
    string AddresseIP;

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
    public void CreateHost()
    {
        InstancierAddresseIP();
        InstancierPort();
        CréerÉquipes();
        NetworkManager.singleton.StartHost();
      //  ÉquipeA[0].JoueurPhysique = GameObject.Find()

        //ÉquipeA[]
    }
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject joueur = (Instantiate(playerPrefab, new Vector3(0,-1f,0), Quaternion.identity));
        joueur.transform.name = string.Format("Player ({0})", ++compteurId);
        NetworkServer.AddPlayerForConnection(conn, joueur, playerControllerId);
        AfficheurIP ip = new AfficheurIP();
        if(NetworkManager.singleton.networkAddress ==  "localhost")
        {
            ÉquipeA[compteurA++].JoueurPhysique = joueur;
        }
        else
        {
            ÉquipeB[compteurB++].JoueurPhysique = joueur;
        }

    }
    void CréerÉquipes()
    {
         ÉquipeA = new Équipe('A');
         ÉquipeB = new Équipe('B');
    }
    void InstancierAddresseIP()
    {
        AddresseIP = GameObject.Find("InputIpServeurÀJoindre").GetComponentInChildren<Text>().text;
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
        GameObject.Find("BtnHost").GetComponent<Button>().onClick.AddListener(() => CreateHost());
        GameObject.Find("BtnJoin").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("BtnJoin").GetComponent<Button>().onClick.AddListener(() => JoindrePartie());
    }
    
    void GérerBoutonsJeu()
    {
        GameObject.Find("BtnDisconnect").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("BtnDisconnect").GetComponent<Button>().onClick.AddListener(() => NetworkManager.singleton.StopClient());
    }
}
