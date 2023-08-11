using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    int hp = 100;
    float speed = 0.35f;
    bool bulletEnable = true;
    float bulletCD = 0.5f;
    ObjectPool pool;
    Joystick leftStick;
    Joystick rightStick;

    IEnumerator BulletCD()
    {
        bulletEnable = false;
        yield return new WaitForSecondsRealtime(bulletCD);
        bulletEnable = true;
    }

    public void Hurt(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        { 
            Destroy(gameObject); 
        }
    }

    Vector3 GetAngle(float x, float y)
    {
        if (x == 0) return new Vector3(0, 0, y > 0 ? Mathf.PI / 2 : - Mathf.PI / 2);
        else if (x > 0) return new Vector3(0, 0, Mathf.Atan(y / x) / Mathf.PI * 180);
        else return new Vector3(0, 0, Mathf.Atan(y / x) / Mathf.PI * 180 + 180);
    }

    void Start()
    {
        pool = GameObject.Find("ObjectPool").GetComponent<ObjectPool>();
        leftStick = GameObject.Find("LeftJoystick").GetComponent<Joystick>();
        rightStick = GameObject.Find("RightJoystick").GetComponent<Joystick>();
    }

    void Update()
    {
        transform.position += speed * leftStick.Direction.normalized;
        if(bulletEnable && rightStick.Direction.magnitude > float.Epsilon)
        {
            GameObject bullet = pool.Get(ref pool.bulletQueue);
            Vector3 dir = rightStick.Direction.normalized;
            bullet.transform.position = transform.position + dir;
            bullet.transform.eulerAngles = GetAngle(dir.x, dir.y);
            bullet.GetComponent<Bullet>().v = 0.5f * dir;
            bullet.SetActive(true);
            StartCoroutine("BulletCD");
        }
    }
}
