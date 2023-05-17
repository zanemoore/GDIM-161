using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private int maxHealth;
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text text;
    [SerializeField] private GameObject bar;
    private Image image;

    private int health;

    private void Start()
    {
        text.text = slider.value.ToString();
        image = bar.GetComponent<Image>();
        //image.enabled = true;

        health = maxHealth;
        SetMaxHealth(maxHealth);
        SetHealth(health);
    }

    void Update()
    {
        image.enabled = true;
        text.text = slider.value.ToString();
        ChangeHealthColor();
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        health -= damage;
        SetHealth(health);

        if (health <= 0)
        {
            PhotonNetwork.Destroy(gameObject);
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
