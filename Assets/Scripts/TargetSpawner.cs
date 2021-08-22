using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [Header("Target Types")]
    [SerializeField] private GameObject targetBase;
    [SerializeField] private GameObject targetBase_v2;
    [SerializeField] private GameObject targetProjectile;
    [SerializeField] private GameObject targetProjectile_v2;
    [SerializeField] private GameObject targetBlink;
    [SerializeField] private GameObject targetBlink_v2;
    [SerializeField] private GameObject targetShield;
    [SerializeField] private GameObject targetShield_v2;
    [SerializeField] private GameObject targetPursue;
    private GameObject chosenType;

    [Header("Positioning")]
    [SerializeField] BoxCollider col;
    private Vector3 center;
    public List<Vector3> positions;
    private Vector3 min;
    private Vector3 max;
    private float xAxis;
    private float yAxis;
    private float zAxis;
    private Vector3 randomPosition;

    [Header("Spawn Rates")]
    [SerializeField] private float percentBase;
    [SerializeField] private float percentProjectile;
    [SerializeField] private float percentBlink;
    [SerializeField] private float percentShield;
    [SerializeField] private float percentPursue;
    [SerializeField] private GameObject trigger;
    bool begun = false;

    bool CanInstantiate()
    {
        if (trigger.GetComponent<TargetTrigger>().canInstantiate)
        {
            return true;
        }

        return false;
    }

    void Awake()
    {
        center = col.center + transform.position;
        SetRanges();
    }

    private void SetRanges()
    {
        //Need to be updated so that it can be used with variable depth as well
        min = new Vector3(center.x - col.size.x / 2f + 1, center.y - col.size.y / 2f + 1, transform.position.z);
        max = new Vector3(center.x + col.size.x / 2f - 1, center.y + col.size.y / 2f - 1, transform.position.z);
    }

    private IEnumerator InstantiateRandomObjects()
    {
        //need to parent targets to spawner
        var target = Instantiate(targetBase, randomPosition, Quaternion.Euler(0f, 0f, 90f));
        //target.transform.SetParent(transform);
        target.GetComponent<TargetBase>().spawnIndex = positions.Count + 1;
        target.GetComponent<TargetBase>().spawner = this;
        positions.Add(randomPosition);
        yield return new WaitForSeconds(3f);
        begun = false;
    }

    void RollPosition()
    {
        //maybe Try to make targets appear, instead of haphazardly, within a "four square" space,
        //in others words make it appear in any of the 9 horizontal rows or 4 vertical columns
        xAxis = Random.Range(min.x, max.x);
        yAxis = Random.Range(min.y, max.y);
        zAxis = Random.Range(min.z, max.z);
    }

    bool TargetsIntersect(float xAxis, float yAxis, float zAxis, Vector3 pos)
    {
        //if ((xAxis + 1 >= pos.x - 1 && xAxis - 1 <= pos.x + 1) && yAxis + 1 <= pos.y )
        //Use equation for overlapping circles (2 is r(sub1) + r(sub)2 - the sum of the radii)
        if(Mathf.Sqrt(Mathf.Pow(xAxis-pos.x, 2f) + Mathf.Pow(yAxis - pos.y, 2f)) >= 2)
        {
            return false;
        }
        return true;
    }

    void Update()
    {
        
        RollPosition();
        //Keeps messing up after third or fourth target, they just pile on each other after that
        if(positions.Count != 0)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                if(TargetsIntersect(xAxis, yAxis, zAxis, positions[i]))
                {
                    RollPosition();
                    i = 0;
                }
            }
            randomPosition = new Vector3(xAxis, yAxis, zAxis);
        }
        else
        {
            randomPosition = new Vector3(xAxis, yAxis, zAxis);
        }
        
        if (CanInstantiate() && !begun)
        {
            begun = true;
            StartCoroutine("InstantiateRandomObjects");
        }

    }


}
