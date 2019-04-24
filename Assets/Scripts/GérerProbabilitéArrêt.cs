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
        if(Balle.transform.parent == null)
        {
         
            if (other.gameObject ==  true)
            {
               
                if (other.gameObject == Balle)
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
        
        string numéro;
        string équipe;
        if(Gardien.GetComponent<TypeÉquipe>().estÉquipeA)
        {
            numéro = "2";
            équipe = "A";
        }
        else
        {
            numéro = "1";
            équipe = "B";
        }
        Vector3 distGB = (Gardien.transform.position - Balle.transform.position);
        Vector3 velo = Balle.GetComponent<Rigidbody>().velocity;
        //Debug.Log(Balle.GetComponent<Rigidbody>().velocity);
        Vector3 distBB = ((Balle.transform.position + velo) - Balle.transform.position + velo);
        Ray directionBalle = new Ray(Balle.transform.position, Balle.transform.position + velo);
        float angle = Vector3.Angle(distGB, distBB);
        //Debug.DrawRay(Balle.transform.position, Balle.transform.position + velo);
        

        RaycastHit hit;
        if(Physics.Raycast(directionBalle, out hit))
        {
            if (hit.collider != null && (hit.collider.gameObject == GameObject.Find("But" + numéro) || hit.collider.gameObject == GameObject.Find("Gardien1" + équipe)))
            {
                //Debug.Log("Rentre dans Ray");
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
                //Debug.Log("chance"+chance);

                if(chance == 1)
                {
                    if(numéro == "2")
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
