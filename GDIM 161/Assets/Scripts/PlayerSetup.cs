using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private ProjectileLauncher projectileLauncher;
    [SerializeField] private CharacterController playerController;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private GameObject hitMarker;

    public void IsLocalPlayer()
    {
        playerMovement.enabled = true;
        playerHealth.enabled = true;
        projectileLauncher.enabled = true;
        playerController.enabled = true;
        playerCamera.SetActive(true);
        hitMarker.SetActive(true);
    }
}
