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
    private float walk;
    private float run;

    private void Start()
    {
        walk = walkSpeed;
        run = runSpeed;
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
            //makes player run
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                //controls speed of running backwards
                controller.Move(movement * (runSpeed / 2f) * Time.deltaTime);
            }
            else
            {
                //controls speed of running forward
                controller.Move(movement * runSpeed * Time.deltaTime);
            }
        }
        else
        {
            //makes player walk
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                //controls speed of walking backwards
                controller.Move(movement * (walkSpeed/2f) * Time.deltaTime);
            }
            else
            {
                //controls speed of walking forward
                controller.Move(movement * walkSpeed * Time.deltaTime);
            }
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
