using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ProjectileLauncher : MonoBehaviour
{
    //Written by Hung Bui
    //[SerializeField] private GameObject projectilePrefab; Now in projectileInfo
    [SerializeField] private Transform handPosition;
    //Perhaps, forward force, vertical force, and shoot time can be put into a scriptable object in the future
    [SerializeField] private ProjectileType projectileInfo;

    private Quaternion rotation;
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
        //string projectileName = projectilePrefab.name; now in projectileInfo

        if (projectileInfo.prefab.name == "Football")
        {
            rotation = transform.rotation * Quaternion.Euler(-70f, -90f, -20f);
        }
        else
        {
            rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);
        }

        var projectile = PhotonNetwork.Instantiate(projectileInfo.prefab.name, handPosition.position, rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(Camera.main.transform.forward * projectileInfo.forwardForce + Camera.main.transform.up * projectileInfo.verticalForce);
        if (Launched != null)
        {
            Launched();
        }
    }
}
