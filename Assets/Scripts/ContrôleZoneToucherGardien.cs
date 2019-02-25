using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContrôleZoneToucherGardien : MonoBehaviour
{
    float compteur = 0;
    GameObject Gardien { get; set; }
    GameObject Balle { get; set; }
    GameObject Joueur { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        Gardien = this.transform.parent.gameObject;
        Balle = GameObject.Find("Balle");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (Balle.transform.parent == null || other.transform.parent == null)
        {

        }
        else
        {
            Joueur = Balle.transform.parent.gameObject;
            if (other.transform.parent.name != Joueur.name)
            { }
            else
            {
                if (Joueur.name == other.transform.parent.name && compteur >= 0.95f)
                {
                    Joueur.GetComponentInChildren<ContrôleBallon>().enabled = false;
                    Balle.transform.parent = null;
                    Joueur.GetComponentInChildren<ContrôleBallon>().enabled = true;
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        compteur += Time.deltaTime;
    }
}
