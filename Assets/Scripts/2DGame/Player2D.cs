using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2D : MonoBehaviour
{
    GameManager2D gm;
    Joystick leftStick, rightStick;
    public int bulletNum;
    public float speed = 10f;
    public GameObject []bullet;
    public (int, float, bool) []bulletData;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager2D>();
        leftStick = GameObject.Find("LeftJoystick").GetComponent<Joystick>();
        rightStick = GameObject.Find("RightJoystick").GetComponent<Joystick>();

        bulletData = new (int, float, bool)[bullet.Length];
        for (int i = 0; i < bullet.Length; i++) 
        {
            for (int j = 0; j < gm.bullet.Length; j++) 
            {
                if (bullet[i].name == gm.bullet[j].name)
                {
                    bulletData[i] = (j, gm.bullet[j].GetComponent<Bullet2D>().cd, true);
                    break;
                }
            }
        }
    }

    void Update()
    {
        transform.position += speed * Time.deltaTime * new Vector3(leftStick.Horizontal, leftStick.Vertical, 0);

        if (bulletData[bulletNum].Item3 && (rightStick.Horizontal != 0 || rightStick.Vertical != 0))
        {
            StartCoroutine(CD(bulletNum));
            GameObject bullet1 = gm.bulletPool[0].Get();
            GameObject bullet = gm.bulletPool[bulletData[bulletNum].Item1].Get();
            bullet.transform.position = transform.position + 1.5f * new Vector3(rightStick.Horizontal, rightStick.Vertical, 0).normalized;
            bullet.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(new Vector2(1, 0), new Vector2(rightStick.Horizontal, rightStick.Vertical)));
        }
    }

    IEnumerator CD(int bulletNum)
    {
        bulletData[bulletNum].Item3 = false;
        yield return new WaitForSeconds(bulletData[bulletNum].Item2);
        bulletData[bulletNum].Item3 = true;
    }
}
