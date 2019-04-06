using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptOeufMystère : MonoBehaviour
{
    [SerializeField]
    GameObject JoueurContact { get; set; }
    int compteur = 0;

    const int IndiceMax = 8;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x + 3.1f*Mathf.Sin(180-(compteur++%360)/5.7f), this.transform.rotation.eulerAngles.y + 1, this.transform.rotation.eulerAngles.z + 2.3f*Mathf.Cos(180 - (compteur++ % 360) / 15.3f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.tag != "Player")
        {
          
        }
        if (other.transform.parent.tag == "Player")
        {
            Destroy(this.transform.gameObject);
            AttribuerObjetJoueur(other.gameObject, UnityEngine.Random.Range(1, IndiceMax));
            GetComponent<ScriptMécaniqueMatch>().nbOeufs -= 1;
        }

    }

    private void AttribuerObjetJoueur(GameObject joueur, int indice)
    {

        //#######################################################
        /*Version Temporaire*/

    }
}
