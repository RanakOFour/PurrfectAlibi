using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using System;

public class DialogueManager : MonoBehaviour
{
    [Header("Typing Speed")]
    [SerializeField] private float m_typingSpeed = 0.04f; // speed changes the speed of the typing the lower the number the faster it types

    [Header("Dialogue UI")]
    [SerializeField] private GameObject m_dialoguePanel;

    [Header("Dialogue Text")]
    [SerializeField] private TextMeshProUGUI m_dialogueText;

    [Header("Display Name")]
    [SerializeField] private TextMeshProUGUI m_displayNameText;

    [Header("Portrait Animation")]
    [SerializeField] private Animator m_portraitAnimation;

    [Header("Audio")]
    [SerializeField] private DialogueAudioInfoSO m_defaultAudioInfo;

    [Header("Audio Infos")]
    [SerializeField] private DialogueAudioInfoSO[] m_audioInfos;

    public event Action OnDialogueStart;
    public event Action OnDialogueEnd;

    public bool m_dialogueIsPlaying { get; private set; }
    private DialogueAudioInfoSO m_currentAudioInfo;
    private Dictionary<string,  DialogueAudioInfoSO> m_audioInfoDictionary;
    private AudioSource m_audioSource;
    private Animator m_layoutAnimator;
    private Story m_currentStory;
    private Coroutine m_displayLineCoroutine;
    private static DialogueManager m_instance;

    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";
    private const string AUDIO_TAG = "audio";

    private void Awake()
    {
        if (m_instance != null)
        {
            Debug.LogWarning("Found more than one dialogue manager in the scene");
        }
        m_instance = this;

        m_audioSource = this.gameObject.AddComponent<AudioSource>();
        m_currentAudioInfo = m_defaultAudioInfo;
    }

    private void Start()
    {
        m_dialogueIsPlaying = false;
        m_dialoguePanel.SetActive(false);

        m_layoutAnimator = m_dialoguePanel.GetComponent<Animator>();

        InitializeAudioInfoDictionary();
    }

    private void InitializeAudioInfoDictionary()
    {
        m_audioInfoDictionary = new Dictionary<string, DialogueAudioInfoSO>();
        m_audioInfoDictionary.Add(m_defaultAudioInfo.m_id, m_defaultAudioInfo);
       foreach (DialogueAudioInfoSO audioInfo in m_audioInfos)
       {
            m_audioInfoDictionary.Add(audioInfo.m_id,audioInfo);
       }     
    }
    
    private void Update()
    {
        //return if dialoge isn't playing
        if (!m_dialogueIsPlaying)
        {
            return;
        }

        if(InputManager.GetInstance().GetSubmitPressed())
        {
            ContinueStory();
        }
    }

    // Sets the current audio info giving it an id
    private void SetCurrentAudioInfo(string _id)
    {
        DialogueAudioInfoSO audioInfo = null;
        m_audioInfoDictionary.TryGetValue(_id, out audioInfo);
        if (audioInfo != null)
        {
            this.m_currentAudioInfo = audioInfo; // Sets current audio info to the audio info pulled out of the dictionary
        }
        else
        {
            Debug.LogWarning("Failed to find audio info for id:" + _id);
        }
    }

    public static DialogueManager GetInstance()
    {
        return m_instance;
    }

    public void EnterStoryChoice(int _index)
    {
        List<Choice> choices = m_currentStory.currentChoices;
        Debug.Log("Number of choices: " + choices.Count);
        m_currentStory.ChooseChoiceIndex(_index);
    }

    public void EnterDialogueMode(Story _story)
    {
        // Dialoguebox will show the inkJSON
        m_currentStory = _story;
        m_dialogueIsPlaying = true;
        m_dialoguePanel.SetActive(true);

        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        m_dialogueIsPlaying = false;
        m_dialoguePanel.SetActive(false);
        m_dialogueText.text = "";

        OnDialogueEnd.Invoke();

        SetCurrentAudioInfo(m_defaultAudioInfo.m_id);
    }

    private void ContinueStory()
    {
        if (m_currentStory.canContinue)
        {
            // Stops the issue where you press f during text and it mumbles the words
            if(m_displayLineCoroutine != null)
            {
                StopCoroutine(m_displayLineCoroutine); // Stops the coroutine so it doesn't affect the next line
            }

            string nextline = m_currentStory.Continue();
            m_displayLineCoroutine = StartCoroutine(Displayline(nextline)); // The lines come in character by character

           
           HandleTags(m_currentStory.currentTags);
        }
        else
        {
            ExitDialogueMode();
        } 
    }
    
    // TextDisplay Speed
    private IEnumerator Displayline(string _lineToDisplay)
    {
        m_dialogueText.text = "";

        foreach (char letter in _lineToDisplay.ToCharArray())
        {
            PlayDialogueSound(m_dialogueText.maxVisibleCharacters);
            m_dialogueText.maxVisibleCharacters++;
            m_dialogueText.text += letter;
            yield return new WaitForSeconds(m_typingSpeed);
        }
    }

    private void PlayDialogueSound(int _currentDisplayedCharacterCount)
    {
        // Sets the variables to the currentaudioinfo versions
        AudioClip[] dialogueTypingSoundClips = m_currentAudioInfo.m_dialogueTypingSoundclips;
        int frequencyLevel = m_currentAudioInfo.m_frequencyLevel; 
        float minPitch = m_currentAudioInfo.m_minPitch; 
        float maxPitch = m_currentAudioInfo.m_maxPitch;
        bool stopAudioSource = m_currentAudioInfo.m_stopAudioSource;

        // Plays new random sound when each character in the text has finished being displayed
        if (_currentDisplayedCharacterCount % frequencyLevel == 0)
        {
            if (stopAudioSource)
            {
                m_audioSource.Stop();
            }
            int randomIndex = UnityEngine.Random.Range(0, dialogueTypingSoundClips.Length);
            AudioClip soundClip = dialogueTypingSoundClips[randomIndex];
            m_audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
            m_audioSource.PlayOneShot(soundClip);
        }
    }

    private void HandleTags(List<string> _currentTags)
    {
        // Loops through each tag and handle it 
        foreach(string tag in _currentTags)
        {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
            }
            // Depending on the value of the tag in the inkjson this will effect the display name, portrait and layout.
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();
            
            // Applies Ink tags to the current textbox
            switch (tagKey)
            {
                case SPEAKER_TAG:
                    m_displayNameText.text = tagValue;
                    break;
                
                case PORTRAIT_TAG:
                    m_portraitAnimation.Play(tagValue);
                    break;

                case LAYOUT_TAG:
                    m_layoutAnimator.Play(tagValue);
                    break;
                case AUDIO_TAG:
                    SetCurrentAudioInfo(tagValue);
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
