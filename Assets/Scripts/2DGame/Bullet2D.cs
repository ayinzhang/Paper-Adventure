using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet2D : MonoBehaviour
{
    public float hp = 10, dmg = 10, speed = 17, time = 3, cd = 0.5f;
    public int bulletNum;
    GameManager2D gm; float t;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;
        switch (obj.tag)
        {
            case "Bullet":
                if ((hp -= obj.GetComponent<Bullet2D>().dmg) <= 0)
                    Recycle();
                break;
            case "Player":
                Recycle();
                break;
        }
    }

    void Update()
    {
        transform.position += speed * Time.deltaTime * transform.right;
        if ((t += Time.deltaTime) >= time) { t = 0; gm.bulletPool[bulletNum].Release(gameObject); }
    }

    void Recycle()
    {
        t = 0;
        gm.bulletPool[bulletNum].Release(gameObject);
        GameObject effect = gm.bulletEffectPool[bulletNum].Get();
        effect.transform.position = transform.position;
        effect.GetComponent<BulletEffect2D>().bulletNum = bulletNum;
    }
}
