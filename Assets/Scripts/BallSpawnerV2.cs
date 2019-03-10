using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BallSpawnerV2 : NetworkBehaviour
{
    public GameObject ballePrefab;

    public override void OnStartServer()
    {
        Vector3 spawnPos = new Vector3(0, 0.5f, 0);
        Quaternion spawnRot = Quaternion.Euler(0, 0, 0);

        GameObject balle = (GameObject)Instantiate(ballePrefab, spawnPos, spawnRot);
        NetworkServer.Spawn(balle);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
