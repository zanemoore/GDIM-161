using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class RoomItem : MonoBehaviour
{
    [SerializeField] private TMP_Text roomName;

    LobbyManager manager;

    private void Start()
    {
        manager = FindObjectOfType<LobbyManager>();
    }

    public void SetRoomName(string room)
    {
        roomName.text = room;
    }

    public void OnClickItem()
    {
        manager.JoinRoom(roomName.text);
    }
}
