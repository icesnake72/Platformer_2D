using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollwer : MonoBehaviour
{
    [SerializeField]
    private GameObject[] wayPoints;
    private int curWayPointIdx = 0;

    [SerializeField]
    private float speed = 2f;

    private void Update()
    {
        if (Vector2.Distance(wayPoints[curWayPointIdx].transform.position, transform.position) < 0.1f)
        {
            curWayPointIdx++;
            if ( curWayPointIdx >= wayPoints.Length )
            {
                curWayPointIdx = 0;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, wayPoints[curWayPointIdx].transform.position, speed * Time.deltaTime);
    }
}
