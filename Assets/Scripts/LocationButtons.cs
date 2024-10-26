using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationButtons : MonoBehaviour
{
    private GameHandler m_gameHandler;
    private GameObject[] m_buttons;
    
    // Start is called before the first frame update
    void Start()
    {
        m_gameHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameHandler>();
        m_buttons = GameObject.FindGameObjectsWithTag("LocationButtons");
        for(int i = 0; i < 5; i++)
        {
            m_buttons[i].GetComponent<Button>().onClick.AddListener(() => m_gameHandler.MoveScene(i));
        }
    }
}
