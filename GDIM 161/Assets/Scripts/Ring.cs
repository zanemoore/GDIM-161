using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Chat;
using UnityEngine.UI;

public class Ring : MonoBehaviourPunCallbacks
{
    //perhaps make this a base class, in future versions
    [SerializeField] private float damage = 30f;
    [SerializeField] private int bouncesLeft = 4;
    private HashSet<GameObject> alreadyCollided = new HashSet<GameObject>();
    private bool canDamage = true;
    public AudioSource src;
    public AudioClip impact1, impact2, impact3;
    private AudioClip impactToUse;
    private Rigidbody rb;
    [SerializeField] private RawImage hitMarker;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void newImpact()
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
        hitMarker = GameObject.FindWithTag("JenHitMarker").GetComponentInChildren<RawImage>();
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




    public void Hit(GameObject zombie)
    {
        Debug.Log(bouncesLeft);
        if (alreadyCollided.Contains(zombie)){ return; }
        newImpact();
        FlashHitMarker();
        if (true) 
        {
            ZombieHealth healthscript = zombie.GetComponentInParent<ZombieHealth>();
            if (healthscript != null && canDamage)
            {
                healthscript.GetComponent<PhotonView>().RPC("Damage", RpcTarget.All, damage);
                bouncesLeft--;
                alreadyCollided.Add(zombie);
                //if there are still bounces left, bounce to a new target
                if (bouncesLeft > 0)
                {
                    //find a new target. if a target is found, bounce directly to it
                    Transform newTarget = findNewTarget();
                    if (newTarget != null)
                    {
                        this.rb.useGravity = false; 
                        rb.velocity = Vector3.zero;
                        Vector3 forceDirection = (newTarget.position - this.transform.position);
                        forceDirection.Set(forceDirection.x, 0, forceDirection.z);
                        rb.AddForce(forceDirection.normalized * 2000f);
                    }
                    else
                    {
                        this.rb.useGravity = true;
                    }
                }
            }
        }
        src.volume = 1.5f;
        src.spatialBlend = 1f;
        src.PlayOneShot(impactToUse);
        Destroy(this.gameObject, 5f); // hardcoded to destroy after 5 seconds
    }

    private Transform findNewTarget()
    {
        //Selects a new target to bounce toward. Should be the closest zombie. For some reason it isnt exactly that
        Transform target = null;
        Collider[] possibleTargets = Physics.OverlapSphere(this.transform.position, 5);
        float shortest_distance = float.MaxValue;
        foreach(Collider c in possibleTargets)
        {
            if (c.gameObject.layer != 10) {continue; }
            if (!alreadyCollided.Contains(c.gameObject) && (c.transform.position - this.transform.position).magnitude < shortest_distance)
            {
                target = c.transform;
                shortest_distance = (c.transform.position - this.transform.position).magnitude;
            }
        }

        return target;
    }
}

