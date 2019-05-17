using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPopupTrigger : MonoBehaviour
{

public Text PopupText;    
public float destroyTimer;

    void Start()
    {
        PopupText.GetComponent<Text>().enabled = false;
    }
 void Update()
 {
     
            if(Input.GetKeyDown(KeyCode.E) && PopupText.isActiveAndEnabled)
            {
                PopupText.GetComponent<Text>().enabled = false;
            }
 }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PopupText.GetComponent<Text>().enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            PopupText.GetComponent<Text>().enabled = false;
            Debug.Log("Destorying "+this.gameObject.name+" in "+destroyTimer+" seconds.");
            Destroy(gameObject, destroyTimer);
        }
    }
}
