using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    int hp = 100;
    float speed = 0.35f;
    public int bulletNum = 0;
    bool isDush = false;
    public bool isDushCD = false;
    Rigidbody2D rb;
    Joystick leftStick;
    Joystick rightStick;
    public GameObject[] bulletObj;
    Bullet[] bullet;

    public void Hurt(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        { 
            Destroy(gameObject); 
        }
    }

    IEnumerator Dush()
    {
        isDush = isDushCD = true;
        yield return new WaitForSeconds(0.2f);
        isDush = false;
        yield return new WaitForSeconds(1.8f);
        isDushCD = false;
    }

    IEnumerator BulletCD(int bulletNum)
    {
        bullet[bulletNum].enable = false;
        yield return new WaitForSeconds(bullet[bulletNum].cd);
        bullet[bulletNum].enable = true;
    }

    Vector3 GetAngle(float x, float y)
    {
        if (x == 0) return new Vector3(y > 0 ? -Mathf.PI / 2 : Mathf.PI / 2, 90, 0);
        else if (x > 0) return new Vector3(-Mathf.Atan(y / x) / Mathf.PI * 180, 90, 0);
        else return new Vector3(180 - Mathf.Atan(y / x) / Mathf.PI * 180, 90, 0);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bullet = new Bullet[bulletObj.Length];
        for (int i = 0; i < bulletObj.Length; ++i) bullet[i] = bulletObj[i].GetComponent<Bullet>();
        leftStick = GameObject.Find("Left Joystick").GetComponent<Joystick>();
        rightStick = GameObject.Find("Right Joystick").GetComponent<Joystick>();
    }

    void Update()
    {
        rb.velocity = speed * leftStick.Direction.normalized;
        if(bullet[bulletNum].enable && rightStick.Direction.magnitude > float.Epsilon)
        {
            Vector3 dir = rightStick.Direction.normalized;
            Instantiate(bulletObj[bulletNum], transform.position + dir, Quaternion.Euler(GetAngle(dir.x, dir.y)));
            StartCoroutine("BulletCD", bulletNum);
        }
    }
}
