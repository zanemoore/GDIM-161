using Photon.Pun.Demo.Cockpit;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class SpectatorMode : MonoBehaviourPunCallbacks
{
    [Header("Spectator Camera Settings")]
    [SerializeField] private Vector2 clampInDegrees = new Vector2(360, 180);
    [SerializeField] private Vector2 smoothing = new Vector2(3, 3);
    [SerializeField] private float boundMinimumX;
    [SerializeField] private float boundMaximumX;
    [SerializeField] private float boundMinimumY;
    [SerializeField] private float boundMaximumY;
    [SerializeField] private float boundMinimumZ;
    [SerializeField] private float boundMaximumZ;
    [SerializeField] private float sensitivity;
    [SerializeField] private float normalSpeed;
    [SerializeField] private float verticalSpeed;
    [SerializeField] private float slowSpeed;
    [SerializeField] private float fastSpeed;
    [SerializeField] private KeyCode slowKey;
    [SerializeField] private KeyCode fastKey;

    [Header("Spectator Components")]
    [SerializeField] private GameObject spectatorCanvas;
    [SerializeField] private GameObject spectatorCamera;
    [SerializeField] private GameObject spectatorControls;


    [Header("Player Components")]
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private GameObject playerReticle;
    [SerializeField] private GameObject playerHealthbar;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private ProjectileLauncher projectileLauncher;
    [SerializeField] private CharacterAudio characterAudio;
    [SerializeField] private PhotonView playerView;

    private Vector2 mouseAbsolute;
    private Vector2 smoothMouse;
    private Vector2 targetDirection;
    private Vector2 mouseDelta;
    private float currentSpeed;
    private bool cursorOn;

    void Start()
    {
        cursorOn = false;

        if (playerView.IsMine)
        {
            playerCamera.SetActive(false);
        }

        spectatorCanvas.SetActive(true);
        playerView.RPC("DisablePlayer", RpcTarget.AllBuffered);

        targetDirection = transform.localRotation.eulerAngles;
    }

    void Update()
    {
        if (cursorOn == false)
        {
            Movement();
            Rotation();
        }

        if (Input.GetKeyDown(KeyCode.Tab) && cursorOn == false)
        {
            cursorOn = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && cursorOn == true)
        {
            cursorOn = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void LateUpdate()
    {
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, boundMinimumX, boundMaximumX), 
            Mathf.Clamp(transform.position.y, boundMinimumY, boundMaximumY), 
            Mathf.Clamp(transform.position.z, boundMinimumZ, boundMaximumZ));
    }

    private void Movement()
    {
        float y = 0;

        if (Input.GetKey(KeyCode.Q))
        {
            y = verticalSpeed;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            y = -verticalSpeed;
        }

        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), y, Input.GetAxis("Vertical"));
        
        if (Input.GetKey(fastKey))
        {
            currentSpeed = fastSpeed;
        }
        else if (Input.GetKey(slowKey))
        {
            currentSpeed = slowSpeed;
        }
        else
        {
            currentSpeed = normalSpeed;
        }

        transform.Translate(input * currentSpeed * Time.deltaTime, Space.Self);
    }

    private void Rotation()
    {
        var targetOrientation = Quaternion.Euler(targetDirection);

        mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity * smoothing.x, sensitivity * smoothing.y));

        smoothMouse.x = Mathf.Lerp(smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
        smoothMouse.y = Mathf.Lerp(smoothMouse.y, mouseDelta.y, 1f / smoothing.y);

        mouseAbsolute += smoothMouse;

        if (clampInDegrees.x < 360)
        {
            mouseAbsolute.x = Mathf.Clamp(mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);
        }

        if (clampInDegrees.y < 360)
        {
            mouseAbsolute.y = Mathf.Clamp(mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);
        }

        transform.localRotation = Quaternion.AngleAxis(-mouseAbsolute.y, targetOrientation * Vector3.right) * targetOrientation;
        transform.localRotation *= Quaternion.AngleAxis(mouseAbsolute.x, transform.InverseTransformDirection(Vector3.up));
    }

    public void ControlsPanel()
    {
        if (spectatorControls.activeInHierarchy == true)
        {
            spectatorControls.SetActive(false);
        }
        else
        {
            spectatorControls.SetActive(true);
        }
    }
}
