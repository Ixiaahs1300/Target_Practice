using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static ObjectPoolingManager instance;

    public static ObjectPoolingManager Instance { get { return instance; } }

    public GameObject bulletPrefab;
    public int bulletAmount = 20;

    public GameObject projectilePrefab;
    public int projectileAmount = 10;

    private List<GameObject> bullets;
    private List<GameObject> projectiles;

    private void Awake()
    {
        instance = this;

        //Preload bullets
        bullets = new List<GameObject>(bulletAmount);
        projectiles = new List<GameObject>(projectileAmount);

        for(int i = 0; i < bulletAmount; i++)
        {
            GameObject prefabInstance = Instantiate(bulletPrefab);
            prefabInstance.transform.SetParent(transform);
            prefabInstance.SetActive(false);

            bullets.Add(prefabInstance);
        }

        for (int i = 0; i < projectileAmount; i++)
        {
            GameObject prefabInstance = Instantiate(projectilePrefab);
            prefabInstance.transform.SetParent(transform);
            prefabInstance.SetActive(false);

            projectiles.Add(prefabInstance);
        }

    }

    public GameObject GetBullet()
    {
        foreach (GameObject bullet in bullets) 
        {
            if(!bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                //bullet.transform.localRotation = Quaternion.identity;
                //print("hi");
                return bullet;
            }
        }

        GameObject prefabInstance = Instantiate(bulletPrefab);
        prefabInstance.transform.SetParent(transform);

        bullets.Add(prefabInstance);

        return prefabInstance;
    }

    public GameObject GetProjectile()
    {
        foreach (GameObject proj in projectiles)
        {
            if (!proj.activeInHierarchy)
            {
                proj.SetActive(true);
                //bullet.transform.localRotation = Quaternion.identity;
                //print("hi");
                return proj;
            }
        }

        GameObject prefabInstance = Instantiate(projectilePrefab);
        prefabInstance.transform.SetParent(transform);

        projectiles.Add(prefabInstance);

        return prefabInstance;
    }
}
