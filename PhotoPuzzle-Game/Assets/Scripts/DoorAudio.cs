﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAudio : MonoBehaviour
{

    AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.Play(0);
        Debug.Log("Creak");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
