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
            items.Add(item.name, new Potion(1));
        }

        printCurrentInventory();

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
        

    /*public Text liveText;
    public Scrollbar healthSlider;
    public Image healthImage;
    [Space]
    public int lives;
    private int maxLives = 3;
    public float currentHelth;
    public float maxHelth;

    public void Initialize()
    {
        lives = maxLives;
        currentHelth = maxHelth;
    }

    public void OnDie()
    {
        if (lives > 0)
        {
            --lives;
            UpdateLives();
            currentHelth = maxHelth;
            UpdateHelth();
        }
        {
            Debug.Log("Game Over");
        }
    }

    public void TakeHit()
    {
        if (currentHelth > 0)
        {
            currentHelth -= 30;
            UpdateHelth();
        }
        else
        {
            OnDie();
        }
    }

    private void UpdateLives()
    {
        liveText.text = lives.ToString() + "X";
    }

    private void UpdateHelth()
    {
        healthImage.fillAmount = currentHelth / maxHelth;
        healthSlider.size = currentHelth / maxHelth;
    }*/
}
