using TMPro;
using UnityEngine;

public class DateTextController : MonoBehaviour
{
    TextMeshProUGUI dateText;
    TextMeshProUGUI dayPartText;

    // Start is called before the first frame update
    void Start()
    {
        dateText = GetComponent<TextMeshProUGUI>();
        dayPartText = GameObject.FindGameObjectWithTag("TimeOfDay").GetComponent<TextMeshProUGUI>();
        GameController gameHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        ShowTime(gameHandler.GetDate(), gameHandler.GetTimeOfDay());
    }

    public void ShowTime(int date, int dayPart)
    {

        switch(dayPart)
        {
            case 0:
                dayPartText.text = "Morning";
                break;
            case 1:
                dayPartText.text = "Midday";
                break;
            case 2:
                dayPartText.text = "Evening";
                break;
        }

        dateText.text = "Date: " + date.ToString() + "/8";
    }
}
