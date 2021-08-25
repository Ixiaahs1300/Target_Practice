using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeflectorShield : MonoBehaviour
{
    [SerializeField] KeyCode shieldKey = KeyCode.Mouse1;
    [SerializeField] GameObject shield;
    private float maxShieldSize;
    [SerializeField] private float shieldShrinkTime = 1f;
    [SerializeField] private float shieldGrowTime = 1f;
    private Vector3 shieldShrinkVelocity = new Vector3(0f, 0f, 0f);
    private Vector3 shieldGrowVelocity = new Vector3(0f, 0f, 0f);
    private bool isShieldShrinking;
    [SerializeField] private float deflectForce = 30f;
    Vector3 relVel;

    // Start is called before the first frame update
    void Awake()
    {
        isShieldShrinking = false;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (shield.activeInHierarchy && other.gameObject.tag.Equals("Projectile"))
        {
            //other.transform.GetComponent<Rigidbody>().AddForce(transform.forward * deflectForce, ForceMode.Impulse);
            Transform otherTrans = other.transform;
            otherTrans.GetComponent<Rigidbody>().AddForce(otherTrans.forward * deflectForce * -1, ForceMode.Impulse);
        }
    }
    

    void ActivateShield()
    {
        if(Input.GetKey(shieldKey))
        {
            isShieldShrinking = true;
            shield.SetActive(true);
        }
        else
        {
            isShieldShrinking = false;
            shield.SetActive(false);
        }
    }

    IEnumerator ShrinkShield()
    {
        while (shield.transform.localScale.y >= 0 && shield.transform.localScale.z >= 0)
        {
            if (isShieldShrinking)
            {
                shield.transform.localScale = Vector3.SmoothDamp(shield.transform.localScale, new Vector3(shield.transform.localScale.x, shield.transform.localScale.y - 0.1f, shield.transform.localScale.z - 0.1f), ref shieldShrinkVelocity, shieldShrinkTime);
                if(shield.transform.localScale.y < 0 || shield.transform.localScale.z < 0)
                {
                    shield.transform.localScale = new Vector3(shield.transform.localScale.x, 0f, 0f);
                }
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                break;
            }
        }
        
    }

    IEnumerator GrowShield()
    {
        while (shield.transform.localScale.y <= 0.75f && shield.transform.localScale.z <= 0.75f)
        {
            if (!isShieldShrinking)
            {
                //energyShield.transform.localScale = Vector3.Lerp(energyShield.transform.localScale, new Vector3(shieldScale, shieldScale, 1f), 1000f);
                shield.transform.localScale = Vector3.SmoothDamp(shield.transform.localScale, new Vector3(shield.transform.localScale.x, shield.transform.localScale.y + 0.05f, shield.transform.localScale.z + 0.05f), ref shieldGrowVelocity, shieldGrowTime);
                if (shield.transform.localScale.y > 0.75f || shield.transform.localScale.z > 0.75f)
                {
                    shield.transform.localScale = new Vector3(shield.transform.localScale.x, 0.75f, 0.75f);
                }
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        ActivateShield();
        StartCoroutine(ShrinkShield());
        StartCoroutine(GrowShield());
    }
}
