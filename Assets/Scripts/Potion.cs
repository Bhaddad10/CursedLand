using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion
{
    public int quantity;
    public int healthToRestore = 50;
    public string sprite;

    public Potion(int quantity, string sprite)
    {
        this.quantity = quantity;
        this.sprite = sprite;
    }

    internal void Consume()
    {
        //GameManager.Instance.playerState.life += 20;
        Debug.Log("Consuming potion..");
        GameManager.Instance.playerState.restoreHp(healthToRestore);
    }
}
