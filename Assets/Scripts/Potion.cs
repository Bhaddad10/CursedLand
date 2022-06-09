using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PotionType
{
    Health,
    Speed
}

public abstract class Potion
{
    public int quantity;
    public int healthToRestore = 50;
    public string sprite;

    public Potion(int quantity, string sprite)
    {
        this.quantity = quantity;
        this.sprite = sprite;
    }

    internal abstract void Consume();
}
