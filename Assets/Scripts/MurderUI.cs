using TMPro;
using UnityEngine;

public class MurderUI : MonoBehaviour
{
    [Header("Top Text GameObject")]
    [SerializeField] private TextMeshProUGUI m_topText;

    [Header("Lose Text GameObject")]
    [SerializeField] private GameObject m_loseText;

    [Header("Win Text GameObject")]
    [SerializeField] private GameObject m_winText;


    private GameController m_gameHandler;
    private TextMeshProUGUI[] m_buttons;

    void Start()
    {
        m_gameHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        m_buttons = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        string victimName = m_gameHandler.GetVictim();

        foreach (TextMeshProUGUI button in m_buttons)
        {
            if(button.text == victimName)
            {
                button.gameObject.gameObject.SetActive(false);
                break; 
            }
        }

        m_topText.text += m_gameHandler.GetVictim() + "?";
    }

    public void GuessKiller(int killer)
    {
        if(m_gameHandler.GuessKiller(killer))
        {
            m_winText.SetActive(true);
        }
        else
        {
            m_loseText.SetActive(true);
        }    
    }
}
