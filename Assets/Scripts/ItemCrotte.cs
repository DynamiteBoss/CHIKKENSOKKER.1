using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        }
    }
}
