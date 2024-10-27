using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationButtons : MonoBehaviour
{
    private GameHandler m_gameHandler;
    private GameObject[] m_buttons;

    void Start()
    {
        m_gameHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameHandler>();
        m_buttons = GameObject.FindGameObjectsWithTag("LocationButtons");

        m_buttons[0].GetComponent<Button>().onClick.AddListener(() => m_gameHandler.MoveScene(0));
        m_buttons[1].GetComponent<Button>().onClick.AddListener(() => m_gameHandler.MoveScene(4));
        m_buttons[2].GetComponent<Button>().onClick.AddListener(() => m_gameHandler.MoveScene(1));
        m_buttons[3].GetComponent<Button>().onClick.AddListener(() => m_gameHandler.MoveScene(3));
        m_buttons[4].GetComponent<Button>().onClick.AddListener(() => m_gameHandler.MoveScene(2));
        
        SetButtonsInteractable(false);

        DialogueManager.GetInstance().OnDialogueStart += () => SetButtonsInteractable(false);
        DialogueManager.GetInstance().OnDialogueEnd += () => SetButtonsInteractable(true);
    }

    private void SetButtonsInteractable(bool state)
    {
        // Refresh the buttons array in case any buttons were destroyed
        m_buttons = GameObject.FindGameObjectsWithTag("LocationButtons");

        foreach (var button in m_buttons)
        {
            if (button != null)
            {
                var buttonComponent = button.GetComponent<Button>();
                if (buttonComponent != null)
                {
                    buttonComponent.interactable = state;
                }
            }
        }
    }
}
