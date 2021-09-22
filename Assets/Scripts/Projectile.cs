using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Bullet
{
    protected override void OnCollisionEnter(Collision other)
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        
        if(other.gameObject.layer != 9)
        {
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
