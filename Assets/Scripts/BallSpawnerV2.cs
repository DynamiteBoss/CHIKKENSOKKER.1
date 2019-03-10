using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BallSpawnerV2 : NetworkBehaviour
{
    public GameObject ballePrefab;

    public override void OnStartServer()
    {
        Vector3 spawnPos = new Vector3(1, 0.5f, 5);
        Quaternion spawnRot = Quaternion.Euler(0, 0, 0);

        GameObject balle = (GameObject)Instantiate(ballePrefab, spawnPos, spawnRot);
        balle.transform.name = "Balle";
        NetworkServer.Spawn(balle);
    }
}
