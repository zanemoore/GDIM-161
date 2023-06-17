using Photon.Pun;
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

    [Header("Spectator Components")]
    [SerializeField] private GameObject spectatorCanvas;
    [SerializeField] private GameObject spectatorCamera;

    [Header("Player Components")]
    [SerializeField] private GameObject playerReticle;
    [SerializeField] private GameObject playerHealthbar;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private CharacterAudio characterAudio;

    public void IsLocalPlayer()
    {
        playerMovement.enabled = true;
        projectileLauncher.enabled = true;
        playerController.enabled = true;
        playerCamera.SetActive(true);
        playerCanvas.SetActive(true);
    }

    [PunRPC]
    public void DisablePlayer()
    {
        playerReticle.SetActive(false);
        playerHealthbar.SetActive(false);
        meshRenderer.enabled = false;
        capsuleCollider.enabled = false;
        playerController.enabled = false;
        playerHealth.enabled = false;
        playerMovement.enabled = false;
        projectileLauncher.enabled = false;
        characterAudio.enabled = false;
    }

    [PunRPC]
    public void EnablePlayer()
    {
        playerReticle.SetActive(true);
        playerHealthbar.SetActive(true);
        meshRenderer.enabled = true;
        capsuleCollider.enabled = true;
        playerController.enabled = true;
        playerHealth.enabled = true;
        playerMovement.enabled = true;
        projectileLauncher.enabled = true;
        characterAudio.enabled = true;

        spectatorCanvas.SetActive(false);
    }
}
