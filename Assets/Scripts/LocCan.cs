using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocCan : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI m_text;

    private GameController m_gameHandler;

    void Start()
    {
        m_gameHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        m_text.text = m_gameHandler.GetLocation();
    }
}
