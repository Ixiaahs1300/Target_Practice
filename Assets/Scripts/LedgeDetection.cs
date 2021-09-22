using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetection : MonoBehaviour
{
    [SerializeField] private Transform detectVertical;
    [SerializeField] private Transform detectHorizontal;
    [SerializeField] private Transform orientation;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private PlayerMovement pm;
    [SerializeField] private GameObject camHolder;
    private RaycastHit ledgeHorizontal;
    private RaycastHit ledgeVertical;
    [SerializeField] private float horizontalWallDistance = 0.6f;
    [SerializeField] private float verticalWallDistance = 1.2f;
    [SerializeField] private bool horizontalHit = false;
    [SerializeField] private bool verticalHit = false;
    public bool isHanging = false;
    private Vector3 hangPosition = Vector3.zero;
    private Vector3 camPosition = Vector3.zero;
    private KeyCode ledgeClimbKey = KeyCode.Space;
    public float jumpForce = 15f;


    bool CanHang()
    {
        if(!pm.isGrounded && LedgeAhead() && rb.velocity.y < 0) 
        {
            return true;
        }

        return false;

    }
    
    bool LedgeAhead()
    {
        horizontalHit = Physics.Raycast(transform.position, orientation.forward, out ledgeHorizontal, horizontalWallDistance);
        verticalHit = Physics.Raycast(detectVertical.position, -orientation.up, out ledgeVertical, verticalWallDistance); 
        return horizontalHit && verticalHit;
    }

    void Hang()
    {
        if(CanHang())
        {
            print("eyyy");
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
            isHanging = true;
            hangPosition = transform.position;
            camPosition = camHolder.transform.position;
        }
    }

    void Update()
    {
        Debug.DrawRay(detectHorizontal.position, orientation.forward, Color.blue);
        //Debug.DrawRay(detectVertical.position, new Vector3(0, -1.2f, 0), Color.green);

        if (!isHanging)
        {
            LedgeAhead();
            Hang();
        }
        else
        {
            transform.position = hangPosition;
            camPosition = camHolder.transform.position;
        }

        /*if (Input.GetKeyDown(ledgeClimbKey))
        {
            rb.useGravity = true;
            isHanging = false;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }*/
    }

    private void FixedUpdate()
    {
        
    }
}
