using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject zombieSpawner;
    [SerializeField] private GameObject zombie;

    int numberPlayers;
    GameObject localPlayer;
    private Health health;

    void Start()
    {
        Debug.Log(message:"Connecting...");
        PhotonNetwork.ConnectUsingSettings();
        health = zombie.GetComponent<Health>();
    }

    void Update()
    {
        CheckPlayers();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log(message:"Connected to Server");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;

        base.OnJoinedLobby();
        PhotonNetwork.JoinOrCreateRoom(roomName: "NA", roomOptions: options, typedLobby: null);
        Debug.Log(message:"Joined a Lobby");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log(message: "Connected to a Room");

        //determine which spawn point to use based on the number of players
        if (numberPlayers == 1)
        {
            localPlayer = PhotonNetwork.Instantiate(player.name, spawnPoints[0].position, spawnPoints[0].rotation, 0);
            numberPlayers = 2;
        }
        else if (numberPlayers == 2)
        {
            localPlayer = PhotonNetwork.Instantiate(player.name, spawnPoints[1].position, spawnPoints[1].rotation, 0);
            numberPlayers = 3;
        }
        else if (numberPlayers == 3)
        {
            localPlayer = PhotonNetwork.Instantiate(player.name, spawnPoints[2].position, spawnPoints[2].rotation, 0);
            numberPlayers = 4;
        }
        else if (numberPlayers == 4)
        {
            localPlayer = PhotonNetwork.Instantiate(player.name, spawnPoints[3].position, spawnPoints[3].rotation, 0);
            numberPlayers = 1;
        }

        localPlayer.GetComponent<PlayerSetup>().IsLocalPlayer();

        zombieSpawner.SetActive(true);
        health.enabled = true;
    }

    void CheckPlayers()
    {
        numberPlayers = PhotonNetwork.CountOfPlayers;
        //if the number of player is heigher than the number of spawnpoint in the game spawn the players in round order
        for (int i = 0; i <= numberPlayers; i++)
        {
            if (numberPlayers > 4)
            {
                numberPlayers -= 4;
            }

        }
    }

}
