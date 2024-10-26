using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange; //Checks if player is in range

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }

    private void Update()
    {
       if (playerInRange) 
       {
        visualCue.SetActive(true);
        if(InputManager.GetInstance().GetInteractPressed())
        {
            DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
        }

       }
       else
       {
        visualCue.SetActive(false);
       }
    }

    private void OnTriggerEnter(Collider collide)
    {
        if (collide.gameObject.tag == "Player")
        {
            playerInRange = true;
           
        }
    }

    private void OnTriggerExit(Collider collide)
    {
          if (collide.gameObject.tag == "Player")
        {
            playerInRange = false;
            
        }
    }
}
