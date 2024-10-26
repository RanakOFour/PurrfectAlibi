using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// This script acts as a single point for all other scripts to get
// the current input from. It uses Unity's new Input System and
// functions should be mapped to their corresponding controls
// using a PlayerInput component with Unity Events.

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
            Debug.Log("Interact key pressed.");
        }

        if (Input.GetKeyUp(KeyCode.E)) // e key pressed
        {
            interactPressed = false;
            Debug.Log("Interact key released.");
        }

        if (Input.GetKeyDown(KeyCode.F)) // e key pressed
        {
            submitPressed = true;
            Debug.Log("Submit key pressed.");
        }

        if (Input.GetKeyUp(KeyCode.F)) // e key pressed
        {
            submitPressed = false;
            Debug.Log("Submit key released.");
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

