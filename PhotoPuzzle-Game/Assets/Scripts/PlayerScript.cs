using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public int InteractRange = 4;
    float UITime = 0;
    bool HaveItems = false, wait = false, UIMenuActive = false,MoveCam = false;
    private int currentViewPiece = 0, rounds = 0, CurrentUIMenu = 0;
    [SerializeField] float SphereCastRadius = 0.5f,UITransitionTime = 1.5f;
    [SerializeField] GameObject showPieces;
    public bool InteractInRange = false, InPuzzleRange = false, PuzzleMode = false ;
    SphereCollider coll;
    public List<Text> UISelections, TempUISelections, UIMainMenu;
    //public List<GameObject> UIMainMenu;
    public List<GameObject> Inventory, answer;
    private GameObject IFCam, MainCam, UICanvas;
    RaycastHit hit;
    Vector3 ViewPieceScale = new Vector3(0.1f, 0.1f, 0.1f);
    Quaternion NeutralRot = new Quaternion(0, 0, 0,0);
    int layermask;
    

    void Start()
    {
        coll = GetComponent<SphereCollider>();
        coll.radius = InteractRange;
        IFCam = GetComponent<UserInput>().VideoCamera;
        MainCam = transform.Find("FirstPersonCharacter").gameObject;
        UICanvas = transform.Find("DigeticUICanvas").gameObject;
        layermask = 1 << 9;
    }

    // Update is called once per frame
    void Update()
    {
        if (PuzzleMode == false) 
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (Physics.SphereCast(MainCam.transform.position, SphereCastRadius, MainCam.transform.forward, out hit, InteractRange, layermask))
                {
                    if (hit.transform.tag == "Pickup_Cube" || hit.transform.tag == "Pickup_Cyclinder" || hit.transform.tag == "Pickup_Sphere"
                       || hit.transform.tag == "Pickup_EmptyRune" || hit.transform.tag == "Pickup_FRune" || hit.transform.tag == "Pickup_HRune"
                       || hit.transform.tag == "Pickup_HouseRune" || hit.transform.tag == "Pickup_MRune" || hit.transform.tag == "Pickup_TRune"
                       || hit.transform.tag == "Pickup_ThornRune")
                    {
                        Inventory.Add(hit.transform.gameObject);
                        hit.transform.GetComponent<Rigidbody>().useGravity = false;
                        hit.transform.GetComponent<Rigidbody>().freezeRotation = true;
                        hit.transform.rotation = NeutralRot;
                        hit.transform.localScale = ViewPieceScale;
                        hit.transform.gameObject.SetActive(false);
                    }

                    if (hit.transform.tag == "Puzzle")
                    {
                        PuzzleMode = true;
                        wait = false;
                    }
                    Debug.Log("object is " + hit.transform.gameObject.name);
                }


            }
        }
      if ( IFCam.activeInHierarchy == false)
        {
            if (MoveCam == true)
            {
                UITime += (1 * Time.deltaTime) / UITransitionTime;

                if (UIMenuActive == true)
                {
                     CurrentUIMenu = 0;
                     float x = Mathf.Lerp(MainCam.transform.rotation.x, 27, UITime);
                     MainCam.transform.localEulerAngles = new Vector3 (x, 0, 0);
                }

                else
                {
                    float x = Mathf.Lerp(27, 0, UITime);
                    MainCam.transform.localEulerAngles = new Vector3(x, 0, 0);
                }
            }

            if (PuzzleMode == false)
            {

                if(Input.GetKeyDown(KeyCode.Tab))
                {
                    if (MoveCam == false)
                    {
                        UITime = 0;
                        UIMenuActive = !UIMenuActive;
                        MoveCam = true;
                        StartCoroutine(SwitchingUIToggle());
                    }

                }

            }
          
            if (UIMenuActive == true)
            {
                if (MoveCam == false)
                {
                    if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        if(UISelections.Count > 1)
                        {

                            UISelections[CurrentUIMenu].fontSize = 14;
                            CurrentUIMenu = (CurrentUIMenu + 1) % UISelections.Count;
                            UISelections[CurrentUIMenu].fontSize = 16;
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        if (CurrentUIMenu > 0)
                        {
                            if(UISelections.Count > 1)
                            {
                                UISelections[CurrentUIMenu].fontSize = 14;
                                CurrentUIMenu--;
                                UISelections[CurrentUIMenu].fontSize = 16;
                            }
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        foreach (Text obj in UISelections)
                        {
                            obj.gameObject.SetActive(false);
                        }
                        UISelections.Clear();
                        UISelections.AddRange(UIMainMenu);
                        foreach (Text obj in UISelections)
                        {
                            obj.gameObject.SetActive(true);
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        Debug.Log("current ui is " + CurrentUIMenu);
                        foreach (Text obj in UISelections)
                        {
                            obj.gameObject.SetActive(false);
                        }
                        TempUISelections.AddRange(UISelections[CurrentUIMenu].transform.GetComponent<UIObjectScript>().ListOfTransitions);
                        UISelections.Clear();
                        UISelections.AddRange(TempUISelections);
                        TempUISelections.Clear();
                        foreach (Text obj in UISelections)
                        {
                            obj.gameObject.SetActive(true);
                        }
                    }

                }
            }

            if(PuzzleMode == true)
            {
                if(rounds == hit.transform.GetComponent<PuzzleDoor>().PuzzlePieces.Count)
                {
                    rounds = 0;
                    HaveItems = false;
                    PuzzleMode = false;
                    wait = false;
                    GetComponentInParent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
                    hit.transform.GetComponent<PuzzleDoor>().Passed = true;

                    for (int x = 0; x< hit.transform.GetComponent<PuzzleDoor>().PuzzlePieces.Count; x++)
                    {
                       if(answer[x].gameObject.tag != hit.transform.GetComponent<PuzzleDoor>().PuzzlePieces[x].gameObject.tag)
                       {
                            hit.transform.GetComponent<PuzzleDoor>().Passed = false;
                            Debug.Log("failed");
                       }
                    }

                    if (hit.transform.GetComponent<PuzzleDoor>().Passed == false)
                    {
                       foreach (GameObject obj in answer)
                        {
                            Inventory.Add(obj);
                        }
                       
                    }
                    answer.Clear();

                }
                else if (Inventory.Count >= hit.transform.GetComponent<PuzzleDoor>().PuzzlePieces.Count)
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
                    if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        Inventory[currentViewPiece].SetActive(false);
                        currentViewPiece--;
                        Debug.Log("current view number is " + currentViewPiece);
                        Debug.Log("Invent capp is " + Inventory.Count);
                    }

                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        if(answer.Count == 0 )
                        {
                            rounds = 0;
                            wait = false;
                            HaveItems = false;
                            PuzzleMode = false;
                            GetComponentInParent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
                            Inventory[currentViewPiece].SetActive(false);
                        }
                        else
                        {
                            rounds--;
                            int x = (answer.Count - 1);
                            Inventory.Add(answer[x]);
                            answer.RemoveAt(x);
                            Inventory[currentViewPiece].SetActive(false);
                            currentViewPiece = 0;
                        }
                    }

                   if(wait == true)
                    {
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            answer.Add(Inventory[currentViewPiece]);
                            Inventory[currentViewPiece].SetActive(false);
                            Inventory.RemoveAt(currentViewPiece);
                            currentViewPiece = 0;
                            Debug.Log("piece added");
                            rounds++;
                        }
                    }
                }
                wait = true;
            }
        }
    }

    IEnumerator SwitchingUIToggle()
    {
        if (UIMenuActive == true)
        {
            GetComponentInParent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;

            yield return new WaitForSeconds(UITransitionTime);
            UICanvas.SetActive(true);
            MoveCam = false;
            UISelections.AddRange(UIMainMenu);
            foreach (Text obj  in UISelections)
            {
                obj.gameObject.SetActive(true);
            }
            UISelections[CurrentUIMenu].fontSize = 16;
        }

        else
        {
            UISelections[CurrentUIMenu].fontSize = 14;
            UISelections.Clear();
            UICanvas.SetActive(false);
            yield return new WaitForSeconds(UITransitionTime);

            GetComponentInParent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
            MoveCam = false;
        }
    }

    public void PuzzleModeAction()
    {
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
