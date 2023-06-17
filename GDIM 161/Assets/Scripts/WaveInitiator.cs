using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveInitiator : MonoBehaviour
{
    [SerializeField] private GameObject _waveManagerPrefab;
    [SerializeField] private float _minDistanceToInteract;

    private WaveManager _waveManager;
    private int _numPlayersNeeded;
    Dictionary<string, GameObject> _playersInZone;

    void Start()
    {
        _waveManager = _waveManagerPrefab.GetComponent<WaveManager>();
        _playersInZone = new Dictionary<string, GameObject>();
    }


    void Update()
    {
        RaycastHit hit;

        if (Camera.main == null)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, _minDistanceToInteract))
        {
            if (hit.collider.gameObject.tag == "WaveInitiator" && Input.GetKeyDown(KeyCode.F))
            {
                if (IsAllPlayersInZone())
                {
                    Set("WaveInitObjective", false);
                    Set("AllPlayersInZone", false);

                    _waveManager.SetUp();

                    PhotonNetwork.Destroy(this.gameObject);
                }
                else
                {
                    Set("AllPlayersInZone", true);
                }
            }
        }
    }


    // Each time a player enters the zone increase _playersInZone
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject player = other.gameObject;

            if (!_playersInZone.ContainsKey(player.name))
            {
                _playersInZone.Add(player.name, player);
                Set("WaveInitObjective", true, player.name);
            }
        }
    }


    // Each time a player exits the zone decrease _playersInZone
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject player = other.gameObject;

            if (_playersInZone.ContainsKey(player.name))
            {
                Set("WaveInitObjective", false, player.name);
                Set("AllPlayersInZone", false, player.name);
                _playersInZone.Remove(player.name);
            }
        }
    }


    private bool IsAllPlayersInZone()
    {
        return GameObject.FindGameObjectsWithTag("Player").Length == _playersInZone.Count;
    }


    private void Set(string text, bool active, string player = null)
    {
        GameObject canvas;
        GameObject textUI;

        foreach (var p in _playersInZone)
        {
            if (player == null || player == p.Key)
            {
                canvas = _playersInZone[p.Key].transform.Find("Player Canvas").gameObject;
                textUI = canvas.transform.Find(text).gameObject;
                textUI.SetActive(active);
            }
        }
    }
}