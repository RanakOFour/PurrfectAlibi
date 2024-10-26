using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    private bool interactPressed = false;
    private bool submitPressed = false;

    private static InputManager instance;

    // Get the PlayerInput component
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Input Manager in the scene.");
        }
        instance = this;
    }

    public static InputManager GetInstance() 
    {
        return instance;
    }


   private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // e key pressed
        {
            interactPressed = true;
        }

        if (Input.GetKeyUp(KeyCode.E)) // e key pressed
        {
            interactPressed = false;
        }

        if (Input.GetKeyDown(KeyCode.F)) // e key pressed
        {
            submitPressed = true;
        }

        if (Input.GetKeyUp(KeyCode.F)) // e key pressed
        {
            submitPressed = false;
        }
    }

    public void SubmitPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            submitPressed = true;
        }
        else if (context.canceled)
        {
            submitPressed = false;
        } 
    }

    public bool GetInteractPressed() 
    {
        bool result = interactPressed;
        interactPressed = false;
        return result;
    }

    public bool GetSubmitPressed() 
    {
        bool result = submitPressed;
        submitPressed = false;
        return result;
    }

    public void RegisterSubmitPressed() 
    {
        submitPressed = false;
    }
}

