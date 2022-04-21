using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopManager : MonoBehaviour
{

    public ShopItem[] items;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        foreach (ShopItem item in items)
        {
            item.button.onClick.AddListener(() => buyItem(item));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Buy()
    {
        
    }

    void buyItem(ShopItem item)
    {
        Debug.Log("Buying " + item.name);
    }
}
