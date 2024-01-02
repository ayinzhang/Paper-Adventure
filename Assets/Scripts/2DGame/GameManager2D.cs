using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager2D : MonoBehaviour
{
    static GameManager2D _instance;
    public ObjectPool[] bulletPool, bulletEffectPool;
    public GameObject[] bullet, bulletEffect;

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
        bulletPool = new ObjectPool[bullet.Length];
        bulletEffectPool = new ObjectPool[bullet.Length];

        for (int i = 0; i < bullet.Length; i++)
        {
            bullet[i].GetComponent<Bullet2D>().bulletNum = i;
            bulletPool[i] = new ObjectPool(bullet[i]); 
            bulletEffectPool[i] = new ObjectPool(bulletEffect[i]);
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
