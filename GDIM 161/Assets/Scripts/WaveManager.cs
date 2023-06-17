using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance = null;

    [Header("Game Objects")]
    [SerializeField] private GameObject _entrancePrefab;
    [SerializeField] private float _closedEntranceHeight;
    [SerializeField] private GameObject _exitPrefab;
    [SerializeField] private float _openedExitHeight;

    [Header("Wave Spawners")]
    [SerializeField] private List<GameObject> _waveZombieSpawners;
    [SerializeField] private float _totalWaveTime;
    [SerializeField] private int _spawnX;
    [SerializeField] private float _perYtime;
    [SerializeField] private Transform _waveZombiesDestination;

    [Header("UI")]
    [SerializeField] private float _timeOnScreen;

    private bool _sendWave;
    private float _startTime;
    private float _timeSinceLastWave;
    private int _numZombiesSpawned;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }


    void Start()
    {
        _sendWave = false;
        _timeSinceLastWave = 0f;
        _startTime = 0f;
        _numZombiesSpawned = 0;
    }


    void Update()
    {
        if (_sendWave && (Time.time - _timeSinceLastWave > _perYtime))
        {
            SendWave();
        }

        if (Time.time - _startTime > _totalWaveTime)
        {
            CleanUp();
        }
    }


    public void SetUp()
    {
        // Might be nice to make it smooth in the future - Diego
        _entrancePrefab.transform.position = new Vector3(_entrancePrefab.transform.position.x, _closedEntranceHeight, _entrancePrefab.transform.position.z);

        Set("WaveManagerInstruction1", true);
        Invoke("DisableWaveManagerInstruction1", _timeOnScreen);

        Set("Time", true);
        Invoke("DisableTime", _timeOnScreen + 2f);

        PrepareWaveSpawners();
        SendWave();
        _sendWave = true;

        _startTime = Time.time;
    }


    private void PrepareWaveSpawners()
    {
        foreach (GameObject spawnerPrefab in _waveZombieSpawners)
        {
            ZombieSpawner spawner = spawnerPrefab.GetComponent<ZombieSpawner>();
            spawner.SetZombiesDestination(_waveZombiesDestination);
            spawner.SetNumberOfZombiesToSpawn(_spawnX);
        }
    }


    private void SendWave()
    {
        _timeSinceLastWave = Time.time;

        foreach (GameObject spawnerPrefab in _waveZombieSpawners)
        {
            spawnerPrefab.GetComponent<ZombieSpawner>().Spawn();
        }
    }


    private void CleanUp()
    {
        // Might be nice to make it smooth in the future - Diego
        _exitPrefab.transform.position = new Vector3(_exitPrefab.transform.position.x, _openedExitHeight, _exitPrefab.transform.position.z);

        Set("WaveManagerInstruction2", true);
        Invoke("DisableWaveManagerInstruction2", _timeOnScreen);

        _sendWave = false;
    }


    private void DisableWaveManagerInstruction1()
    {
        Set("WaveManagerInstruction1", false);
    }


    private void DisableWaveManagerInstruction2()
    {
        Set("WaveManagerInstruction2", false);
    }


    private void DisableTime()
    {
        Set("Time", false);
    }


    private void Set(string text, bool active)
    {
        GameObject canvas;
        GameObject textUI;
        GameObject secondsText;

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            canvas = player.transform.Find("Player Canvas").gameObject;
            textUI = canvas.transform.Find(text).gameObject;
            textUI.SetActive(active);

            if (text == "Time")
            {
                secondsText = textUI.transform.Find("SecondsText").gameObject;
                secondsText.GetComponent<TextMeshProUGUI>().text = _totalWaveTime.ToString();
            }
        }
    }
}
