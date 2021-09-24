using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Bullet
{
    protected override void OnCollisionEnter(Collision other)
    {
        int layer = other.gameObject.layer;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        
        if(layer != 9)
        {
            print("Should be destroyed");
            Destroy(gameObject);
        }
        
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            Destroy(gameObject);
        }
    }
}
