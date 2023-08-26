using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float nearRate;
    float speedRate;
    Transform player;
    int hp = 100;
    float speed = 0.35f;
    int leftRight = 1;
    bool bulletEnable = true;
    float bulletCD = 3f;

    IEnumerator ChangeDir()
    {
        while(true)
        {
            leftRight = speedRate < 0.6f ? 1 : -1;
            yield return new WaitForSeconds(Random.Range(1, 3));
        }
    }

    IEnumerator SetBulletCD()
    {
        bulletEnable = false;
        yield return new WaitForSecondsRealtime(bulletCD);
        bulletEnable = true;
    }

    public void Hurt(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    Vector3 GetAngle(float x, float y)
    {
        if (x == 0) return new Vector3(0, 0, y > 0 ? Mathf.PI / 2 : -Mathf.PI / 2);
        else if (x > 0) return new Vector3(0, 0, Mathf.Atan(y / x) / Mathf.PI * 180);
        else return new Vector3(0, 0, Mathf.Atan(y / x) / Mathf.PI * 180 + 180);
    }

    void Start()
    {
        player = GameObject.Find("Player").transform;
        StartCoroutine("ChangeDir");
    }

    void Update()
    {
        Vector3 toDir = player.position - transform.position;
        Vector3 tanDir = new Vector3(toDir.y, - toDir.x, toDir.z);
        speedRate = Random.Range(0.5f, 0.7f);
        //if (toDir.magnitude < 10) nearRate = Mathf.SmoothStep(-1, 0, toDir.magnitude / 10);
        //else nearRate = Mathf.SmoothStep(0, 1, toDir.magnitude / 10 - 1);
        //transform.position += speed * speedRate * (nearRate * toDir + leftRight * (1 - Mathf.Abs(nearRate)) * tanDir).normalized;

        toDir = toDir.normalized;
        //if (bulletEnable) 
        //{
        //    GameObject bullet = pool.Get(ref pool.bulletQueue);
        //    bullet.transform.position = transform.position + toDir;
        //    bullet.transform.eulerAngles = GetAngle(toDir.x, toDir.y);
        //    bullet.GetComponent<Bullet>().v = 0.5f * toDir;
        //    bullet.SetActive(true);
        //    StartCoroutine("SetBulletCD");
        //}
    }
}
