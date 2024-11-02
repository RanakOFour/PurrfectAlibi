using Ink.Runtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Introduction Text")]
    [SerializeField] private TextAsset m_introductionText;

    [Header("Blank NPCStory")]
    [SerializeField] private TextAsset m_npcText;


    private bool m_gameOver;
    private bool m_shouldAdvanceTime = false;
    private int m_dayPart;
    private int m_currentDay;
    private int m_currentRoom = -1;
    private CharacterInfoHandler m_characterInfos;
    private DialogueManager m_dialogueManager;
    private Notebook m_notebook;
    private LocationHandler m_locationHandler;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        m_currentRoom = -1;
        m_dayPart = 0;
        m_currentDay = 23;

        m_notebook = GameObject.FindGameObjectWithTag("Notebook").GetComponent<Notebook>();

        //Gets seed as a long number that I can easily scan through digits with
        string strSeed = "";
        while (strSeed.Length < 64)
        {
            //Substring cuts out the '0.' at the beginning
            strSeed += UnityEngine.Random.value.ToString().Substring(2);
        }
        //Pass seed into character info handler
        strSeed = strSeed.Substring(0, 64);
        m_characterInfos = new CharacterInfoHandler();
        m_characterInfos.Initialise(strSeed, m_npcText);

        //Get a random char from the seed and that digit mod 7 is the murderer
        int victim = int.Parse(strSeed[Mathf.RoundToInt(UnityEngine.Random.Range(0, 63))].ToString()) % 7;
        m_characterInfos.AssignVictim(victim);

        int murderLocation = m_characterInfos.GetMurderer().GetCurrentHangout(2);
        string initMurderInfo = "The murder took place near Location " + (murderLocation + 1).ToString() + " on the night of the 22nd.";

        m_notebook.AddClue(7, initMurderInfo);
        m_notebook.AddClue(m_characterInfos.GetVictim().GetId(), "Died at Location " + (murderLocation + 1).ToString());

        Story introduction = new Story(m_introductionText.text);
        introduction.variablesState["murderLocation"] = (murderLocation + 1).ToString();
        introduction.variablesState["victim"] = m_characterInfos.GetVictim().Name();

        m_dialogueManager = DialogueManager.GetInstance();
        m_dialogueManager.EnterDialogueMode(introduction);
    }

    private void Update()
    {
       if(m_shouldAdvanceTime && !m_dialogueManager.m_dialogueIsPlaying)
       {
            AdvanceTime();

            if(m_gameOver)
            {
                MoveScene(-2);
            }
            else
            {
                MoveScene(-1);
            }
       }
    }

    public void MoveScene(int _roomId)
    {
        m_currentRoom = _roomId;

        if(_roomId == -1)
        {
            SceneManager.LoadScene("MainMap");
        }
        else if (_roomId == -2)
        {
            SceneManager.LoadScene("EndOfGame");
        }
        else
        {
            SceneManager.LoadScene("Location");
        }
    }

    public string GetLocation()
    {
        return "Location " + (m_currentRoom + 1);
    }

    public bool GuessKiller(int _killer)
    {
        if(m_characterInfos.GetMurderer().GetId() == _killer)
        {
            return true;
        }

        return false;
    }

    public string GetVictim()
    {
        return m_characterInfos.GetVictim().Name();
    }

    public void AdvanceTime()
    {
        m_dayPart++;

        if (m_dayPart == 3)
        {
            m_dayPart = 0;
            m_currentDay++;
        }

        if(m_currentDay == 31)
        {
            m_gameOver = true;
        }

        m_shouldAdvanceTime = false;
    }

    public int GetTimeOfDay()
    { return m_dayPart; }

    public int GetDate()
        { return m_currentDay; }

    public List<int> GetPresentCharacters()
    {
        return m_characterInfos.GetCharactersAtLocation(m_currentRoom, m_dayPart);
    }

    public int GetRoom()
    {
        return m_currentRoom;
    }

    public CharacterInfo GetCharInfo(int _charId)
    {
        return m_characterInfos.GetInfo(_charId);
    }

    public void IncrementDate()
    {
        AdvanceTime(); // Use the existing method to update time
    }

    public void AddClue(int _charId, string _clue)
    {
        m_notebook.AddClue(_charId, _clue);
    }
}
