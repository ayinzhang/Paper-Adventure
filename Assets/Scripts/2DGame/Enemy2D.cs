using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2D : MonoBehaviour
{
    GameManager2D gm;
    Transform playerTrans;
    SpriteRenderer render;
    int bulletNum; float h, dis, subLR;
    public float hp = 100, speed = 10f;
    public GameObject[] bullets;
    public (int, float, bool)[] bulletDatas;

    void Start()
    {
        h = hp;
        render = gameObject.GetComponent<SpriteRenderer>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager2D>();
        playerTrans = GameObject.Find("Player").transform;

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

        StartCoroutine(ChangeDirAndWeapon());
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 collisionPoint = new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y, 0);
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if ((h -= collision.gameObject.GetComponent<Bullet2D>().dmg) > 0)
            {
                GameObject effect = gm.effectPools[0].Get();
                effect.transform.parent = transform;
                effect.transform.position = collisionPoint;
                effect.transform.right = (collisionPoint - transform.position).normalized;
                render.color = new Color(1, 1, 1, h / hp);
            }
            else
            {
                GameObject effect = gm.effectPools[1].Get();
                effect.transform.position = transform.position;
                gm.Lose(); Destroy(gameObject);
            }
        }
    }

    void Update()
    {
        float fac = Mathf.Clamp((transform.position - playerTrans.position).magnitude - dis, -4, 4) / 4,
           subFac = Mathf.Sqrt(1 - fac * fac);
        Vector3 toDir = (playerTrans.position - transform.position).normalized,
               subDir = Vector3.Cross(toDir, new Vector3(0, 0, 1));
        transform.position += speed * Time.deltaTime * (fac * toDir + subLR * subFac * subDir);
        transform.rotation = Quaternion.Euler(0, 0, -100 * Time.deltaTime) * transform.rotation;

        if (bulletDatas[bulletNum].Item3)
        {
            StartCoroutine(CD(bulletNum));
            GameObject bullet = bulletDatas[bulletNum].Item1 != -1 ? gm.bulletPools[bulletDatas[bulletNum].Item1].Get() : Instantiate(bullets[bulletNum]);
            bullet.transform.position = transform.position + 2f * toDir;
            bullet.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(new Vector2(1, 0), toDir));
            Bullet2D[] bulletGroup = bullet.GetComponentsInChildren<Bullet2D>();
            for (int i = 0; i < bulletGroup.Length; i++)
            {
                bulletGroup[i].targetTrans = playerTrans;
                bulletGroup[i].gameObject.layer = LayerMask.NameToLayer("EnemyBullet");
            }

        }
    }

    IEnumerator CD(int bulletNum)
    {
        bulletDatas[bulletNum].Item3 = false;
        yield return new WaitForSeconds(bulletDatas[bulletNum].Item2);
        bulletDatas[bulletNum].Item3 = true;
    }

    IEnumerator ChangeDirAndWeapon()
    {
        while (true)
        {
            dis = Random.Range(7f, 10f);
            subLR = Random.Range(-1f, 1f);
            bulletNum = Random.Range(0, bullets.Length);
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        }
    }
}
