﻿using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BallSpawner : NetworkBehaviour
{
    
    public GameObject Balle;
    

    public bool EstCrée = false;
    public override void OnStartServer()
    {
        var balleJeu = (GameObject)Instantiate(Balle, new Vector3(0, 1, 0), Quaternion.identity);
        balleJeu.name = "Balle";
        NetworkServer.Spawn(Balle);
        Balle.name = "Balle";
        EstCrée = true;
    }

    //[Command]
    //void CmdSpawn(GameObject objetÀSpawn)
    //{
    //    //NetworkAnimator.Instantiate(objetÀSpawn);
    //    RpcSpawn(objetÀSpawn);
    //}

    //[ClientRpc]
    //void RpcSpawn(GameObject objetÀSpawn)
    //{
    //  //  NetworkAnimator.Instantiate(objetÀSpawn);
    //}
}
