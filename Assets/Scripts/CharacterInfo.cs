using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class CharacterInfo
{
    private bool isMurderer;
    private string name;
    private string description;
    private Clue alibi;
    private Clue[] clues;

    private CharacterInfo(bool isMurderer, string name, string description, Clue alibi, Clue[] clues)
    {
        this.isMurderer = isMurderer;
        this.name = name;
        this.description = description;
        this.alibi = alibi;
        this.clues = clues;
    }

    public CharacterInfo(string fileData)
    {
        string[] seperated = fileData.Split(' ');
        this.isMurderer = Convert.ToBoolean(seperated[0]);
        this.name = seperated[1];
        this.description = seperated[2];
    }

    public bool IsMurderer()
        { return isMurderer; }

    public string Name()
        { return name; }

    public string Description() { return description; }
}