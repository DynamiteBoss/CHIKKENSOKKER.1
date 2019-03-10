using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SyncPosBalle : NetworkBehaviour
{
    float varLerp = 5;
    private Transform posBalle;
    [SyncVar] private Vector3 syncPosBalle;
    private NetworkIdentity id;
    private Vector3 lastPos;
    private float seuilMax = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        posBalle = GetComponent<Transform>();
        syncPosBalle = GetComponent<Transform>().position;
    }
    private void FixedUpdate()
    {
        TransmettrePosition();
        ModifierLerp();
    }

    void ModifierLerp()
    {
        if (!hasAuthority)
        {
            posBalle.position = Vector3.Lerp(posBalle.position, syncPosBalle, Time.deltaTime * varLerp);
        }
    }

    [Command]
    void CmdUpdatePositionAuServeur(Vector3 pos)
    {
        syncPosBalle = pos;
    }

    [ClientCallback]
    void TransmettrePosition()
    {
        if (hasAuthority && Vector3.Distance(posBalle.position, lastPos) > seuilMax)
        {
            CmdUpdatePositionAuServeur(posBalle.position);
            lastPos = posBalle.position;
        }
    }
}
