using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    public Transform PathHolder;
    public float Speed = 5.0f;
    public float TurnSpeed = 90.0f;
    public float WaitAtPointTime = 0.0f;
    public bool LoopPath = false;

    private void Start()
    {
        if (!PathHolder)
            PathHolder = GameObject.FindGameObjectWithTag("PathHolder").transform;

        Vector3[] waypoints = new Vector3[PathHolder.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = PathHolder.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
        }

        transform.position = waypoints[0];

        StartCoroutine(FollowPath(waypoints));
    }

    IEnumerator TurnToFace(Vector3 lookTarget)
    {
        Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

        while (Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle) > 0.05f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, TurnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }

    IEnumerator FollowPath(Vector3[] waypoints)
    {
        transform.position = waypoints[0];
        int targetWaypointIndex = 1;
        Vector3 targetWaypoint = waypoints[targetWaypointIndex];
        transform.LookAt(targetWaypoint);

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, Speed * Time.deltaTime);
            if (transform.position == targetWaypoint)
            {
                int nextWaypointIndex = targetWaypointIndex + 1;
                if (!LoopPath && nextWaypointIndex == waypoints.Length)
                {
                    CompletedPath();
                    yield break;
                }
                targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
                targetWaypoint = waypoints[targetWaypointIndex];
                yield return new WaitForSeconds(WaitAtPointTime);
                yield return StartCoroutine(TurnToFace(targetWaypoint));
            }
            yield return null;
        }
    }

    private void CompletedPath()
    {
        Debug.Log("Path Completed");
    }

    private void OnDrawGizmos()
    {
        Vector3 startPos = PathHolder.GetChild(0).position;
        startPos.y += 0.3f;
        Vector3 prevPos = startPos;
        int waypointCount = PathHolder.childCount;

        for (int i = 0; i < waypointCount; i++)
        {
            Vector3 nextPos = PathHolder.GetChild(i).position;
            nextPos.y += 0.3f;
            if (i == 0)
            {
                Gizmos.DrawIcon(nextPos, "pathnode_start.tga", false);
            }
            else if (i == waypointCount - 1)
            {
                Gizmos.DrawIcon(nextPos, "pathnode_end.tga", false);
            }
            else
            {
                Gizmos.DrawIcon(nextPos, "pathnode.tga", false);
            }
            // 
            Gizmos.DrawLine(prevPos, nextPos);
            prevPos = nextPos;
        }

        if (LoopPath)
            Gizmos.DrawLine(prevPos, startPos);
    }

}
