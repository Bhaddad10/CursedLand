using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator _animator;
    private Transform target;

    public int health = 100;
    public float speed = 10.0f;

    private float attackX = 0;
    private float attackY = 0;

    public float followRange = 5f;
    public float maxRange = 1.5f;

    public float attackRange = 1.5f;
    public float cooldown = 1.5f;
    private float nextAttack;
    public int damage = 30;

    private bool isDead = false;
    private bool isAttacking = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        target = FindObjectOfType<PlayerController>().transform;
    }
    public void Update()
    {
        followPlayer();
    }
    public void followPlayer()
    {
        if (Vector3.Distance(target.position, transform.position) <= followRange &&
            Vector3.Distance(target.position, transform.position) >= maxRange && 
            !isDead && !isAttacking)
        {
            _animator.SetBool("isMoving", true);

            _animator.SetFloat("moveX", target.position.x - transform.position.x);
            _animator.SetFloat("moveY", target.position.y - transform.position.y);

            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

            if (Vector3.Distance(target.position, transform.position) <= attackRange)
            {
                Debug.Log("Tentando Atacar");
                attack();
            }
            else
            {
                isAttacking = false;
            }
        }
        else
        {
            isAttacking = false;
            _animator.SetBool("isMoving", false);
        }
    }
    public void stop()
    {
        _animator.SetBool("isMoving", false);
    }
    public void attack()
    {     
        if(Time.time > nextAttack)
        {
            isAttacking = true;

            nextAttack = Time.time + cooldown;

            _animator.SetFloat("lastX", attackX = target.position.x);
            _animator.SetFloat("lastY", attackY = target.position.y);

            _animator.SetTrigger("isAttacking");
        }
        else
        {
            isAttacking = false;
        }
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
        isDead = true;
        _animator.SetBool("isDead", true);
        Destroy(gameObject, 5f);
    }
    public void distanceFromPlayer()
    {
        float dist = Vector3.Distance(target.position, transform.position);
        Debug.Log(dist);
    }
    //Logica de receber dano e morrer inspirado e adaptado deste video : https://www.youtube.com/watch?v=wkKsl1Mfp5M&t
    //Logica de seguir o jogador adaptado deste video https://www.youtube.com/watch?v=dy8hkDmygRI&t
}