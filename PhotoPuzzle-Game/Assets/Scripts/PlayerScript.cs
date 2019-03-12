using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public int InteractRange = 4;
    private Camera cam;
    public bool InteractInRange = false, InPuzzleRange = false, PuzzleMode = false;
    SphereCollider coll;
    public List<GameObject> Inventory;
    public GameObject pickableObject, puzzleObject;
    private GameObject IFCam;

    void Start()
    {
        coll = GetComponent<SphereCollider>();
        cam = GetComponentInChildren<Camera>();
        coll.radius = InteractRange;
        IFCam = GetComponent<UserInput>().VideoCamera;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 direction = (pickableObject.transform.position - this.transform.position).normalized;
        direction.y = 0;

        Quaternion rotate = Quaternion.LookRotation(direction);

        Debug.Log("rotate is " + rotate);

      if ( IFCam.activeInHierarchy == false)
        {
            if (InteractInRange == true)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (pickableObject != null)
                    {
                        Inventory.Add(pickableObject);
                        pickableObject.SetActive(false);
                        pickableObject = null;
                        InteractInRange = false;
                    }

                    if (InPuzzleRange == true)
                    {
                        PuzzleModeAction();
                    }

                }
            }
        }
    }

    public void PuzzleModeAction()
    {
        PuzzleMode = !PuzzleMode;  
        if (PuzzleMode == true)
        {
            GetComponentInParent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
            Debug.Log("cant move");
            
            if(puzzleObject.GetComponent<PuzzleDoor>().PuzzlePieces.Count == Inventory.Count)
            {
                Debug.Log("same capacity");
                puzzleObject.GetComponent<PuzzleDoor>().Passed = true;
            }
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
            puzzleObject = other.gameObject;
            InPuzzleRange = true;
            InteractInRange = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Pickup")
        {
            pickableObject = null;
            InteractInRange = false;
        }

        if (other.tag == "Puzzle")
        {
            InPuzzleRange = false;
            puzzleObject = null;
            InteractInRange = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, InteractRange);
        Debug.Log("interact range " + InteractRange);
    }
}
