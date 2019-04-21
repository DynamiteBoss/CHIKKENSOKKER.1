using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ScriptGestionZones : MonoBehaviour
{
    List<GameObject> Zones { get; set; }
    GameObject Balle { get; set; }
    [SerializeField]
    int iDernièreZoneQuittée = -1;
    [SerializeField]
    int iAvantDernièreZoneQuittée = -2;

    string[] TerrainActif = { "T1", "T2", "T3", "T4", "T5" };

    bool BalleEntrée = false;

    // Start is called before the first frame update
    void Start()
    {
        Zones = this.GetComponents<GameObject>().ToList();
        Zones = Zones.OrderBy(x => x.name).ToList();
        Balle = GameObject.Find("Balle");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == Balle && !BalleEntrée)
            BalleEntrée = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == Balle && BalleEntrée)
        {
            BalleEntrée = false;
            iAvantDernièreZoneQuittée = iDernièreZoneQuittée;
            iDernièreZoneQuittée = Zones.IndexOf(Zones.Find(x => x == other /*!= null ? other : */));
            //iDernièreZoneQuittée
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
