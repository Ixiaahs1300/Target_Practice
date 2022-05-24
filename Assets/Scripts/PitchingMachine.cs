using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchingMachine : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    // What the pitching machine is aiming at
    [SerializeField] private Transform target;
    [SerializeField] private float shootForce = 30f;
    // Whether it's aiming at the target
    [SerializeField] private bool aimAtTarget = false;
    // Whether it's shooting at the target, or shooting forward
    private bool pitchingToTarget = false;

    // Start is called before the first frame update
    void Start()
    {
        //
        if (aimAtTarget)
        {
            StartCoroutine("PitchProjectileAtTarget");
            aimAtTarget = true;
            pitchingToTarget = true;
        }
        else
        {
            StartCoroutine("PitchProjectile");
            aimAtTarget = false;
            pitchingToTarget = false;
        }
    }

    IEnumerator PitchProjectileAtTarget()
    {
        // Continuously shoots at targeted object
        while(true)
        {
            GameObject proj = Instantiate(projectile);
            // Places projectile in front of pitching machine
            proj.transform.position = transform.position + transform.forward;
            proj.transform.LookAt(target);
            proj.GetComponent<Rigidbody>().freezeRotation = true;
            proj.GetComponent<Rigidbody>().AddForce((target.position - proj.transform.position).normalized * shootForce, ForceMode.Impulse);
            yield return new WaitForSeconds(2f);
        }
        
    }

    IEnumerator PitchProjectile()
    {
        // Continuously shoots forward
        while (true)
        {
            GameObject proj = Instantiate(projectile);
            // Places projectile in front of pitching machine
            proj.transform.position = transform.position + transform.forward;
            proj.GetComponent<Rigidbody>().freezeRotation = true;
            proj.GetComponent<Rigidbody>().AddForce(transform.forward * shootForce, ForceMode.Impulse);
            yield return new WaitForSeconds(2f);
        }

    }

    // Update is called once per frame
    void Update()
    {
        /*if (aimAtTarget && !pitchingToTarget)
        {
            StopCoroutine("PitchProjectile");
            StartCoroutine("PitchProjectileAtTarget");
        }
        else if (!aimAtTarget && pitchingToTarget)
        {
            StopCoroutine("PitchProjectileAtTarget");
            StartCoroutine("PitchProjectile");
        }*/
    }
}
