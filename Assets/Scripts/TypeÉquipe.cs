using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TypeÉquipe : NetworkBehaviour
{
    [SyncVar(hook ="OnEstÉquipeAChange")]public bool estÉquipeA;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEstÉquipeAChange(bool changement)
    {
        estÉquipeA = changement;
    }
}
