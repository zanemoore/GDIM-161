using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject[] players;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject[] zombieSpawner;

    public int numberPlayers;

    private void Start()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties["characterName"] == null)
        {
            PhotonNetwork.LocalPlayer.CustomProperties["characterName"] = 0;
        }

        if (PhotonNetwork.LocalPlayer.CustomProperties["characterIcon"] == null)
        {
            PhotonNetwork.LocalPlayer.CustomProperties["characterIcon"] = 0;
        }

        int randomNumber = Random.Range(0, spawnPoints.Length);
        spawnPoint = spawnPoints[randomNumber];
        GameObject playerToSpawn = players[(int)PhotonNetwork.LocalPlayer.CustomProperties["characterName"]];
        PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.position, spawnPoint.rotation);


        //determine which spawn point to use based on the number of players
        /*
        if (numberPlayers == 1)
        {
            PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoints[0].position, spawnPoints[0].rotation, 0);
        }
        else if (numberPlayers == 2)
        {
            PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoints[1].position, spawnPoints[1].rotation, 0);
        }
        else if (numberPlayers == 3)
        {
            PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoints[2].position, spawnPoints[2].rotation, 0);
        }
        else if (numberPlayers == 4)
        {
            PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoints[3].position, spawnPoints[3].rotation, 0);
        }
        */

        //for (int i = 0; i < zombieSpawner.Length; i++)
        {
            //zombieSpawner[i].SetActive(true);
        }
    }

    void Update()
    {
        CheckPlayers();
    }

    void CheckPlayers()
    {
        numberPlayers = PhotonNetwork.CountOfPlayers;
        for (int i = 0; i <= numberPlayers; i++)
        {
            if (numberPlayers > 4)
            {
                numberPlayers -= 4;
            }
        }
    }

}
