using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ShopManager : MonoBehaviour
{

    public Text currentStatusText;

    public GameObject buyStatusPanel;
    public Text buyStatusText;
    public ShopItem[] items;
    public PlayerController player;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        foreach (ShopItem item in items)
        {
            item.button.onClick.AddListener(() => tryBuyItem(item));
        }
        buyStatusPanel.SetActive(false);
        buyStatusText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        currentStatusText.text = player.playerState.credits.ToString();
    }

    void tryBuyItem(ShopItem item)
    {
        if (player.playerState.credits < item.price)
        {
            Debug.Log("Out of credits..");
            return;
        }

        buyStatusPanel.SetActive(true);
        buyStatusText.text = "-" + item.price;
        player.buyItem(item);
        //Debug.Log("Buying " + item.name);
        StartCoroutine(DelayAction(1f));
    }

    IEnumerator DelayAction(float delayTime)
    {
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);
        //Do the action after the delay time has finished.
        buyStatusPanel.SetActive(false);
    }

    public void exitShop()
    {
        // Debug.Log("Exitting shop.");
        GameManager.Instance.ChangeToPreviousScene();
    }
}
