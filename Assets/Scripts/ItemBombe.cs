using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemBombe : MonoBehaviour
{
    GameObject zoneExplosion { get; set; }
    private bool explosée;
    private bool changementGrandeurZone;
    float scaleMax;
    float scale = 0.5f;
    float tempsExplosion = 30; //le temps ou le scale augmente, durant les premieres frames (30)
    float tempsFade = 127.5f;
    float transparence = 254;
    float transparenceMin = 0;
    float transparenceMax = 255;

    void Start()
    {
        changementGrandeurZone = false;
    }
    void Update()
    {
        if (changementGrandeurZone)
        {
            if(scale < scaleMax)
            {
                zoneExplosion.transform.localScale = zoneExplosion.transform.localScale + new Vector3(scaleMax / tempsExplosion, scaleMax / tempsExplosion, scaleMax / tempsExplosion);
                scale = zoneExplosion.transform.localScale.x;
            }
            else
            {
                //CA VA PA DANS CE CODE LA (POUR LA TRANSPAREMNCE) DONC CA NE DESTROY PA
                if (transparence > transparenceMin)
                {
                    transform.GetComponentInChildren<Material>().color = new Color(transform.GetComponentInChildren<Material>().color.r, transform.GetComponentInChildren<Material>().color.g, transform.GetComponentInChildren<Material>().color.b, (transparence - (transparenceMax / tempsFade)));
                    transparence = transform.GetComponentInChildren<Material>().color.a;
                }
                else
                {
                    changementGrandeurZone = false;
                    Destroy(zoneExplosion);
                    Destroy(this.transform.gameObject);
                }
            }
        }
    }

    public static void FaireEffetItem(GameObject item)
    {
        // OEUF BOMBE QUI FAIT UN ARC DE CERCLE AVANT DEXPLOSER A TERRE 
        // ON TRIGGER ENTER, LAUTRE JOUEUR SE FAIR RAPE SOLIDE
    }
    public void OnTriggerEnter(Collider other)
    {
        if (this.GetComponentInChildren<GestionAudio>())
            this.GetComponentInChildren<GestionAudio>().FaireJouerSon(this.GetComponents<AudioSource>().Where(x => x.clip.name.StartsWith("Explosion")).First()); //Fait jouer le son explosion

        if (other.transform.tag == "Player" && !explosée)  //si a touche un player AVANT de toucher a terre apres/avant son arc de cercle, ELLE EXPLOSE TOUT DE SUITE
        {
            Explose(12);


        }
        if (!(other.transform.tag == "Player") && !explosée) //si a touche a terre ou a un mur
        {
            Explose(15);
        }
    }
    private void Explose(float scale) 
    {
        zoneExplosion = Instantiate(Item.RetournerItemListe(8).ItemPhysique, this.transform.position, Quaternion.identity);
        changementGrandeurZone = true;
        scaleMax = scale;
        this.transform.GetComponentInChildren<MeshRenderer>().enabled = false;

        // créer la zone d'explosion avec le rayon donné
        // agrandir la zone d'explosion avec le temps
    }
}
