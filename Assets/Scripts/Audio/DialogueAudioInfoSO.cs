using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[currentAssetMenu(fileName = "DialogueAudioInfo", menuName = "ScriptableObjects/DialogueAudioInfoSO", order = 1)]

public class DialogueAudioInfoSO : ScriptableObject
{
    public string id;

    //Audio stuff
    [Header("Audio")]
    public AudioClip[] dialogueTypingSoundclips;
    public bool stopAudioSource;
    [Range(1, 5)]
    public int frequencyLevel = 2;

    [Range(-3, 3)]
    public float minPitch = 0.5f;
    [Range(-3, 3)]
    public float maxPitch = 3f;

}
