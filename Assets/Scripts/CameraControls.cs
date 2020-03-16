using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public Camera[] cameras;

    private int currentCameraIndex;

    // Start is called before the first frame update
    void Start()
    {
        currentCameraIndex = 0;

        //turn all cameras off, except the first default one
        for (int i = 1; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }

        //If any cameras were added to the controller, enable the first one
        if (cameras.Length > 0)
        {
            cameras[0].gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Press the 'C' key to cycle through cameras in the array
        if (Input.GetKeyDown(KeyCode.C))
        {
            //Cycle to the next camera
            currentCameraIndex++;

            //If cameraIndex is in bounds, set this camera active and last one inactive
            if (currentCameraIndex < cameras.Length)
            {
                cameras[currentCameraIndex - 1].gameObject.SetActive(false);
                cameras[currentCameraIndex].gameObject.SetActive(true);
            }
            //If last camera, cycle back to first camera
            else
            {
                cameras[currentCameraIndex - 1].gameObject.SetActive(false);
                currentCameraIndex = 0;
                cameras[currentCameraIndex].gameObject.SetActive(true);
            }
        }
    }

    void OnGUI()
    {
        string instructions = "Press 'F' to spawn a Fish.\nPress 'D' to toggle Debug Lines.\nPress 'C' to change camera views";
        string cameraNumber = "\nCamera " + (currentCameraIndex + 1);
        string cameraDiscription = "";

        switch (currentCameraIndex)
        {
            case 0: cameraDiscription = "\nWhole Environment"; break;
            case 1: cameraDiscription = "\nFlock Average"; break;
            case 2: cameraDiscription = "\nFlock Front"; break;
            case 3: cameraDiscription = "\nPath Follower"; break;
            default: break;
        }

        GUI.Box(new Rect(10, 10, 250, 80), instructions + cameraNumber + cameraDiscription);
    }
}
