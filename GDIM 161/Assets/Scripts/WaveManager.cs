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

    [Header("Wave Zombie Spawners")]
    [SerializeField] private float _totalWaveTime;
    [SerializeField] private List<GameObject> _waveZombieSpawners;
    [SerializeField] private bool _useDefaultNumZombiesToSpawn;
    [SerializeField] private int _totalNumZombiesToSpawn;
    [SerializeField] private Transform _waveZombiesDestination;

    [Header("UI")]
    [SerializeField] private float _timeOnScreen;

    private bool _waveSent;
    private int _numZombiesInZone;
    private float _startTime;


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
        _waveSent = false;
        _numZombiesInZone = 0;
        _startTime = 0f;
    }


    void Update()
    {
        if (_waveSent && ((Time.time - _startTime > _totalWaveTime) || IsWaveFinished()))
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

        SendWave();
        _waveSent = true;
        _startTime = Time.time;
    }


    private void SendWave()
    {
        foreach (GameObject spawnerPrefab in _waveZombieSpawners)
        {
            ZombieSpawner spawner = spawnerPrefab.GetComponent<ZombieSpawner>();

            spawner.SetZombiesDestination(_waveZombiesDestination);

            if (!_useDefaultNumZombiesToSpawn)
            {
                int numOfZombiesToSpawn = _totalNumZombiesToSpawn / _waveZombieSpawners.Count;
                spawner.SetNumberOfZombiesToSpawn(numOfZombiesToSpawn);
            }

            spawner.Spawn();
            _numZombiesInZone += spawner.NumberZombiesToSpawn;
        }
    }


    private void CleanUp()
    {
        // Might be nice to make it smooth in the future - Diego
        _exitPrefab.transform.position = new Vector3(_exitPrefab.transform.position.x, _openedExitHeight, _exitPrefab.transform.position.z);

        Set("WaveManagerInstruction2", true);
        Invoke("DisableWaveManagerInstruction2", _timeOnScreen);

        _waveSent = false;
    }


    private bool IsWaveFinished()
    {
        return _numZombiesInZone == 0;
    }


    public void ZombieDied(bool isWaveZombie)
    {
        if (_waveSent && isWaveZombie)
        {
            _numZombiesInZone = Mathf.Max(0, --_numZombiesInZone);
        }
    }


    private void DisableWaveManagerInstruction1()
    {
        Set("WaveManagerInstruction1", false);
    }


    private void DisableWaveManagerInstruction2()
    {
        Set("WaveManagerInstruction2", false);
    }


    private void Set(string text, bool active)
    {
        GameObject canvas;
        GameObject textUI;

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            canvas = player.transform.Find("Player Canvas").gameObject;
            textUI = canvas.transform.Find(text).gameObject;
            textUI.SetActive(active);
        }
    }
}
