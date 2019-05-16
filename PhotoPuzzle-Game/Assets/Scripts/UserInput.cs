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
        if(Input.GetKeyDown(KeyCode.F))
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

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (Application.isEditor)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
            else
            {
                Application.Quit();
            }
        }
    }
}
