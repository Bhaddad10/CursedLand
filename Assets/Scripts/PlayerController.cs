using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool DEBUG = true;

    [Space]
    [Space]
    //Velocidade do personagem
    public int hp = 150;
    public float speed = 10.0f;

    //Variaveis para definir um intervalo de ataque
    public float cooldown = 1.5f;
    public float nextSkill;

    private Rigidbody2D _rigidbody;
    private Animator _animator;

    private Vector2 _movement = Vector2.zero;

    //Variaveis para guardar a ultima posi��o do personagem
    private float lastX = 0;
    private float lastY = 0;
    
    //Variavel para guardar a informa��o de ataque do personagem
    private bool bIsAttacking = false;

    //Variaveis para leitura mais r�pida
    private static readonly int InputXHash = Animator.StringToHash("inputX");
    private static readonly int InputYHash = Animator.StringToHash("inputY");
    private static readonly int InputAttackHash = Animator.StringToHash("Attacking");
    private static readonly int IsMovingHash = Animator.StringToHash("isMoving");
    private static readonly int lastXHash = Animator.StringToHash("lastX");

    private static readonly int lastYHash = Animator.StringToHash("lastY");
    private static readonly string IdleTreeAnimation = "Idle Tree";

    public Transform firePosition;
    public GameObject projectile;

    public LayerMask npcLayerMask;

    //private bool bSceneContainsDialogManager = true;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        //playerState.Initialize();
    }

    private void Start()
    {
        if (DialogManager.Instance != null)
        {
            if (DEBUG)
                Debug.Log("No DialogManager found for this scene. Running without dialogs.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        talkToNpc();
        _movement = Vector2.zero;

        // If scene doesn't contain DialogManager
        //      or, if it does, and it's not on dialog
        if (DialogManager.Instance == null || (DialogManager.Instance != null && !DialogManager.Instance.IsDialogActive()))
        {
            move();
            attack();
        }
        animate();
    }

    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled
    private void FixedUpdate()
    {
        Vector2 _velocity = _movement.normalized * speed;
        _rigidbody.velocity = _velocity;
        
    }
    //M�todo para captar o input e validar o movimento do personagem 
    void move()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
                            //Meio de bloquear a movimenta��o do jogador caso ele tente andar na diagonal
        if (!bIsAttacking && !(inputX == 1 && inputY == 1 || inputX == 1 && inputY == -1 || inputX == -1 && inputY == 1 || inputX == -1 && inputY == -1))
        {
            _movement = new Vector2(inputX, inputY);
        }
        else
        {
            _movement = Vector2.zero;
        }
    }
    //M�todo para ativar a as arovres de anima��o
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
    //M�todo para verificar quando o jogador atacar
    void attack()
    {
        AnimatorStateInfo animStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (Input.GetKeyDown(KeyCode.Z) && Time.time > nextSkill)
        {
            bIsAttacking = true;
            nextSkill = Time.time + cooldown;
            _animator.SetTrigger(InputAttackHash);
        }
        else if(animStateInfo.IsName(IdleTreeAnimation) && bIsAttacking)
        {
            bIsAttacking = false;
            fireBall();
        }
    }
    //M�todo para spawnar uma bola de fogo
    void fireBall()
    {
        Vector2 direction = new Vector2(lastX, lastY).normalized;
        Vector3 distance = new Vector3(lastX, lastY * 1.5f, 0);

        GameObject magicProjectile = Instantiate(projectile, firePosition.position + distance, Quaternion.identity);
        
        magicProjectile.GetComponent<Rigidbody2D>().velocity = direction * magicProjectile.GetComponent<Projectile>().speed;
        magicProjectile.transform.Rotate(0f, 0f, Mathf.Atan2(lastY, lastX) * Mathf.Rad2Deg);

        //L�gica de rota��o do objeto:
        //https://stackoverflow.com/questions/53899781/top-down-shooter-bullet-not-accurate-at-all
    }

    void talkToNpc()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            //Debug.Log("Pressed X.");
            var closeObjects = Physics2D.OverlapCircleAll(_rigidbody.position, 1.5f, npcLayerMask);
            foreach (Collider2D collider in closeObjects)
            {
                if (collider.tag == "NPC")
                {
                    //Debug.Log("Found npc close.");
                    NpcController npcController = collider.gameObject.GetComponent<NpcController>();
                    DialogManager.Instance.StartDialog(npcController);
                    return;
                }
            }
        }
    }
}
