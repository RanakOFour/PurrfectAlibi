using Ink.Runtime;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject m_visualCue;

    [Header("Story")]
    [SerializeField] public Story m_charStory;

    private bool m_playerInRange; //Checks if player is in range
    private GameController m_gameHandler;

    private void Awake()
    {
        // by default these are set to false so when you enter it later it becomes true
        m_playerInRange = false;
        m_visualCue.SetActive(false); 
        m_gameHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    private void Update()
    {
       if (m_playerInRange && !DialogueManager.GetInstance().m_dialogueIsPlaying) 
       {
            m_visualCue.SetActive(true); // sets the visual cue to be active which is the icon above the players head
            if(InputManager.GetInstance().GetInteractPressed()) // if e is pressed
            {
                DialogueManager.GetInstance().EnterDialogueMode(m_charStory); // enter the inkJSON file into the dialogue box
                m_gameHandler.AdvanceTime();

                /* TODO:
                 * 
                 * Redo how clues move from CharaterInfo (m_Story) to Notebook clues because of Dialogue Trigger
                 * Ideas:
                 *  - Set flag dialogueOver that, when true log to notebook through characterhandler
                */

                /* OLD CODE
                m_gameHandler.AddClue(m_charInfo.GetId(), charInfo.GetClues()[charInfo.GetSpokenTo()]);
                m_charInfo.AddSpoken();
                */

            } 

       }
       else
       {
            m_visualCue.SetActive(false); // hides the visual cue from your screen
       }
    }

    private void OnTriggerEnter(Collider collide)
    {
        if (collide.gameObject.tag == "Player")
        {
            m_playerInRange = true; // player in range when colliding with player tag
        }
    }

    private void OnTriggerExit(Collider collide)
    {
        if (collide.gameObject.tag == "Player")
        {
            m_playerInRange = false; // when players not in range set false
        }
    }
}
