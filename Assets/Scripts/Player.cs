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
    public GameObject bullet;

    IEnumerator SetBulletCD()
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
        if (x == 0) return new Vector3(y > 0 ? -Mathf.PI / 2 : Mathf.PI / 2, 90, 0);
        else if (x > 0) return new Vector3(-Mathf.Atan(y / x) / Mathf.PI * 180, 90, 0);
        else return new Vector3(180 - Mathf.Atan(y / x) / Mathf.PI * 180, 90, 0);
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
            //GameObject bullet = pool.Get(ref pool.bulletQueue);
            //Vector3 dir = rightStick.Direction.normalized;
            //bullet.transform.position = transform.position + dir;
            //bullet.transform.eulerAngles = GetAngle(dir.x, dir.y);
            //bullet.GetComponent<Bullet>().v = 0.5f * dir;
            //bullet.SetActive(true);
            //StartCoroutine(SetCD(bulletEnable, bulletCD));
            Vector3 dir = rightStick.Direction.normalized;
            Instantiate(bullet, transform.position + dir, Quaternion.Euler(GetAngle(dir.x, dir.y)));
            StartCoroutine("SetBulletCD");
        }
    }
}
