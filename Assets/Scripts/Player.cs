using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    int hp = 100;
    int cnt = 100;
    float speed = 0.25f;
    ObjectPool pool;
    FloatingJoystick leftStick;
    FloatingJoystick rightStick;

    public void Hurt(int dmg)
    {
        hp -= dmg;
    }

    Vector3 GetAngle(float x, float y)
    {
        if (y >= 0) return new Vector3(0, 0, Mathf.Acos(y / x) / Mathf.PI * 180);
        else return new Vector3(0, 0, (Mathf.Acos(- y / x) > 0 ? 1 : -1) * 180 - Mathf.Acos(-y / x) / Mathf.PI * 180);
    }

    void Start()
    {
        pool = GameObject.Find("ObjectPool").GetComponent<ObjectPool>();
        leftStick = GameObject.Find("Left Joystick").GetComponent<FloatingJoystick>();
        rightStick = GameObject.Find("Right Joystick").GetComponent<FloatingJoystick>();
    }

    void Update()
    {
        cnt--;
        transform.position += speed * leftStick.Direction.normalized;
        if(rightStick.Direction.magnitude > float.Epsilon && cnt < 0)
        {
            GameObject bullet = pool.bulletQueue.Dequeue();
            Vector3 dir = rightStick.Direction.normalized;
            bullet.transform.position = transform.position + 0.5f * dir;
            //bullet.transform.eulerAngles = GetAngle(dir.x, dir.y);
            bullet.GetComponent<Bullet>().v = 0.1f * dir;
            bullet.SetActive(true);
            cnt = 100;
        }
    }
}
