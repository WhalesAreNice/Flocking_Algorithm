using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowerCamera : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject whale;
    Whale whaleScript;
    GameObject whaleObject;

    // Start is called before the first frame update
    void Start()
    {
        whale = GameObject.Find("humpbackholder");
        whaleObject = GameObject.Find("humpback");
        whaleScript = whale.GetComponent<Whale>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 test = new Vector3(90f, whaleScript.velocity.y, whaleScript.velocity.z);
        transform.position = whaleObject.transform.position;
        transform.position += new Vector3(0f, 30f, 0f);
        transform.eulerAngles = test;
    }
}
