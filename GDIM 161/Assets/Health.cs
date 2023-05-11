using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    //created by Hung Bui
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    private bool hit = false;
    private RawImage hitMarker;
    public Action Died; //subscribe functions upon death
    public Action<float> Damaged; //subscribe functions upon taking damage

    private void Awake()
    {
        currentHealth = maxHealth;
        hitMarker = GameObject.FindGameObjectWithTag("HitMarker").GetComponent<RawImage>();
        Died += () => Destroy(this.gameObject); //currently, destroys the game object this is attached to. Delete this line later.
        hitMarker.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Damage(10); (Hung this was making us mald lmaoooo) - DIego
        }
    }
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
            FlashHitMarker();
            ResetHitMarker();
            Died(); 
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
}
