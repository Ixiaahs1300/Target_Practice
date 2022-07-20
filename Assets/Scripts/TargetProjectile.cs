using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetProjectile : TargetBase
{
    [SerializeField] GameObject projectile;
    [SerializeField] Transform player;
    [SerializeField] private float shootForce;
    [SerializeField] private float timeBetweenShots;
    // Focus of target's fire
    [SerializeField] private Transform attackPoint;

    // Called when the object is initialized
    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform; 
    }

    // Called when the object is made "active"
    private void Start()
    {
        StartCoroutine(ShootPlayer()); 
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        
        //Allows target to be destroyed by its deflected projectile
        if(collision.transform.CompareTag("Projectile"))
        {
            Destroy(gameObject);
        }
    }

    /*
     * Continuously shoots at player
    */
    IEnumerator ShootPlayer()
    {
        while (true) 
        { 
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
