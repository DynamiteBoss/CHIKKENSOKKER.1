using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ItemBrouillé : NetworkBehaviour
{
    void Start()
    {

    }
    void Update()
    {

    }
    public static void FaireEffetItem(GameObject joueur)  //y faut un joueur qui se fait donner le mode furax
    {
        joueur.GetComponent<MouvementPlayer>().modeFurax = true;
        AttendrePourDésactivation(6, joueur);   // LE DÉSACTIVER APRES UN CERTAIN MOMENT (APRES UNE COROUTINE PEUT-ETRE)
    }
    static IEnumerator AttendrePourDésactivation(float durée, GameObject joueur)
    {
        yield return new WaitForSeconds(durée);
        joueur.GetComponent<MouvementPlayer>().modeFurax = false;
    }
}
