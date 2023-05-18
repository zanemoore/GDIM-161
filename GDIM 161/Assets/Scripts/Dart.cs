using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dart : MonoBehaviour
{
    //perhaps make this a base class, in future versions
    [SerializeField] private float damage = 10f;
    [SerializeField] float currentHealth;
    private bool canDamage = true;
    private bool hit = false;
    public AudioSource src;
    public AudioClip impact1;
    private AudioClip impactToUse;
    [SerializeField] private RawImage hitMarker;

    private void Start()
    {
        impactToUse = impact1;

        hitMarker = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<RawImage>();
        hitMarker.enabled = false;
    }

    public float getDamage()
    {
        return damage;
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

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.gameObject.layer == 10) //currently hardcoded to layer 10 = zombie
        {
            ZombieHealth healthscript = collision.collider.GetComponentInParent<ZombieHealth>();
            Debug.Log(healthscript);
            if (healthscript != null && canDamage)
            {
                Debug.Log("Collision");
                healthscript.GetComponent<PhotonView>().RPC("Damage", RpcTarget.All, damage); //healthscript.Damage(damage);
                //currentHealth = healthscript.getHealth();
                //currentHealth -= damage;
                FlashHitMarker();
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
