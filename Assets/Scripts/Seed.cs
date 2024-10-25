using System;
using UnityEngine;

public class Seed
{
    private float seed;
    
    Seed()
    {
        seed = UnityEngine.Random.value;
    }

    public float GetSeed()
    {
        return seed;
    }
}