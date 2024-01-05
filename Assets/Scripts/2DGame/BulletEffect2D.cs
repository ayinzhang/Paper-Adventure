using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEffect2D : MonoBehaviour
{
    BulletGroup2D bulletGroup;
    public float time = 0.5f;

    void OnEnable()
    {
        StartCoroutine(Recycle());
    }

    void Start()
    {
        bulletGroup = gameObject.GetComponentInParent<BulletGroup2D>();
    }

    IEnumerator Recycle()
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false); bulletGroup.RecycleEffect();
    }
}
