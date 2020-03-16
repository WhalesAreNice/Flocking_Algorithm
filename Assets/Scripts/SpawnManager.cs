using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject fishPrefab;
    public GameObject bigFishPrefab;
    public GameObject turtlePrefab;
    public GameObject lavaPrefab;

    public List<GameObject> fishes;
    public GameObject turtle;
    public GameObject centerFish;
    public List<GameObject> obstacles;

    public GameObject groundObjects;
    int obstacleStartCount;

    Vector3 centerFishPosition;
    public Vector3 centerVelocity;
    bool debugLines = true;
    public Material orange;

    //times to spawn magma
    int magmaTime = 10;
    int magmaResetTime = 120;
    Vector3 magmaVelocity = new Vector3(0f, 0.03f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        fishes = new List<GameObject>();
        obstacles = new List<GameObject>();
        obstacles.Add(GameObject.Find("humpback"));

        AddGroundObstacles();

        obstacleStartCount = obstacles.Count;

        centerFish = Instantiate(bigFishPrefab, Vector3.zero, Quaternion.identity);
        centerFish.transform.localScale *= 3;
        centerVelocity = Vector3.zero;
        centerFishPosition = Vector3.zero;

        for (int i = 0; i < 50; i++)
        {
            SpawnFish();
        }
    }

    //helper method to add all the ground objects to obstacles to avoid
    void AddGroundObstacles()
    {
        for(int i = 0; i < groundObjects.transform.childCount; i++)
        {
            obstacles.Add(groundObjects.transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            SpawnFish();

        //checks when to spawn magma
        magmaTime--;
        if (magmaTime <= 0)
        {
            SpawnMagma();
            magmaTime += magmaResetTime;
        }
        LavalMovement();
        ObstacleDestroy();
        ChangeCenterFishPosition();

        if (Input.GetKeyDown(KeyCode.D))
        {
            debugLines = !debugLines;
            centerFish.SetActive(!centerFish.active);
        }
    }

    void ObstacleDestroy()
    {
        if (obstacles.Count > obstacleStartCount)
        {
            //start the cycle at obstacleStartCount because I don't want any chance to delete the whale or ground objects
            for (int i = 1; i < obstacles.Count; i++)
            {
                //checks to see if the lava gets high enough to be at the ceiling, if so destroy it
                if (obstacles[i].transform.position.y >= 100f)
                {
                    Destroy(obstacles[i]);
                    obstacles.RemoveAt(i);
                }
            }
        }
    }

    void LavalMovement()
    {
        if (obstacles.Count > 1)
        {
            //start the cycle at obstacleStartCount because I don't want any chance to delete the whale or ground objects
            for (int i = obstacleStartCount; i < obstacles.Count; i++)
            {
                obstacles[i].transform.position += magmaVelocity;
            }
        }
    }

    void SpawnMagma()
    {
        //randomly spawn magma and add them to the obstacles list
        float xAxis = Random.Range(-40f, 40f);
        float zAxis = Random.Range(-40f, 40f);
        GameObject newLava = Instantiate(lavaPrefab, new Vector3(xAxis, -1f, zAxis), Quaternion.identity);
        obstacles.Add(newLava);
    }

    void SpawnFish()
    {
        //randomly spawn fishes and add them to the fishes list
        float xAxis = Random.Range(-40f, 40f);
        float yAxis = Random.Range(10f, 80f);
        float zAxis = Random.Range(-40f, 40f);
        GameObject newFish = Instantiate(fishPrefab, new Vector3(xAxis, yAxis, zAxis), Quaternion.identity);
        fishes.Add(newFish);
    }

    void ChangeCenterFishPosition()
    {
        centerVelocity = Vector3.zero;

        //get the position and velocity vector needed to change the center fish's rotation and position
        for (int i = 0; i < fishes.Count; i++)
        {
            centerFish.transform.position += fishes[i].transform.position;
            centerVelocity += fishes[i].GetComponent<Fish>().velocity;
        }
        //changes the position and the way it is facing determined by the other fishes
        centerFish.transform.position /= fishes.Count;
        centerFish.transform.rotation = Quaternion.LookRotation(centerVelocity.normalized, Vector3.up);
    }


    private void OnRenderObject()
    {
        if (debugLines)
        {
            //GL line for forward vector
           orange.SetPass(0);
           GL.Begin(GL.LINES);
           GL.Vertex(centerFish.transform.position);
           GL.Vertex(centerFish.transform.position + centerVelocity.normalized * 20);
           GL.End();
           
        }
    }
}
