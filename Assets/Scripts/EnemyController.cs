using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator _animator;
    private Transform target;
    private SpriteRenderer sprite;
    public GameObject credit;
    
    //variaveis para definir vida e velocidade 
    public int health = 100;
    public float speed = 10.0f;
    
    [Space]
    [Space]

    //variaveis para definir intervalo de ataque e dano
    public float attackRange = 1.5f;
    public float cooldown = 1.5f;
    private float nextAttack;
    public int damage = 30;

    //variaives para salvar a direcao de ataque
    private float attackX = 0;
    private float attackY = 0;

    //variaveis para controle de movimentacao do inimigo
    public float followRange = 5f;
    public float maxRange = 1.5f;

    //variaveis para controle de estado
    private bool isDead = false;
    private bool isAttacking = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        target = FindObjectOfType<PlayerController>().transform;
        sprite = GetComponent<SpriteRenderer>();
    }
    public void Update()
    {
        followPlayer();
    }

    // Metodo para definir quando o inimigo ira seguir o jogador
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

    // Metodo para fazer o inimigo parar de se mover
    public void stop()
    {
        _animator.SetBool("isMoving", false);
    }

    // Metodo que realiza o ataque 
    public void attack()
    {     
        if(Time.time > nextAttack)
        {
            isAttacking = true;

            nextAttack = Time.time + cooldown;

            _animator.SetFloat("lastX", attackX = target.position.x);
            _animator.SetFloat("lastY", attackY = target.position.y);

            _animator.SetBool("isAttacking", true);
        }
        else
        {
            _animator.SetBool("isAttacking", false);
            isAttacking = false;
        }
    }

    // Metodo que causa dano ao jogador 
    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (!player) return;
        if (isDead) return;

        player.takeDamage(damage);
    }

    // Metodo que recebe o dano causado pelo jogador
    public void takeDamage(int damage)
    {
        health -= damage;
        StartCoroutine(blinkSprite());
        if (health <= 0)
        {
            die();
        }   
    }

    // Metodo pra definir que o inimigo esta morto 
    public void die()
    {
        isDead = true;
        _animator.SetBool("isDead", true);
        Destroy(gameObject, 1.5f);
        dropCredit();

    }

    // Metodo que aplica efeito visual ao tomar dano
    IEnumerator blinkSprite()
    {
        for (float i = 0f; i < 1f; i += 0.3f)
        {
            sprite.enabled = false;
            yield return new WaitForSeconds(0.15f);
            sprite.enabled = true;
            yield return new WaitForSeconds(0.15f);
        }        
    }

    // Metodo para saber a distancia entre o jogador e o inimigo
    public void distanceFromPlayer()
    {
        float dist = Vector3.Distance(target.position, transform.position);
        Debug.Log(dist);
    }

    // Metodo para soltar uma moeda quando o inimigo morre
    public void dropCredit()
    {
        // Credit e referenciado dentro do unity para saber qual objeto deve ser spawnado
        Instantiate(credit, transform.position, credit.transform.rotation);
    }

    //Logica de receber dano e morrer inspirado e adaptado deste video : https://www.youtube.com/watch?v=wkKsl1Mfp5M&t
    //Logica de seguir o jogador adaptado deste video https://www.youtube.com/watch?v=dy8hkDmygRI&t
}