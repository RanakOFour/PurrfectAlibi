using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    public bool shouldAdvanceTime = false;
    private bool m_gameOver;
    private int m_dayPart;
    private int m_currentDay;
    private int m_currentRoom = -1;
    private CharacterInfoHandler m_characterInfo;
    private DialogueManager dm;
    private Notebook m_Notebook;
    private LocationHandler m_LocationHandler;
    [SerializeField] TextAsset IntroductionText;
    [SerializeField] TextAsset NPCText;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
       if(shouldAdvanceTime && !dm.dialogueIsPlaying)
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

    public void StartNewGame()
    {
        m_currentRoom = -1;
        m_dayPart = 0;
        m_currentDay = 23;

        m_Notebook = GameObject.FindGameObjectWithTag("Notebook").GetComponent<Notebook>();

        //Gets seed as a long number that I can easily scan through digits with
        string strSeed = "";
        while(strSeed.Length < 64)
        {
            //Substring cuts out the '0.' at the beginning
            strSeed += UnityEngine.Random.value.ToString().Substring(2);
        }
        //Pass seed into character info handler
        strSeed = strSeed.Substring(0, 64);
        m_characterInfo = new CharacterInfoHandler();
        m_characterInfo.Initialise(strSeed, NPCText);

        //Get a random char from the seed and that digit mod 7 is the murderer
        int victim = int.Parse(strSeed[Mathf.RoundToInt(UnityEngine.Random.Range(0, 63))].ToString()) % 7;
        m_characterInfo.AssignVictim(victim);

        int murderLocation = m_characterInfo.GetMurderer().GetCurrentHangout(2);
        string initMurderInfo = "The murder took place near Location " + (murderLocation + 1).ToString() + " on the night of the 22nd.";

        m_Notebook.AddClue(7, initMurderInfo);
        m_Notebook.AddClue(m_characterInfo.GetVictim().GetId(), "Died at Location " + (murderLocation + 1).ToString());
        SceneManager.LoadScene("MainMap");

        Story introduction = new Story(IntroductionText.text);
        introduction.variablesState["murderLocation"] = (murderLocation + 1).ToString();
        introduction.variablesState["victim"] = m_characterInfo.GetVictim().Name();

        dm = DialogueManager.GetInstance();
        dm.EnterDialogueMode(introduction);
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

    public bool GuessKiller(int killer)
    {
        if(m_characterInfo.GetMurderer().GetId() == killer)
        {
            return true;
        }

        return false;
    }

    public string GetVictim()
    {
        return m_characterInfo.GetVictim().Name();
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

        shouldAdvanceTime = false;
    }

    public int GetTimeOfDay()
    { return m_dayPart; }

    public int GetDate()
        { return m_currentDay; }

    public List<int> GetPresentCharacters()
    {
        return m_characterInfo.GetCharactersAtLocation(m_currentRoom, m_dayPart);
    }

    public int GetRoom()
    {
        return m_currentRoom;
    }

    public CharacterInfo GetInfo(string character)
    {
        return m_characterInfo.GetInfo(character);
    }

    public void IncrementDate()
    {
        AdvanceTime(); // Use the existing method to update time
    }

    public void AddClue(int id, string c)
    {
        m_Notebook.AddClue(id, c);
    }
}
