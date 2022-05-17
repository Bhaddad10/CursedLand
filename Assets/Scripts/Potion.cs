using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion
{
    public int quantity;

    public Potion(int quantity)
    {
        this.quantity = quantity;
    }

    internal void Consume()
    {
        //GameManager.Instance.playerState.life += 20;
        Debug.Log("Consuming potion..");
    }
}
