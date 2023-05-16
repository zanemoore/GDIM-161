using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ProjectileLauncher : MonoBehaviour
{
    //Written by Hung Bui
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform handPosition;
    //Perhaps, forward force, vertical force, and shoot time can be put into a scriptable object in the future
    [SerializeField] private ProjectileType projectileInfo;



    private float timeSinceLastLaunch = 0f;
    public Action Launched;//subscribe functions upon launching a projectile
    void Update()
    {
        timeSinceLastLaunch += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && timeSinceLastLaunch > projectileInfo.timeBetweenLaunches)
        {
            Launch();
            timeSinceLastLaunch= 0f;
        }
    }

    void Launch()
    {
        string projectileName = projectilePrefab.name;

        var projectile = PhotonNetwork.Instantiate(projectileName, handPosition.position, Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up));
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(Camera.main.transform.forward * projectileInfo.forwardForce + Camera.main.transform.up * projectileInfo.verticalForce);
        if (Launched != null)
        {
            Launched();
        }
    }
}
