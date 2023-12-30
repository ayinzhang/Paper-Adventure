using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GameManager2D : MonoBehaviour
{
    static GameManager2D _instance;
    public ObjectPool<GameObject> arrowPool;
    public GameObject arrow;

    public static GameManager2D instance
    {
        get
        {
            if(_instance != null) return _instance;
            return _instance = new GameManager2D();
        }
    }

    void Start()
    {
        arrowPool = new ObjectPool<GameObject>(
            () => { return Instantiate(arrow); },//Create
            (go) => { go.SetActive(true); },//Get
            (go) => { go.SetActive(false); },//Release
            (go) => { Destroy(go); }//Destroy
            ) ;
    }
}
