using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPotion : Potion
{
    
    public SpeedPotion(int quantity, Sprite sprite) : base(quantity, sprite) { }

    internal override bool Consume()
    {
        if (GameManager.Instance.playerState.isUsingSpeedPotion)
            return false;
        Debug.Log("Consuming Speed Potion..");
        GameManager.Instance.playerState.lastSpeed = GameManager.Instance.playerState.speed;
        GameManager.Instance.playerState.speed *= 1.4f;
        GameManager.Instance.playerState.isUsingSpeedPotion = true;
        GameManager.Instance.StartCoroutine(ResetSpeed(5f));
        return true;
    }

    IEnumerator ResetSpeed(float delayTime)
    {
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);
        //Do the action after the delay time has finished.
        GameManager.Instance.playerState.speed = GameManager.Instance.playerState.lastSpeed;
        GameManager.Instance.playerState.isUsingSpeedPotion = false;
    }
}
