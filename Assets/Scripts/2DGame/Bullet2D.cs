using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet2D : MonoBehaviour
{
    public float hp = 10, dmg = 10, speed = 0.5f, time = 3;
    GameManager2D gm;
    float t;

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
        transform.position += speed * transform.right;
        if ((t += Time.deltaTime) >= time) { t = 0; gm.arrowPool.Release(gameObject); }
    }

    void Recycle()
    {
        t = 0; 
        gm.arrowPool.Release(gameObject);
    }
}
