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

        /*
        current: 현재 위치 벡터입니다. 이 벡터에서 출발하여 이동할 것입니다.
        target: 목표 위치 벡터입니다. current에서 target으로 이동할 것입니다.
        maxDistanceDelta: 최대 이동 거리입니다. 이 값은 current에서 target로의 이동이 이 값 이내로 제한되도록 합니다.
        Vector2.MoveTowards() 메소드는 current에서 target로 선형 보간을 수행하여 최대 이동 거리인 maxDistanceDelta 이내의 이동 벡터를 반환합니다. 이를 통해 오브젝트가 목표 위치로 이동하는 것을 부드럽게 제어할 수 있습니다. 
        */
        transform.position = Vector2.MoveTowards(transform.position, wayPoints[curWayPointIdx].transform.position, speed * Time.deltaTime);
    }
}
