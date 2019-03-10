using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDoor : MonoBehaviour
{
    public List<GameObject> PuzzlePieces;
    public bool Passed = false;

    private void Awake()
    {
        this.gameObject.tag = "Puzzle";
    }

    // Update is called once per frame
    void Update()
    {
        if (Passed == true)
        {
            Debug.Log("starting opening");
            StartCoroutine(Opening());
        }
    }

    IEnumerator Opening()
    {
        yield return new WaitForSeconds (1.5f);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().PuzzleModeAction();
        Debug.Log("Passed");

        this.gameObject.SetActive(false);
        yield return null;

    }
}
