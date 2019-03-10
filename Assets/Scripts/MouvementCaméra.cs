using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MouvementCaméra : NetworkBehaviour
{
    bool estCrée = false;
    bool vérificationBalle = true;
    Rigidbody ballon { get; set; }
    const float dimensions = 30;
    void Update()
    {
        if (vérificationBalle)
        {
            estCrée = GameObject.Find("BallSpawnerV2");
            if(GameObject.FindGameObjectWithTag("Balle") == true)
            {
                estCrée = true;
            }
            if (estCrée)
            {
                vérificationBalle = false;
                ballon = GameObject.FindGameObjectWithTag("Balle").GetComponent<Rigidbody>();
            }
        }
        if (estCrée)
        {
            if (ballon.transform.position.x > dimensions || ballon.transform.position.x < -dimensions)
            {
                transform.position = transform.position;
            }
            else
            {
                transform.position = new Vector3(ballon.transform.position.x, transform.position.y, transform.position.z);
            }
        }
    }
}
