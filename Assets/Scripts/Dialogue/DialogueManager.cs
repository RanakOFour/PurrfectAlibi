using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
//using UnityEditor.VisionOS;

public class DialogueManager : MonoBehaviour
{

    [Header("Params")]
    [SerializeField] private float typingSpeed = 0.04f; // speed changes the speed of the typing the lower the number the faster it types
    //Dialogue box 
    //canvas dialogue box
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel; 
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private Animator portraitAnimation;

    //Audio stuff
    [Header("Audio")]
    [SerializeField] private DialogueAudioInfoSO defaultAudioInfo;
    [SerializeField] private DialogueAudioInfoSO[] audioInfos;
    private DialogueAudioInfoSO currentAudioInfo;
    private Dictionary<string,  DialogueAudioInfoSO> audioInfoDictionary;

    private AudioSource audioSource;

    private Animator layoutAnimator;
    private Story currentStory;
    public bool dialogueIsPlaying{get; private set; }

    private Coroutine displayLineCoroutine;

    private static DialogueManager instance;

    private const string SPEAKER_TAG = "speaker"; //gets the speaker tag from the ink file which is used to set name

    private const string PORTRAIT_TAG = "portrait"; //gets the portrait tag from the ink file which is used to set portrait

    private const string LAYOUT_TAG = "layout"; //gets the layout tag from the ink file which is used to set if its on the right or left side

    private const string AUDIO_TAG = "audio"; //gets the Audio tag from the ink file which is used to set the audio

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one dialogue manager in the scene");
        }
        instance = this;

        audioSource = this.gameObject.AddComponent<AudioSource>();
        currentAudioInfo = defaultAudioInfo; //currentAudiInfo = info from Dialogue Audio
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

        InitializeAudioInfoDictionary(); // initialised the audioinfo dictionary at the start
    }

    private void InitializeAudioInfoDictionary()
    {
       audioInfoDictionary = new Dictionary<string, DialogueAudioInfoSO>();
       audioInfoDictionary.Add(defaultAudioInfo.id, defaultAudioInfo);
       foreach (DialogueAudioInfoSO audioInfo in audioInfos)
       {
            audioInfoDictionary.Add(audioInfo.id,audioInfo);
       }     
    }
    
    //sets the current audio info giving it an id
    private void SetCurrentAudioInfo(string id)
    {
        DialogueAudioInfoSO audioInfo = null;
        audioInfoDictionary.TryGetValue(id, out audioInfo);
        if(audioInfo != null)
        {
            this.currentAudioInfo = audioInfo; // sets current audio info to the audio info pulled out of the dictionary
        }
        else
        {
            Debug.LogWarning("Failed to find audio info for id:" + id);
        }
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
        List<Choice> choices = currentStory.currentChoices;
        Debug.Log("Number of choices: " + choices.Count);
        currentStory.ChooseChoiceIndex(index);
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text); // dialogue will show the inkJSON
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    public void EnterDialogueMode(Story story)
    {
        currentStory = story; // dialogue will show the inkJSON
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        SetCurrentAudioInfo(defaultAudioInfo.id); // sets audio back to default
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
           string nextline = currentStory.Continue();
           displayLineCoroutine = StartCoroutine(Displayline(nextline)); // The lines come in character by character
           //Handletags
           HandleTags(currentStory.currentTags);
        }
        else
        {
            ExitDialogueMode();
        } 
    }
    
    //TextDisplay Speed
    private IEnumerator Displayline(string line)
    {
        dialogueText.text = ""; //sets dialogue text to an empty string

        foreach (char letter in line.ToCharArray())
        {
            PlayDialogueSound(dialogueText.maxVisibleCharacters);
            dialogueText.maxVisibleCharacters++;
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    private void PlayDialogueSound(int currentDisplayedCharacterCount)
    {
        //Sets the variables to the currentaudioinfo versions
        AudioClip[] dialogueTypingSoundClips = currentAudioInfo.dialogueTypingSoundclips;
        int frequencyLevel = currentAudioInfo.frequencyLevel; 
        float minPitch = currentAudioInfo.minPitch; 
        float maxPitch = currentAudioInfo.maxPitch;
        bool stopAudioSource = currentAudioInfo.stopAudioSource;

        if (currentDisplayedCharacterCount % frequencyLevel == 0) // how many character until next sound plays
        {
            if (stopAudioSource)
            {
                audioSource.Stop();
            }
            int randomIndex = Random.Range(0, dialogueTypingSoundClips.Length); // 
            AudioClip soundClip = dialogueTypingSoundClips[randomIndex]; // plays a random clip
            audioSource.pitch = Random.Range(minPitch, maxPitch); // sets pitcg
            audioSource.PlayOneShot(soundClip); // plays audio
        }
    }

// handles the tags from inky 
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
                    displayNameText.text = tagValue;  // sets display text to tagvalue
                    break;
                
                case PORTRAIT_TAG:
                    portraitAnimation.Play(tagValue); // sets Animation  to tagvalue
                    break;

                case LAYOUT_TAG:
                    layoutAnimator.Play(tagValue); // sets layout to tagvalue
                    break;
                case AUDIO_TAG:
                     SetCurrentAudioInfo(tagValue); // sets Audio to tagvalue
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
