using OscJack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEvoker : MonoBehaviour
{
    public OSCLow_level oscSound;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("Left 90 Jump");
            oscSound.soundSend_Left90Jump();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Right 90 Jump");
            oscSound.soundSend_Right90Jump();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("Left 90 Spring");
            oscSound.soundSend_Left90Spring();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Right 90 Spring");
            oscSound.soundSend_Right90Spring();
        }
    }
}
