using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private ProjectileLauncher projectileLauncher;
    [SerializeField] private CharacterController playerController;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private GameObject playerCanvas;
    [SerializeField] private GameObject hitMarker;

    public void IsLocalPlayer()
    {
        playerMovement.enabled = true;
        projectileLauncher.enabled = true;
        playerController.enabled = true;
        playerCamera.SetActive(true);
        playerCanvas.SetActive(true);
        hitMarker.SetActive(true);
    }
}
