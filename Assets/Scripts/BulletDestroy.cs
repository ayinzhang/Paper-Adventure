using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroy : MonoBehaviour
{
    IEnumerator Recycle()
    {
        yield return new WaitForSeconds(GetComponent<ParticleSystem>().main.duration);
        ObjectPool pool = GameObject.Find("ObjectPool").GetComponent<ObjectPool>();
        pool.Return(ref pool.bulletEffectQueue, gameObject);
    }

    void OnEnable()
    {
        StartCoroutine("Recycle");
    }
}
