using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2D : MonoBehaviour
{
    GameManager2D gm;
    Transform playerTrans;
    int bulletNum; float dis, subLR;
    public float speed = 10f;
    public GameObject[] bullets;
    public (float, bool)[] bulletDatas;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager2D>();
        playerTrans = GameObject.Find("Player").transform;

        bulletDatas = new (float, bool)[bullets.Length];
        for (int i = 0; i < bullets.Length; i++)
            bulletDatas[i] = (bullets[i].GetComponent<Bullet2D>().cd, true);

        StartCoroutine(ChangeDirAndWeapon());
    }

    void Update()
    {
        float fac = Mathf.Clamp((transform.position - playerTrans.position).magnitude - dis, -4, 4) / 4,
           subFac = Mathf.Sqrt(1 - fac * fac);
        Vector3 toDir = (playerTrans.position - transform.position).normalized,
               subDir = Vector3.Cross(toDir, new Vector3(0, 0, 1));
        transform.position += speed * Time.deltaTime * (fac * toDir + subLR * subFac * subDir);

        if (bulletDatas[bulletNum].Item2)
        {
            StartCoroutine(CD(bulletNum));
            GameObject bullet = bulletNum == 0 ? gm.bulletPools[0].Get() : Instantiate(bullets[bulletNum]);
            bullet.layer = LayerMask.NameToLayer("EnemyBullet");
            bullet.GetComponent<Bullet2D>().targetTrans = playerTrans;
            bullet.transform.position = transform.position + 1.5f * toDir;
            bullet.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(new Vector2(1, 0), toDir));
        }
    }

    IEnumerator CD(int bulletNum)
    {
        bulletDatas[bulletNum].Item2 = false;
        yield return new WaitForSeconds(bulletDatas[bulletNum].Item1);
        bulletDatas[bulletNum].Item2 = true;
    }

    IEnumerator ChangeDirAndWeapon()
    {
        while (true)
        {
            dis = Random.Range(7f, 10f);
            subLR = Random.Range(-1f, 1f);
            bulletNum = Random.Range(0, bullets.Length);
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        }
    }
}
