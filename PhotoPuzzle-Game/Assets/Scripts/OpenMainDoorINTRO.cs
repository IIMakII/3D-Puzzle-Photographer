using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenMainDoorINTRO : MonoBehaviour
{
   [SerializeField] private bool _hasTriggered;

   private void Start()
   {
      _hasTriggered = false;
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.tag == "Player")
      {
         _hasTriggered = true;
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
      }
   }
}
