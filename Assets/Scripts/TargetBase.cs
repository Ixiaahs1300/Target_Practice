using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBase : MonoBehaviour
{
    // Not yet implemented
    [SerializeField] private int health; //TODO
    // Target's index in the TargetSpawner
    [SerializeField] public int spawnIndex;
    // Spawner target is apart of 
    [SerializeField] public TargetSpawner spawner;

    protected virtual void OnCollisionEnter(Collision collision)
    {
        // Removes target from spawner and destroys it if hit by bullet
        if (collision.gameObject.name.Contains("bullet"))
        {
            if (spawner != null)
            {
                spawner.positions.Remove(transform.position);
            }
            Destroy(gameObject);
        }
    }
}
