using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProjectileGun : MonoBehaviour
{
    public GameObject bullet;
    public GameObject crosshair;

    [Header("Projectile Forces")]
    // "Forward" force for projectile
    [SerializeField]
    private float shootForce;
    // Upward force for projectile. Mainly meant for items like grenades
    [SerializeField]
    private float upwardForce;

    [Header("Bullet Attributes")]
    [SerializeField]
    private float timeBetweenShooting;
    // Bullet spread
    [SerializeField]
    private float spread;
    private float reloadTime;
    private float timeBetweenShots;
    [SerializeField]
    private int magazineSize;
    [SerializeField]
    private int bulletsPerTap;
    // Turns gun from semi-automatic to automatic
    [SerializeField]
    private bool allowButtonHold;
    private int bulletsLeft;
    private int bulletsShot;

    private bool shooting;
    private bool readyToShoot;
    private bool reloading;

    [Header("Camera/Focal Point")]
    public Camera fpsCam;
    // Point from which the bullet leaves the gun
    public Transform attackPoint;

    // Allows for a reload to be performed
    private bool allowInvoke = true;

    [Header("Keybindings")]
    [SerializeField] KeyCode shootKey = KeyCode.Mouse0;
    [SerializeField] KeyCode reloadKey = KeyCode.R;

    [Header("Graphics")]
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] TextMeshProUGUI ammunitionDisplay;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void MyInput()
    {
        // Says the player is shooting for the entire time they hold down the shoot key
        if (allowButtonHold)
        {
            shooting = Input.GetKey(shootKey);
        }
        // Only allows shots to be taken per key press
        else
        {
            shooting = Input.GetKeyDown(shootKey);
        }

        // Reloads gun 
        if(Input.GetKeyDown(reloadKey) && bulletsLeft < magazineSize && !reloading)
        {
            Reload();
        }

        //Reload if shooting with no bullets
        if(readyToShoot && shooting && !reloading && bulletsLeft <= 0)
        {
            Reload();
        }

        // Shoots the gun
        if(readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;

            Shoot();
        }
    }

    void Shoot()
    {
        readyToShoot = false;

        // Creates a ray pointing at the center of the screen 
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // middle of screen
        RaycastHit hit;

        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point; //hit "something" in front of the player
        }
        else
        {
            targetPoint = ray.GetPoint(75f); //hit the air, so we hit a point a ways from the player
        }

        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        //Calculate spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate new direction with spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0); 
        
        GameObject currentBullet = ObjectPoolingManager.Instance.GetBullet();
        currentBullet.transform.position = attackPoint.position;
        currentBullet.transform.forward = directionWithSpread.normalized;
        //Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse); //not needed for normal bullets, moreso for boucning grenades
        
        if(muzzleFlash != null)
        {
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        }

        bulletsLeft--;
        bulletsShot++;

        //Invoke resetShot function (if not already)
        if(allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

        //if more than one bulletsPerTap make sure to repeat shoot function
        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();

        if(ammunitionDisplay != null)
        {
            ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
        }
    }
}
