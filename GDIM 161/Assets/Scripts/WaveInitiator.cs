using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveInitiator : MonoBehaviour
{
    [SerializeField] private GameObject _waveManagerPrefab;
    [SerializeField] private float _minDistanceToInteract;
    [SerializeField] private TextMeshProUGUI _objectiveInstructions;
    [SerializeField] private TextMeshProUGUI _allPlayersInZoneReminder;

    private WaveManager _waveManager;
    private int _numPlayersNeeded;
    List<string> _playersInZone;

    void Start()
    {
        _waveManager = _waveManagerPrefab.GetComponent<WaveManager>();
        _playersInZone = new List<string>();

        Invoke("GetNumberPlayersNeeded", 10f);  // Waiting on this just cause it takes time for people to load into the game
    }


    // I'm using this function with Invoke because if I try to get the number of players in the room in Start, then
    // it will be zero by default. If anyone can help me figure out of a better way I'd appreciate it! - Diego
    private void GetNumberPlayersNeeded()
    {
        _numPlayersNeeded = GameObject.FindGameObjectsWithTag("Player").Length;
        print(string.Format("GetNumberPlayersNeed: {0}", _numPlayersNeeded));
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

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, _minDistanceToInteract))
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
        print(string.Format("IsAllPlayersInZone: {0} {1}", _playersInZone.Count, _numPlayersNeeded == _playersInZone.Count));
        return _numPlayersNeeded == _playersInZone.Count;
    }
}
