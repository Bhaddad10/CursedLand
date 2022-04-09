using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Velocidade do personagem
    public float speed = 10.0f;

    //Variaveis para definir um intervalo de ataque
    public float cooldown = 1.5f;
    public float nextSkill;

    private Rigidbody2D _rigidbody;
    private Animator _animator;

    private Vector2 _movement = Vector2.zero;

    //Variaveis para guardar a ultima posição do personagem
    public float lastX = 0;
    public float lastY = 0;
    
    //Variavel para guardar a informação de ataque do personagem
    private bool bIsAttaking = false;

    //Variaveis para leitura mais rápida
    private static readonly int InputXHash = Animator.StringToHash("inputX");
    private static readonly int InputYHash = Animator.StringToHash("inputY");
    private static readonly int InputAttackHash = Animator.StringToHash("Attacking");
    private static readonly int IsMovingHash = Animator.StringToHash("isMoving");
    private static readonly int lastXHash = Animator.StringToHash("lastX");
    private static readonly int lastYHash = Animator.StringToHash("lastY");
    private static readonly string IdleTreeAnimation = "Idle Tree";

    public Transform firePosition;
    public GameObject projectile;
    
    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        move();
        attack();
        animate();
    }

    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled
    private void FixedUpdate()
    {
        Vector2 _velocity = _movement.normalized * speed;
        _rigidbody.velocity = _velocity;
        
    }
    //Método para captar o input e validar o movimento do personagem 
    void move()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
                            //Meio de bloquear a movimentação do jogador caso ele tente andar na diagonal
        if (!bIsAttaking && !(inputX == 1 && inputY == 1 || inputX == 1 && inputY == -1 || inputX == -1 && inputY == 1 || inputX == -1 && inputY == -1))
        {
            _movement = new Vector2(inputX, inputY);
        }
        else
        {
            _movement = Vector2.zero;
        }
    }
    //Método para ativar a as arovres de animação
    void animate()
    {
        if (_movement.sqrMagnitude > 0.01f)
        {
            _animator.SetBool(IsMovingHash, true);
           
            lastX = _movement.x;
            lastY = _movement.y;

            _animator.SetFloat(lastXHash, lastX);
            _animator.SetFloat(lastYHash, lastY);
        }
        else
        {
            _animator.SetBool(IsMovingHash, false);
            _rigidbody.velocity = Vector2.zero;
        }
        _animator.SetFloat(InputXHash, _movement.x);
        _animator.SetFloat(InputYHash, _movement.y);
    }
    //Método para verificar quando o jogador atacar
    void attack()
    {
        AnimatorStateInfo animStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (Input.GetKeyDown(KeyCode.Z) && Time.time > nextSkill)
        {
            bIsAttaking = true;
            nextSkill = Time.time + cooldown;
            _animator.SetTrigger(InputAttackHash);
        }
        else if(animStateInfo.IsName(IdleTreeAnimation) && bIsAttaking)
        {
            bIsAttaking = false;
            fireBall();
        }
    }
    //Método para spawnar uma bola de fogo
    void fireBall()
    {
        Vector2 direction = new Vector2(lastX, lastY).normalized;
        Vector3 distance = new Vector3(lastX, lastY * 1.5f, 0);
 
        GameObject magicProjectile = Instantiate(projectile, firePosition.position + distance, firePosition.rotation);
        
        magicProjectile.GetComponent<Rigidbody2D>().velocity = direction * magicProjectile.GetComponent<Projectile>().speed;

        //_rigidbody.velocity = transform.right * speed;
    }
}
