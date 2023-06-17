using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using Photon.Realtime;
using Photon.Pun.Demo.Cockpit;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject[] players;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject[] zombieSpawner;

    [SerializeField] private List<GameObject> playerList;
    [SerializeField] private List<GameObject> spectatorCanvas = new List<GameObject>();

    public int numberPlayers;
    public int playersDead;
    public bool notLoaded;
    private bool checkCanvas;
    GameObject localPlayer;

    void Awake()
    {
        for (int i = 0; i < zombieSpawner.Length; i++)
        {
            zombieSpawner[i].SetActive(true);
        }
    }

    void Start()
    {
        checkCanvas = false;
        notLoaded = true;

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
        localPlayer = PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.position, spawnPoint.rotation);
        localPlayer.GetComponent<PlayerSetup>().IsLocalPlayer();
    }

    void Update()
    {
        CheckPlayers();
        CheckPlayersAlive();
        GetSpectatorCanvas();
    }

    void CheckPlayers()
    {
        numberPlayers = PhotonNetwork.CurrentRoom.PlayerCount;
        for (int i = 0; i <= numberPlayers; i++)
        {
            if (numberPlayers > 4)
            {
                numberPlayers -= 4;
            }
        }
    }

    public void CheckPlayersAlive()
    {
        if (playersDead == numberPlayers)
        {
            foreach (GameObject canvas in spectatorCanvas)
            {
                canvas.SetActive(false);
            }

            if (PhotonNetwork.IsMasterClient && notLoaded == true)
            {
                PhotonNetwork.AutomaticallySyncScene = true;
                PhotonNetwork.LoadLevel("Lose Screen");
                notLoaded = false;
            }
        }
    }

    public void GetSpectatorCanvas()
    {
        if (checkCanvas == false)
        {
            playerList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));

            foreach (GameObject p in playerList)
            {
                foreach (Transform child in p.transform)
                {
                    if (child.tag == "Spectator Canvas")
                    {
                        spectatorCanvas.Add(child.gameObject);
                        checkCanvas = true;
                    }
                }
            }
        }
    }
}
