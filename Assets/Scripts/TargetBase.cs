﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBase : MonoBehaviour
{
    [SerializeField] private int health; //TODO
    [SerializeField] public int spawnIndex;
    [SerializeField] public TargetSpawner spawner;

    private void OnCollisionEnter(Collision collision)
    {
        print("collide: " + collision.gameObject.name);
        if (collision.gameObject.name.Contains("bullet"))
        {
            if (spawner != null)
            {
                spawner.positions.Remove(transform.position);
            }
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
