using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.AI;

public class ZombieHealth : MonoBehaviour
{
    //created by Hung Bui
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private float despawnTime;
    private bool hit = false;
    public Action Died; //subscribe functions upon death
    public Action<float> Damaged; //subscribe functions upon taking damage

    [SerializeField] Animator animator;

    private void Awake()
    {
        currentHealth = maxHealth;
        Debug.Log(WaveManager.Instance);
        Died += WaveManager.Instance.ZombieDied;
    }

    [PunRPC]
    public void Damage(float amount)
    {
        currentHealth -= amount;
        CheckHealth();
        if (Damaged != null)
        {
            Damaged(amount);
        }
    }
    
    public void Heal(float amount)
    {
        currentHealth += amount;
        if (amount > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public float getHealth()
    {
        return currentHealth;
    }

    private float CheckHealth()
    {
        if (currentHealth <= 0) {
            animator.SetBool("Death", true);
            StartCoroutine("Despawn");
            Died(); // Hope I'm using this right, trying to connect it to WaveManager lol - Diego
        }
        return currentHealth;
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(despawnTime);
        if (GetComponent<PhotonView>().IsMine == true)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        Dart dart = collision.GetComponent<Dart>();
        if (dart != null)
        {
            dart.Hit(this.gameObject);
            //Damage(dart.getDamage());
        }

        Ring ring = collision.GetComponent<Ring>();
        if (ring != null)
        {
            ring.Hit(this.gameObject);
        }
    }
}
