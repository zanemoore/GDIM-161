using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PlayerItem : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text playerName;
    public GameObject leftArrowButton;
    public GameObject rightArrowButton;
    public Outline outlineBackground;

    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
    public TMP_Text characterName;
    public string[] characterNames;
    public Image characterIcon;
    public Sprite[] characterIcons;
    public Sprite[] characterStats;
    private bool statsShown;
    public Image statImg;

    Player player;

    private void Start()
    {
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public void SetPlayerInfo(Player p)
    {
        playerName.text = p.NickName;
        player = p;
        UpdatePlayerItem(player);
    }

    public void ApplyLocalChanges()
    {
        outlineBackground.enabled = true;
        leftArrowButton.SetActive(true);
        rightArrowButton.SetActive(true);
    }

    public void OnClickLeftArrow()
    {
        if ((int)playerProperties["characterName"] == 0)
        {
            playerProperties["characterName"] = characterNames.Length - 1;
        }
        else
        {
            playerProperties["characterName"] = (int)playerProperties["characterName"] - 1;
        }

        if ((int)playerProperties["characterIcon"] == 0)
        {
            playerProperties["characterIcon"] = characterIcons.Length - 1;
        }
        else
        {
            playerProperties["characterIcon"] = (int)playerProperties["characterIcon"] - 1;
        }
        statImg.overrideSprite = null;
        statsShown = false;
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public void OnClickRightArrow()
    {
        if ((int)playerProperties["characterName"] == characterNames.Length - 1)
        {
            playerProperties["characterName"] = 0;
        }
        else
        {
            playerProperties["characterName"] = (int)playerProperties["characterName"] + 1;
        }

        if ((int)playerProperties["characterIcon"] == characterIcons.Length - 1)
        {
            playerProperties["characterIcon"] = 0;
        }
        else
        {
            playerProperties["characterIcon"] = (int)playerProperties["characterIcon"] + 1;
        }
        statImg.overrideSprite = null;
        statsShown = false;
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public void OnClickIcon()
    {
        if (!statsShown)
        {
            statImg.overrideSprite = characterStats[(int)playerProperties["characterIcon"]];
            statsShown = true;
        }
        else
        {
            statImg.overrideSprite = null;
            statsShown = false;
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (player == targetPlayer)
        {
            UpdatePlayerItem(targetPlayer);
        }
    }

    void UpdatePlayerItem(Player player)
    {
        if (player.CustomProperties.ContainsKey("characterName"))
        {
            characterName.text = characterNames[(int)player.CustomProperties["characterName"]];
            playerProperties["characterName"] = (int)player.CustomProperties["characterName"];
        }
        else
        {
            playerProperties["characterName"] = 0;
        }

        if (player.CustomProperties.ContainsKey("characterIcon"))
        {
            characterIcon.sprite = characterIcons[(int)player.CustomProperties["characterIcon"]];
            playerProperties["characterIcon"] = (int)player.CustomProperties["characterIcon"];
        }
        else
        {
            playerProperties["characterIcon"] = 0;
        }
    }
}
