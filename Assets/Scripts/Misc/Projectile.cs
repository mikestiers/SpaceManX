using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    AudioSourceManager asm;
    public AudioClip enemyHit;
    public float lifetime;

    [HideInInspector]
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        asm = GetComponent<AudioSourceManager>();
        if (lifetime <= 0)
            lifetime = 10.0f;

        GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0);
        Destroy(gameObject, lifetime);
        Debug.LogError("Projectile spawned");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CompareTag("EnemyProjectile") && collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.lives--;
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Wall"))
            Destroy(gameObject);

        if (collision.gameObject.CompareTag("Enemy") && !CompareTag("EnemyProjectile"))
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(1);
            asm.PlayOneShot(enemyHit, false);
            Destroy(gameObject);
        }
    }
}

