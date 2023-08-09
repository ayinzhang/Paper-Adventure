using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float nearRate;
    float speedRate;
    Transform player;
    int hp = 100;
    float speed = 0.25f;
    int leftRight = 1;

    IEnumerator ChangeDir()
    {
        while(true)
        {
            leftRight = speedRate < 0.6f ? 1 : -1;
            yield return new WaitForSeconds(Random.Range(1, 3));
        }
    }

    public void Hurt(int dmg)
    {
        hp -= dmg;
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
        if (toDir.magnitude < 10) nearRate = Mathf.SmoothStep(-1, 0, toDir.magnitude / 10);
        else nearRate = Mathf.SmoothStep(0, 1, toDir.magnitude / 10 - 1);
        //transform.position += speed * speedRate * (nearRate * toDir + leftRight * (1 - Mathf.Abs(nearRate)) * tanDir).normalized;
    }
}
