using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerState
{
    private const string SPEED_POTION_NAME = "SpeedPotion";
    public int credits = 50;
    public Dictionary<string, Potion> items = new Dictionary<string, Potion>();

    // Player's position on the map for leaving at the same position
    internal Vector3 lastPosition = Vector3.zero;
    internal Vector3 lastStashPosition = Vector3.zero;
    internal bool hasLastPosition = false;

    internal bool isUsingSpeedPotion = false;
    internal float lastSpeed;

    internal bool tryBuyItem(ShopItem item)
    {
        if (credits < item.price)
        {
            Debug.Log("Out of credits..");
            return false;
        }

        credits -= item.price;
        if (items.ContainsKey(item.name))
        {
            items[item.name].quantity += 1;
        }
        else
        {
            Potion potion = (item.name == SPEED_POTION_NAME) ? (Potion) new SpeedPotion(1, item.sprite) : new HealthPotion(1, item.sprite);

            items.Add(item.name, potion);
        }

        return true;
    }

    public void addCredits()
    {
        credits += 10;
    }

    // Print inventory
    public void printCurrentInventory()
    {
        if (items.Count == 0)
        {
            Debug.Log("Inventário vazio.");
            return;
        }

        foreach (var x in items)
        {
            Debug.Log(x.Key + " - " + x.Value.quantity);
        }
    }

    internal void saveLastPosition()
    {
        hasLastPosition = true;
        lastPosition = lastStashPosition;
    }
    internal void stashLastPosition(Vector3 position)
    {
        lastStashPosition = position;
    }

    public float speed = 10.0f;
    public bool isDead = false;
    //public Text liveText;
    //public Scrollbar healthSlider;
    [HideInInspector]
    public Image healthBar;
    [Space]
    //public int lives;
    //private int maxLives = 3;
    public float maxHealth = 150;
    public float currentHealth = 150;

    public void Initialize()
    {
        //lives = maxLives;
        currentHealth = maxHealth;
    }

    /*
public void OnDie()
{
   if (lives > 0)
   {
       --lives;
       UpdateLives();
       currentHealth = maxHealth;
       UpdateHelth();
   }
   {
       Debug.Log("Game Over");
   }
}*/
    public void OnDie()
    {
        isDead = true;
        GameManager.Instance.playerController.die();
    }

    public void TakeHit(int damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            UpdateHealth();
        }
        else
        {
            OnDie();
        }
    }

    /*
    private void UpdateLives()
    {
        liveText.text = lives.ToString() + "X";
    }*/

    public void UpdateHealth()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
        //healthSlider.size = currentHealth / maxHealth;
    }

    internal void restoreHp(int healthToRestore)
    {
        currentHealth = Math.Min(currentHealth + healthToRestore, maxHealth);
        UpdateHealth();
    }
}
