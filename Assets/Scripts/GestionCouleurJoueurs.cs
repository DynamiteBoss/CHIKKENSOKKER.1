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
            AppliquerCouleurÉquipe(Color.red);
        }
        else AppliquerCouleurÉquipe(Color.blue);
    }

    void AppliquerCouleurÉquipe(Color couleur)
    {
        transform.GetComponent<MeshRenderer>().material.color = couleur;
    }
}
