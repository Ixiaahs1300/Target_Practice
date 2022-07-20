using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    [Header("Movement")]
    // Orientation of player
    [SerializeField] Transform orientation;

    [SerializeField] Transform playerGun;
    
    // Amount of time it takes for gun to switch to opposite side
    float playerGunSwitchTime = 20f;

    [SerializeField] PlayerMovement playerMovement;
    // Right hand position of gun
    Vector3 rightGunPosition;
    // Left hand position of gun
    Vector3 leftGunPosition;
    
    // Gun's current position
    float gunPosition;

    [Header("Detection")]
    /*  
     *  How close the wall must be for it to be detected
     *  The value of 0.5f results in the wall needing to 
     *  be touching the player as the player is 1 unit wide
    */
    [SerializeField] float wallDistance = 0.5f;
    // Minimum height needed for wall jump to be possible
    [SerializeField] float minimumJumpHeight = 1.5f;

    [Header("Wall Running")]
    // Unique gravity while wall running 
    [SerializeField] private float wallRunGravity;
    // Force with which character jumps from wall
    [SerializeField] private float wallRunJumpForce;

    [Header("Camera")]
    [SerializeField] private Camera cam;
    // Field of view
    [SerializeField] private float fov;
    [SerializeField] private float wallRunFov;
    [SerializeField] private float wallRunFovTime;
    // Camera Tilt
    [SerializeField] private float camTilt;
    [SerializeField] private float camTiltTime;

    // Wall run screen tilt
    public float tilt { get; private set; }

    private Rigidbody rb;

    [SerializeField]
    private bool wallLeft = false;
    [SerializeField]
    private bool wallRight = false;
    [SerializeField]
    private bool isWallRunning = false;

    RaycastHit leftWallHit;
    RaycastHit rightWallHit;

    [SerializeField] LedgeDetection ld;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rightGunPosition = playerGun.transform.position;
        leftGunPosition = new Vector3(-0.52f, rightGunPosition.y, rightGunPosition.z);
    }

    void CheckWall()
    {
        // Cast ray to left and right of player to check if they are touching a wayy
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallDistance);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallDistance);
    }

    bool CanWallRun()
    {
        // Cast ray downwards to check if player is above the minimum jump height
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
    }


    void StartWallRun()
    {
        // Disables normal gravity
        rb.useGravity = false;

        // Applies unique gravity to player
        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);
        
        // Smoothly changes the field of view
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunFov, wallRunFovTime * Time.deltaTime);


        if(wallLeft)
        {
            isWallRunning = true;
            // Transitions gun position and screen tilt
            tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
            gunPosition = Mathf.Lerp(playerGun.localPosition.x, rightGunPosition.x, playerGunSwitchTime * Time.deltaTime);
            playerGun.localPosition = new Vector3(gunPosition, playerGun.localPosition.y, playerGun.localPosition.z);
        }
        else if(wallRight)
        {
            isWallRunning = true;
            // Transitions gun position and screen tilt
            tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);
            gunPosition = Mathf.Lerp(playerGun.localPosition.x, leftGunPosition.x, playerGunSwitchTime * Time.deltaTime);
            playerGun.localPosition = new Vector3(gunPosition, playerGun.localPosition.y, playerGun.localPosition.z);
        }

        // Wall jumping from wall run
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(wallLeft)
            {
                Vector3 wallRunJumpDirection = transform.up + leftWallHit.normal;
                // Resetting upward velocity before applying the force
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); 
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);

            }
            else if(wallRight)
            {
                Vector3 wallRunJumpDirection = transform.up + rightWallHit.normal;
                // Resetting upward velocity before applying the force
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); 
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
            }
        }
    }

    public bool getWallRunning()
    {
        return isWallRunning;
    }

    /*
     * Checks whether or not there is a wall adjacent to the player
    */
    public bool isWallAdjacent()
    {
        return wallLeft || wallRight;
    }

    void StopWallRun()
    {
        // Reverts gravity back to normal
        rb.useGravity = true;
        // Transitions gun position and screen tilt
        gunPosition = Mathf.Lerp(playerGun.localPosition.x, rightGunPosition.x, playerGunSwitchTime * Time.deltaTime);
        playerGun.localPosition = new Vector3(gunPosition, playerGun.localPosition.y, playerGun.localPosition.z);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, wallRunFovTime * Time.deltaTime);
        isWallRunning = false;
        // Resets screen tilt
        tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);
    }

    //Need to check if moving forward
    // Update is called once per frame
    void Update()
    {
        CheckWall();
        // Checks to see if:
        // 1. The player is above the minimum jump height
        // 2. The player isn't hanging onto a ledge
        // 3. The player is either moving forward while jumping or is already wall running
        if (CanWallRun() && !ld.isHanging && (rb.velocity.z != 0 || isWallRunning))
        {
            if(wallLeft)
            {
                StartWallRun();
            }
            else if(wallRight)
            {
                StartWallRun();
            }
            else
            {
                StopWallRun();
            }
        }

    }
}
