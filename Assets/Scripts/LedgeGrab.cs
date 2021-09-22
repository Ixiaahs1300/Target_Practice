using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrab : MonoBehaviour
{
    [SerializeField] Transform orientation;
    
    
    [Header("Detection")]
    [SerializeField] float minimumJumpHeight = 2.5f;
    [SerializeField] float wallDistance = 0.6f;
    public bool wallInFront = false;

    RaycastHit frontWallHit;

    [Header("Keybinds")]
    [SerializeField] KeyCode ledgeGrabKey = KeyCode.LeftShift;

    void CheckWall()
    {
        wallInFront = Physics.Raycast(transform.position - new Vector3(0, 0.2f, 0), orientation.forward, out frontWallHit, wallDistance);
        //Debug.DrawRay(transform.position, orientation.forward, Color.blue);
        //Maybe instead of raycast it uses a PHysics.checkbox to see if it is above other collider
    }

    bool CanLedgeGrab()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckWall();
        if (CanLedgeGrab())
        {
            print("Can ledge grab");
        }



        //if(CanLedgeGrab())
    }
}
