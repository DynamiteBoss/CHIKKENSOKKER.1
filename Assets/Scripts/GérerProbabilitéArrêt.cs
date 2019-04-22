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
        Debug.Log("BALLE");
        Debug.Log(Balle.transform.parent);
        if(Balle.transform.parent == null)
        {
            if(other.transform.parent)
            {
                if(other.transform.parent == Balle)
                {
                    CalculerProbabilité(other);
                }
            }
        }
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    void CalculerProbabilité(Collider other)
    {
        string équipe;
        if(Gardien.GetComponent<TypeÉquipe>().estÉquipeA)
        {
            équipe = "2";
        }
        else
        {
            équipe = "1";
        }
        float distGB = Vector3.Distance(Gardien.transform.position, Balle.transform.position);
        Vector3 velo = Balle.GetComponent<Rigidbody>().velocity;
        Debug.Log(Balle.GetComponent<Rigidbody>().velocity);
        float distBB = Vector3.Distance(Balle.transform.position, Balle.transform.position + velo);
        Ray a = new Ray(Balle.transform.position, Balle.transform.position + velo);
        float angle = Mathf.Sqrt(Mathf.Pow(distGB, 2) + Mathf.Pow(distBB, 2));

        RaycastHit hit;
        if(Physics.Raycast(a,out hit))
        {
            if (hit.collider != null && hit.collider.transform.parent == GameObject.Find("But" + équipe))
            {
                int chance;
                float probabilité = Mathf.Round(angle / 5);
                if(probabilité < 1)
                {
                    chance = 1;
                }
                else
                {
                    chance = (int)Random.Range(0, probabilité);
                }

                if(chance == 1)
                {
                    if(équipe == "2")
                    {
                        Gardien.transform.position = new Vector3(Balle.transform.position.x - 0.5f,Balle.transform.position.y, Balle.transform.position.z);
                    }
                    else
                    {
                        Gardien.transform.position = new Vector3(Balle.transform.position.x + 0.5f, Balle.transform.position.y, Balle.transform.position.z);
                    }
                    
                }

            }
        }
        

    }
  
}
