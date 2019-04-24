using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GérerProbabilitéArrêt : NetworkBehaviour
{
    [SyncVar(hook = "OnChanceChange")]
    public float chance;

    string numéro;
    string équipe;
    int compteur;

    GameObject Gardien { get; set; }
    GameObject Balle { get; set; }
    bool déplacerGardien = false;
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
    void OnChanceChange(float changement)
    {
        chance = changement;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (déplacerGardien)
        {
            if (numéro == "2")
            {
                Gardien.transform.position += (new Vector3(Balle.transform.position.x - 0.5f, Gardien.transform.position.y, Balle.transform.position.z) - Gardien.transform.position).normalized * 0.5f;
            }
            else
            {
                Gardien.transform.position += (new Vector3(Balle.transform.position.x + 0.5f, Gardien.transform.position.y, Balle.transform.position.z) - Gardien.transform.position).normalized * 0.5f;
            }
            if ((Gardien.transform.position - Balle.transform.position).magnitude < 1 || Gardien.transform.Find("Balle"))
                déplacerGardien = false;
        }
        */
    }
    void CalculerProbabilité(Collider other)
    {
       
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
        //Ray directionBalle = new Ray(Balle.transform.position, Balle.transform.position + velo);
        //Debug.DrawRay(Balle.transform.position, Balle.transform.position + velo * 3, Color.red, 1);
        float angle = Vector3.Angle(distGB, distBB);
        //Debug.DrawRay(Balle.transform.position, Balle.transform.position + velo);
        float probabilité;
        Debug.Log(angle);
        if (angle >= 29f)
        {
            probabilité = 0.95f;
        }
        else
        {
            probabilité = angle / 30f;
        }
        chance = Random.Range(0f, 1f);
        Debug.Log(chance);
        Debug.Log(probabilité);
        //Debug.Log("chance"+chance);
        if(probabilité >= chance)
        {
            Gardien.GetComponent<ContrôleGardien>().enabled = false;

            if (numéro == "2")
            {
                Gardien.transform.position = new Vector3(Balle.transform.position.x - 0.5f, Balle.transform.position.y, Balle.transform.position.z);
            }
            else
            {
                Gardien.transform.position = new Vector3(Balle.transform.position.x + 0.5f, Balle.transform.position.y, Balle.transform.position.z);
            }
        }
        else
        {

        }
        //déplacerGardien = chance == 1;
        /*if (chance == 0)
        {
            Gardien.GetComponent<ContrôleGardien>().enabled = false;
            if (numéro == "2")
            {
                déplacerGardien = true;
            }
            else
            {
                Gardien.transform.position = new Vector3(Balle.transform.position.x + 0.5f, Balle.transform.position.y, Balle.transform.position.z);
            }

        }*/

        //RaycastHit hit;
        /*if(Physics.Raycast(directionBalle, out hit))
        {
            if (hit.collider != null && (hit.collider.gameObject == GameObject.Find("But" + numéro) || hit.collider.gameObject == GameObject.Find("Gardien1" + équipe) || hit.collider.transform.parent == GameObject.Find("Gardien1" + équipe)))
            {
                transform.position = Vector3.zero;
                /*
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
        }*/


    }
  
}
