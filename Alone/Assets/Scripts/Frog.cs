using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    private Collider2D coll;
    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private float jumpHeight = 7.5f;
    [SerializeField] private LayerMask ground;


    private void Start()
    {
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(0, jumpHeight);
            anim.SetBool("jumping", true);
        }

        if (anim.GetBool("jumping") == true) {
            if (rb.velocity.y < .1f)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
            if(coll.IsTouchingLayers(ground) && anim.GetBool("falling"))
            {
                anim.SetBool("falling", false);
                anim.SetBool("jumping", true);
            }
        }
    }

    public void JumpedOn()
    {
        anim.SetTrigger("death");

    }
    private void Death_Frog()
    {
        Destroy(this.gameObject);
    }
}
