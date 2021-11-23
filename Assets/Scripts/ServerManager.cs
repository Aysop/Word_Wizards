using UnityEngine;
using Photon.Pun;

public class ServerManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public GameObject playerPrefab2;
    public GameObject popUp;


    public float X;
    public float Y;


    public override void OnJoinedRoom()
    {
        //print("entered Joined Room function" + PhotonNetwork.LocalPlayer.nickname);

        
        if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
        {
            Vector2 position = new Vector2(X, Y);
            PhotonNetwork.Instantiate(playerPrefab.name, position, Quaternion.identity);
        }
        else
        {
            X = 525;
            Y = -54;
            Vector2 position = new Vector2(X, Y);
            PhotonNetwork.Instantiate(playerPrefab2.name, position, Quaternion.identity);
        }

    }

}
