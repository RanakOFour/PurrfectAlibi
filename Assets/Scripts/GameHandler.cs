using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    private int m_currentRoom = -1;
    private List<CharacterInfo> m_characterInfo;
    private Dictionary<string, Clue> m_KnownClues;

    private void Start()
    {
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private List<CharacterInfo> GenerateInfo(string seed)
    {
        List<CharacterInfo> generatedInfo = new List<CharacterInfo>();

        Debug.Log("Seed: " + seed);
        //Create a file reader
        StreamReader sr = new StreamReader("./Assets/FileData/CharacterInfo.txt");
        
        //Fill in character relations & appearance patterns
        for (int i = 0; i < 8; i++)
        {
            Debug.Log("Current string partition: " + seed.Substring(i * 8, 8));
            generatedInfo.Add(new CharacterInfo(sr.ReadLine(), seed.Substring(i * 8, 8)));
        }

        //Set character relations to be the highest between each pair of generated 'relation levels'
        for(int i = 0; i < 8; i++)
        {
            for(int j = i; j < 8; j++)
            {
               if (generatedInfo[i].GetRelationWith(j) > generatedInfo[j].GetRelationWith(i))
               {
                   generatedInfo[j].SetRelationWith(i, generatedInfo[i].GetRelationWith(j));
               }
               else
               {
                   generatedInfo[i].SetRelationWith(j, generatedInfo[j].GetRelationWith(i));
               }
            }
        }

        //Get a random char from the seed and that digit mod 7 is the murderer
        int murderer = int.Parse(seed[Mathf.RoundToInt(UnityEngine.Random.Range(0, 63))].ToString()) % 7;

        //To do!!
        // Alibis and clue generation
        // Deciding the murderer (happened previous evening near (but not at) location X

        return generatedInfo;
    }    

    public void StartNewGame()
    {
        //Gets seed as a long number that I can easily scan through digits with
        m_currentRoom = -1;
        string strSeed = "";

        while(strSeed.Length < 64)
        {
            strSeed += UnityEngine.Random.value.ToString().Substring(2);
        }

        strSeed = strSeed.Substring(0, 64);
        m_characterInfo = GenerateInfo(strSeed);
        SceneManager.LoadScene("MainMap");
    }

    public void MoveScene(int _roomId)
    {
        m_currentRoom = _roomId;
        SceneManager.LoadScene("Location");
    }
}
