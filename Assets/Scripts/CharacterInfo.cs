using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;


public class CharacterInfo
{
    private bool m_isMurderer;
    private string m_name;
    private string m_description;
    private List<int> m_hangoutSpots;
    private Dictionary<int, int> m_characterRelations;
    private Clue m_alibi;
    private Clue[] m_clues;

    public CharacterInfo(string fileData, string seed)
    {
        string[] seperated = fileData.Split(']');
        this.m_name = seperated[0];
        this.m_description = seperated[1];
        this.m_characterRelations = new Dictionary<int, int>();
        this.m_hangoutSpots = new List<int>();

        for (int i = 0; i < 8; i++)
        {
            if(i % 2 == 1)
            {
                m_hangoutSpots.Add(int.Parse(seed[i].ToString()) % 5);
            }

            m_characterRelations.Add(i, 0);
            m_characterRelations[i] += int.Parse(seed[i].ToString());
        }


        //Print finished character info
        string logString = "Name: " + m_name + "\n" +
                  "Desc: " + m_description + "\n" +
                  "Hangouts: ";
        
        for(int i = 0; i < m_hangoutSpots.Count - 1; i++)
        {
            logString += m_hangoutSpots[i].ToString() + ", ";
        }
        logString += "\nCharacter relations: 0  1  2  3  4  5  6  7\n                               ";

        for (int i = 0; i < m_characterRelations.Count - 1; i++)
        {
            logString += m_characterRelations[i].ToString() + " ";
            if(m_characterRelations[i] < 10)
            {
                logString += " ";
            }
        }

        Debug.Log(logString);
    }

    public bool IsMurderer()
    { return m_isMurderer; }

    public string Name()
    { return m_name; }

    public string Description()
    { return m_description; }

    public int GetCurrentHangout(int time)
    {
        return m_hangoutSpots[time];
    }
}