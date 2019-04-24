using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GestionLimiteTerrain : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x > 42|| transform.position.x <-42 ||
           transform.position.z > 20 || transform.position.z < -20)
        {
            //transform.position = new Vector3(0, -1, 0);
        }
    }
}
