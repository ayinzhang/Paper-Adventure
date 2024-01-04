using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2D : MonoBehaviour
{
    GameManager2D gm;
    Joystick leftStick, rightStick;
    Transform enemyTrans;
    public int bulletNum;
    public float speed = 10f;
    public GameObject []bullets;
    public (float, bool) []bulletDatas;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager2D>();
        enemyTrans = GameObject.Find("Enemy").GetComponent<Transform>();
        leftStick = GameObject.Find("LeftJoystick").GetComponent<Joystick>();
        rightStick = GameObject.Find("RightJoystick").GetComponent<Joystick>();

        bulletDatas = new (float, bool)[bullets.Length];
        for (int i = 0; i < bullets.Length; i++) 
            bulletDatas[i] = (bullets[i].GetComponent<Bullet2D>().cd, true);
    }

    void Update()
    {
        transform.position += speed * Time.deltaTime * new Vector3(leftStick.Horizontal, leftStick.Vertical, 0);

        if (bulletDatas[bulletNum].Item2 && (rightStick.Horizontal != 0 || rightStick.Vertical != 0))
        {
            StartCoroutine(CD(bulletNum));
            GameObject bullet = bulletNum == 0 ? gm.bulletPools[0].Get() : Instantiate(bullets[bulletNum]);
            bullet.layer = LayerMask.NameToLayer("PlayerBullet");
            bullet.GetComponent<Bullet2D>().targetTrans = enemyTrans;
            bullet.transform.position = transform.position + 1.5f * new Vector3(rightStick.Horizontal, rightStick.Vertical, 0).normalized;
            bullet.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(new Vector2(1, 0), new Vector2(rightStick.Horizontal, rightStick.Vertical)));
        }
    }

    IEnumerator CD(int bulletNum)
    {
        bulletDatas[bulletNum].Item2 = false;
        yield return new WaitForSeconds(bulletDatas[bulletNum].Item1);
        bulletDatas[bulletNum].Item2 = true;
    }
}
