using Ink.Runtime;
using System.Collections.Generic;
using UnityEngine;


public class CharacterInfo
{
    private int m_id;
    private bool m_isVictim;
    private bool m_isMurderer;
    private string m_name;
    private string m_description;
    private int[] m_hangoutSpots;
    private Dictionary<int, int> m_characterRelations;
    private Story m_story;
    private Clue m_alibi;
    private Clue[] m_clues;

    public CharacterInfo(string fileData, string seed, int id, TextAsset story)
    {
        string[] seperated = fileData.Split(']');
        this.m_id = id;
        this.m_name = seperated[0];
        this.m_description = seperated[1];
        this.m_characterRelations = new Dictionary<int, int>();
        this.m_hangoutSpots = new int[3];
        this.m_story = new Story(story.text);

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

        m_alibi = new Clue("I was at 'Location " + m_hangoutSpots[2] + ".");
        m_story.variablesState["alibi"] = m_alibi.Peek();
    }

    public Story Story()
        { return m_story; }

    public int GetId()
    {
        return m_id;
    }

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
}