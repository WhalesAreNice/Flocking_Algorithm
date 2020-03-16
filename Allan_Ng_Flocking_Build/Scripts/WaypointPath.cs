using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointPath : MonoBehaviour
{
    public GameObject bubblesPrefab;
    public List<GameObject> waypoints = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < waypoints.Count; i++)
        {
            //sets the next index so the particle can look in that direction
            int nextIndex = i + 1;
            if (nextIndex == waypoints.Count)
                nextIndex = 0;

            //creates the bubbles and makes it look at the direction of the next waypoint
            Vector3 lookVector = waypoints[nextIndex].transform.position - waypoints[i].transform.position;
            GameObject newBubble = Instantiate(bubblesPrefab, waypoints[i].transform.position, Quaternion.identity);
            newBubble.transform.rotation = Quaternion.LookRotation(lookVector.normalized, Vector3.up);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
