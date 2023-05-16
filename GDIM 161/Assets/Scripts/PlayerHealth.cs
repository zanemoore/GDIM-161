using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private int maxHealth;
    
    private int health;

    private void Start()
    {
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(health);
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.SetHealth(health);

        if (health <= 0)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

}
