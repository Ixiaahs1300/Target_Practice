﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] Transform orientation;

    [SerializeField] Transform playerGun;
    float playerGunSwitchTime = 20f;

    [SerializeField] PlayerMovement playerMovement;

    Vector3 rightGunPosition;
    Vector3 leftGunPosition;
    float gunPosition;

    [Header("Detection")]
    [SerializeField] float wallDistance = 0.5f;
    [SerializeField] float minimumJumpHeight = 1.5f;

    [Header("Wall Running")]
    [SerializeField] private float wallRunGravity;
    [SerializeField] private float wallRunJumpForce;

    [Header("Camera")]
    [SerializeField] private Camera cam;
    [SerializeField] private float fov;
    [SerializeField] private float wallRunFov;
    [SerializeField] private float wallRunFovTime;
    [SerializeField] private float camTilt;
    [SerializeField] private float camTiltTime;

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

    bool stillOnWall; // problem lies in CanWallRun, if last stopwallrun is commented out,
    // then wall running doesn't last long. Need to check if player is still on wall. aka 
    //need to see if player can was and is still running

    void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallDistance);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallDistance);
    }

    bool CanWallRun()
    {
        //print(!Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight));
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
    }


    void StartWallRun()
    {
        rb.useGravity = false;

        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);
        
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunFov, wallRunFovTime * Time.deltaTime);

        if(wallLeft)
        {
            isWallRunning = true;
            tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
            //playerGun.localPosition = rightGunPosition;
            gunPosition = Mathf.Lerp(playerGun.localPosition.x, rightGunPosition.x, playerGunSwitchTime * Time.deltaTime);
            playerGun.localPosition = new Vector3(gunPosition, playerGun.localPosition.y, playerGun.localPosition.z);
        }
        else if(wallRight)
        {
            isWallRunning = true;
            tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);
            //playerGun.localPosition = leftGunPosition;
            gunPosition = Mathf.Lerp(playerGun.localPosition.x, leftGunPosition.x, playerGunSwitchTime * Time.deltaTime);
            playerGun.localPosition = new Vector3(gunPosition, playerGun.localPosition.y, playerGun.localPosition.z);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(wallLeft)
            {
                Vector3 wallRunJumpDirection = transform.up + leftWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // Reseting velocity before applying the force
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);

            }
            else if(wallRight)
            {
                Vector3 wallRunJumpDirection = transform.up + rightWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // Reseting velocity before applying the force
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
            }
        }
    }

    public bool getWallRunning()
    {
        return isWallRunning;
    }

    public bool isWallAdjacent()
    {
        return wallLeft || wallRight;
    }

    void StopWallRun()
    {
        rb.useGravity = true;
        //playerGun.localPosition = rightGunPosition;
        gunPosition = Mathf.Lerp(playerGun.localPosition.x, rightGunPosition.x, playerGunSwitchTime * Time.deltaTime);
        playerGun.localPosition = new Vector3(gunPosition, playerGun.localPosition.y, playerGun.localPosition.z);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, wallRunFovTime * Time.deltaTime);
        isWallRunning = false;
        tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rightGunPosition = playerGun.transform.position;
        leftGunPosition = new Vector3(-0.52f, rightGunPosition.y, rightGunPosition.z);
    }

    //Need to check if moving forward
    // Update is called once per frame
    void Update()
    {
        CheckWall();
        print("Vel: " + rb.velocity.z);
        if(CanWallRun() && !ld.isHanging && (rb.velocity.z != 0 || isWallRunning))//&& !stillOnWall)
        {
            if(wallLeft)
            {
                StartWallRun();
                //Debug.Log("wall running on the left");
            }
            else if(wallRight)
            {
                StartWallRun();
                //Debug.Log("wall running on the right");
            }
            else
            {
                StopWallRun();
            }
        }
        else if(!playerMovement.isSprinting)
        {
            StopWallRun(); 
        }

    }
}
