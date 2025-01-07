using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWP : MonoBehaviour
{
    public GameObject[] waypoints;
    int currentWP = 0;

    GameObject tracker;

    public float speed = 10.0f;
    public float rotSpeed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (waypoints.Length > 0)  // Ensuring there are waypoints to follow
        {
            tracker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            DestroyImmediate(tracker.GetComponent<Collider>());
            tracker.transform.position = this.transform.position;
            tracker.transform.rotation = this.transform.rotation;
        }
        else
        {
            Debug.LogWarning("No waypoints assigned!");
        }
        Debug.Log("Waypoints assigned: " + waypoints.Length);  // Debugging number of waypoints
    }

    void ProgressTracker()
    {
        if (waypoints.Length == 0)
        {
            return;  // If there are no waypoints, do nothing
        }

        // Check the distance to the current waypoint
        if (Vector3.Distance(this.transform.position, waypoints[currentWP].transform.position) < 3f)
        {
            currentWP++;  // Increment after checking the condition
        }

        // Check if currentWP is out of bounds, reset it to 0 if it is
        if (currentWP >= waypoints.Length)
        {
            currentWP = 0;
        }

        // Make the tracker look at the next waypoint
        tracker.transform.LookAt(waypoints[currentWP].transform);

        // Move the tracker towards the next waypoint based on speed
        float step = speed * Time.deltaTime;  // Calculate movement step
        tracker.transform.Translate(0, 0, step);  // Move the tracker
    }

    // Update is called once per frame
    void Update()
    {
        ProgressTracker();

        // Rotate the object towards the next waypoint based on rotSpeed
        if (waypoints.Length > 0)
        {
            Quaternion lookatWP = Quaternion.LookRotation(waypoints[currentWP].transform.position - this.transform.position);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookatWP, rotSpeed * Time.deltaTime);
        }

        // Move the object based on speed (if needed)
        // this.transform.Translate(0, 0, speed * Time.deltaTime);
    }
}
