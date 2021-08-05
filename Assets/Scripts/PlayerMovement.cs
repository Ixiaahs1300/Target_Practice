﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float playerHeight = 2f;

    [SerializeField] Transform orientation;
    [SerializeField] WallRun wallRun;

    [Header("Movement")]
    public float moveSpeed = 6f;
    float movementMultiplier = 10f;
    [SerializeField] float airMultiplier = 0.4f;

    [Header("Sprinting")]
    [SerializeField] float walkSpeed = 6f;
    [SerializeField] float sprintSpeed = 10f;
    [SerializeField] float acceleration = 10f;

    [Header("Camera")]
    [SerializeField] private Camera cam;
    [SerializeField] private float fov;
    [SerializeField] private float wallRunFov;
    [SerializeField] private float wallRunFovTime;

    [Header("Jumping")]
    public float jumpForce = 15f;

    [Header("Keybinds")]
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] KeyCode jumpKey = KeyCode.Space;

    [Header("Drag")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 2f;

    float horizontalMovement;
    float verticalMovement;

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    Rigidbody rb;

    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    public bool isGrounded { get; private set; }
    [SerializeField] float groundDistance = 0.2f;

    RaycastHit slopeHit;

    void ControlSpeed()
    {
        if(Input.GetKey(sprintKey) && isGrounded)
        {
            
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
            print("hello");
        }
        else if(!Input.GetKey(sprintKey) && isGrounded)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
            print("hi");
        }

    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if(slopeHit.normal != Vector3.up)
            {
                return true;
            }
        }

        return false;
    }


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        //isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight / 2f + 0.1f);
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        //print("IsGrounded: " + isGrounded);
        
        MyInput();
        ControlDrag();
        ControlSpeed();

        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            //Jump
            Jump();
        }

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void ControlDrag()
    {
        if(isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }

        
    }

    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.transform.forward * verticalMovement + orientation.transform.right * horizontalMovement;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        if(isGrounded && !OnSlope())
        {
            rb.useGravity = true;
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if(isGrounded && OnSlope())
        {
            rb.useGravity = false; //Stops sliding
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if ((!isGrounded && wallRun.wallLeft) || (!isGrounded && wallRun.wallRight))
        {
            rb.AddForce(moveDirection.normalized * (moveSpeed / 2) * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.useGravity = true;
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
        
    }
}
