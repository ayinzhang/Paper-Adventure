using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2D : MonoBehaviour
{
    GameManager2D gm;
    Transform playerTrans;
    int bulletNum; float dis, subLR;
    public float speed = 10f;
    public GameObject[] bullet;
    public (int, float, bool)[] bulletData;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager2D>();
        playerTrans = GameObject.Find("Player").transform;

        bulletData = new (int, float, bool)[bullet.Length];
        for (int i = 0; i < bullet.Length; i++)
        {
            for (int j = 0; j < gm.bullet.Length; j++)
            {
                if (bullet[i].name == gm.bullet[j].name)
                {
                    bulletData[i] = (j, gm.bullet[j].GetComponent<Bullet2D>().cd, true);
                    break;
                }
            }
        }

        StartCoroutine(ChangeDirAndWeapon());
    }

    void Update()
    {
        float fac = Mathf.Clamp((transform.position - playerTrans.position).magnitude - dis, -4, 4) / 4,
           subFac = Mathf.Sqrt(1 - fac * fac);
        Vector3 toDir = (playerTrans.position - transform.position).normalized,
               subDir = Vector3.Cross(toDir, new Vector3(0, 0, 1));
        transform.position += speed * Time.deltaTime * (fac * toDir + subLR * subFac * subDir);

        if (bulletData[bulletNum].Item3)
        {
            StartCoroutine(CD(bulletNum));
            GameObject bullet1 = gm.bulletPool[0].Get();
            GameObject bullet = gm.bulletPool[bulletData[bulletNum].Item1].Get();
            bullet.transform.position = transform.position + 1.5f * toDir;
            bullet.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(new Vector2(1, 0), toDir));
        }
    }

    IEnumerator CD(int bulletNum)
    {
        bulletData[bulletNum].Item3 = false;
        yield return new WaitForSeconds(bulletData[bulletNum].Item2);
        bulletData[bulletNum].Item3 = true;
    }

    IEnumerator ChangeDirAndWeapon()
    {
        while (true)
        {
            dis = Random.Range(7f, 10f);
            subLR = Random.Range(-1f, 1f);
            bulletNum = Random.Range(0, bullet.Length);
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        }
    }
}
