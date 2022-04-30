using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private Transform target;

    public int health = 100;
    public float speed = 9;
    private float range = 5;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        target = FindObjectOfType<PlayerController>().transform;
    }
    public void Update()
    {
        if(Vector3.Distance(target.position, transform.position) <= range)
        {
            followPlayer();
        }
    }
    public void followPlayer()
    {
        _animator.SetBool("isMoving", true);
        _animator.SetFloat("moveX", target.position.x - transform.position.x);
        _animator.SetFloat("moveY", target.position.y - transform.position.y);
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
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
}