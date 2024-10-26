using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    private int m_timeIndex = 0;
    private List<CharacterInfo> m_characterInfo;
    private Dictionary<string, Clue> m_KnownClues;

    private void Start()
    {
    }

    private void Update()
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
        for (int i = 0; i < 8; i++)
        {
            Debug.Log("Current string partition: " + seed.Substring(i * 8, 8));
            generatedInfo.Add(new CharacterInfo(sr.ReadLine(), seed.Substring(i * 8, 8)));

            //Input names from 
            //Debug.Log("Name:  " + generatedInfo[i].Name() + "\n" + "Desc: " + generatedInfo[i].Description() + "\n");
        }


        return generatedInfo;
    }    

    public void StartNewGame()
    {
        //Gets seed as a long number that I can easily scan through digits with
        string strSeed = "";

        while(strSeed.Length < 64)
        {
            strSeed += UnityEngine.Random.value.ToString().Substring(2);
        }

        strSeed = strSeed.Substring(0, 64);
        m_characterInfo = GenerateInfo(strSeed);
    }

    public void MoveScene(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }
}
