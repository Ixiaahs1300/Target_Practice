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
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
        
        
    }
}
