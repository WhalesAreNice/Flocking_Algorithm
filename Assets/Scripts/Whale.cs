using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whale : Vehicle
{
    public List<GameObject> waypoints = new List<GameObject>();
    int currentIndex;

    bool debugLines = true;
    public Material green;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        currentIndex = 0;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.D))
        {
            debugLines = !debugLines;
        }
    }

    public override void CalcSteeringForces()
    {
        Vector3 ultimateForce = Vector3.zero;
        //find the distance between whale and the currentWaypoint
        float dist = Vector3.Distance(waypoints[currentIndex].transform.position, transform.position);
        //if the whale is within a certain distance of the current waypoint change waypoints to the next
        if (dist < 7f)
        {
            currentIndex++;
        }
        //resets the currentIndex so the whale follows the waypoints from the first one again
        if(currentIndex >= 24)
        {
            currentIndex = 0;
        }

        ultimateForce += Seek(waypoints[currentIndex].transform.position);

        ultimateForce.Normalize();
        ultimateForce *= maxSpeed;
        ApplyForce(ultimateForce);
    }

    public override Vector3 ObstacleAvoidance()
    {
        Vector3 steeringForce = Vector3.zero;
        return steeringForce;
    }

    private void OnRenderObject()
    {
        if (debugLines)
        {
            int previousIndex = currentIndex -1;
            if(previousIndex <= 0)
            {
                previousIndex = 23;
            }

            //GL line for waypoints
            green.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Vertex(waypoints[currentIndex].transform.position);
            GL.Vertex(waypoints[previousIndex].transform.position);
            GL.End();
        }
    }
}
