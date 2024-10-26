using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{

    [Header("Params")]
    [SerializeField] private float typingSpeed = 0.04f; // speed changes the speed of the typing the lower the number the faster it types
    //Dialogue box 
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private Animator portraitAnimation;

    //Audio stuff
    [Header("Audio")]
    [SerializeField] private AudioClip dialogueTypingSoundclip;
    [SerializeField] private bool stopAudioSource;


    private AudioSource audioSource;

    private Animator layoutAnimator;
    private Story currentStory;
    public bool dialogueIsPlaying{get; private set; }

    private Coroutine displayLineCoroutine;

    private static DialogueManager instance;

    private const string SPEAKER_TAG = "speaker";

    private const string PORTRAIT_TAG = "portrait";

    private const string LAYOUT_TAG = "layout";



    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one dialogue manager in the scene");
        }
        instance = this;

        audioSource = this.gameObject.AddComponent<AudioSource>();
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }
    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        layoutAnimator = dialoguePanel.GetComponent<Animator>();
    }
    private void Update()
    {
        //return if dialoge isn't playing
        if (!dialogueIsPlaying)
        {
            return;
        }

        if(InputManager.GetInstance().GetSubmitPressed())
        {
            ContinueStory();
        }
    }

    public void EnterStoryChoice(int index)
    {

        currentStory.ChooseChoiceIndex(index);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        if (currentStory.currentChoices.Count > 0)
        {
            for (int i = 0; i < currentStory.currentChoices.Count; ++i)
            {
                Choice choice = currentStory.currentChoices[i];
                Debug.Log("Choice " + (i + 1) + ". " + choice.text);
            }
        }
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            // stops the issue where you press f during text and it mumbles the words
            if(displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine); //stops the coroutine so it doesn't affect the next one
            }
           displayLineCoroutine = StartCoroutine(Displayline(currentStory.Continue())); // The lines come in character by character
           //Handletags
           HandleTags(currentStory.currentTags);
        }
        else
        {
            ExitDialogueMode();
        } 
    }

    private IEnumerator Displayline(string line)
    {
        dialogueText.text = ""; //sets dialogue text to an empty string

        foreach (char letter in line.ToCharArray())
        {
            PlayDialogueSound(dialogueText.maxVisibleCharacters);
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    private void PlayDialogueSound(int currentDisplayedCharacterCount)
    {
        if (currentDisplayedCharacterCount % 3 == 0)
        {
            if (stopAudioSource)
            {
                audioSource.Stop();
            }
            audioSource.PlayOneShot(dialogueTypingSoundclip);
        }
    }

    private void HandleTags(List<string> currentTags)
    {
        // loops through each tag and handle it 
        foreach(string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
            }
            //depending on the value of the tag in the inkjson this will effect the display name, portrait and layout.
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();
            
            switch (tagKey)
            {
                case SPEAKER_TAG:
                    displayNameText.text = tagValue; 
                    break;
                
                case PORTRAIT_TAG:
                    portraitAnimation.Play(tagValue);
                    break;

                case LAYOUT_TAG:
                    layoutAnimator.Play(tagValue);
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }
        }
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
    }
}
