using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDoor : MonoBehaviour
{
    public List<GameObject> PuzzlePieces;
    public bool Passed = false;
    Animator open;
    AudioSource key;

    private void Awake()
    {
        this.gameObject.tag = "Puzzle";
        open = GetComponent<Animator>();
        key = GetComponent<AudioSource>();
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
        yield return new WaitForSeconds (0.5f);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().PuzzleMode = false;
        Debug.Log("Passed");

        open.enabled = true;
        key.enabled = true;
        yield return new WaitForSeconds(3.0f);
        this.gameObject.SetActive(false);
        yield return null;

    }
}
