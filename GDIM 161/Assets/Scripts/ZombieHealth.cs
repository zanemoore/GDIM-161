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
    [SerializeField] private float maxHealth = 100f;
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
        hitMarker = GameObject.FindGameObjectWithTag("HitMarker").GetComponent<RawImage>();
        hitMarker.enabled = false;
        ///Died += () => PhotonNetwork.Destroy(this.gameObject); //currently, destroys the game object this is attached to. Delete this line later.
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Damage(10); (Hung this was making us mald lmaoooo) - DIego
        }
    }

    [PunRPC]
    public void Damage(float amount)
    {
        currentHealth -= amount;
        FlashHitMarker();
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
            ResetHitMarker();
            animator.SetBool("Death", true);
            StartCoroutine("Despawn");
        }
        return currentHealth;
    }

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

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(despawnTime);
        PhotonNetwork.Destroy(this.gameObject);
    }
}
