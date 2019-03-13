using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;



public class AfficheurIP : MonoBehaviour
{
    bool EstVisible;
    public string IPLocal = "";

    // Ce code a été pris sur un forum de UNITY, voici le lien : 
    //https://answers.unity.com/questions/1004969/local-ip-adress-in-unet.html 
    void Start()
    {
        EstVisible = true;
        IPHostEntry hôte;
        hôte = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in hôte.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                IPLocal = ip.ToString();
                break;
            }
        }
        transform.GetComponent<Text>().text = IPLocal;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            transform.GetComponent<Text>().enabled = !EstVisible;
            EstVisible = !EstVisible;
        }
    }
}
