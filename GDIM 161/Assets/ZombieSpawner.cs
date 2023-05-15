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
    [SerializeField]
    private float spawnRadius;

    private int i;

    void Start()
    {
        string name = zombiePrefab.name;

        for (int i = 0; i < numberOfZombiesToSpawn; i++)
        {
            Vector3 position = Random.insideUnitSphere * spawnRadius;
            position.y = this.transform.position.y;
            position += this.transform.position;
            GameObject gameObject = PhotonNetwork.Instantiate(name, position, Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.up));
        }
    }
}
