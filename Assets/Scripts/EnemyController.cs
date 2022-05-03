using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private Transform target;

    private Vector2 _movement = Vector2.zero;

    public float attackX = 0;
    public float attackY = 0;

    public int health = 100;
    public float speed = 9;


    public float followRange = 5f;
    public float attackRange = 1.5f;
    public float cooldown = 2f;
    private float nextAttack;

    private bool bIsAttacking = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        target = FindObjectOfType<PlayerController>().transform;
    }
    public void Update()
    {
        if(Vector3.Distance(target.position, transform.position) <= followRange)
        {
            followPlayer();
            
            if (Vector3.Distance(target.position, transform.position) <= attackRange)
            {
                stop();
                attack();
            }
        }
        else
        {
            stop();
        }
    }
    public void followPlayer()
    {
        _animator.SetBool("isMoving", true);
        _animator.SetFloat("moveX", target.position.x - transform.position.x);
        _animator.SetFloat("moveY", target.position.y - transform.position.y);

        _animator.SetFloat("lastX", attackX = target.position.x);
        _animator.SetFloat("lastY", attackY = target.position.y);

        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        //float dist = Vector3.Distance(target.position, transform.position);
        //Debug.Log(dist);
    }
    public void stop()
    {
        _animator.SetBool("isMoving", false);
    }
    public void attack()
    {     
        if(Time.time > nextAttack)
        {
            bIsAttacking = true;
            nextAttack = Time.time + cooldown;
            _animator.SetTrigger("attack");
        }
        else if (bIsAttacking)
        {
            bIsAttacking = false;
        }
        //float dist = Vector3.Distance(target.position, transform.position);
        //Debug.Log(dist);
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
        _animator.SetBool("isDead", true);
        Destroy(gameObject, 5f);
    }
    //Logica de receber dano e morrer inspirado e adaptado deste video : https://www.youtube.com/watch?v=wkKsl1Mfp5M&t
    //Logica de seguir o jogador adaptado deste video https://www.youtube.com/watch?v=dy8hkDmygRI&t
}