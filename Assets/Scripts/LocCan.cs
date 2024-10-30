using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocCan : MonoBehaviour
{
    GameController gameHandler;
    [SerializeField] TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        gameHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        text.text = gameHandler.GetLocation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
