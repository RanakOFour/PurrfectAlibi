using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocationHandler : MonoBehaviour
{
    List<GameObject> m_characters = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        m_characters = GameObject.FindGameObjectsWithTag("Character").ToList();
        Debug.Log("Characters: " + m_characters.Count);
        GameHandler gameHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameHandler>();
        SetupScene(gameHandler.GetPresentCharacters());
    }

    public void SetupScene(List<int> characters)
    {
        for(int i = 0; i < characters.Count; i++)
        {
            if (!characters.Contains(i))
            {
                m_characters[i].SetActive(false);
            }
                
        }
    }
}
