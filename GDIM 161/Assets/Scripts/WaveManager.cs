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
    [SerializeField] private int _totalNumWaves;
    [SerializeField] private List<GameObject> _waveZombieSpawners;
    [SerializeField] private bool _useDefaultNumZombiesToSpawn;
    [SerializeField] private int _initialTotalNumZombies;
    [SerializeField] private int _addtionalNumZombiesPerWave;
    [SerializeField] private Transform _waveZombiesDestination;

    [Header("UI")]
    [SerializeField] private float _timeOnScreen;

    private int _currWave;
    private bool _sendWaves;
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
        _currWave = 0;
        _sendWaves = false;
        _numZombiesInZone = 0;
    }


    void Update()
    {
        if (_sendWaves)
        {
            if (Time.time - _startTime > _totalWaveTime)
            {
                CleanUp();
            }
            else if (IsCurrWaveFinished())
            {
                if (_currWave < _totalNumWaves)
                {
                    SendNewWave();
                }
                else
                {
                    CleanUp();
                }
            }
        }
    }


    public void SetUp()
    {
        _sendWaves = true;
        _startTime = Time.time;

        // Might be nice to make it smooth in the future - Diego
        _entrancePrefab.transform.position = new Vector3(_entrancePrefab.transform.position.x, _closedEntranceHeight, _entrancePrefab.transform.position.z);

        Set("WaveManagerInstruction1", true);
        Invoke("DisableWaveManagerInstruction1", _timeOnScreen);
    }


    private void SendNewWave()
    {
        foreach (GameObject spawnerPrefab in _waveZombieSpawners)
        {
            ZombieSpawner spawner = spawnerPrefab.GetComponent<ZombieSpawner>();

            spawner.SetZombiesDestination(_waveZombiesDestination);

            if (!_useDefaultNumZombiesToSpawn)
            {
                int numberOfZombiesToSpawn = (_initialTotalNumZombies + (_currWave * _addtionalNumZombiesPerWave)) / _waveZombieSpawners.Count;
                spawner.SetNumberOfZombiesToSpawn(numberOfZombiesToSpawn);
            }

            spawner.Spawn();
            _numZombiesInZone += spawner.NumberZombiesToSpawn;
        }

        // IMPORTANT: These two lines must come after setting the number of zombies to spawn - Diego
        _currWave++;
    }


    private void CleanUp()
    {
        _sendWaves = false;

        // Might be nice to make it smooth in the future - Diego
        _exitPrefab.transform.position = new Vector3(_exitPrefab.transform.position.x, _openedExitHeight, _exitPrefab.transform.position.z);

        Set("WaveManagerInstruction2", true);
        Invoke("DisableWaveManagerInstruction2", _timeOnScreen);
    }


    private bool IsCurrWaveFinished()
    {
        return _numZombiesInZone == 0;
    }


    public void ZombieDied(bool isWaveZombie)
    {
        if (_sendWaves && isWaveZombie)
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
