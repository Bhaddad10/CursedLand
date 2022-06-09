using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Potion
{
    public HealthPotion(int quantity, string sprite) : base(quantity, sprite) { }

    internal override bool Consume()
    {
        Debug.Log("Consuming Health Potion..");
        GameManager.Instance.playerState.restoreHp(healthToRestore);
        return true;
    }
}
