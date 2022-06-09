using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Potion
{
    public int quantity;
    public Sprite sprite;

    public Potion(int quantity, Sprite sprite)
    {
        this.quantity = quantity;
        this.sprite = sprite;
    }

    internal abstract bool Consume();
}
