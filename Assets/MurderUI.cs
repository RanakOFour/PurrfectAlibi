using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MurderUI : MonoBehaviour
{
    GameHandler gameHandler;
    TextMeshProUGUI[] buttons;
    [SerializeField] private TextMeshProUGUI topText;
    [SerializeField] private GameObject loseText;
    [SerializeField] private GameObject winText;

    // Start is called before the first frame update
    void Start()
    {
        gameHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameHandler>();
        buttons = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        string victimName = gameHandler.GetVictim();

        foreach (TextMeshProUGUI button in buttons)
        {
            if(button.text == victimName)
            {
                button.gameObject.gameObject.SetActive(false);
                break; 
            }
        }

        topText.text += gameHandler.GetVictim() + "?";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GuessKiller(int killer)
    {
        if(gameHandler.GuessKiller(killer))
        {
            winText.SetActive(true);
        }
        else
        {
            loseText.SetActive(true);
        }    
    }
}
