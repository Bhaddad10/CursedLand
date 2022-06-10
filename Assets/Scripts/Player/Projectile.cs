using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 18f;
    public int damage = 50;
    public GameObject inpactEffectPrefab;

    private Rigidbody2D _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Aplica dano
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject effect = Instantiate(inpactEffectPrefab, transform.position, Quaternion.identity);
        Destroy(effect, 0.5f);
        Destroy(gameObject);
        EnemyController enemy = collision.GetComponent<EnemyController>();
       
        if (enemy != null)
        {
            enemy.takeDamage(damage);
        }
    }

    // Destr�i se sair do campo de vis�o
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
//c�digo inspirado neste video: https://www.youtube.com/watch?v=uKWbNWPAZq4&t
