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
    private RawImage hitMarker;
    public Action Died; //subscribe functions upon death
    public Action<float> Damaged; //subscribe functions upon taking damage

    [SerializeField] Animator animator;

    private void Awake()
    {
        currentHealth = maxHealth;
        hitMarker = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<RawImage>();
        hitMarker.enabled = false;
    }

    [PunRPC]
    public void Damage(float amount)
    {
        currentHealth -= amount;
        CheckHealth();
        //FlashHitMarker();
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
        }
        return currentHealth;
    }

    /*
    private void FlashHitMarker()
    {
        hitMarker.enabled = true;
        hit = true;
        Invoke("ResetHitMarker", 0.2f);
    }
    private void ResetHitMarker()
    {
        hitMarker.enabled = false;
        hit = false;
    }
    */

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
            Damage(dart.getDamage());
        }
    }
}
