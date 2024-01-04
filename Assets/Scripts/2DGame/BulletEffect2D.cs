using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEffect2D : MonoBehaviour
{
    GameManager2D gm;
    public float time = 0.5f;

    void OnEnable()
    {
        StartCoroutine(Recycle());
    }

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager2D>();
    }

    IEnumerator Recycle()
    {
        yield return new WaitForSeconds(time);
        gm.bulletEffectPools[0].Release(gameObject);
    }
}
