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
        print(spawnIndex);
    }
    
    public int RandomExcept(int min, int max, int except)
    {
        int random = Random.Range(min, max);
        if (random >= except) random = (random + 1) % max;
        return random;
    }

    IEnumerator Shield()
    {
        while (true)
        {
            int guardTarget = 0;
            if (spawner != null && spawner.positions.Count > 1)
            {
                Debug.Log("Count: " + spawner.positions.Count);
                guardTarget = Random.Range(0, spawner.positions.Count);
            }
            //else if(shielding != )
            else
            {
                yield return new WaitForSeconds(3f);
                print("We ON!");
            }
            //print(Random.Range(0, 0));
            if (guardTarget == spawnIndex)
            {
                while (guardTarget == spawnIndex)
                {
                    guardTarget = Random.Range(0, spawner.positions.Count);
                }
            }

            shieldInstance.transform.position = spawner.positions[guardTarget];
            shieldInstance.SetActive(true);

            yield return new WaitForSeconds(3f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
