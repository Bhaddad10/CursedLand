using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerState
{
    private const string SPEED_POTION_NAME = "SpeedPotion";

    [HideInInspector]
    public float currentHealth;
    public float maxHealth = 150;
    public float speed = 10.0f;
    public int credits = 50;
    [HideInInspector]
    public bool isDead = false;

    // Última posição do player no mapa
    internal Vector3 lastPosition = Vector3.zero;
    internal Vector3 lastStashPosition = Vector3.zero;
    internal bool hasLastPosition = false;

    // Controle para evitar sobreposição de poção de velocidade
    internal bool isUsingSpeedPotion = false;
    internal float lastSpeed;
    
    // Inventory
    public Dictionary<string, Potion> items = new Dictionary<string, Potion>();

    [HideInInspector]
    public Image healthBar;
    


    // Executa compra de item se houver créditos
    // Returns:
    //     true - Sucesso
    //     false - Créditos insuficientes
    internal bool tryBuyItem(ShopItem item)
    {
        if (credits < item.price) return false;

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

    // Realiza "stash" (salvamento temporário da posição atual no mapa)
    internal void stashLastPosition(Vector3 position)
    {
        lastStashPosition = position;
    }

    // Salvamento permanente da última posição no mapa
    internal void saveLastPosition()
    {
        hasLastPosition = true;
        lastPosition = lastStashPosition;
    }

    public void Initialize()
    {
        currentHealth = maxHealth;
    }

    public void OnDie()
    {
        isDead = true;
        GameManager.Instance.playerController.die();
    }

    // Recebe dano
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

    // Atualiza UI
    public void UpdateHealth()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
    }

    // Restaura HP
    internal void restoreHp(int healthToRestore)
    {
        currentHealth = Math.Min(currentHealth + healthToRestore, maxHealth);
        UpdateHealth();
    }
}
