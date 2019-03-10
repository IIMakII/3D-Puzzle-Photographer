using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public int InteractRange = 3;
    private Camera cam;
    public bool InteractInRange = false, InPuzzleRange = false, PuzzleMode = false;
    SphereCollider coll;
    public List<GameObject> Inventory;
    public GameObject pickableObject;

    void Start()
    {
        coll = GetComponent<SphereCollider>();
        cam = GetComponentInChildren<Camera>();
        coll.radius = InteractRange;
    }

    // Update is called once per frame
    void Update()
    {
        if(InteractInRange == true)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                if(pickableObject != null)
                {
                    Inventory.Add(pickableObject);
                    pickableObject.SetActive(false);
                }
                
                if(InPuzzleRange == true)
                {
                    PuzzleModeAction();
                }

            }
        }
    }

    void PuzzleModeAction()
    {
        PuzzleMode = !PuzzleMode;  
        if (PuzzleMode == true)
        {
            GetComponentInParent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
            Debug.Log("cant move");
        }

        if(PuzzleMode == false)
        {
            GetComponentInParent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
            Debug.Log("can move");
        }
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Pickup")
        {
            pickableObject = other.gameObject;
            InteractInRange = true;
        }

        if(other.tag == "Puzzle")
        {
            InPuzzleRange = true;
            InteractInRange = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        InteractInRange = false;

        if (other.tag == "Pickup")
        {
            pickableObject = null;
        }

        if (other.tag == "Puzzle")
        {
            InPuzzleRange = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, InteractRange);
    }
}
