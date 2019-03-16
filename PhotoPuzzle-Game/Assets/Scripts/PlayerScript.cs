using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public int InteractRange = 4;
    bool HaveItems = false,wait = false;
    private int currentViewPiece = 0, rounds = 0;
    [SerializeField] float SphereCastRadius = 0.5f;
    [SerializeField] GameObject showPieces;
    private Camera cam;
    public bool InteractInRange = false, InPuzzleRange = false, PuzzleMode = false;
    SphereCollider coll;
    public List<GameObject> Inventory, answer;
    private GameObject IFCam;
    RaycastHit hit;
    Vector3 ViewPieceScale = new Vector3(0.1f, 0.1f, 0.1f);

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
            if (PuzzleMode == false)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if(Physics.SphereCast(cam.transform.position, SphereCastRadius, cam.transform.forward, out hit, InteractRange, 9))
                    {
                        if (hit.transform.tag == "Pickup")
                        {
                            Inventory.Add(hit.transform.gameObject);
                            hit.transform.GetComponent<Rigidbody>().useGravity = false;
                            hit.transform.localScale = ViewPieceScale;
                            hit.transform.gameObject.SetActive(false);
                        }

                        if (hit.transform.tag == "Puzzle")
                        {
                            PuzzleMode = true;
                        }
                        Debug.Log("object is " + hit.transform.gameObject.name);
                    }


                }
            }
          
            if(PuzzleMode == true)
            {
                if(rounds == hit.transform.GetComponent<PuzzleDoor>().PuzzlePieces.Count)
                {
                    HaveItems = false;
                    PuzzleMode = false;
                    if(answer == hit.transform.GetComponent<PuzzleDoor>().PuzzlePieces)
                    {
                        hit.transform.GetComponent<PuzzleDoor>().Passed = true;
                        Debug.Log("correct answer");
                    }
                    else
                    {
                        Debug.Log("incorrect answer");
                    }
                }
                if (Inventory.Count >= hit.transform.GetComponent<PuzzleDoor>().PuzzlePieces.Count)
                {
                    HaveItems = true;
                }
                if(HaveItems == false)
                {
                    PuzzleMode = false;
                }

               
                if(HaveItems == true)
                {
                    if (currentViewPiece < 0)
                    {
                        currentViewPiece = 0;
                    }

                    GetComponentInParent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;

                    foreach (GameObject obj in Inventory)
                    {
                        obj.transform.position = showPieces.transform.position;
                    }
                    Inventory[currentViewPiece].SetActive(true);

                    if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        Inventory[currentViewPiece].SetActive(false);
                        currentViewPiece = (currentViewPiece + 1) % Inventory.Count;
                        Debug.Log("current view number is " + currentViewPiece);
                        Debug.Log("Invent capp is " + Inventory.Count);
                    }
                   if(wait == true)
                    {
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            answer.Add(Inventory[currentViewPiece]);
                            Inventory[currentViewPiece].SetActive(false);
                            Inventory.RemoveAt(currentViewPiece);
                            Debug.Log("piece added");
                            rounds++;
                        }
                    }
                }
                wait = true;
            }
        }
    }

    public void PuzzleModeAction()
    {

       
            
            
           /* if(hit.transform.GetComponent<PuzzleDoor>().PuzzlePieces.Count == Inventory.Count)
            {
                Debug.Log("same capacity");
                hit.transform.GetComponent<PuzzleDoor>().Passed = true;
            }
            */
        

        if(PuzzleMode == false)
        { 
            GetComponentInParent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
            foreach(GameObject obj in Inventory)
            {
                obj.SetActive(false);
            }
            Debug.Log("can move");
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, InteractRange);
        //Debug.Log("interact range " + InteractRange);
    }
}
