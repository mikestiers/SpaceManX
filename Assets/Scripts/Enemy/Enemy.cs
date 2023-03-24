using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer),typeof(Animator))]
public class Enemy : MonoBehaviour
{
    protected SpriteRenderer sr;
    protected Animator anim;
    AudioSourceManager asm;

    protected int _health;
    public int maxHealth;
    public AudioClip enemyDeath;

    // Health interface
    public int health
    {
        get => _health;
        set
        {
            _health = value;

            if (_health > maxHealth)
                _health = maxHealth;

            if (_health <= 0)
                Death();
        }
    }

    public virtual void Death() // virtual lets child classes override the function
    {
        asm.PlayOneShot(enemyDeath, false);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        anim.SetTrigger("Death");
        Invoke("DestroyMyself", anim.GetCurrentAnimatorStateInfo(0).length);
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        asm = GetComponent<AudioSourceManager>();

        if (maxHealth <= 0)
            maxHealth = 5;

        health = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage; // health - damage
        asm.PlayOneShot(enemyDeath, false);
        Debug.Log(health);
    }

    public void DestroyMyself()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        Destroy(gameObject);
    }

}
