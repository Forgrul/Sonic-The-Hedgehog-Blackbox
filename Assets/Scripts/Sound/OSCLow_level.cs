using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;

namespace OscJack
{
    public class OSCLow_level : MonoBehaviour
    {
        // Start is called before the first frame update
        OscClient client;
        void Start()
        {
            client = new OscClient("192.168.224.220", 5566);
            //client.Send("/jump",       // OSC address
                //-90,     // First element
                //2.0f,
               // 1);
        }
        public void soundSend_Right90Spring()
        {
            client.Send("spring",       // OSC address
                90,     // First element
                2.0f,
                1);
        }
        public void soundSend_Left90Spring()
        {
            client.Send("spring",       // OSC address
                -90,     // First element
                2.0f,
                1);
        }
        public void soundSend_Right90Jump()
        {
            client.Send("jump",       // OSC address
                90,     // First element
                2.0f,
                1);
        }
        public void soundSend_Left90Jump()
        {
            client.Send("jump",       // OSC address
                -90,     // First element
                2.0f,
                1);
        }


    }

}
