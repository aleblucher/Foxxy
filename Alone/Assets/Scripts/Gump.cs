using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gump : MonoBehaviour
{
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;

    private Animator anim;
    private Collider2D coll;
    private Rigidbody2D rb;
      
    private bool facingLeft = true;
    private float speed = 6f;

    private void Start()
    {
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (facingLeft)
        {
            
            // check left cap
            if(transform.position.x > leftCap)
            {
            
                // make sure is facing left
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }
                rb.velocity = new Vector2(-speed, rb.velocity.y);
            }
            else
            {
                facingLeft = false;
            
            }
        }

        else
        {
            // check right cap
            if (transform.position.x < rightCap)
            {
                // make sure is facing right
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }
            else
            {
                facingLeft = true;

            }
        }
    }

    public void JumpedOn()
    {
        anim.SetTrigger("death");
        
    }

    private void Death_Gump()
    {
        Destroy(this.gameObject);
    }
}
