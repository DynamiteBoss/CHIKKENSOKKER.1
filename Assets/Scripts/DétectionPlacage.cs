using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DétectionPlacage : NetworkBehaviour
{
    GameObject JoueurÀPlaquer { get; set; }
    GameObject Balle { get; set; }
    public bool estEnPlacage = false;
    // Start is called before the first frame update
    void Awake()
    {

        //JoueurÀPlaquer = this.transform.parent.GetComponent<ActionsPlayerV2>().JoueurÀPlaquer == null ? null : this.transform.parent.GetComponent<ActionsPlayerV2>().JoueurÀPlaquer;
        //Balle = this.transform.parent.GetComponent<ActionsPlayerV2>().Balle;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        //ca marche pas parce que ca appelle ce fonction la quan nimporte quoi touche a la zone de placage
        //faut genre mettre le OnTriggerEnter dans le CmdFairePlacage ou le FrapperAdversaire
        if ((other.transform.parent.tag == "Player" || other.transform.parent.tag == "AI") && other.transform.parent.GetComponent<TypeÉquipe>().estÉquipeA != this.transform.parent.GetComponent<TypeÉquipe>().estÉquipeA && other.transform.parent.gameObject != this.transform.parent.gameObject)
        {
            if (other.transform.parent != this.transform.parent)
                JoueurÀPlaquer = other.transform.parent.gameObject;

            if (JoueurÀPlaquer.transform.Find("Balle"))
            {
                Balle = JoueurÀPlaquer.transform.Find("Balle").gameObject;
            }
            this.transform.parent.GetComponent<ActionsPlayerV2>().JoueurÀPlaquer = JoueurÀPlaquer;
            this.transform.parent.GetComponent<ActionsPlayerV2>().Balle = Balle;

            if (estEnPlacage)
            {
                this.GetComponentInParent<ActionsPlayerV2>().PlaquerJoueur();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player" && other.transform.GetComponent<TypeÉquipe>().estÉquipeA != this.transform.parent.GetComponent<TypeÉquipe>().estÉquipeA && other.transform.parent.gameObject != this.transform.parent.gameObject)
        {
            JoueurÀPlaquer = null;

            if (JoueurÀPlaquer.transform.Find("Balle"))
            {
                Balle = null;
            }
            this.transform.parent.GetComponent<ActionsPlayerV2>().JoueurÀPlaquer = JoueurÀPlaquer; //null
            this.transform.parent.GetComponent<ActionsPlayerV2>().Balle = Balle; //null
        }
    }    
}
