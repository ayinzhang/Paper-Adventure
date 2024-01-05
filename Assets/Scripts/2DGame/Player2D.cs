using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2D : MonoBehaviour
{
    GameManager2D gm;
    Joystick leftStick, mainStick, subStick;
    Transform enemyTrans;
    public float speed = 10f;
    public GameObject []bullets;
    public (int, float, bool) []bulletDatas;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager2D>();
        enemyTrans = GameObject.Find("Enemy").GetComponent<Transform>();
        leftStick = GameObject.Find("LeftJoystick").GetComponent<Joystick>();
        mainStick = GameObject.Find("MainJoystick").GetComponent<Joystick>();
        subStick = GameObject.Find("SubJoystick").GetComponent<Joystick>();

        bulletDatas = new (int, float, bool)[bullets.Length];
        for (int i = 0; i < bullets.Length; i++)
        {
            bulletDatas[i] = (-1, bullets[i].GetComponentInChildren<Bullet2D>().cd, true);
            for (int j = 0; j < gm.bullets.Length; j++)
                if (bullets[i].name == gm.bullets[j].name)
                {
                    bulletDatas[i].Item1 = j;
                    break;
                }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 collisionPoint = new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y, 0);
        if(collision.gameObject.CompareTag("Bullet"))
        {
            GameObject effect = gm.effectPools[0].Get();
            effect.transform.parent = transform;
            effect.transform.position = collisionPoint;
            effect.transform.right = (collisionPoint - transform.position).normalized;
        }
    }

    void Update()
    {
        transform.position += speed * Time.deltaTime * new Vector3(leftStick.Horizontal, leftStick.Vertical, 0);

        if (bulletDatas[0].Item3 && (mainStick.Horizontal != 0 || mainStick.Vertical != 0))
        {
            StartCoroutine(CD(0));
            GameObject bullet = gm.bulletPools[0].Get();
            bullet.transform.position = transform.position + 1.5f * new Vector3(mainStick.Horizontal, mainStick.Vertical, 0).normalized;
            bullet.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(new Vector2(1, 0), new Vector2(mainStick.Horizontal, mainStick.Vertical)));
            Bullet2D[] bulletGroup = bullet.GetComponentsInChildren<Bullet2D>();
            for (int i = 0; i < bulletGroup.Length; i++)
            {
                bulletGroup[i].targetTrans = enemyTrans;
                bulletGroup[i].gameObject.layer = LayerMask.NameToLayer("PlayerBullet");
            }
        }
        else if (bulletDatas[1].Item3 && (subStick.Horizontal != 0 || subStick.Vertical != 0))
        {
            StartCoroutine(CD(1));
            GameObject bullet = bulletDatas[1].Item1 != -1 ? gm.bulletPools[bulletDatas[1].Item1].Get(): Instantiate(bullets[1]);
            bullet.transform.position = transform.position + new Vector3(mainStick.Horizontal, mainStick.Vertical, 0).normalized;
            bullet.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(new Vector2(1, 0), new Vector2(subStick.Horizontal, subStick.Vertical)));
            Bullet2D[] bulletGroup = bullet.GetComponentsInChildren<Bullet2D>();
            for (int i = 0; i < bulletGroup.Length; i++)
            {
                bulletGroup[i].targetTrans = enemyTrans;
                bulletGroup[i].gameObject.layer = LayerMask.NameToLayer("PlayerBullet");
            }
        }
    }

    IEnumerator CD(int bulletNum)
    {
        bulletDatas[bulletNum].Item3 = false;
        yield return new WaitForSeconds(bulletDatas[bulletNum].Item2);
        bulletDatas[bulletNum].Item3 = true;
    }
}
