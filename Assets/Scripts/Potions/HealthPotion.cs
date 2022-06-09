using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Potion
{
    public int healthToRestore = 50;

    public HealthPotion(int quantity, Sprite sprite) : base(quantity, sprite) { }
    internal override bool Consume()
    {
        //Debug.Log("Consuming Health Potion..");
        GameManager.Instance.playerState.restoreHp(healthToRestore);
        return true;
    }
}
