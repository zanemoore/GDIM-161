using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MenuManager : MonoBehaviour
{
    public void LoadMainMenu()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Main Menu");
    }
}
