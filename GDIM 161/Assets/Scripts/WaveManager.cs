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
    [SerializeField] private List<ZombieSpawner> _zombieSpawners;
    [SerializeField] private bool _useDefaultNumZombiesToSpawn;
    [SerializeField] private int _initialNumZombies;
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

    void Start()
    {
        _currWave = 0;
        _sendWaves = false;
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
        // Might be nice to make it smooth in the future - Diego
        _entrancePrefab.transform.position = new Vector3(_entrancePrefab.transform.position.x, _closedEntranceHeight, _entrancePrefab.transform.position.z);

        _objectiveInstruction.gameObject.SetActive(true);
        _objectiveInstruction.text = "Survive Until the Gate Opens";
        Invoke("DisableObjectiveInstruction", _timeOnScreen);

        _sendWaves = true;
    }


    private bool IsCurrWaveFinished()
    {
        // Check if all the zombies died lol
        // Keep track of number of zombie kills?
        // ^ use actions from Die script to here :p?
        return true;
    }


    private void SendNewWave()
    {
        _currWave++;
        UpdateWavesIndicator();

        // Make zombies stronger if needed

        // Increase how much spawner will spawn if needed
        // ^ update spawner so it can randomly choose a player to chase omg slaaay (these probably faster than normal zombies? faster than player?)
        // ^^ make it so that you can change zombies speed and damage as well
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

        Destroy(this);
    }


    private void DisableObjectiveInstruction()
    {
        _objectiveInstruction.gameObject.SetActive(false);
    }


    private void UpdateWavesIndicator()
    {
        _wavesIndicator.gameObject.SetActive(true);
        _wavesIndicator.text = string.Format("{0} / {1} WAVES", _currWave, _totalNumWaves);
    }
}
