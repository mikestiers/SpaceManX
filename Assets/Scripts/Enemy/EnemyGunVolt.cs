using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Shoot))]
public class EnemyGunVolt : Enemy
{
    public float projectileFireRate;
    float timeSinceLastFire;
    Shoot shootScript;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        shootScript = GetComponent<Shoot>();
        shootScript.OnProjectileSpawned.AddListener(UpdateTimeSinceLastFire);
    }

    //public override void Death()
    //{
    //    Destroy(gameObject);
    //}

    private void OnDisable()
    {
        shootScript.OnProjectileSpawned.RemoveListener(UpdateTimeSinceLastFire); // remove listeners best practice
    }

    void UpdateTimeSinceLastFire()
    {
        timeSinceLastFire = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorClipInfo[] curClips = anim.GetCurrentAnimatorClipInfo(0);

        if (curClips[0].clip.name != "Shoot")
        {
            float distance = gameObject.transform.position.x - (GameObject.FindWithTag("Player")).transform.position.x;
            Debug.Log(distance);
            if ((distance < 1 && distance > 0) || (distance > -1 && distance < 0))
            {
                if (distance < 0)
                    sr.flipX = true;
                else if (distance > 0)
                    sr.flipX = false;
                if (Time.time >= timeSinceLastFire + projectileFireRate)
                {
                    anim.SetTrigger("Shoot");
                    Debug.Log("GunVolt X: " + gameObject.transform.position.x + "Player X: " + (GameObject.FindWithTag("Player")).transform.position.x);
                }
            }
        }
    }
}
