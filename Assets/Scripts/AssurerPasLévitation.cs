using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssurerPasLévitation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localPosition = new Vector3(transform.localPosition.x,-1f, transform.localPosition.z);
    }
}
