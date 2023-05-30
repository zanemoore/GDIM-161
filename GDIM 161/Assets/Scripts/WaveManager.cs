using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
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

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _objectiveInstruction;
    [SerializeField] private float _timeOnScreen;
    [SerializeField] private TextMeshProUGUI _wavesIndicator;

    private int _currWave;
    private bool _sendWaves;
    private int _numZombiesSpawned;


    void Start()
    {
        _currWave = 0;
        _sendWaves = false;
        _numZombiesSpawned = 0;
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
        // Check if all the zombies died lol
        // Keep track of number of zombie kills?
        // ^ use actions from Die script to here :p?
        return _numZombiesSpawned == 0;
    }


    private void SendNewWave()
    {
        // Make zombies stronger here (speed and damage)

        foreach (GameObject spawnerPrefab in _waveZombieSpawners)
        {
            ZombieSpawner spawner = spawnerPrefab.GetComponent<ZombieSpawner>();

            if (!_useDefaultNumZombiesToSpawn)
            {
                int numberOfZombiesToSpawn = (_initialTotalNumZombies + (_currWave * _addtionalNumZombiesPerWave)) / _waveZombieSpawners.Count;
                spawner.SetNumberOfZombiesToSpawn(numberOfZombiesToSpawn);
            }

            // update spawner so it can randomly choose a player to chase omg slaaay (these probably faster than normal zombies? faster than player?)
            spawner.Spawn();
            _numZombiesSpawned += spawner.NumberZombiesToSpawn;
        }

        // IMPORTANT: These two lines must come after setting the number of zombies to spawn
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

        Destroy(this);  // Does this mess up the top one?
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
