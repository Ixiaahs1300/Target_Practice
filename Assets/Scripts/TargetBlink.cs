using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBlink : TargetBase
{
    MeshRenderer mr;
    [SerializeField] Material red;
    [SerializeField] Material blue;
    Material currentMat;
    [SerializeField] float switchTime = 3f;

    void Awake()
    {
        mr = GetComponent<MeshRenderer>();
        currentMat = red;
    }

    private void Start()
    {
        InvokeRepeating("ChangeType", switchTime, switchTime);
    }

    void ChangeType()
    {
        if (currentMat.Equals(blue))
        {
            mr.materials[0] = red;
        }
        else if(currentMat.Equals(red))
        {
            mr.materials[0] = blue;
        }
    }

    // Update is called once per frame
    void Update()
    {

        /*if(currentMat.Equals(red))
        {

        }
        else if(currentMat.Equals(blue))
        {

        }*/
    }
}
