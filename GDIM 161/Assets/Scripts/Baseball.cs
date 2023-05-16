using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baseball : MonoBehaviour
{
    //perhaps make this a base class, in future versions
    [SerializeField] private float damage = 10f;
    private bool canDamage = true;
    public AudioSource src;
    public AudioClip impact1, impact2, impact3;
    private AudioClip impactToUse;

    private void Start()
    {
        switch(Random.Range(1, 4))
        {
            case 1:
                impactToUse = impact1;
                break;
            case 2:
                impactToUse = impact2;
                break;
            case 3:
                impactToUse = impact3;
                break;
        }
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
                canDamage= false;
            }

        }
        src.volume = 1f;
        src.spatialBlend = 1f;
        src.PlayOneShot(impactToUse);
        Destroy(this.gameObject, 5f); // hardcoded to destroy after 5 seconds
    }
}
