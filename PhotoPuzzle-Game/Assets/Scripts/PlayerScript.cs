using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public int InteractRange = 4;
    [SerializeField] float SphereCastRadius = 0.5f;
    private Camera cam;
    public bool InteractInRange = false, InPuzzleRange = false, PuzzleMode = false;
    SphereCollider coll;
    public List<GameObject> Inventory;
    public GameObject pickableObject, puzzleObject;
    private GameObject IFCam;
    RaycastHit hit;

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
      if ( IFCam.activeInHierarchy == false)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (Physics.SphereCast(cam.transform.position, SphereCastRadius, cam.transform.forward, out hit, InteractRange, 9))
                {
                    if(hit.transform.tag == "Pickup")
                    {
                        Inventory.Add(hit.transform.gameObject);
                        hit.transform.gameObject.SetActive(false);
                    }

                
                    if (hit.transform.tag == "Puzzle")
                    {
                        PuzzleModeAction();
                    }
                }
                Debug.Log("object is " + hit.transform.gameObject.name);
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
            
            if(hit.transform.GetComponent<PuzzleDoor>().PuzzlePieces.Count == Inventory.Count)
            {
                Debug.Log("same capacity");
                hit.transform.GetComponent<PuzzleDoor>().Passed = true;
            }
        }

        if(PuzzleMode == false)
        {
            GetComponentInParent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
            Debug.Log("can move");
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, InteractRange);
        Debug.Log("interact range " + InteractRange);
    }
}
