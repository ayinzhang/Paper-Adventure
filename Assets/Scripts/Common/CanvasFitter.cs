using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasFitter : MonoBehaviour
{
    float targetHeight = 1440;

    void Start()
    {
        gameObject.GetComponent<CanvasScaler>().scaleFactor = Screen.height / targetHeight;
    }


    void Update()
    {
        
    }
}
