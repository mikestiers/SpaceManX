using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public float lifetime;

    [HideInInspector]
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        if (lifetime <= 0)
            lifetime = 10.0f;

        GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0);
        Destroy(gameObject, lifetime);
        Debug.LogError("Projectile spawned");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CompareTag("EnemyProjectile") && collision.gameObject.CompareTag("Player"))
            GameManager.instance.lives--;

        if (collision.gameObject.CompareTag("Wall"))
                Destroy(gameObject);

        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(1);
            Destroy(gameObject);
        }
    }
}
