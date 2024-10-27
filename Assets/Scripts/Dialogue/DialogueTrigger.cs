using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Story")]
    [SerializeField] public Story charStory;

    private CharacterInfo charInfo;

    
    private bool playerInRange; //Checks if player is in range
    private GameHandler gameHandler;

    public void SetCharInfo(CharacterInfo charInfo)
    {
        this.charInfo = charInfo;
        charInfo.GetSpokenTo();
    }

    private void Awake()
    {
        // by default these are set to false so when you enter it later it becomes true
        playerInRange = false;
        visualCue.SetActive(false); 
        gameHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameHandler>();
    }

    private void Update()
    {
       if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying) 
       {
            visualCue.SetActive(true); // sets the visual cue to be active which is the icon above the players head
            if(InputManager.GetInstance().GetInteractPressed()) // if e is pressed
            {
                DialogueManager.GetInstance().EnterDialogueMode(charStory); // enter the inkJSON file into the dialogue box
                gameHandler.shouldAdvanceTime = true;
                gameHandler.AddClue(charInfo.GetId(), charInfo.GetClues()[charInfo.GetSpokenTo()]);
                charInfo.AddSpoken();

            } 

       }
       else
       {
            visualCue.SetActive(false); // hides the visual cue from your screen
       }
    }

    private void OnTriggerEnter(Collider collide)
    {
        if (collide.gameObject.tag == "Player")
        {
            playerInRange = true; // player in range when colliding with player tag
        }
    }

    private void OnTriggerExit(Collider collide)
    {
        if (collide.gameObject.tag == "Player")
        {
            playerInRange = false; // when players not in range set false
        }
    }
}
