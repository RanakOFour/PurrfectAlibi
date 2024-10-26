using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DateIndicator : MonoBehaviour
{
    GameHandler gameHandler;
    TextMeshProUGUI dateText;
    TextMeshProUGUI timeOfDayText;
    DateTime currentDate;
    int dayPart;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        dayPart = -1;
        currentDate = new DateTime(2024, 8, 23);
        gameHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameHandler>();
        dateText = GetComponent<TextMeshProUGUI>();
        timeOfDayText = GameObject.FindGameObjectWithTag("TimeOfDay").GetComponent<TextMeshProUGUI>();
        AdvanceTime();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdvanceTime()
    {
        dayPart++;

        if (dayPart == 3)
        {
            dayPart = 0;
            currentDate.AddDays(1);
        }

        switch(dayPart)
        {
            case 0:
                timeOfDayText.text = "Morning";
                break;
            case 1:
                timeOfDayText.text = "Midday";
                break;
            case 2:
                timeOfDayText.text = "Evening";
                break;
        }

        dateText.text = "Date: " + currentDate.Day.ToString() + "/" + currentDate.Month.ToString();
    }

    public int GetTime()
    {
        return (currentDate.Day - 23) * 3 + dayPart;
    }
}
