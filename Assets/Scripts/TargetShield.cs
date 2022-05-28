using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetShield : TargetBase
{
    // energy shield prefab
    [SerializeField] private GameObject shield;
    // Instance of energy shield prefab
    private GameObject shieldInstance;

    // Start is called before the first frame update
    private void Awake()
    {
        shieldInstance = Instantiate(shield, new Vector3(0, 0, 0), Quaternion.identity);
        shieldInstance.transform.rotation = transform.rotation;
        shieldInstance.SetActive(false);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
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
    
    /*
     * Generates a random number within a certain value range,
     * excluding a specified number
     */
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
            // The target of the Shield Target's shield ability
            int guardTarget = 0;

            // Chooses a random target to shield from the list,
            if (spawner != null && spawner.positions.Count > 1)
            {
                Debug.Log("Count: " + spawner.positions.Count);
                guardTarget = Random.Range(0, spawner.positions.Count);
            }
            // Pauses for three seconds if it's the first spawned target
            // or not associated with a target spawner
            else
            {
                yield return new WaitForSeconds(3f);
                print("We ON!");
            }
            // If the chosen target is this Shield Target, 
            // then choose another position
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
