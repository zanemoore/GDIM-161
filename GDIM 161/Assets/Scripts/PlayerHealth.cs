using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text text;
    [SerializeField] private GameObject bar;

    private Image image;
    public int health;
    public bool isAlive;

    public RoomManager roomManager;
    public CharacterAudio characterAudio;
    public GameObject spectatorCamera;

    private void Start()
    {
        roomManager = GameObject.Find("Room Manager").GetComponent<RoomManager>();
        text.text = slider.value.ToString();
        image = bar.GetComponent<Image>();
        //image.enabled = true;

        health = maxHealth;
        SetMaxHealth(maxHealth);
        SetHealth(health);
    }

    private void Update()
    {
        image.enabled = true;
        text.text = slider.value.ToString();
        ChangeHealthColor();
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        characterAudio.playDmg();
        slider.value = health;
        health -= damage;

        if (health <= 0)
        {
            if (GetComponent<PhotonView>().IsMine == true)
            {
                characterAudio.playDeath();
                spectatorCamera.SetActive(true);
                isAlive = false;
                roomManager.playersDead++;
                //PhotonNetwork.Destroy(gameObject);
            }
        }
        else
        {
            isAlive = true;
        }
    }

    private void SetHealth(int health)
    {
        slider.value = health;
    }

    private void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
    }

    private void ChangeHealthColor()
    {
        if (slider.value <= 25)
        {
            image.color = Color.red;
        }
        else if (slider.value > 50)
        {
            image.color = Color.green;
        }
        else
        {
            image.color = Color.yellow;
        }
    }
}
