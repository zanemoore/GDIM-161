using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private int numberOfZombiesToSpawn;
    [SerializeField] private float spawnRadius;

    void Start()
    {
        Spawn();
    }


    public void Spawn()
    {
        string name = zombiePrefab.name;

        for (int i = 0; i < numberOfZombiesToSpawn; i++)
        {
            Vector3 randZombiePos = Random.insideUnitSphere * spawnRadius;
            randZombiePos.y = this.transform.position.y;
            randZombiePos += this.transform.position;

            Quaternion randZombieRotation = Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.up);

            GameObject gameObject = PhotonNetwork.Instantiate(name, randZombiePos, randZombieRotation);
        }
    }
}
