using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    //Written by Hung Bui
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform handPosition;
    //Perhaps, forward force, vertical force, and shoot time can be put into a scriptable object in the future
    [SerializeField] private float forwardForce = 100f;
    [SerializeField] private float verticalForce = 0f;
    [SerializeField] private float timeBetweenLaunches = 0.2f;

    private float timeSinceLastLaunch = 0f;
    public Action Launched;//subscribe functions upon launching a projectile
    void Update()
    {
        timeSinceLastLaunch += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && timeSinceLastLaunch > timeBetweenLaunches)
        {
            Launch();
        }
    }

    void Launch()
    {
        var projectile = Instantiate(projectilePrefab, handPosition.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(Camera.main.transform.forward * forwardForce + Camera.main.transform.up * verticalForce);
        if (Launched != null)
        {
            Launched();
        }
    }
}
