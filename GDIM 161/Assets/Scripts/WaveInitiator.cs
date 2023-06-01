using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// DO NOT LOOK AT THIS CODE DO NOT LOOK AT THIS CODE DO NOT LOOK AT THIS CODE 
// DO NOT - DIEGO
public class WaveInitiator : MonoBehaviour
{
    [SerializeField] private GameObject _waveManagerPrefab;
    [SerializeField] private float _minDistanceToInteract;

    private WaveManager _waveManager;
    private int _numPlayersNeeded;
    List<string> _playersInZone;

    void Start()
    {
        _waveManager = _waveManagerPrefab.GetComponent<WaveManager>();
        _playersInZone = new List<string>();
    }


    void Update()
    {
        if (_playersInZone.Count > 0)
        {
            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                foreach (Transform child in player.transform)
                {
                    if (child.gameObject.name == "Player Canvas")
                    {
                        GameObject canvas = child.gameObject;

                        foreach (Transform canvasChild in canvas.transform)
                        {
                            if (canvasChild.gameObject.name == "WaveInitiatorObjectiveInstructions")
                            {
                                canvasChild.gameObject.SetActive(true);
                            }
                        }

                    }
                }
            }
        }
        else
        {
            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                foreach (Transform child in player.transform)
                {
                    if (child.gameObject.name == "Player Canvas")
                    {
                        GameObject canvas = child.gameObject;

                        foreach (Transform canvasChild in canvas.transform)
                        {
                            if (canvasChild.gameObject.name == "WaveInitiatorObjectiveInstructions")
                            {
                                canvasChild.gameObject.SetActive(false);
                            }
                            if (canvasChild.gameObject.name == "AllPlayersInZoneReminder")
                            {
                                canvasChild.gameObject.SetActive(false);
                            }
                        }

                    }
                }
            }
        }

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            Camera cam = player.GetComponent<Camera>();

            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, _minDistanceToInteract))
            {
                if (hit.collider.gameObject.tag == "WaveInitiator" && Input.GetKeyDown(KeyCode.F))
                {
                    if (IsAllPlayersInZone())
                    {
                        foreach (GameObject player1 in GameObject.FindGameObjectsWithTag("Player"))
                        {
                            foreach (Transform child in player1.transform)
                            {
                                if (child.gameObject.name == "Player Canvas")
                                {
                                    GameObject canvas = child.gameObject;

                                    foreach (Transform canvasChild in canvas.transform)
                                    {
                                        if (canvasChild.gameObject.name == "WaveInitiatorObjectiveInstructions")
                                        {
                                            canvasChild.gameObject.SetActive(false);
                                        }
                                        if (canvasChild.gameObject.name == "AllPlayersInZoneReminder")
                                        {
                                            canvasChild.gameObject.SetActive(false);
                                        }
                                    }

                                }
                            }
                        }

                        _waveManager.SetUp();

                        Destroy(this);
                    }
                    else
                    {
                        foreach (GameObject player1 in GameObject.FindGameObjectsWithTag("Player"))
                        {
                            foreach (Transform child in player1.transform)
                            {
                                if (child.gameObject.name == "Player Canvas")
                                {
                                    GameObject canvas = child.gameObject;

                                    foreach (Transform canvasChild in canvas.transform)
                                    {
                                        if (canvasChild.gameObject.name == "AllPlayersInZoneReminder")
                                        {
                                            canvasChild.gameObject.SetActive(true);
                                        }
                                    }

                                }
                            }
                        }
                    }
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

            if (!_playersInZone.Contains(player.name))
            {
                _playersInZone.Add(player.name);
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
        return GameObject.FindGameObjectsWithTag("Player").Length == _playersInZone.Count;
    }
}
