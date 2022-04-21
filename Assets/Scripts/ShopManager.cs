using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ShopManager : MonoBehaviour
{

    public Text buyStatus;
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
        buyStatus.text = "";
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
        buyStatus.enabled = true;
        buyStatus.text = "-" + item.credits;
        Debug.Log("Buying " + item.name);
        StartCoroutine(DelayAction(1f));
    }

    IEnumerator DelayAction(float delayTime)
    {
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);
        //Do the action after the delay time has finished.
        buyStatus.enabled = false;
    }
}
