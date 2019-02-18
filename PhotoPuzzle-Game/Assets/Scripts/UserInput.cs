using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    public GameObject VideoCamera;
    public Component MainCam;
    private void Start()
    {
        MainCam = GetComponentInChildren<Camera>();
        VideoCamera.active = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            if(VideoCamera.active == false)
            {
                MainCam.GetComponent<Camera>().enabled = false;
                VideoCamera.active = true;
            }
            else
            {
                MainCam.GetComponent<Camera>().enabled = true;
                VideoCamera.active = false;
            }
        }
    }
}
