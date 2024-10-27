using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocCan : MonoBehaviour
{
    GameHandler gameHandler;
    [SerializeField] TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        gameHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameHandler>();
        text.text = gameHandler.GetLocation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
