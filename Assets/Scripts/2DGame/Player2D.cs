using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2D : MonoBehaviour
{
    Joystick leftStick, rightStick;
    float speed = 0.3f;

    void Start()
    {
        leftStick = GameObject.Find("Left Joystick").GetComponent<Joystick>();
        rightStick = GameObject.Find("Right Joystick").GetComponent<Joystick>();
    }

    void Update()
    {
        transform.position += speed * new Vector3(leftStick.Horizontal, leftStick.Vertical, 0);
    }
}
