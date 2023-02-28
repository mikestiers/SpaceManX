using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBallDeVoux : Enemy
{
    Rigidbody2D rb;
    public float speed;

    // Start is called before the first frame update
    public override void Start()  // default void Start() removed since it is an override from Enemy class
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();

        if (speed <= 0)
            speed = 1.5f;
    }


    // Update is called once per frame
    void Update()
    {
        AnimatorClipInfo[] curClips = anim.GetCurrentAnimatorClipInfo(0);

        if (curClips[0].clip.name == "Walk")
        {
            if (!sr.flipX) // heading left
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
            }
            else // heading right
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }
        }
    }

    // Collision with barrier and flip
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Barrier"))
        {
            sr.flipX = !sr.flipX;
        }
    }
    
    public void Squish()
    {
        rb.velocity = new Vector2(0, 0);
        anim.SetTrigger("Squish");
        Invoke("DestroyMyselfLocalGroup", anim.GetCurrentAnimatorStateInfo(0).length); // wait for squish animation to finish, then destroy
    }
    
    public void DestroyMyselfLocalGroup()
    {
        // parent's parent (barriers, etc... aka "localgroup" game object) .. up two levels
        //anim.SetTrigger("Death");
        Destroy(gameObject.transform.parent.gameObject.transform.parent.gameObject);
    }
}
