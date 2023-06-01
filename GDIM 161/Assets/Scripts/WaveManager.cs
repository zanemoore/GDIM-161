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
    [SerializeField] private int _totalNumWaves;
    [SerializeField] private List<GameObject> _waveZombieSpawners;
    [SerializeField] private bool _useDefaultNumZombiesToSpawn;
    [SerializeField] private int _initialTotalNumZombies;
    [SerializeField] private int _addtionalNumZombiesPerWave;
    [SerializeField] private Transform _waveZombiesDestination;

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

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            foreach (Transform child in player.transform)
            {
                if (child.gameObject.name == "Player Canvas")
                {
                    GameObject canvas = child.gameObject;

                    foreach (Transform canvasChild in canvas.transform)
                    {
                        if (canvasChild.gameObject.name == "WaveManagerObjectiveInstruction1")
                        {
                            canvasChild.gameObject.SetActive(true);
                            Invoke("DisableObjectiveInstruction", _timeOnScreen);
                        }
                    }

                }
            }
        }

        // _objectiveInstruction.text = "Survive Until the Gate Opens";
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

            spawner.SetZombiesDestination(_waveZombiesDestination);

            // Make function to make zombies stronger here (speed and damage)

            if (!_useDefaultNumZombiesToSpawn)
            {
                int numberOfZombiesToSpawn = (_initialTotalNumZombies + (_currWave * _addtionalNumZombiesPerWave)) / _waveZombieSpawners.Count;
                spawner.SetNumberOfZombiesToSpawn(numberOfZombiesToSpawn);
            }

            spawner.Spawn();
            _numZombiesInZone = spawner.NumberZombiesToSpawn * _waveZombieSpawners.Count;
            print(string.Format("{0} zombs in zone", _numZombiesInZone)); ///////////
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


        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            foreach (Transform child in player.transform)
            {
                if (child.gameObject.name == "Player Canvas")
                {
                    GameObject canvas = child.gameObject;

                    foreach (Transform canvasChild in canvas.transform)
                    {
                        if (canvasChild.gameObject.name == "WaveManagerObjectiveInstruction2")
                        {
                            canvasChild.gameObject.SetActive(true);
                            Invoke("DisableObjectiveInstruction", _timeOnScreen);
                            Destroy(this, _timeOnScreen);
                        }
                    }

                }
            }
        }

        // _objectiveInstruction.text = "Door is Open!\nEscape!";
    }


    public void ZombieDied(bool isWaveZombie)
    {
        print(string.Format("{0} zombies in wave after one considered died", _numZombiesInZone)); ///////////
        if (_sendWaves && isWaveZombie)
        {
            _numZombiesInZone = Mathf.Max(0, --_numZombiesInZone);
        }
    }


    private void UpdateWavesIndicator()
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
                        if (canvasChild.gameObject.name == "WavesIndicator")
                        {
                            canvasChild.gameObject.SetActive(true);
                            canvasChild.gameObject.GetComponent<TextMeshProUGUI>().text = string.Format("{0} / {1} WAVES", _currWave, _totalNumWaves);
                        }
                    }

                }
            }
        }
    }


    private void DisableObjectiveInstruction()
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
                        if (canvasChild.gameObject.name == "WaveManagerObjectiveInstructions1")
                        {
                            canvasChild.gameObject.SetActive(false);
                        }
                        else if (canvasChild.gameObject.name == "WaveManagerObjectiveInstructions2")
                        {
                            canvasChild.gameObject.SetActive(false);
                        }
                    }

                }
            }
        }
    }
}
