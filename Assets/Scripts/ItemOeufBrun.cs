using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class ItemOeufBrun : NetworkBehaviour
{
    void Start()
    {

    }
    void Update()
    {

    }

    public static void FaireEffetItem(GameObject item)
    {
        // OEUF A TETE CHERCHEUSE PRENDRE LANCIEN SCRIPT DE L'AI 
        // ON TRIGGER ENTER, LAUTRE JOUEUR SE FAIR RAPE SOLIDE
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" /* ET QU'IL EST DE L'ÉQUIPE ADVERSE */)
        {
            this.GetComponentInChildren<GestionAudio>().FaireJouerSon(this.GetComponents<AudioSource>().Where(x => x.clip.name.StartsWith("PouKehh")).First());
        }
    }
}
