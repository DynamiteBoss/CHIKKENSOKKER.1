using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangerContrôleGardien : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject b = GameObject.Find("Joueur");
        b.transform.tag = "gardien";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
