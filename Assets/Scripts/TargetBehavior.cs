using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBehavior : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        print("collide: " + collision.gameObject.name);
        if (collision.gameObject.name.Contains("bullet"))
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
