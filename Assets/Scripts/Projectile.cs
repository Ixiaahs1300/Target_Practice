using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Bullet
{
    protected override void OnCollisionEnter(Collision other)
    {
        int layer = other.gameObject.layer;
        // Resets the projectile's velocity
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        
        // Stops projectile from being destroyed on contact with deflector shield
        if(layer != 9)
        {
            //print("Should be destroyed");
            Destroy(gameObject);
        }
        
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        // Depletes life timer
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            // Resets velocity and destroys object
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            Destroy(gameObject);
        }
    }
}
