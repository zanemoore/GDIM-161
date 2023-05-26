using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class Baseball : MonoBehaviourPunCallbacks
{
    //perhaps make this a base class, in future versions
    [SerializeField] private float damage = 10f;
    [SerializeField] float currentHealth;
    private bool canDamage = true;
    private bool hit = false;
    public AudioSource src;
    public AudioClip impact1, impact2, impact3;
    private AudioClip impactToUse;
    [SerializeField] private RawImage hitMarker;

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

        hitMarker = GameObject.Find("Player Canvas").GetComponentInChildren<RawImage>();
        hitMarker.enabled = false;
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
                canDamage = false;
                //currentHealth = healthscript.getHealth();
                //currentHealth -= damage;
                FlashHitMarker();
            }
        }
        src.volume = 1f;
        src.spatialBlend = 1f;
        src.PlayOneShot(impactToUse);
        Destroy(this.gameObject, 5f); // hardcoded to destroy after 5 seconds
    }
}
