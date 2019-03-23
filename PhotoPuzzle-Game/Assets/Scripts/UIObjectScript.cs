using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIObjectScript : MonoBehaviour
{
    public List<GameObject> ListOfTransitions;
    public GameObject cam;

    private void Start()
    {
        Vector3 direction = (cam.transform.position - this.transform.position).normalized;
        Quaternion rotate = Quaternion.LookRotation(direction);
        this.transform.rotation = rotate;
    }
}
