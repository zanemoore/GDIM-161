using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baseball : MonoBehaviour
{
    //perhaps make thiss the base class
    [SerializeField] private float damage = 10f;
    private bool canDamage = true;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == 10) //currently hardcoded to layer 10 = zombie
        {
            Health healthscript = collision.collider.GetComponent<Health>();
            if (healthscript != null && canDamage)
            {
                healthscript.Damage(damage);
                canDamage= false;
            }

        }
        Destroy(this.gameObject, 5f); // hardcoded to destroy after 5 seconds
    }
}
