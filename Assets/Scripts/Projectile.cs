using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 8f;
    public GameObject inpactEffectPrefab;

    private Rigidbody2D _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        //_rigidbody.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject effect = Instantiate(inpactEffectPrefab, transform.position, Quaternion.identity);
        Destroy(effect, 0.5f);
        //Todo: causa dano ao inimigo
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }


}
//código inspirado neste video: https://www.youtube.com/watch?v=uKWbNWPAZq4&t
