using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    public int pickable = 0;

    public float cooldown = 1.5f;
    public float nextSkill;

    private Rigidbody2D _rigidbody;
    private Animator _animator;

    private Vector2 _movement = Vector2.zero;

    public float lastX = 0;
    public float lastY = 0;

    private bool bIsAttaking = false;

    private static readonly int InputXHash = Animator.StringToHash("inputX");
    private static readonly int InputYHash = Animator.StringToHash("inputY");
    private static readonly int InputAttackHash = Animator.StringToHash("Attacking");

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {

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
    void move()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        if (!bIsAttaking)
        {
            _movement = new Vector2(inputX, inputY);
        }
        else
        {
            _movement = Vector2.zero;
        }
    }
    void animate()
    {
        if (_movement.sqrMagnitude > 0.01f)
        {
            _animator.SetBool("isMoving", true);
           
            lastX = _movement.x;
            lastY = _movement.y;

            _animator.SetFloat("lastX", lastX);
            _animator.SetFloat("lastY", lastY);
        }
        else
        {
            _animator.SetBool("isMoving", false);
            _rigidbody.velocity = Vector2.zero;
        }
        _animator.SetFloat(InputXHash, _movement.x);
        _animator.SetFloat(InputYHash, _movement.y);
    }
    void attack()
    {

        AnimatorStateInfo animStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        

        if (Input.GetKeyDown(KeyCode.Z) && Time.time > nextSkill)
        {
            bIsAttaking = true;
            nextSkill = Time.time + cooldown;
            //_animator.SetBool("Attacking", bIsAttaking);
            _animator.SetTrigger("Attacking");
        }
        else if(animStateInfo.IsName("Idle Tree") && bIsAttaking)
        {
            bIsAttaking = false;
            // Todo: Spawn fire ball
            //_animator.SetBool("Attacking", bIsAttaking);
        }
    }
}
