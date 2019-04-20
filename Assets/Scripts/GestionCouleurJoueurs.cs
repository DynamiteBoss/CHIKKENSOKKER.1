using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionCouleurJoueurs : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (this.transform.parent.GetComponent<TypeÉquipe>().estÉquipeA)
        {
            AppliquerCouleurÉquipe(Color.red);
        }
        else AppliquerCouleurÉquipe(Color.blue);

    }
    void AppliquerCouleurÉquipe(Color couleur)
    {
        GameObject[] skins = transform.GetComponentsInChildren<GameObject>();
        for (int i = 0; i != skins.Length; i++)
        {
            skins[i].GetComponent<Material>().color = couleur;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
