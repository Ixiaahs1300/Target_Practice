using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFade : MonoBehaviour
{
    [SerializeField] 
    Light light;
    [SerializeField]
    bool fading = true;
    float lightLevel =  1.0f;
    
    private void Awake()
    {
        light = GetComponent<Light>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }


    IEnumerator TransitionLight()
    {
        //Fade in
        if(fading && light.intensity > 0)
        {
            light.intensity -= 0.01f;
            yield return new WaitForSeconds(2f);
            print("working");
        }
        else if(fading && light.intensity <= 0)
        {
            fading = false;
        }
        else if (!fading && light.intensity < 1)
        {
            light.intensity += 0.01f;
            yield return new WaitForSeconds(2f);
            print("working");
        }
        else if (!fading && light.intensity >= 1)
        {
            fading = true;
        }
        //Fade out


    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(TransitionLight());
        //light.intensity = Mathf.PingPong(Time.time, 1);
    }
}
