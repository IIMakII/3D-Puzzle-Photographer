using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickable : MonoBehaviour
{
    [SerializeField] int playerRange = 4;
    public bool CheckPuzzleRange = false;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, playerRange);
        
        if(CheckPuzzleRange == true)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, (playerRange * 2) + 1);
        }
    }
}
