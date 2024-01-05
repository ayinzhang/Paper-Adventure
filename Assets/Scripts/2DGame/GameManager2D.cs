using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager2D : MonoBehaviour
{
    static GameManager2D _instance;
    public ObjectPool[] bulletPools, effectPools;
    public GameObject[] bullets, effects;

    public static GameManager2D instance
    {
        get
        {
            if(_instance != null) return _instance;
            return _instance = new GameManager2D();
        }
    }

    void Start()
    {
        bulletPools = new ObjectPool[bullets.Length];
        effectPools = new ObjectPool[effects.Length];

        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i].GetComponent<BulletGroup2D>().bulletNum = i;
            bulletPools[i] = new ObjectPool(bullets[i]); 
        }

        for (int i = 0; i < effects.Length; i++) 
        {
            effects[i].GetComponent<Effect2D>().effectNum = i;
            effectPools[i] = new ObjectPool(effects[i]);
        }
    }
}

public class ObjectPool
{
    GameObject obj;
    Stack<GameObject> pool;

    public ObjectPool(GameObject go)
    {
        obj = go;
        pool = new Stack<GameObject>();
    }

    public GameObject Get()
    {
        if (pool.Count == 0) pool.Push(GameObject.Instantiate(obj));
        GameObject go = pool.Pop(); go.SetActive(true); return go;
    }

    public void Release(GameObject go)
    {
        go.SetActive(false);
        pool.Push(go);
    }
}
