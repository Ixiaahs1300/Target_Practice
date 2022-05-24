using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTrigger : MonoBehaviour
{
    public bool canInstantiate = false;

    private void OnTriggerEnter(Collider other)
    {
        // Begins instantiating targets when player enters trigger area
        if (other.tag.Equals("Player"))
        {
            canInstantiate = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Stops instantiating targets when player exits trigger area
        if (other.tag.Equals("Player"))
        {
            canInstantiate = false;
        }
    }
}