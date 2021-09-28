using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public InputField createInput;
    public InputField joinInput;




    public void CreateOrJoinRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = false;
        roomOptions.MaxPlayers = 2;
        if (createInput.text.Length > 0)
        {

            PhotonNetwork.JoinOrCreateRoom(createInput.text, roomOptions, TypedLobby.Default);
        }
        else if (joinInput.text.Length > 0)
        {
            PhotonNetwork.JoinOrCreateRoom(joinInput.text, roomOptions, TypedLobby.Default);
        }
        PhotonNetwork.LoadLevel("Game");
    }



}
