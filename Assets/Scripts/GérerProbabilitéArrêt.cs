using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GérerProbabilitéArrêt : MonoBehaviour
{
    GameObject Balle { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        Balle = GameObject.FindGameObjectWithTag("Balle");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
  
}
