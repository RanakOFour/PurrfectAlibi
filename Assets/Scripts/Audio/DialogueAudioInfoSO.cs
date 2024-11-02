using UnityEngine;

[CreateAssetMenu(fileName = "DialogueAudioInfo", menuName = "ScriptableObjects/DialogueAudioInfoSO", order = 1)]
public class DialogueAudioInfoSO : ScriptableObject
{
    //Audio stuff
    [Header("Audio")]
    public AudioClip[] m_dialogueTypingSoundclips;

    [Header("Frequency Level")]
    [Range(1, 5)]
    public int m_frequencyLevel = 2;

    [Header("Minimum Pitch")]
    [Range(-3, 3)]
    public float m_minPitch = 0.5f;

    [Header("Maximum Pitch")]
    [Range(-3, 3)]
    public float m_maxPitch = 3f;


    public string m_id { get; set; }
    public bool m_stopAudioSource { get; set; }
}
