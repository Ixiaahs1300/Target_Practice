using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    // Start is called before the first frame update

    // Singleton instance of OPM
    public static ObjectPoolingManager instance;

    // Singleton getter method of instance
    public static ObjectPoolingManager Instance { get { return instance; } }

    // Bullet prefab being used 
    [SerializeField]
    private GameObject bulletPrefab;
    // The amount of bullets in the pool 
    [SerializeField]
    private int bulletAmount = 20;

    // General projectile prefab being used
    [SerializeField]
    private GameObject projectilePrefab;

    // The amount of projectiles in the pool
    [SerializeField]
    private int projectileAmount = 10;

    // Player's bullets
    private List<GameObject> bullets;
    // Targets' projectiles
    private List<GameObject> projectiles;

    private void Awake()
    {
        // Instantiate Singleton instance
        instance = this;

        
        bullets = new List<GameObject>(bulletAmount);
        projectiles = new List<GameObject>(projectileAmount);

        //Preload bullets
        for (int i = 0; i < bulletAmount; i++)
        {
            GameObject prefabInstance = Instantiate(bulletPrefab);
            prefabInstance.transform.SetParent(transform);
            prefabInstance.SetActive(false);

            bullets.Add(prefabInstance);
        }

        // Preload projectiles
        foreach (GameObject proj in projectiles)
        {
            GameObject prefabInstance = Instantiate(projectilePrefab);
            // Assigns OPM as parent for hierarchy organization 
            prefabInstance.transform.SetParent(transform);
            prefabInstance.SetActive(false);

            projectiles.Add(prefabInstance);
        }

    }


    /*
     * Returns a bullet prefab from the bullets list.
     * If all bullets are active in the hierarchy then
     * a new bujllet is created and added to the list.
     */
    public GameObject GetBullet()
    {
        // Checks if any bullets are inactive in the hierarchy.
        // If so, returns one.
        foreach (GameObject bullet in bullets) 
        {
            if(!bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                return bullet;
            }
        }

        // Creates new bullet, adds it to the list, and then
        // returns it.
        GameObject prefabInstance = Instantiate(bulletPrefab);
        prefabInstance.transform.SetParent(transform);

        bullets.Add(prefabInstance);

        return prefabInstance;
    }

    public GameObject GetProjectile()
    {
        // Checks if any projectiles are inactive in the hierarchy.
        // If so, returns one.
        foreach (GameObject proj in projectiles)
        {
            if (!proj.activeInHierarchy)
            {
                proj.SetActive(true);
                return proj;
            }
        }

        // Creates new projectile, adds it to the list, and then
        // returns it.
        GameObject prefabInstance = Instantiate(projectilePrefab);
        prefabInstance.transform.SetParent(transform);

        projectiles.Add(prefabInstance);

        return prefabInstance;
    }
}
