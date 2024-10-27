using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CharacterInfoHandler
{
    private int murdererId;
    private int victimId;
    private List<CharacterInfo> m_characterInfo;

    public CharacterInfoHandler()
    {
        m_characterInfo = new List<CharacterInfo>();
    }

    public void Initialise(string seed, TextAsset npcStory)
    {
        //Create a file reader
        StreamReader sr = new StreamReader("./Assets/FileData/CharacterInfo.txt");

        //Fill in character relations & appearance patterns
        for (int i = 0; i < 7; i++)
        {
            Debug.Log("Current string partition: " + seed.Substring(i * 8, 8));
            m_characterInfo.Add(new CharacterInfo(sr.ReadLine(), seed.Substring(i * 8, 8), i, npcStory));
        }

        //Set character relations to be the highest between each pair of generated 'relation levels'
        for (int i = 0; i < 7; i++)
        {
            for (int j = i; j < 7; j++)
            {
                if (m_characterInfo[i].GetRelationWith(j) > m_characterInfo[j].GetRelationWith(i))
                {
                    m_characterInfo[j].SetRelationWith(i, m_characterInfo[i].GetRelationWith(j));
                }
                else
                {
                    m_characterInfo[i].SetRelationWith(j, m_characterInfo[j].GetRelationWith(i));
                }
            }
        }
        //To do!!
        // Alibis and clue generation
        // Deciding the murderer (happened previous evening near (but not at) location X
    }

    public List<int> GetCharactersAtLocation(int roomId, int time)
    {
        List<int> toReturn = new List<int>();
        foreach(CharacterInfo ci in m_characterInfo)
        {
            if(ci.GetCurrentHangout(time) == roomId)
            {
                toReturn.Add(ci.GetId());
            }
        }

        return toReturn;
    }

    public void AssignVictim(int id)
    {
        m_characterInfo[id].SetVictim(true);
        victimId = id;

        int lowest = 6;
        int potMur = -1;
        for(int i = 0; i < 7; i++)
        {
            int relation = m_characterInfo[id].GetRelationWith(i);
            if(relation < lowest)
            {
                lowest = relation;
                potMur = i;
            }
        }

        m_characterInfo[potMur].SetMurderer(true);
        murdererId = potMur;

        Debug.Log("Victim: " + m_characterInfo[victimId].Name() + "\nMurderer: " + m_characterInfo[murdererId].Name());

        foreach(CharacterInfo ci in m_characterInfo)
        {
            ci.SetRelationClue(victimId);
        }
    }

    public CharacterInfo GetInfo(string name)
    {
        for(int i = 0; i < m_characterInfo.Count; i++)
        {
            if(name == m_characterInfo[i].Name())
            {
                return m_characterInfo[i];
            }
        }

        return null;
    }

    public CharacterInfo GetMurderer()
    {
        return m_characterInfo[murdererId];
    }

    public CharacterInfo GetVictim()
    {
        return m_characterInfo[victimId];
    }
}
