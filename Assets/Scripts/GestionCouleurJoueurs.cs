using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GestionCouleurJoueurs : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        if (this.transform.parent.transform.parent.GetComponent<TypeÉquipe>().estÉquipeA)
        {
            AppliquerCouleurÉquipe(new Color(0.85f, 0.6f, 0.6f, 1));
        }
        else AppliquerCouleurÉquipe(new Color(0.6f, 0.6f, 0.85f, 1));
    }

    void AppliquerCouleurÉquipe(Color couleur)
    {
        transform.GetComponent<MeshRenderer>().material.color = couleur;
    }
}
