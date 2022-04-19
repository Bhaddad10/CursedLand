using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator animator;

    public int health = 100;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void takeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            die();
        }
    }
    public void die()
    {
        Debug.Log("inimigo morreu");
        animator.SetBool("isDead", true);
        Destroy(gameObject, 5f);
    }

    //Código inspirado e adaptado deste video : https://www.youtube.com/watch?v=wkKsl1Mfp5M&t
}