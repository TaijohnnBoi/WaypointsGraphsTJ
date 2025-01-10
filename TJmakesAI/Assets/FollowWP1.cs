using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWP1 : MonoBehaviour
{
    Transform goal;
    float speed = 5.0f;
    float accuracy = 5.0f;
    float rotSpeed = 2.0f;

    public GameObject wpManager;
    GameObject[] wps;
    GameObject currentNode;
    int currentWP = 0;
    Graph g;

    // Start is called before the first frame update
    void Start()
    {
        wps = wpManager.GetComponent<WPManager>().waypoints;
        g = wpManager.GetComponent<WPManager>().graph;
        currentNode = wps[0];
        Time.timeScale = 5;

        //Invoke("GoToRuin", 2);
    }
    public void GoToHeli()
    {
        g.AStar(currentNode, wps[0]);
        currentWP = 0;
    }

    public void GoToRuin()
    {
        g.AStar(currentNode, wps[1]);
        currentWP = 0;
    }

    public void GoToFactory()
    {
        g.AStar(currentNode, wps[7]);
        currentWP = 0;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (g.pathList.Count == 0 || currentWP == g.pathList.Count)
            return;

        if (Vector3.Distance(g.pathList[currentWP].getId().transform.position, this.transform.position) < accuracy)
        {
            currentNode = g.pathList[currentWP].getId();
            currentWP++;
        }

        if (currentWP != g.pathList.Count) // Corrected condition
        {
            goal = g.pathList[currentWP].getId().transform; // Ensure goal is properly set
            Vector3 lookAtGoal = new Vector3(goal.position.x, this.transform.position.y, goal.position.z);

            Vector3 direction = lookAtGoal - this.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Smoothly rotate towards the target
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, Time.deltaTime * rotSpeed);

            // Move forward
            this.transform.Translate(0, 0, speed * Time.deltaTime);
        }
    }
}
