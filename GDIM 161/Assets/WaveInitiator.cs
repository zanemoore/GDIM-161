using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveInitiator : MonoBehaviour
{
    [SerializeField] private GameObject _roomManagerPrefab;
    [SerializeField] private GameObject _waveManagerPrefab;
    [SerializeField] private float _minDistanceToInteract;
    [SerializeField] private TextMeshProUGUI _objectiveInstructions;
    [SerializeField] private TextMeshProUGUI _allPlayersInZoneReminder;

    private WaveManager _waveManager;
    private int _numPlayersNeeded;
    Dictionary<string, GameObject> _playersInZone;

    void Start()
    {
        _waveManager = _waveManagerPrefab.GetComponent<WaveManager>();
        _numPlayersNeeded = _roomManagerPrefab.GetComponent<RoomManager>().numberPlayers;
        _playersInZone =  new Dictionary<string, GameObject>();
    }


    void Update()
    {
        if (_playersInZone.Count > 0)
        {
            _objectiveInstructions.gameObject.SetActive(true);
        }
        else
        {
            _objectiveInstructions.gameObject.SetActive(false);
            _allPlayersInZoneReminder.gameObject.SetActive(false);
        }

        foreach (GameObject player in _playersInZone.Values)
        {
            foreach (Transform tr in player.transform)
            {
                if (tr.tag == "MainCamera")
                {
                    Camera cam = tr.GetComponent<Camera>();
                    RaycastHit hit;

                    if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, _minDistanceToInteract));
                    {
                        if (hit.collider.gameObject.tag == "WaveInitiator" && Input.GetKeyDown(KeyCode.F))
                        {
                            if (IsAllPlayersInZone())
                            {
                                _objectiveInstructions.gameObject.SetActive(false);
                                _allPlayersInZoneReminder.gameObject.SetActive(false);

                                _waveManager.SetUp();

                                Destroy(this);
                            }
                            else
                            {
                                _allPlayersInZoneReminder.gameObject.SetActive(true);
                            }
                        }
                    }
                }
            }
        }
        /*
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, _minDistanceToInteract))
        {
            if (hit.collider.gameObject.tag == "WaveInitiator" && Input.GetKeyDown(KeyCode.F))
            {
                if (hit.collider.gameObject.tag == "WaveInitiator" && Input.GetKeyDown(KeyCode.F))
                {
                    if (IsAllPlayersInZone())
                    {
                        _objectiveInstructions.gameObject.SetActive(false);
                        _allPlayersInZoneReminder.gameObject.SetActive(false);

                        _waveManager.SetUp();

                        Destroy(this);
                    }
                    else
                    {
                        _allPlayersInZoneReminder.gameObject.SetActive(true);
                    }
                }
            }
        }
        */
    }


    // Each time a player enters the zone increase _playersInZone
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject player = other.gameObject;
            _playersInZone.Add(player.name, player);
        }
    }


    // Covers edge case where all players are already spawned in the zone.
    // NOTE: I did not test to see if edge case passes - Diego
    private void OnTriggerStay(Collider other)
    {
        if (!IsAllPlayersInZone() && other.tag == "Player")
        {
            GameObject player = other.gameObject;

            if (!_playersInZone.ContainsKey(player.name))
            {
                _playersInZone.Add(player.name, player);
            }
        }
    }


    // Each time a player exits the zone decrease _playersInZone
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _playersInZone.Remove(other.gameObject.name);
        }
    }


    private bool IsAllPlayersInZone()
    {
        return _numPlayersNeeded == _playersInZone.Count;
    }
}
