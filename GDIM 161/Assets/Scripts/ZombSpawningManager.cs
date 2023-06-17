using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombSpawningManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _spawnersAhead;
    [SerializeField] private List<GameObject> _spawnersBehind;
    [SerializeField] private Transform _behindZombiesDest;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ZombieSpawner spawner;

            foreach (GameObject spawnerPrefabAhead in _spawnersAhead)
            {
                spawner = spawnerPrefabAhead.GetComponent<ZombieSpawner>();
                spawner.Spawn();
            }

            foreach (GameObject spawnerPrefabBehind in _spawnersBehind)
            {
                spawner = spawnerPrefabBehind.GetComponent<ZombieSpawner>();
                spawner.SetZombiesDestination(_behindZombiesDest);
                spawner.Spawn();
            }

            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
