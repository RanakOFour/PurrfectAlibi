using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Notebook : MonoBehaviour
{
    private string[,] m_KnownClues;
    private int[] m_KnownRelations;

    [SerializeField] private List<TextMeshProUGUI> m_Textboxes;
    [SerializeField] private GameObject m_NotebookImage;
    [SerializeField] private GameObject m_Notes;
    [SerializeField] private GameObject m_Network;
    private GameHandler m_Handler;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        m_Handler = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameHandler>();
        // 7 characters with 4 clues each, plus 1 freebie at the start (at [8, 0])
        m_KnownClues = new string[8, 3];
        m_KnownRelations = new int[8];
        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                m_KnownClues[i, j] = "";
            }
            m_KnownRelations[i] = -1;
        }

        m_NotebookImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            m_NotebookImage.SetActive(!m_NotebookImage.activeSelf);
        }
    }

    public void AddClue(int characterId, string clue)
    {
        int cluenum = 0;
        for(int i = 0; i < 3; i++)
        {
            if(m_KnownClues[characterId, i] == "")
            {
                m_KnownClues[characterId, i] = clue;
                cluenum = i;
                break;
            }
        }

        if(cluenum == 0)
        {
            m_Textboxes[characterId].text += "Alibi: " + clue + "\n";
        }
        else
        {
            m_Textboxes[characterId].text += "Clue " + cluenum + ": " + clue + "\n";
        }
    }

    public void ShowNotes()
    {
        if(!m_Notes.activeSelf)
        {
            m_Network.SetActive(false);
            m_Notes.SetActive(true);
        }
    }

    public void ShowNetwork()
    {
        if(!m_Network.activeSelf)
        {
            m_Notes.SetActive(false);
            m_Network.SetActive(true);
        }
    }
}
