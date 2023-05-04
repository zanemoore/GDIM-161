using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform grounded;
    [SerializeField] private LayerMask groundedMask;
    [SerializeField] private float groundedDistance;

    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -24f;

    private bool isGrounded;
    private Vector3 velocity;

    private void Start()
    {

    }

    void Update()
    {
        //checks if player is on the ground
        isGrounded = Physics.CheckSphere(grounded.position, groundedDistance, groundedMask);

        //resets velocity and forces player on the gorund
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float keyboardX = Input.GetAxis("Horizontal");
        float keyboardZ = Input.GetAxis("Vertical");

        //allows kepyboard inputs (WASD or arrow keys) to change direction of movement
        Vector3 movement = transform.right * keyboardX + transform.forward * keyboardZ;

        //allows player to move and controls the speed of walking

        if (Input.GetKey("left shift"))
        {
            //controls speed of running
            controller.Move(movement * runSpeed * Time.deltaTime);
        }
        else
        {
            //controls speed of walking
            controller.Move(movement * walkSpeed * Time.deltaTime);
        }

        //increases velocity as player falls
        velocity.y += gravity * Time.deltaTime;

        //allows player to fall
        controller.Move(velocity * Time.deltaTime);

        //allows player to jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}
