using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class ExampleFracture : MonoBehaviour
{
    public GameObject[] Asteroids;
    public GameObject   Chunker;


    private int  _counter = 0;
    private bool _isChunkerAvailable = true;


    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))                       // Old Input Manager code - Space key
        //if (Keyboard.current[Key.Space].wasPressedThisFrame)       // New Input System  code - Space key

        //if (Mouse.current.leftButtonv.wasPressedThisFrame)         // New Input System code - mouse left click
        //if (Mouse.current.rightButton.wasPressedThisFrame)         // New Input System code - mouse right click

        try
        {
            if (Asteroids.Length > 0)
            {
                if (_counter < Asteroids.Length)
                {
                    // Use the Space key to loops through asteroids and fractures them.
                    if (Keyboard.current[Key.Space].wasPressedThisFrame)
                    {
                        Asteroids[_counter].GetComponent<Fracture>().FractureObject();
                        _counter++;
                    }
                }
            }

            if (_isChunkerAvailable)
            {
                if (Keyboard.current[Key.I].wasPressedThisFrame)
                {
                    _isChunkerAvailable = false;
                    Chunker.SetActive(true);
                }
            }
        }
        catch (Exception ex)
        {
            string message = ex.Message;
         }

    }   // Update()


}   // class ExampleFracture
