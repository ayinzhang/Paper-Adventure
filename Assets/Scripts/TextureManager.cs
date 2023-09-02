using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureManager : MonoBehaviour
{
    Transform cameraTrans;
    Transform[] objTrans;

    void Start()
    {
        cameraTrans = GameObject.Find("Main Camera").transform;
        objTrans = new Transform[transform.childCount];
        for (int i = 0; i < objTrans.Length; ++i)
            objTrans[i] = transform.GetChild(i);
    }

    void Update()
    {
        if (objTrans.Length > 0 && objTrans[0].eulerAngles.y != cameraTrans.eulerAngles.y)
            for (int i = 0; i < objTrans.Length; ++i)
                objTrans[i].localEulerAngles = new Vector3(transform.eulerAngles.x, cameraTrans.eulerAngles.y, transform.eulerAngles.z);
    }
}
