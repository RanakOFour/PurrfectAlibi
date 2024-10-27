using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    private int m_dayPart;
    private int m_currentDay;
    private int m_currentRoom = -1;
    private CharacterInfoHandler m_characterInfo;
    private Dictionary<string, Clue> m_KnownClues;
    private LocationHandler m_LocationHandler;
    [SerializeField] TextAsset inkJSON;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }  

    public void StartNewGame()
    {
        m_currentRoom = -1;
        m_dayPart = 0;
        m_currentDay = 23;
        m_KnownClues = new Dictionary<string, Clue>();


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
        m_characterInfo.Initialise(strSeed);

        //Get a random char from the seed and that digit mod 7 is the murderer
        int murderer = int.Parse(strSeed[Mathf.RoundToInt(UnityEngine.Random.Range(0, 63))].ToString()) % 7;
        m_characterInfo.AssignMurderer(murderer);

        int murderLocation = m_characterInfo.GetMurderer().GetCurrentHangout(2);
        Clue murderClue = new Clue("A murder took place near Location " + murderLocation.ToString() + " on the night of the 22nd.");

        SceneManager.LoadScene("MainMap");

        DialogueManager dm = DialogueManager.GetInstance();
        dm.EnterDialogueMode(inkJSON);
        dm.EnterStoryChoice(murderLocation + 1);
    }

    public void MoveScene(int _roomId)
    {
        m_currentRoom = _roomId;

        if(_roomId == -1)
        {
            SceneManager.LoadScene("MainMap");
        }
        else
        {
            SceneManager.LoadScene("Location");
        }
    }

    private void AdvanceTime()
    {
        m_dayPart++;

        if (m_dayPart == 3)
        {
            m_dayPart = 0;
            m_currentDay++;
        }
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
}
