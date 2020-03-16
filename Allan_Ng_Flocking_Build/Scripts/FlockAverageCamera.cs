using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockAverageCamera : MonoBehaviour
{
    public GameObject spawnManager;
    SpawnManager SMScript;

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.Find("SpawnManager");
        SMScript = spawnManager.GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = SMScript.centerFish.transform.position;
        transform.position -= SMScript.centerVelocity.normalized * 20;
        transform.rotation = Quaternion.LookRotation(SMScript.centerVelocity.normalized, Vector3.up);
    }
}
