using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
    //perhaps make this a base class, in future versions
    [SerializeField] private float damage = 30f;
    private bool canDamage = true;
    public AudioSource src;
    public AudioClip impact1;
    private AudioClip impactToUse;

    private void Start()
    {
        impactToUse = impact1;
    }

    public float getDamage()
    {
        return damage;
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.gameObject.layer == 10) //currently hardcoded to layer 10 = zombie
        {
            ZombieHealth healthscript = collision.collider.GetComponentInParent<ZombieHealth>();
            Debug.Log(healthscript);
            if (healthscript != null && canDamage)
            {
                Debug.Log("Collision");
                healthscript.Damage(damage);
            }
        }
        else
        {
            this.GetComponent<Rigidbody>().velocity= Vector3.zero;
            this.GetComponent<Collider>().enabled = false;
        }
        src.volume = 1.5f;
        src.spatialBlend = 1f;
        src.PlayOneShot(impactToUse);
        Destroy(this.gameObject, 5f); // hardcoded to destroy after 5 seconds
    }
}
