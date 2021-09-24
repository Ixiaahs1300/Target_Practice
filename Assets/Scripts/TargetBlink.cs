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
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform player;
    [SerializeField] private float shootForce = 60f;

    void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        mr = GetComponent<MeshRenderer>();
        currentMat = red;
    }

    private void Start()
    {
        InvokeRepeating("ChangeType", switchTime, repeatTime);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (collision.transform.CompareTag("Projectile"))
        {
            Destroy(gameObject);
        }
    }

    void ChangeType()
    {
        if (currentMat.Equals(blue))
        {
            mr.material = red;
            currentMat = red;
            
        }
        else if(currentMat.Equals(red))
        {
            mr.material = blue;
            currentMat = blue;

            GameObject orb = Instantiate(projectile);
            orb.transform.position = transform.position + transform.forward;
            orb.transform.LookAt(player);
            orb.GetComponent<Rigidbody>().AddForce((player.position - orb.transform.position).normalized * shootForce, ForceMode.Impulse);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
