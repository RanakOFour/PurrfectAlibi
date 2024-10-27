using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private GameHandler gameHandler;
    private GameObject sprite;
    private DialogueTrigger trigger;
    private CharacterInfo characterInfo;

    // Start is called before the first frame update
    void Start()
    {
        gameHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameHandler>();
        trigger = GetComponentInChildren<DialogueTrigger>();

        //It is 3:19 am, my washing will be done in 9 minutes
        // tl;dr - gets character info
        Debug.Log("sEARCHING FOR: " + gameObject.name);
        characterInfo = gameHandler.GetInfo(gameObject.name);

        trigger.GetComponent<DialogueTrigger>().charStory = characterInfo.Story();
    }
}
