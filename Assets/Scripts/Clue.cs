using System;
using UnityEngine;

public class Clue
{
    public bool isKnown;
    private string Information;

    Clue(bool known, string info)
    {
        isKnown = known;
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