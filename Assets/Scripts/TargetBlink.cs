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
    [SerializeField] float repeatTime = 3f;

    void Awake()
    {
        mr = GetComponent<MeshRenderer>();
        currentMat = red;
    }

    private void Start()
    {
        InvokeRepeating("ChangeType", switchTime, repeatTime);
    }

    void ChangeType()
    {
        print("is called");
        if (currentMat.Equals(blue))
        {
            mr.material = red;
            currentMat = red;
            print("red");
        }
        else if(currentMat.Equals(red))
        {
            mr.material = blue;
            currentMat = blue;
            print("blue");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
