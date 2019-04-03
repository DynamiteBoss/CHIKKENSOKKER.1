using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptSliderBalle : MonoBehaviour
{
    const int NbFramesUpdate = 10;
    int compteur;
    GameObject SldPosBalle { get; set; }
    GameObject Balle { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        SldPosBalle = this.gameObject; //GameObject.Find("SldPosBalle");
        Balle = GameObject.Find("Balle");
    }

    // Update is called once per frame
    void Update()
    {
        if(compteur++ >= NbFramesUpdate)
        {
            SldPosBalle.GetComponentInChildren<Slider>().value = Balle.transform.position.x;
        }
    }
}
