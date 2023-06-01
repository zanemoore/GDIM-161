using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerRevive : MonoBehaviour
{
    [SerializeField] private KeyCode reviveKey;

    [SerializeField] private float reviveRadius;
    [SerializeField] private float initialReviveTime;
    [SerializeField] private float additionalReviveTime;

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask obstructionLayers;

    [SerializeField] private GameObject[] players;
    [SerializeField] private GameObject progressBar;
    [SerializeField] private Slider reviveProgressBar;

    [SerializeField] private GameObject spectatorCamera;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PhotonView playerView;
    //[SerializeField] private PlayerMovement playerMovement;
    //[SerializeField] private ProjectileLauncher projectileLauncher;

    private int deathCounter = 1;
    private float keyHeldStartTime = 0f;
    private float keyHeldTimer;
    private float reviveProgressValue;
    private float reviveProgressAdd;
    private bool keyHeld = false;
    private bool reviving;
    public bool playerRevived = false;
    public bool inRange { get; private set; }
    public static bool reviveDone;

    void Start()
    {
        initialReviveTime = additionalReviveTime * deathCounter;
        reviveProgressValue = 0f;

        players = GameObject.FindGameObjectsWithTag("Player");

        StartCoroutine(RangeCheck());
    }

    // Update is called once per frame
    void Update()
    {
        if (playerRevived == false)
        {
            ReviveKeyPress();
        }
    }

    public void ReviveKeyPress()
    {
        foreach (GameObject p in players)
        {
            if (p.GetComponent<PlayerHealth>().isAlive == true)
            {
                // checks if the player is in range of the player that needs to be revived
                if (inRange)
                {
                    // adds time to the timer while the key is held down
                    if (Input.GetKey(reviveKey) && keyHeld == false)
                    {
                        // revive progress bar
                        reviveProgressAdd = 100 / initialReviveTime;
                        reviveProgressValue += reviveProgressAdd * Time.deltaTime;
                        reviveProgressBar.value = reviveProgressValue;

                        // revive hold timer
                        keyHeldTimer += Time.deltaTime;
                        reviving = true;

                        // when the key is held down for the required amount of time, the timer stops and the player is revived
                        if (keyHeldTimer >= (keyHeldStartTime + initialReviveTime))
                        {
                            keyHeld = true;
                            reviveDone = true;
                            reviving = false;
                            KeyHeld();
                        }
                    }

                    // checks if the revive key has been released to allow the player to be revived if they die again
                    if (Input.GetKeyUp(reviveKey))
                    {
                        keyHeld = false;
                        reviving = false;
                        reviveProgressValue = 0;
                        keyHeldTimer = 0;
                    }
                    else if (Input.GetKeyDown(reviveKey))
                    {
                        reviving = true;
                    }

                    if (reviving == true)
                    {
                        progressBar.SetActive(true);
                    }
                    else
                    {
                        progressBar.SetActive(false);
                    }
                }
            }
        }
    }

    public void KeyHeld()
    {
        Debug.Log("REVIVE COMPLETED!! Key held down for " + initialReviveTime + " seconds.");

        deathCounter++;

        // resets revive timer
        keyHeldTimer = 0f;
        reviving = false;
        reviveProgressValue = 0;
        keyHeld = false;

        playerRevived = true;
        if (!playerView.IsMine)
        {
            spectatorCamera.SetActive(false);
            playerCamera.SetActive(true);
            playerView.RPC("EnablePlayer", RpcTarget.AllBuffered);
            playerRevived = false;
            playerHealth.health = 100;
        }
    }

    public IEnumerator RangeCheck()
    {
        // checks if the player is in range every 0.2 seconds
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return null;
            ReviveRange();
        }
    }

    public void ReviveRange()
    {
        // raycasts a circle around the player that needs to be revived
        Collider[] rangeCheck = Physics.OverlapSphere(transform.position, reviveRadius, playerLayer);

        // checks if the player is within the range of the dead player for the key to be pressed
        if (rangeCheck.Length > 0)
        {
            Transform player = rangeCheck[0].transform;
            Vector3 directionToPlayer = (player.position - transform.position).normalized;

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // checks if the player is in the direct line of sight of the player that needs to be revived
            if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstructionLayers))
            {
                inRange = true;
            }
            else
            {
                inRange = false;
            }
        }
        else if (inRange)
        {
            inRange = false;
        }
    }

    /*
    private void OnDrawGizmos()
    {
        Handles.color = Color.white;
        Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, reviveRadius);
    }
    */
}
