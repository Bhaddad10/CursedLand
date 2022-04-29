using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerState
{

    public int credits = 50;
    public Dictionary<string, Potion> items = new Dictionary<string, Potion>();

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
