using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager2D : MonoBehaviour
{
    static GameManager2D _instance;
    GameObject player, enemy;
    public ObjectPool[] bulletPools, effectPools;
    public GameObject[] players, enemys, bullets, effects;


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
        player = Instantiate(players[Random.Range(1, players.Length)]); player.name = "Player";
        enemy = Instantiate(enemys[Random.Range(0, enemys.Length)]); enemy.name = "Enemy";
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

    public void Win()
    {
        Destroy(player);
        Transform canvasTrans = GameObject.Find("Canvas").transform;
        canvasTrans.GetChild(0).gameObject.SetActive(false);
        StartCoroutine(Show(canvasTrans.GetChild(1).gameObject));
    }

    public void Lose()
    {
        Destroy(enemy);
        Transform canvasTrans = GameObject.Find("Canvas").transform;
        canvasTrans.GetChild(0).gameObject.SetActive(false);
        StartCoroutine(Show(canvasTrans.GetChild(2).gameObject));
    }

    IEnumerator Show(GameObject go)
    {
        go.SetActive(true);
        Image img = go.GetComponent<Image>();
        for (float i = 0; i<1;i+=1f/30)
        {
            img.color = new Color(1, 1, 1, i);
            yield return new WaitForSeconds(1f / 30);
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene("2DGame");
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
