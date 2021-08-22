using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetProjectile : TargetBase
{
    [SerializeField] GameObject projectile;
    [SerializeField] Transform player;
    [SerializeField] private float shootForce;
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private Transform attackPoint;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform; 
    }

    private void Start()
    {
        StartCoroutine(ShootPlayer()); ;
    }

    IEnumerator ShootPlayer()
    {
        while (true) { 
        GameObject orb = Instantiate(projectile);
        orb.transform.position = transform.position + transform.forward;
        orb.transform.LookAt(player);
        orb.GetComponent<Rigidbody>().AddForce((player.position - orb.transform.position).normalized * shootForce, ForceMode.Impulse);

        yield return new WaitForSeconds(timeBetweenShots);
}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
