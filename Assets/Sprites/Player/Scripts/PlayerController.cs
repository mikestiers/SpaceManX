using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    // Components
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;

    // Movement variable
    public float speed;
    public float jumpForce;

    // Ground check
    public bool isGrounded;
    public Transform groundCheck;
    public LayerMask isGroundLayer;
    public float groundCheckRadius;

    // Coroutines
    Coroutine jumpForceChange = null;
    Coroutine speedChange = null;

    // Player Modifiers
    public int health = 50;
    public int maxLives = 10;
    private int _lives = 5;

    public int lives
    {
        get { return _lives; }
        set
        {
            // if (_lives > value)
            //we lost a life - we need to respawn
            _lives = value;

            if (_lives > maxLives)
                _lives = maxLives;

            //if (_lives < 0)
            //game over

            Debug.Log("Lives have been set to " + _lives.ToString());
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        if (speed <= 0)
        {
            speed = 1.5f;
            Debug.Log("speed was set incorrectly.  Defaulting to " + speed.ToString());
        }

        if (jumpForce <= 0)
        {
            jumpForce = 150;
            Debug.Log("jumpForce was set incorrectly.  Defaulting to " + jumpForce.ToString());
        }

        if (groundCheckRadius <= 0)
        {
            groundCheckRadius = 0.2f;
            Debug.Log("groundCheckRadius was set incorrectly.  Defaulting to " + groundCheckRadius.ToString());
        }

        if (!groundCheck)
        {
            groundCheck = GameObject.FindGameObjectWithTag("GroundCheck").transform;
            Debug.Log("groundCheck not set.  Finding it manually");
        }
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorClipInfo[] curPlayingclip = anim.GetCurrentAnimatorClipInfo(0);
        
        float hInput = Input.GetAxisRaw("Horizontal");

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);

        if (curPlayingclip.Length > 0)  // animator has something playing
        {
            if (Input.GetButtonDown("Fire1") && isGrounded && curPlayingclip[0].clip.name != "StandShoot")
            {
                anim.SetTrigger("standShoot");
            }
        }

        if (curPlayingclip.Length > 0)  // animator has something playing
        {
            if (Input.GetButtonDown("Fire1") && hInput > 0 && curPlayingclip[0].clip.name != "RunShoot")
            {
                anim.SetTrigger("runShoot");
            }
        }

        if (curPlayingclip.Length > 0)  // animator has something playing
        {
            if (Input.GetButtonDown("Fire2") && curPlayingclip[0].clip.name != "Slide")
            {
                anim.SetTrigger("slide");
            }
        }

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce);
        }

        //if (curPlayingclip.Length > 0)  // animator has something playing
        //{
        //    if (!isGrounded && Input.GetButtonDown("Fire1") && curPlayingclip[0].clip.name != "jumpShoot")
        //    {
        //        anim.SetTrigger("jumpShoot");
        //    }
        //}

        if (!isGrounded && Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("jumpShoot");
        }

        Vector2 moveDirection = new Vector2(hInput * speed, rb.velocity.y);
        rb.velocity = moveDirection;

        anim.SetFloat("hInput", Mathf.Abs(hInput));
        anim.SetBool("isGrounded", isGrounded);

        // Check for flipped X and create algorithm to flip character
        if (hInput > 0 && sr.flipX || hInput < 0 && !sr.flipX)
                sr.flipX = !sr.flipX;

        // Alternative way.  I find it a bit rough to understand on first glance as a noob, so i'm
        // keeping the other way for now, even though it is pretty cool and clean
        //if (hInput != 0)
        //    sr.flipX = (hInput < 0);
    }

    public void StartJumpForceChange()
    {
        if (jumpForceChange == null)
        {
            jumpForceChange = StartCoroutine(JumpForceChange());
        }
        else // Start the timer over
        {
            StopCoroutine(jumpForceChange);
            jumpForceChange = null;
            jumpForce /= 2;
            jumpForceChange = StartCoroutine(JumpForceChange());
        }
    }

    IEnumerator JumpForceChange()
    {
        jumpForce *= 2;
        yield return new WaitForSeconds(5.0f);
        jumpForce /= 2;
        jumpForceChange = null;
    }

    public void StartSpeedChange()
    {
        if (speedChange == null)
        {
            speedChange = StartCoroutine(SpeedChange());
        }
        else // Start the timer over
        {
            StopCoroutine(speedChange);
            speedChange = null;
            speed /= 2;
            speedChange = StartCoroutine(SpeedChange());
        }
    }

    IEnumerator SpeedChange()
    {
        speed *= 2;
        sr.color = new Color(1f, 0.5f, 0.2f);
        yield return new WaitForSeconds(5.0f);
        speed /= 2;
        speedChange = null;
    }

}
