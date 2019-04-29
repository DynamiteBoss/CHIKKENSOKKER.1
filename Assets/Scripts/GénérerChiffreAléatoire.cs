using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GénérerChiffreAléatoire : NetworkBehaviour
{
    [SyncVar(hook = "OnAléatoireChange")]
    public int aléatoire;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnAléatoireChange(int changement)
    {
        aléatoire = changement;
    }
    public void CréerAléatoire()
    {
        if(isServer)
        {
            aléatoire = Random.Range(0, 3);
        }
        
    }
}
