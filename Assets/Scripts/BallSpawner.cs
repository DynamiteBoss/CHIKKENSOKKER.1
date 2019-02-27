using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BallSpawner : NetworkBehaviour
{
    public GameObject Balle;

    public bool EstCrée = false;
    public override void OnStartServer()
    {
        //OK TAMERE
        ClientScene.RegisterPrefab(Balle);
        ServerSpawnBall();
    }
    public void ServerSpawnBall()
    {
        var balleJeu = (GameObject)Instantiate(Balle, new Vector3(0, 1, 0), Quaternion.identity);
        balleJeu.name = "Balle";
        NetworkServer.Spawn(balleJeu);
        EstCrée = true;

    }
}
