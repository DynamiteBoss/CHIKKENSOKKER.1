using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ÉquipeV2 : MonoBehaviour
{
    List<JoueurV2> ListeJoueur { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public ÉquipeV2(List<JoueurV2> liste)
    {
        ListeJoueur = liste;
    }
}
