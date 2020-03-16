using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : Vehicle
{
    public GameObject spawnManager;
    SpawnManager SMScript;

    //areas to seek to make them look like they're moving around 
    Vector3 center = new Vector3(0f, 35f, 0f);
    Vector3 point1 = new Vector3(40f, 70f, -40f);
    Vector3 point2 = new Vector3(40f, 70f, 40f);
    Vector3 point3 = new Vector3(40f, 20f, -40f);
    Vector3 point4 = new Vector3(40f, 20f, 40f);
    Vector3 point5 = new Vector3(-40f, 70f, -40f);
    Vector3 point6 = new Vector3(-40f, 70f, 40f);
    Vector3 point7 = new Vector3(-40f, 20f, -40f);
    Vector3 point8 = new Vector3(-40f, 20f, 40f);

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        spawnManager = GameObject.Find("SpawnManager");
        SMScript = spawnManager.GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void CalcSteeringForces()
    {
        Vector3 ultimateForce = Vector3.zero;

        //getting distances from each seeking point
        float distCenter = Vector3.Distance((transform.position), center);
        float distPoint1 = Vector3.Distance((transform.position), point1);
        float distPoint2 = Vector3.Distance((transform.position), point2);
        float distPoint3 = Vector3.Distance((transform.position), point3);
        float distPoint4 = Vector3.Distance((transform.position), point4);
        float distPoint5 = Vector3.Distance((transform.position), point5);
        float distPoint6 = Vector3.Distance((transform.position), point6);
        float distPoint7 = Vector3.Distance((transform.position), point7);
        float distPoint8 = Vector3.Distance((transform.position), point8);

        ultimateForce = Vector3.zero;

        float scaleFactor = SMScript.fishes.Count / 50f;

        //logic to make the fish move in a general direction
        ultimateForce += Seek(center) * distCenter / 1000;
        if (distPoint1 > 80f)
            ultimateForce += Seek(point1) * distPoint1 / 100;
        if (distPoint2 > 80f)
            ultimateForce += Seek(point2) * distPoint2 / 100;
        if (distPoint3 > 70f)
            ultimateForce += Seek(point3) * distPoint3 / 300;
        if (distPoint4 > 80f)
            ultimateForce += Seek(point4) * distPoint4 / 250;
        if (distPoint5 > 80f)
            ultimateForce += Seek(point5) * distPoint5 / 100;
        if (distPoint6 > 80f)
            ultimateForce += Seek(point6) * distPoint6 / 100;
        if (distPoint7 > 80f)
            ultimateForce += Seek(point7) * distPoint7 / 250;
        if (distPoint8 > 80f)
            ultimateForce += Seek(point8) * distPoint8 / 300;

        ultimateForce += Alignment()*10* scaleFactor;
        ultimateForce += Cohesion()/2.5f/1.1f* scaleFactor;
        ultimateForce += ObstacleAvoidance() * 20000;
        ultimateForce += Seperation()*1000;
        ultimateForce.y /= 2;
        ultimateForce.Normalize();
        ultimateForce *= maxSpeed;
        ApplyForce(ultimateForce);
    }


    public Vector3 Seperation()
    {
        List<GameObject> fishes = SMScript.fishes;
        Vector3 steeringForce = Vector3.zero;
        if (fishes.Count > 0)
        {
            //checks each fish to see if they are within a certain distance from each other
            for (int i = 0; i < fishes.Count; i++)
            {
                float dist = Vector3.Distance(fishes[i].transform.position, transform.position);
                //if they are close enough add a fleeing force
                if (dist > 0 && dist < 3f)
                {
                    steeringForce += Flee(fishes[i]) / dist;
                }
            }
        }
        return steeringForce;
    }

    public Vector3 Alignment()
    {
        List<GameObject> fishes = SMScript.fishes;
        Vector3 steeringForce = Vector3.zero;
        if(fishes.Count > 0)
        {
            //adds the forward vector of each fish to the steering force
            for (int i = 0; i< fishes.Count; i++)
            {
                steeringForce += fishes[i].transform.forward;
            }
            //average out the steeringforce by dividing by the amount of fishes
            steeringForce /= fishes.Count;
        }

        return steeringForce;
    }

    public Vector3 Cohesion()
    {
        List<GameObject> fishes = SMScript.fishes;
        Vector3 centerPoint = Vector3.zero;
        if (fishes.Count > 0)
        {
            //adds each position of the fishes to the centerPoint
            for (int i = 0; i < fishes.Count; i++)
            {
                centerPoint += fishes[i].transform.position - transform.position;
            }
            //getting the centerPoint by dividing the vector by the amount of fish
            centerPoint /= fishes.Count;
        }
        return centerPoint;
    }

    public override Vector3 ObstacleAvoidance()
    {
        List<GameObject> obstacles = SMScript.obstacles;
        Vector3 finalForce = Vector3.zero;
        if (obstacles != null)
        {
            for (int i = 0; i < obstacles.Count; i++)
            {
                //info needed
                Vector3 steeringForce = Vector3.zero;
                Vector3 desiredVelocity = Vector3.zero;
                Vector3 VtoC = obstacles[i].transform.position - transform.position;
                float dotForward = Vector3.Dot(VtoC, transform.forward);
                float dotRight = Vector3.Dot(VtoC, transform.right);
                float radiiSum = 5f;
                float checkDist = Vector3.Distance((transform.position), transform.position);

                Debug.DrawLine(transform.position, (transform.position + direction * 6), Color.green);

                if (dotForward > 0)
                {
                    if (VtoC.magnitude < 10f)
                    {
                        if (radiiSum > Mathf.Abs(dotRight))
                        {
                            if (dotRight < 0)
                            {
                                desiredVelocity += (transform.right * maxSpeed);
                               //Debug.Log("right");
                            }
                            else
                            {
                                desiredVelocity += (-transform.right * maxSpeed);
                                //Debug.Log("left");
                            }
                        }
                    }
                }
                if (desiredVelocity != Vector3.zero)
                {
                    steeringForce = desiredVelocity - velocity;
                    finalForce += steeringForce;
                }
            }
        }
        return finalForce;
    }
}
