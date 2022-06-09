using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PotionUIManager
{
    public GameObject potionsTray;


    public void UpdateInventory()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject potionUi = potionsTray.transform.GetChild(i).gameObject;

            // If player doesn't have that potion, hide its UI
            if (i >= GameManager.Instance.playerState.items.Count)
            {
                if (potionUi.activeSelf != false)
                    potionUi.SetActive(false);
                continue;
            }


            // Set UI to visible
            if (potionUi.activeSelf != true)
                potionUi.SetActive(true);


            // Populate values
            Potion potion = GameManager.Instance.playerState.items.Values.ElementAt(i);

            Text text = potionUi.GetComponentInChildren<Text>();
            text.text = "x" + potion.quantity;

            Image image = potionUi.GetComponentInChildren<Image>();
            image.sprite = potion.sprite;
        }
    }

    internal void updatePotionTray()
    {
        potionsTray = GameObject.Find("Potions");
        UpdateInventory();
    }

}
