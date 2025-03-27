using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpSpeed;

    [Header("Player components")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask obstacleLayer;
    private float wallJumpCoolDown;
    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D BoxCol;
    
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        BoxCol = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        move();
        jump();
    }

    void move()
    {
        //move player
        if(wallJumpCoolDown < 0.2f)
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed , rb.velocity.y);

        //animate running
        anim.SetBool("Run", Input.GetAxis("Horizontal") != 0);

        //flip player based on the movement direction
        if(Input.GetAxis("Horizontal") > 0.01f)
        {
            transform.localScale = new Vector3(2,2,2);
        }
        else if (Input.GetAxis("Horizontal") < -0.01f)
        {
            transform.localScale = new Vector3(-2,2,2);
        }


    }

    void jump()
    {
        if (Input.GetKey(KeyCode.Space) && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            anim.SetTrigger("jump");
            
            
            
        }
        anim.SetBool("Grounded", isGrounded());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }


    bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(BoxCol.bounds.center, BoxCol.bounds.size, 0, Vector2.down, 0.01f, groundLayer);
        return raycastHit.collider != null;
    }

    bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(BoxCol.bounds.center, BoxCol.bounds.size, 0, new Vector2(transform.localScale.x,0), 0.01f, obstacleLayer);
        return raycastHit.collider != null;
    }
}
