using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; // adding for turrent shoot detection

//[RequireComponent(typeof(PlayerController))] // removed because not needed and used on not just player object
public class Shoot : MonoBehaviour
{
    SpriteRenderer sr;
    AudioSourceManager asm;

    public UnityEvent OnProjectileSpawned; // for turrent guy
    public float projectileSpeed;
    public Transform spawnPointRight;
    public Transform spawnPointLeft;

    public Projectile projectilePrefab;

    public AudioClip fireSound;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        asm = GetComponent<AudioSourceManager>();

        if (projectileSpeed <= 0)
            projectileSpeed = 2.0f;

        if (!spawnPointLeft || !spawnPointRight || !projectilePrefab)
            Debug.Log("Setup default values on " + gameObject.name);

    }

    public void Fire()
    {
        if (gameObject.tag == "Player")
        {
            if (!sr.flipX)
            {
                Projectile curProjectile = Instantiate(projectilePrefab, spawnPointRight.position, spawnPointRight.rotation);
                curProjectile.speed = projectileSpeed;
            }
            else
            {
                Projectile curProjectile = Instantiate(projectilePrefab, spawnPointLeft.position, spawnPointLeft.rotation);
                curProjectile.speed = -(projectileSpeed);
            }
        }

        if (gameObject.tag == "Enemy")
        {
            if (sr.flipX)
            {
                Projectile curProjectile = Instantiate(projectilePrefab, spawnPointRight.position, spawnPointRight.rotation);
                curProjectile.speed = projectileSpeed;
            }
            else
            {
                Projectile curProjectile = Instantiate(projectilePrefab, spawnPointLeft.position, spawnPointLeft.rotation);
                curProjectile.speed = -(projectileSpeed);
            }
        }

        if (asm)
            asm.PlayOneShot(fireSound, false);

        OnProjectileSpawned?.Invoke(); // so we know when fire function has been called - check enemygunvolt.cs
        // same as if (OnProjectileSpanwed != null) { OnProjectileSpawned.Ivnoke(); }
    }

}
