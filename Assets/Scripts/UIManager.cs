using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UIManager
{
    public GameObject potionsTray;


    public void UpdateInventory()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject child = potionsTray.transform.GetChild(i).gameObject;

            if (i >= GameManager.Instance.playerState.items.Count)
            {
                if (child.activeSelf != false)
                    child.SetActive(false);
                continue;
            }


            // print inventory
            //Debug.LogWarning(GameManager.Instance.playerState.items.Values.ToArray() + " .. " + i);
            KeyValuePair<string, Potion> keyValuePair = GameManager.Instance.playerState.items.ElementAt(i);
            Potion value = keyValuePair.Value;

            if (child.activeSelf != true)
                child.SetActive(true);
            Text text = child.GetComponentInChildren<Text>();
            text.text = "x" + value.quantity;

        }
    }
}
