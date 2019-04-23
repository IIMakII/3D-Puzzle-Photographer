using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryManager : MonoBehaviour
{
    [SerializeField] private bool _canUseCamera;
    [SerializeField] private float BatteryLife;
    [SerializeField] private GameObject VideoCameraEffect;

    /*
     * This is only a preliminary Battery Manager
     * BUG: When battery runs out, Ghost Layer is visible through the normal camera lens.
     * 
     */
    private void Update()
    {
        if (BatteryLife > 1)
        {
            _canUseCamera = true;
        }
        else
        {
            _canUseCamera = false;
        }
        if (_canUseCamera == false)
        {
            VideoCameraEffect.GetComponentInChildren<CameraEffect>().m_Enable = false;
        }
        else 
        {
            VideoCameraEffect.GetComponentInChildren<CameraEffect>().m_Enable = true;
        }
    }
}
