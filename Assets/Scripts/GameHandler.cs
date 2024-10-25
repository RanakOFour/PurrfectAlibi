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
    private List<CharacterInfo> m_characterInfo = new List<CharacterInfo>();
    private Dictionary<string, Clue> m_KnownClues = new Dictionary<string, Clue>();

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

    private List<CharacterInfo> GenerateInfo()
    {
        List<CharacterInfo> generatedInfo = new List<CharacterInfo>();
        Debug.Log("Current working directory: " + Directory.GetCurrentDirectory());
        StreamReader sr = new StreamReader("./Assets/FileData/CharacterInfo.txt");
        for (int i = 0; i < 10; i++)
        {
            generatedInfo.Add(new CharacterInfo(i, sr.ReadLine()));

            Debug.Log("Is Murd: " + generatedInfo[i].IsMurderer() + "\n"
                        + "Name:  " + generatedInfo[i].Name() + "\n"
                        + "Desc: " + generatedInfo[i].Description() + "\n");
        }


        return generatedInfo;
    }    

    public void StartNewGame()
    {
        m_timeIndex = 0;
        m_characterInfo = GenerateInfo();
        float seed = UnityEngine.Random.value;
        Debug.Log("Seed: " + seed.ToString());
    }

    public void MoveScene(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }
}
