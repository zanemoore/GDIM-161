using Photon.Pun;
using Photon.Chat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dart : MonoBehaviourPunCallbacks
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

        hitMarker = GameObject.Find("Player Canvas").GetComponentInChildren<RawImage>();
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

    public void Hit(GameObject zombie)
    {
        if (true) //currently hardcoded to layer 10 = zombie
        {
            ZombieHealth healthscript = zombie.GetComponentInParent<ZombieHealth>();
            Debug.Log(healthscript);
            if (healthscript != null && canDamage)
            {
                Debug.Log("Collision");
                healthscript.GetComponent<PhotonView>().RPC("Damage", RpcTarget.All, damage); //healthscript.Damage(damage);
                //currentHealth = healthscript.getHealth();
                //currentHealth -= damage;
            }
        }
        src.volume = 1.5f;
        src.spatialBlend = 1f;
        src.PlayOneShot(impactToUse);
        Destroy(this.gameObject, 5f); // hardcoded to destroy after 5 seconds
    }

    private void OnTriggerEnter(Collider collision)
    {
        Collider trigger = collision.GetComponent<Collider>();

        if (trigger.gameObject.layer == 10)
        {
            FlashHitMarker();
        }
    }
}
