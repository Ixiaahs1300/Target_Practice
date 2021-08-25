using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchingMachine : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform player;
    [SerializeField] private float shootForce = 30f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("PitchProjectile");
    }

    IEnumerator PitchProjectile()
    {
        while(true)
        {
            GameObject proj = Instantiate(projectile);
            //proj.transform.SetParent(transform);
            proj.transform.position = transform.position + transform.forward;
            proj.transform.LookAt(player);
            proj.GetComponent<Rigidbody>().freezeRotation = true;
            proj.GetComponent<Rigidbody>().AddForce((player.position - proj.transform.position).normalized * shootForce, ForceMode.Impulse);
            yield return new WaitForSeconds(2f);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
