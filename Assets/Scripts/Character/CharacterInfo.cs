using Ink.Runtime;
using System.Collections.Generic;
using UnityEngine;


public class CharacterInfo
{
    private int m_id;
    private int spokenTo;
    private bool m_isVictim;
    private bool m_isMurderer;
    private string m_name;
    private string m_description;
    private int[] m_hangoutSpots;
    private Dictionary<int, int> m_characterRelations;
    private Story m_story;
    private string m_alibi;
    private string[] m_clues;

    public CharacterInfo(string fileData, string seed, int id, TextAsset story)
    {
        string[] seperated = fileData.Split(']');
        this.m_id = id;
        this.m_name = seperated[0];
        this.m_description = seperated[1];
        this.m_characterRelations = new Dictionary<int, int>();
        this.m_hangoutSpots = new int[3];
        this.m_story = new Story(story.text);
        this.m_clues = new string[3];
        this.spokenTo = 0;

        for (int i = 0; i < 8; i++)
        {
            if(i <= 2)
            {
                //Locations will be between 0 and 4
                m_hangoutSpots[i] = int.Parse(seed[i].ToString()) % 5;
            }

            m_characterRelations.Add(i, 0);

            //Relationships will be between 0 and 4, indicating a strong dislike to strong like
            m_characterRelations[i] += int.Parse(seed[i].ToString()) % 5;
        }

        //Print finished character info
        string logString = "Name: " + m_name + "\n" +
                  "Desc: " + m_description + "\n" +
                  "Hangouts: ";
        
        for(int i = 0; i < m_hangoutSpots.Length; i++)
        {
            logString += m_hangoutSpots[i].ToString() + ", ";
        }
        logString += "\nCharacter relations: 0  1  2  3  4  5  6  7\n                                    ";

        for (int i = 0; i < m_characterRelations.Count; i++)
        {
            logString += m_characterRelations[i].ToString() + " ";
            if(m_characterRelations[i] < 10)
            {
                logString += " ";
            }
        }

        Debug.Log(logString);

        m_alibi = "I was at 'Location " + (m_hangoutSpots[2] + 1) + "'.";
        m_story.variablesState["alibi"] = m_alibi;
        m_story.variablesState["speaker"] = m_name;

    }

    public Story Story()
        { return m_story; }

    public int GetId()
    { return m_id; }

    public bool IsVictim()
    { return m_isVictim; }

    public void SetVictim(bool vic)
    { m_isVictim = vic; }

    public bool IsMurderer()
    { return m_isMurderer; }

    public void SetMurderer(bool m)
    { m_isMurderer = m; }

    public string Name()
    { return m_name; }

    public string Description()
    { return m_description; }

    public int GetCurrentHangout(int time)
    {
        return m_hangoutSpots[time];
    }

    public int GetRelationWith(int character)
    {
        return m_characterRelations[character];
    }

    public void SetRelationWith(int character, int relation)
    {
        m_characterRelations[character] = relation;
    }

    public void SetAlibi(string alibi)
    {
       m_alibi = alibi;
    }

    public string GetAlibi()
    {
        return m_alibi;
    }

    
    public void SetClues(string[] clues)
    {
        m_clues = clues;
    }

    public string[] GetClues()
    {
        string[] clue = new string[] { m_alibi, m_clues[0], m_clues[1], m_clues[2] };
        return clue;
    }

    public int GetSpokenTo()
    { return spokenTo; }

    public void AddSpoken()
    {
        spokenTo++;
    }

    public void SetRelationClue(int victimId)
    {
        string clue1 = "";
        switch(m_characterRelations[victimId])
        {
            case 0:
                clue1 += "I was not very good friends with them.";
                break;

            case 1:
                clue1 += "We didn't get along.";
                break;

            case 2:
                clue1 += "We hung out a few times.";
                break;

            case 3:
                clue1 += "We were friends.";
                break;

            case 4:
                clue1 += "We were good friends.";
                break;
        }

        m_clues[0] = clue1;
        m_story.variablesState["clue1"] = clue1;
    }
}