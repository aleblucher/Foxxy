using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{ 
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;

    // Máquina de estados finitos
    private enum State {idle, running, jumping, falling, hurt};
    private State state = State.idle;

    // variaveis inspecionávies
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 7.5f;
    [SerializeField] private float jumpforce = 12f;
    [SerializeField] private int cherries = 0;
    [SerializeField] private Text cherryNumber;
    [SerializeField] private float pain = 10f;
    [SerializeField] private int health;
    [SerializeField] private Text healthAmount;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        healthAmount.text = health.ToString();
    }

    private void Update()
    {
        if(state != State.hurt)
        {
            Movement();
        }


        AnimationState();

        anim.SetInteger("state", (int)state);
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "collectable")
        {
            Destroy(collision.gameObject);
            cherries++;

            cherryNumber.text = cherries.ToString();

        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "frog")
        {
            Frog frog = other.gameObject.GetComponent<Frog>();
            if(state == State.falling)
            {
                frog.JumpedOn();
                
                Jump();
            }
            else
            {
                state = State.hurt;
                health = health - 1;
                healthAmount.text = health.ToString();


                if(health <= 0)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                
                if(other.gameObject.transform.position.x > transform.position.x)
                {
                    // inimigo a direita = dano + movimento para a esquerda
                    rb.velocity = new Vector2(-pain, rb.velocity.y);

                }
                else
                {
                    // inimigo a esquerda = dano + movimento para a direita
                    rb.velocity = new Vector2(pain, rb.velocity.y);
                }
            }
            
        }


        else if (other.gameObject.tag == "gump")
        {
            
            Gump gump = other.gameObject.GetComponent<Gump>();
            if (state == State.falling)
            {
                gump.JumpedOn();
                Jump();
            }
            else
            {
                state = State.hurt;
                health = health - 1;
                healthAmount.text = health.ToString();

                if (health <= 0)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }

                
                if (other.gameObject.transform.position.x > transform.position.x)
                {
                    // inimigo a direita = dano + movimento para a esquerda
                    rb.velocity = new Vector2(-pain, rb.velocity.y);

                }
                else
                {
                    // inimigo a esquerda = dano + movimento para a direita
                    rb.velocity = new Vector2(pain, rb.velocity.y);
                }
            }

        }
    }

    private void Movement()
    {
        float hDirection = Input.GetAxis("Horizontal");

        if (hDirection< 0) // esquerda
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);

            transform.localScale = new Vector2(-1, 1);

        }

        else if (hDirection > 0) // direita
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);

            transform.localScale = new Vector2(1, 1);

        }

        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground)) // pulo
        {
            Jump();
  
        }
    }

    private void Jump()
    {
        state = State.jumping;
        rb.velocity = new Vector2(rb.velocity.x, jumpforce);
    }

    private void AnimationState()
    {
        if (state == State.jumping)
        {
            if (rb.velocity.y < .1f)
            {
                state = State.falling;
            }

        }

        else if (state == State.falling)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }

        else if (Mathf.Abs(rb.velocity.x) > 2f && state != State.hurt)
        {
            // moving
            state = State.running;

        }

        else if(state == State.hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < .5f)
            {
                state = State.idle;
            }
        }

        else{
            state = State.idle;
        }
    }

}
