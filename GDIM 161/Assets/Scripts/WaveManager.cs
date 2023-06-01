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

    [Header("Spawners")]
    [SerializeField] private int _totalNumWaves;
    [SerializeField] private List<GameObject> _waveZombieSpawners;
    [SerializeField] private bool _useDefaultNumZombiesToSpawn;
    [SerializeField] private int _initialTotalNumZombies;
    [SerializeField] private int _addtionalNumZombiesPerWave;

    [Header("Zombies")]
    [SerializeField] private float _damageIncreasePerXWave;
    [SerializeField] private float _speedIncreasePerXWave;
    [SerializeField] private int _X;
    [SerializeField] private int __numZombiesInZone = 0;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _objectiveInstruction;
    [SerializeField] private float _timeOnScreen;
    [SerializeField] private TextMeshProUGUI _wavesIndicator;

    private int _currWave;
    private bool _sendWaves;
    private int _numZombiesInZone;

    void Awake()
    {
        if (Instance == null){
            Instance = this;
        }
        else{
            Debug.Log("THERE CAN ONLY BE ONE WAVE MANAGER");
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
        if (_sendWaves && IsCurrWaveFinished())
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


    public void SetUp()
    {
        _sendWaves = true;

        // Might be nice to make it smooth in the future - Diego
        _entrancePrefab.transform.position = new Vector3(_entrancePrefab.transform.position.x, _closedEntranceHeight, _entrancePrefab.transform.position.z);

        _objectiveInstruction.gameObject.SetActive(true);
        _objectiveInstruction.text = "Survive Until the Gate Opens";
        Invoke("DisableObjectiveInstruction", _timeOnScreen);
    }


    private bool IsCurrWaveFinished()
    {
        return _numZombiesInZone == 0;
    }


    private void SendNewWave()
    {
        foreach (GameObject spawnerPrefab in _waveZombieSpawners)
        {
            ZombieSpawner spawner = spawnerPrefab.GetComponent<ZombieSpawner>();

            // Make function to make zombies stronger here (speed and damage)

            if (!_useDefaultNumZombiesToSpawn)
            {
                int numberOfZombiesToSpawn = (_initialTotalNumZombies + (_currWave * _addtionalNumZombiesPerWave)) / _waveZombieSpawners.Count;
                spawner.SetNumberOfZombiesToSpawn(numberOfZombiesToSpawn);
            }

            spawner.Spawn();
            _numZombiesInZone = spawner.NumberZombiesToSpawn * _waveZombieSpawners.Count;
        }

        // IMPORTANT: These two lines must come after setting the number of zombies to spawn - Diego
        _currWave++;
        UpdateWavesIndicator();
    }


    private void CleanUp()
    {
        _sendWaves = false;

        // Might be nice to make it smooth in the future - Diego
        _exitPrefab.transform.position = new Vector3(_exitPrefab.transform.position.x, _openedExitHeight, _exitPrefab.transform.position.z);

        _wavesIndicator.gameObject.SetActive(false);

        _objectiveInstruction.gameObject.SetActive(true);
        _objectiveInstruction.text = "Door is Open!\nEscape!";
        Invoke("DisableObjectiveInstruction", _timeOnScreen);

        Destroy(this, _timeOnScreen);
    }


    public void ZombieDied()
    {
        if (_sendWaves)
        {
            _numZombiesInZone = Mathf.Max(0, --_numZombiesInZone);
        }
    }


    private void UpdateWavesIndicator()
    {
        _wavesIndicator.gameObject.SetActive(true);
        _wavesIndicator.text = string.Format("{0} / {1} WAVES", _currWave, _totalNumWaves);
    }


    private void DisableObjectiveInstruction()
    {
        _objectiveInstruction.gameObject.SetActive(false);
    }
}
