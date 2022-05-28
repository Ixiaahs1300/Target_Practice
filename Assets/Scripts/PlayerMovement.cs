using System.Collections;
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
    [SerializeField] private float fov = 90f;
    [SerializeField] private float sprintFov = 60f;
    [SerializeField] private float sprintFovTime = 20f;

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
    public bool isSprinting = false;

    void ControlSpeed()
    {
        // Smoothly transitions player into sprint
        if(Input.GetKey(sprintKey) && isGrounded)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, sprintFov, sprintFovTime * Time.deltaTime);
            isSprinting = true;
        }
        // Smoothly transitions player to a normal walkspeed
        else if (!Input.GetKey(sprintKey) && isGrounded) 
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, sprintFovTime * Time.deltaTime);
            isSprinting = false;
        }

    }

    private bool OnSlope()
    {
        // Shoots raycast directly at player's "feet", to see if they're touching something
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            // Checks to see if the ground underneath the player is "level"/ not a slope
            // by checking the direction of it's normal vector, aka the vector perpendicular to
            // its surface
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
        // Stops player from being rotated by objects colliding with it
        rb.freezeRotation = true;
    }

    private void Update()
    {
        // Uses the collision data of a sphere at the bottom of the player to see if they're on solid ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        MyInput();
        ControlDrag();
        ControlSpeed();

        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

        // The "direction" the player must go to move up a slope
        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    void Jump()
    {
        // Resets velocity in the "up" direction to avoid complications
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        // Adds force in the "up" direction
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void ControlDrag()
    {
        // Adjusts drag force to be for ground
        if(isGrounded)
        {
            rb.drag = groundDrag;
        }
        // adjusts drag force to be for air
        else
        {
            rb.drag = airDrag;
        }

        
    }

    void MyInput()
    {
        // Detects whether the player has pressed movement keys.
        // Values can range between 0 and 1, but will be either 0
        // or 1 since keyboard doesn't allow for "slight" input
        // like analog sticks on controllers
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        // Creates movement vector based upon player input
        moveDirection = orientation.transform.forward * verticalMovement + orientation.transform.right * horizontalMovement;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        // Player is simply moving on the ground
        if (isGrounded && !OnSlope())
        {
            rb.useGravity = true;
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        // Player is moving up slope
        else if (isGrounded && OnSlope())
        {
            rb.useGravity = false; //Stops sliding
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        // Player is wall running
        else if (!isGrounded && wallRun.isWallAdjacent())
        {
            // get rid of vector in up direction
            rb.AddForce(moveDirection.normalized * (moveSpeed / 2) * movementMultiplier * airMultiplier, ForceMode.Acceleration);
            print("Vector: " + moveDirection.normalized);
        }
        // If the player is jumping through the air
        else if (!isGrounded)
        {
            rb.useGravity = true;
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
        
    }
}
