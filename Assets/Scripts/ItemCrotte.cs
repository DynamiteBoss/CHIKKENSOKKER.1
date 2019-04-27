using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemCrotte : MonoBehaviour
{
    void Start()
    {

    }
    void Update()
    {

    }

    public static void FaireEffetItem(GameObject[] listNbCrotte)
    {
        foreach (GameObject crotte in listNbCrotte)
        {

        }
        // DONNER UNE PETITE FORCE VERS L'AVANT DU JOUEUR ET LEUR METTRE UN DRAG SIGNIFICATIF
        // ON TRIGGER ENTER, LAUTRE JOUEUR SE PLANTE OU KEKCHOSE COMME CA
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            this.GetComponentInChildren<GestionAudio>().FaireJouerSon(this.GetComponents<AudioSource>().Where(x => x.clip.name.StartsWith("Caca2")).First()); //Fait jouer le son de caca quand qqun pile dessus
        }
    }
}
