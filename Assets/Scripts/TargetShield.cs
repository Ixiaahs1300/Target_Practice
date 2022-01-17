using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetShield : TargetBase
{
    [SerializeField] private GameObject shield;
    private GameObject shieldInstance;
    //private bool shi elding = false;

    // Start is called before the first frame update
    private void Awake()
    {
        shieldInstance = Instantiate(shield, new Vector3(0, 0, 0), Quaternion.identity);
        shieldInstance.transform.rotation = transform.rotation;
        shieldInstance.SetActive(false);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        Destroy(shieldInstance);
        base.OnCollisionEnter(collision);
        if (collision.transform.CompareTag("Projectile"))
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartCoroutine("Shield");
    }
    //Make it in TargetSpawner so that shield can't be first or second spawned target
    IEnumerator Shield()
    {
        int guardTarget = 0;
        if (spawner != null && spawner.positions.Count < 2)
        {
            guardTarget = Random.Range(0, spawner.positions.Count);
        }
        //else if(shielding != )

        else
        {
            yield return new WaitForSeconds(3f);
        }
        //print(Random.Range(0, 0));
        if(guardTarget == spawnIndex)
        {
            while(guardTarget == spawnIndex)
            {
                guardTarget = Random.Range(0, spawner.positions.Count);
            }
        }
        shieldInstance.transform.position = spawner.positions[guardTarget];
        shieldInstance.SetActive(true);

        yield return new WaitForSeconds(3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
