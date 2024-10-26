using System;
using UnityEngine;

public class Clue
{
    public bool isKnown;
    private string Information;

    Clue(string info)
    {
        isKnown = false;
        Information = info;
    }

    public string Peek()
    {
        if(isKnown)
        {
            return Information;
        }

        return null;
    }

    public string Discover()
    {
        if(!isKnown)
        {
            isKnown = true;
        }

        return Information;
    }
}