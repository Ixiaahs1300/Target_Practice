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
    
    private List<GameObject> bullets;

    private void Awake()
    {
        instance = this;

        //Preload bullets
        bullets = new List<GameObject>(bulletAmount);

        for(int i = 0; i < bulletAmount; i++)
        {
            GameObject prefabInstance = Instantiate(bulletPrefab);
            prefabInstance.transform.SetParent(transform);
            prefabInstance.SetActive(false);

            bullets.Add(prefabInstance);
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
}
