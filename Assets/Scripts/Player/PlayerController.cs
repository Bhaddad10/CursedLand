using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool DEBUG = true   ;

    private Rigidbody2D _rigidbody;
    private Animator _animator;

    private Vector2 _movement = Vector2.zero;

    // Intervalo de ataque
    public float cooldown = 1.5f;
    private float nextSkill;

    // Última posição do player
    private float lastX = 0;
    private float lastY = 0;
    
    // Estado de ataque do personagem
    private bool bIsAttacking = false;

    // Hashes para animação
    private static readonly int InputXHash = Animator.StringToHash("inputX");
    private static readonly int InputYHash = Animator.StringToHash("inputY");
    private static readonly int InputAttackHash = Animator.StringToHash("Attacking");
    private static readonly int IsMovingHash = Animator.StringToHash("isMoving");
    private static readonly int lastXHash = Animator.StringToHash("lastX");
    private static readonly int lastYHash = Animator.StringToHash("lastY");
    private static readonly string IdleTreeAnimation = "Idle Tree";

    // Projétil
    public Transform firePosition;
    public GameObject projectile;

    // Identificação de NPC (dialogo)
    public LayerMask npcLayerMask;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (DialogManager.Instance == null && DEBUG)
            Debug.Log("No DialogManager found for this scene. Running without dialogs.");
    }


    // Update is called once per frame
    void Update()
    {
        talkToNpc();
        _movement = Vector2.zero;

        // If scene doesn't contain DialogManager
        //      or, if it does, and it's not on dialog
        bool notOnDialog = DialogManager.Instance == null ||
            (DialogManager.Instance != null && !DialogManager.Instance.IsDialogActive());
        if (notOnDialog)
        {
            move();
            attack();
            takePotion();
        }
        animate();
    }

    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled
    private void FixedUpdate()
    {
        Vector2 _velocity = _movement.normalized * GameManager.Instance.playerState.speed;
        _rigidbody.velocity = _velocity;
    }

    // Processa movimento do player 
    void move()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
                            //Meio de bloquear a movimentação do jogador caso ele tente andar na diagonal
        if (!GameManager.Instance.playerState.isDead 
            && !bIsAttacking && !(inputX == 1 && inputY == 1 || inputX == 1 && inputY == -1 || inputX == -1 && inputY == 1 || inputX == -1 && inputY == -1))
        {
            _movement = new Vector2(inputX, inputY);
        }
        else
        {
            _movement = Vector2.zero;
        }
    }

    // Ativar árvores de animação
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

    // Verificar quando o ataque
    void attack()
    {
        AnimatorStateInfo animStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (Input.GetKeyDown(KeyCode.Z) && Time.time > nextSkill && !GameManager.Instance.playerState.isDead)
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

    // Método para spawnar uma bola de fogo
    void fireBall()
    {
        Vector2 direction = new Vector2(lastX, lastY).normalized;
        Vector3 distance = new Vector3(lastX, lastY * 1.5f, 0);

        GameObject magicProjectile = Instantiate(projectile, firePosition.position + distance, Quaternion.identity);
        
        magicProjectile.GetComponent<Rigidbody2D>().velocity = direction * magicProjectile.GetComponent<Projectile>().speed;
        magicProjectile.transform.Rotate(0f, 0f, Mathf.Atan2(lastY, lastX) * Mathf.Rad2Deg);

        //Lógica de rotação do objeto:
        //https://stackoverflow.com/questions/53899781/top-down-shooter-bullet-not-accurate-at-all
    }    

    public void takeDamage(int damage)
    {
        GameManager.Instance.playerState.TakeHit(damage);
    }

    public void die()
    {
        _animator.SetBool("isDead", true);
    }

    // Diálogo com NPC próximo
    void talkToNpc()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            var closeObjects = Physics2D.OverlapCircleAll(_rigidbody.position, 1.5f, npcLayerMask);
            foreach (Collider2D collider in closeObjects)
            {
                if (collider.tag == "NPC")
                {
                    NpcController npcController = collider.gameObject.GetComponent<NpcController>();
                    DialogManager.Instance.StartDialog(npcController);
                    return;
                }
            }
        }
    }

    // Consome poção
    void takePotion()
    {
        int pressedKey = ConsumePotionKeyPressed();
        if (pressedKey != -1)
        {
            
            if (GameManager.Instance.playerState.items.Count == 0)
            {
                
                return;
            }
            if (pressedKey >= GameManager.Instance.playerState.items.Count)
            {
                
                return;
            }

            KeyValuePair<string, Potion> keyValuePair = GameManager.Instance.playerState.items.ElementAt(pressedKey);
            string potionName = keyValuePair.Key;
            Potion potion = keyValuePair.Value;

            if (potion.quantity <= 1)
            {
                if (!potion.Consume())
                    return;
                GameManager.Instance.playerState.items.Remove(potionName);
                GameManager.Instance.potionUiManager.UpdateInventory();
                return;
            }

            if (!potion.Consume())
                return;
            potion.quantity -= 1;
            GameManager.Instance.potionUiManager.UpdateInventory();
        }
    }

    // Coleta créditos
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pickable"))
        {
            Destroy(other.gameObject);
            GameManager.Instance.playerState.addCredits();
        }
    }

    // Retorna índice para tecla digitada ou -1 se fora de range
    int ConsumePotionKeyPressed()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            return 0;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            return 1;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            return 2;

        return -1;

    }

    // Realiza "stash" (salvamento temporário da posição atual no mapa)
    internal void stashLastPosition()
    {
        GameManager.Instance.playerState.stashLastPosition(transform.position);
    }

    // Restaura última posição no mapa
    internal void goToLastPosition()
    {
        if (GameManager.Instance.playerState.hasLastPosition)
            transform.position = GameManager.Instance.playerState.lastPosition;
    }

    // Salvamento permanente da última posição no mapa
    internal void saveLastPosition()
    {
        GameManager.Instance.playerState.saveLastPosition();
    }
}
