using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MyAStarAi : MonoBehaviour
{
    public Transform target;
    public Path path;
    public float speed = 3f;
    CharacterController cc;
    Seeker seeker;
    float nextWaypointDistance = 3;
    int currentWaypoint = 0;
    bool reachedEndOfPath;
    void Start()
    {
        seeker = GetComponent<Seeker>();
        cc = GetComponent<CharacterController>();
        seeker.pathCallback += OnPathComplete;
        seeker.StartPath(transform.position, target.position);
   
      
    }

    private void Update()
    {
        if (path == null) return;
        reachedEndOfPath=false;
        float distanceToWaypoint;
        while (true)
        {
            distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distanceToWaypoint < nextWaypointDistance)
            {
                if (currentWaypoint + 1 < path.vectorPath.Count)
                {
                    currentWaypoint++;
                }
                else
                {
                    reachedEndOfPath = true;
                    break;
                }
            }
            else
            {
                break;
            }
        }       
        float speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;       
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        Vector3 velocity = dir * speed * speedFactor;
        cc.SimpleMove(velocity);      
    }
    public void OnPathComplete(Path p)
    {
        Debug.Log("done,have error?" + p.error);
        Debug.Log(p.vectorPath.Count);
        Debug.Log(p.path.Count);
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    //禁用或者销毁脚本时，回调并不会移除，防止回调异常，将回调移除
    private void OnDisable()
    {
        seeker.pathCallback -= OnPathComplete;
    }
}
