using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 v;

    IEnumerator Recycle()
    {
        yield return new WaitForSeconds(3);
        ObjectPool pool = GameObject.Find("ObjectPool").GetComponent<ObjectPool>();
        pool.Return(ref pool.bulletQueue, gameObject);
    }

    void OnEnable()
    {
        StartCoroutine("Recycle");
    }

    void Update()
    {
        transform.position += v;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.gameObject.GetComponent<Player>().Hurt(10);
        else if(collision.gameObject.CompareTag("Enemy"))
            collision.gameObject.GetComponent<Enemy>().Hurt(10);
        ObjectPool pool = GameObject.Find("ObjectPool").GetComponent<ObjectPool>();
        GameObject effect = pool.Get(ref pool.bulletEffectQueue);
        effect.transform.position = transform.position;
        effect.SetActive(true);
        pool.Return(ref pool.bulletQueue, gameObject);
    }
}
