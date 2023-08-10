using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject bullet;
    public GameObject bulletEffect;
    [HideInInspector]
    public Queue<GameObject> bulletQueue;
    [HideInInspector]
    public Queue<GameObject> bulletEffectQueue;

    void Push(ref Queue<GameObject> q, GameObject p, int n)
    {
        q = new Queue<GameObject>();
        for (int i = 0; i < n; i++) 
        {
            GameObject obj = Instantiate(p, transform);
            obj.SetActive(false);
            q.Enqueue(obj);
        }
    }

    public GameObject Get(ref Queue<GameObject> q)
    {
        return q.Dequeue();
    }

    public void Return(ref Queue<GameObject> q, GameObject obj)
    {
        obj.SetActive(false);
        q.Enqueue(obj);
    }

    void Start()
    {
        Push(ref bulletQueue, bullet, 10);
        Push(ref bulletEffectQueue, bulletEffect, 10);
    }
}
