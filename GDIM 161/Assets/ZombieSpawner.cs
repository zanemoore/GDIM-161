using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject zombiePrefab;
    [SerializeField]
    private int numberOfZombiesToSpawn;

    private int i;

    void Start()
    {
        i = 0;
    }


    void Update()
    {
        string name = zombiePrefab.name;

        if (i < numberOfZombiesToSpawn)
        {
            GameObject gameObject = PhotonNetwork.Instantiate(name, this.transform.position, Quaternion.identity);
            i++;
        }
    }
}
