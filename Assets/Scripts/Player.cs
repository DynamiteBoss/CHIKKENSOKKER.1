using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 class Player : JoueurV2
{
    GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
    public Player(string nom,string équipe):base(nom,équipe)
    {

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
