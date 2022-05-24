using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    // The camera's position
    [SerializeField]
    Transform cameraPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Moves the camera to this object's position
        transform.position = cameraPosition.position;
    }
}
