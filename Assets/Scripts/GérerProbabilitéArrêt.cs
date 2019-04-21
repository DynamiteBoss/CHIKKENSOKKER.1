using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GérerProbabilitéArrêt : MonoBehaviour
{
    GameObject Gardien { get; set; }
    GameObject Balle { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        Balle = GameObject.FindGameObjectWithTag("Balle");
        Gardien = this.transform.parent.gameObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.parent.gameObject != Balle)
        {
            
        }
        else
        {
            CalculerProbabilité(other);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void CalculerProbabilité(Collider other)
    {
        float distGB = Vector3.Distance(Gardien.transform.position, Balle.transform.position);
        Vector3 velo = Balle.GetComponent<Rigidbody>().velocity;
        Debug.Log(Balle.GetComponent<Rigidbody>().velocity);
        float distBB = Vector3.Distance(Balle.transform.position, Balle.transform.position + velo);
        Ray a = new Ray(Balle.transform.position, Balle.transform.position + velo);
        

    }
  
}
