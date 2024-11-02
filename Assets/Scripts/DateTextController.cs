using TMPro;
using UnityEngine;

public class DateTextController : MonoBehaviour
{
    private TextMeshProUGUI m_dateText;
    private TextMeshProUGUI m_dayPartText;

    // Start is called before the first frame update
    void Start()
    {
        m_dateText = GetComponent<TextMeshProUGUI>();
        m_dayPartText = GameObject.FindGameObjectWithTag("TimeOfDay").GetComponent<TextMeshProUGUI>();
        GameController gameHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        ShowTime(gameHandler.GetDate(), gameHandler.GetTimeOfDay());
    }

    public void ShowTime(int date, int dayPart)
    {

        switch(dayPart)
        {
            case 0:
                m_dayPartText.text = "Morning";
                break;
            case 1:
                m_dayPartText.text = "Midday";
                break;
            case 2:
                m_dayPartText.text = "Evening";
                break;
        }

        m_dateText.text = "Date: " + date.ToString() + "/8";
    }
}
