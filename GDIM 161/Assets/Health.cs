using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    //created by Hung Bui
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    public Action Died; //subscribe functions upon death
    public Action<float> Damaged; //subscribe functions upon taking damage

    private void Awake()
    {
        currentHealth = maxHealth;
        Died += () => Destroy(this.gameObject); //currently, destroys the game object this is attached to. Delete this line later.
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Damage(10);
        }
    }
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
        if (currentHealth <= 0) { Died(); }
        return currentHealth;
    }



}
