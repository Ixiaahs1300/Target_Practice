﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{   
    //Lifetime remaining 
    public float lifeTimer;

    //Full lifetime
    public float lifeDuration = 30f;

    protected Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        lifeTimer = lifeDuration;
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        // Resets velocity and disables object on impact 
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // Decrements lifetime over time
        lifeTimer -= Time.deltaTime;
        if(lifeTimer <= 0f)
        {
            // Resets velocity and disables object at end of lifetime
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            gameObject.SetActive(false);
        }

        //Debug.DrawRay(transform.position, transform.forward * 2, Color.black);
    }
}
