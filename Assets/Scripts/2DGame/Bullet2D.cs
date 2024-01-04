using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet2D : MonoBehaviour
{
    public float dmg = 10, speed = 17, time = 3, cd = 0.5f;
    public enum BulletType { Normal, Track};
    public BulletType type;
    [HideInInspector]
    public int bulletNum;
    [HideInInspector]
    public Transform targetTrans;
    GameManager2D gm;
    TrailRenderer tr;
    float t;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager2D>();
        tr = gameObject.GetComponentInChildren<TrailRenderer>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;
        switch (obj.tag)
        {
            case "Bullet":
                if ((dmg -= obj.GetComponent<Bullet2D>().dmg) <= 0)
                    Recycle();
                break;
            case "Player":
                if (targetTrans.name.Equals("Player"))
                    Recycle();
                break;
            case "Enemy":
                if (targetTrans.name.Equals("Enemy"))
                    Recycle();
                break;
        }
    }

    void Update()
    {
        switch (type) 
        {
            case BulletType.Normal:
                transform.position += speed * Time.deltaTime * transform.right;
                break;
            case BulletType.Track:
                Vector3 toDir = (targetTrans.position - transform.position).normalized;
                transform.right = Vector3.Slerp(transform.right, toDir, Mathf.Min(1, 1.5f / Vector3.Angle(toDir, transform.right)));
                transform.position += speed * Time.deltaTime * transform.right;
                break;
        }

        if ((t += Time.deltaTime) >= time) 
            if (bulletNum == 0) { t = 0; gm.bulletPools[bulletNum].Release(gameObject); }
            else Destroy(gameObject);
    }

    void Recycle()
    {
        t = 0;
        GameObject effect = gm.bulletEffectPools[0].Get();
        effect.transform.position = transform.position;
        if (bulletNum == 0) gm.bulletPools[0].Release(gameObject); else Destroy(gameObject);
    }
}
