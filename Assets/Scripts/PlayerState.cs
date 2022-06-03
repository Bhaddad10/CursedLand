using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerState
{

    public int credits = 50;
    public Dictionary<string, Potion> items = new Dictionary<string, Potion>();

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
            items.Add(item.name, new Potion(1, item.sprite));
        }

        return true;
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


    //public Text liveText;
    //public Scrollbar healthSlider;
    public Image healthBar;
    [Space]
    public int lives;
    //private int maxLives = 3;
    public float currentHealth;
    public float maxHealth;

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

    private void UpdateHealth()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
        //healthSlider.size = currentHealth / maxHealth;
    }
}
