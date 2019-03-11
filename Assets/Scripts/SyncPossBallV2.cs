using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SyncPossBallV2 : NetworkBehaviour
{
    [SyncVar(hook = "OnMvtChange")] Vector3 position = new Vector3(0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        position = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnMvtChange()
    {
        position = this.transform.localPosition;
    }
}
