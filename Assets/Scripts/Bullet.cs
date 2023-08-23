using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject hit;
    public float recycleTime = 5;
    public float speed = 15f;
    public bool enable = true;
    public float cd = 0.5f;
    public int damage = 10;
    Rigidbody2D rb;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, recycleTime);
    }

    void FixedUpdate()
    {
        if (speed != 0)
            transform.position += speed * transform.forward * Time.deltaTime;         
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.gameObject.GetComponent<Player>().Hurt(damage);
        else if(collision.gameObject.CompareTag("Enemy"))
            collision.gameObject.GetComponent<Enemy>().Hurt(damage);

        ContactPoint2D contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);

        if (hit != null)
        {
            var hitInstance = Instantiate(hit, contact.point, rot);

            var hitPs = hitInstance.GetComponent<ParticleSystem>();
            if (hitPs != null)
            {
                Destroy(hitInstance, hitPs.main.duration);
            }
            else
            {
                var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitInstance, hitPsParts.main.duration);
            }
        }
        Destroy(gameObject);
        //ObjectPool pool = GameObject.Find("ObjectPool").GetComponent<ObjectPool>();
        //GameObject effect = pool.Get(ref pool.bulletEffectQueue);
        //effect.transform.position = transform.position;
        //effect.SetActive(true);
        //pool.Return(ref pool.bulletQueue, gameObject);
    }
}
