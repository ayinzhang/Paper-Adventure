using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvent : MonoBehaviour
{
    Player player;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    public void Dush()
    {
        if (player.isDushCD) player.StartCoroutine("Dush");
    }

    public void SubEqu()
    {
        player.bulletNum = 0;
    }

    public void Equ()
    {
        player.bulletNum = 1;
    }
}
