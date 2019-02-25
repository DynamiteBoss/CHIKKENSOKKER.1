using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class CombinerMeshPlayer : MonoBehaviour
{
    [SerializeField]
    public bool estÉquipeA;
    void Start()
    {
        MeshFilter[] listMesh = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] listCombines = new CombineInstance[listMesh.Length];
        for (int i = 0; i < listMesh.Length; i++)
        {
            listCombines[i].mesh = listMesh[i].sharedMesh;
            listCombines[i].transform = listMesh[i].transform.localToWorldMatrix;
        }
        Mesh meshFinale = new Mesh();
        meshFinale.CombineMeshes(listCombines);
        GetComponent<MeshFilter>().sharedMesh = meshFinale;
        GetComponent<MeshFilter>().sharedMesh.name = "meshCombinée";
    }
    void Update()
    {
        
    }
}
