using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect2D : MonoBehaviour
{
    public float effectTime;
    //[HideInInspector] 
    public int effectNum = -1;
    GameManager2D gm;

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
        yield return new WaitForSeconds(effectTime);
        gm.effectPools[effectNum].Release(gameObject);
    }
}
