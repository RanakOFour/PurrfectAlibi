using UnityEngine;

public class CharacterHandler : MonoBehaviour
{
    [Header("Character ID")]
    [SerializeField] private int m_id;


    private GameController m_gameController;
    private GameObject m_sprite;
    private DialogueTrigger m_trigger;
    private CharacterInfo m_characterInfo;


    void Start()
    {
        // What to do? Get charInfo, if hangout (currentTime) is at point, don't set disable

        m_gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        m_characterInfo = m_gameController.GetCharInfo(m_id);

        m_trigger = GetComponentInChildren<DialogueTrigger>();
        m_trigger.GetComponent<DialogueTrigger>().m_charStory = m_characterInfo.Story();
        //m_trigger.GetComponent<DialogueTrigger>().SetCharInfo(m_characterInfo);
    }
}
