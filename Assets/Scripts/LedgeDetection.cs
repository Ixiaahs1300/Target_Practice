using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetection : MonoBehaviour
{
    // Looks for ledge in grabbing distance above player
    [SerializeField] private Transform detectVertical; 
    // looks to see if there is a wall in front of the player
    [SerializeField] private Transform detectHorizontal;
    [SerializeField] private Transform orientation;
    
    [SerializeField] private Rigidbody rb;
    [SerializeField] private PlayerMovement pm;
    
    [SerializeField] private float horizontalWallDistance = 0.6f;
    [SerializeField] private float verticalWallDistance = 1.2f;
    
    // Whether a raycast is detecting a wall in front
    [SerializeField] private bool horizontalHit = false;
    // Whether a raycast is detecting a grabbable ledge above
    [SerializeField] private bool verticalHit = false;
    
    public bool isHanging = false;
    
    // Key for jumping onto ledge
    private KeyCode ledgeClimbKey = KeyCode.Space;
    public float jumpForce = 15f;
    private bool isJumping = false;

    // Allows player to hang if they are falling in front of a ledge
    bool CanHang()
    {
        if(!pm.isGrounded && LedgeAhead() && rb.velocity.y < 0) 
        {
            return true;
        }

        return false;

    }
    
    // Detects if a grabbable ledge is ahead 
    bool LedgeAhead()
    {
        horizontalHit = Physics.Raycast(detectHorizontal.position, orientation.forward, horizontalWallDistance);
        verticalHit = Physics.Raycast(detectVertical.position, -orientation.up, verticalWallDistance); 
        return horizontalHit && verticalHit;
    }

    void Hang()
    {
        // Turns off physics while character hangs to stop jittering
        if(CanHang())
        {
            isHanging = true;
            rb.isKinematic = true;
        }
    }

    void Update()
    {
        Debug.DrawRay(detectVertical.position, -orientation.up);
        // Checks if character is now hanging
        if (!isHanging)
        {
            Hang();
        }

        if (Input.GetKeyDown(ledgeClimbKey) && isHanging)
        {
            isJumping = true;
        }
    }

    private void FixedUpdate()
    {
        // Allows player to jump up from a legdge hang
        if (isJumping)
        {
            rb.isKinematic = false;
            isHanging = false;
            isJumping = false;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }
}
