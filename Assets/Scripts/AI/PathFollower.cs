using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class PathFollower : MonoBehaviour
{
    public Transform PathHolder;
    public float Speed = 5.0f;
    public float TurnSpeed = 90.0f;
    public float WaitAtPointTime = 0.0f;
    public float CurrentSpeed;

    private void Start()
    {
        if (!PathHolder)
            PathHolder = GameObject.FindGameObjectWithTag("PathHolder").transform;

        Vector3[] waypoints = new Vector3[PathHolder.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = PathHolder.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, waypoints[i].y, waypoints[i].z);
        }

        transform.position = waypoints[0];

        StartCoroutine(FollowPath(waypoints));
    }

    IEnumerator TurnToFace(Vector3 lookTarget)
    {
        Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

        CurrentSpeed = 0.0f;
        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) >= 0.01f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, TurnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }

    IEnumerator FollowPath(Vector3[] waypoints)
    {
        transform.position = waypoints[0];
        int targetWaypointIndex = 0;
        Vector3 targetWaypoint = waypoints[targetWaypointIndex];
        transform.LookAt(targetWaypoint);

        while (true)
        {
            CurrentSpeed = Speed;
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, Speed * Time.deltaTime);
            if (transform.position == targetWaypoint)
            {
                targetWaypointIndex = targetWaypointIndex + 1;
                CurrentSpeed = 0.0f;
                if (targetWaypointIndex == waypoints.Length)
                {
                    CompletedPath();
                    yield break;
                }
                else
                {
                    targetWaypoint = waypoints[targetWaypointIndex];
                    yield return new WaitForSeconds(WaitAtPointTime);
                    yield return StartCoroutine(TurnToFace(targetWaypoint));
                }
            }
            yield return null;
        }
    }

    private void CompletedPath()
    {
        Debug.Log("Path Completed");
        LevelManager lm = GameObject.FindObjectOfType<LevelManager>();
        if (lm)
            lm.NextLevel();
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (Application.isEditor && PathHolder)
        {
            if (Camera.current == SceneView.lastActiveSceneView.camera || Camera.current == Camera.main)
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
            }
        }
    }
    #endif

}
