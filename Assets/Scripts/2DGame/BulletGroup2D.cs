using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGroup2D : MonoBehaviour
{
    [HideInInspector] public int bulletNum = -1;
    GameManager2D gm; Bullet2D[] bullets; (Vector3, Quaternion)[] trans; int cnt; bool b;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager2D>();
        bullets = gameObject.GetComponentsInChildren<Bullet2D>();
        trans = new (Vector3, Quaternion)[cnt = bullets.Length];
        for (int i= 0; i < bullets.Length;i++)
            trans[i] = (bullets[i].transform.localPosition, bullets[i].transform.localRotation);
    }

    public void RecycleEffect()
    {
        if (--cnt == 0)
        {
            cnt = bullets.Length;
            gameObject.transform.position = Vector3.zero;
            gameObject.transform.rotation = Quaternion.identity;
            for (int i = 0; i < bullets.Length; i++)
            { 
                bullets[i].gameObject.SetActive(true);
                bullets[i].gameObject.transform.position = trans[i].Item1;
                bullets[i].gameObject.transform.rotation = trans[i].Item2;
            }
            if (bulletNum != -1) gm.bulletPools[bulletNum].Release(gameObject);
            else Destroy(gameObject);
        }
    }
}
