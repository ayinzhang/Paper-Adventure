using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2D : MonoBehaviour
{
    GameManager2D gm;
    Joystick leftStick, rightStick;
    public float speed = 0.3f, arrowCD = 0.5f;
    float arrowTimer;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager2D>();
        leftStick = GameObject.Find("LeftJoystick").GetComponent<Joystick>();
        rightStick = GameObject.Find("RightJoystick").GetComponent<Joystick>();
    }

    void Update()
    {
        transform.position += speed * new Vector3(leftStick.Horizontal, leftStick.Vertical, 0);

        if(arrowTimer >= arrowCD && (rightStick.Horizontal != 0 || rightStick.Vertical != 0))
        {
            arrowTimer = 0; GameObject arrow = gm.arrowPool.Get();
            arrow.transform.position = transform.position + 1.5f * new Vector3(rightStick.Horizontal, rightStick.Vertical, 0).normalized;
            arrow.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(new Vector2(1, 0), new Vector2(rightStick.Horizontal, rightStick.Vertical)));
        }
        else
        {
            arrowTimer += Time.deltaTime;
        }
    }
}
