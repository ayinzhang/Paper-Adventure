using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet2D : MonoBehaviour
{
    public float dmg = 10, speed = 17, time = 3, cd = 0.5f;
    public enum BulletType { Normal, Track, Laser};
    public BulletType type;
    public GameObject effect;
    [HideInInspector] public int bulletNum = -1;
    [HideInInspector] public Transform targetTrans;
    float t;

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;
        Vector3 collisionPoint = new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y, 0);
        switch (obj.tag)
        {
            case "Bullet":
                if (type <= BulletType.Track)
                    Recycle(collisionPoint);
                break;
            case "Player":
                if (targetTrans.name.Equals("Player"))
                    Recycle(collisionPoint);
                break;
            case "Enemy":
                if (targetTrans.name.Equals("Enemy"))
                    Recycle(collisionPoint);
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
                transform.right = Vector3.RotateTowards(transform.right, toDir, 0.05f, 0);
                transform.position += speed * Time.deltaTime * transform.right;
                break;
            case BulletType.Laser:
                transform.position += speed * Time.deltaTime * transform.right;
                break;
        }

        if ((t += Time.deltaTime) >= time) Recycle(transform.localPosition);
    }

    void Recycle(Vector3 collisionPoint)
    {
        t = 0; gameObject.SetActive(false);
        effect.gameObject.transform.position = collisionPoint;
        effect.gameObject.transform.rotation = transform.rotation;
        effect.SetActive(true);
    }
}
