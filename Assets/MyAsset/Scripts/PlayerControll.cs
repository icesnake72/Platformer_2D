using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControll : MonoBehaviour
{
    [SerializeField]
    private int moveMethod = 3;

    [SerializeField]
    private int jumpMethod = 2;

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float max_speed = 7f;

    [SerializeField]
    private float jump_power = 10f;

    [SerializeField]
    private float grivity_weight = 1f;

    [SerializeField]
    private LayerMask jumpableGround;

    private Animator anim;
    private Rigidbody2D rigid;
    private SpriteRenderer sr;
    private BoxCollider2D coll;
        
    private enum PlayerState { idle, running, jumping, falling };
    private PlayerState ps;

    private void Awake()
    {        
        ps = PlayerState.idle;

        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
    }

    private bool isGoingDown()
    {
        if (ps == PlayerState.idle || ps == PlayerState.running)
            return false;

        return rigid.velocity.y < 0;
    }

    private bool Jumped
    {
        get
        {
            return (ps == PlayerState.jumping) || (ps == PlayerState.falling);
        }
    }

    private bool isOverSpeed()
    {
        return Mathf.Abs(rigid.velocity.x) > max_speed;
    }

    private bool isStop
    {
        get
        {            
            return Mathf.Abs(rigid.velocity.x) == 0f;
        }        
    }

    private bool isStop2
    {
        get
        {
            return Mathf.Abs(rigid.velocity.x) < 0.1f;
        }
    }

    private bool isGround()
    {        
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 1f, jumpableGround);
    }



    // Update is called once per frame
    private void Update()
    {
        // rigidbody를 이용한 움직임은 FixedUpdate()에서 처리해주는게 좋다
        switch (moveMethod)
        {
            case 1:
                MoveMethod1();
                break;

            case 2:
                MoveMethod2();
                break;

            case 3:
                MoveMethod3();
                break;
        }
        
        switch (jumpMethod)
        {
            case 1:
                JumpMethod1();
                break;

            case 2:
                JumpMethod2();
                break;

            case 3:
                JumpMethod3();
                break;
        }

        updateAnimationState();
    }

    private void MoveMethod1()
    {
        float horiz = Input.GetAxis("Horizontal");
        Vector3 vec = new Vector3(horiz, 0f, 0f);
        transform.position += vec * speed * Time.deltaTime;
    }

    private void MoveMethod2()
    {
        // horiz 값은 왼쪽 방향키가 눌리면 음수, 반대인 경우 양수가 된다 
        float horiz = Input.GetAxis("Horizontal");
        Vector2 vec = new Vector2(horiz, 0);

        // 현재 속도와 목표 속도 사이의 차이를 구합니다.
        //Vector2 velocityChange = targetVelocity - rigid.velocity;

        // 
        rigid.AddForce(vec, ForceMode2D.Impulse);
        sr.flipX = Mathf.Sign(rigid.velocity.x) > 0 ? false : true;

        if (isOverSpeed())
        {
            // 최대 속도 제한 
            rigid.velocity = new Vector2(Mathf.Sign(rigid.velocity.x) * max_speed, rigid.velocity.y);            
        }
    }

    private void MoveMethod3()
    {
        float horiz = Input.GetAxis("Horizontal");
        rigid.velocity = new Vector2(horiz * speed, rigid.velocity.y);
        sr.flipX = Mathf.Sign(horiz) > 0 ? false : true;

        if (isOverSpeed())
        {
            // 최대 속도 제한 
            rigid.velocity = new Vector2(Mathf.Sign(rigid.velocity.x)*max_speed, rigid.velocity.y);            
        }       
    }

    private void JumpMethod1()
    {
        if (Input.GetButtonDown("Jump") && isGround())
        {
            Vector3 vec = new Vector3(0f, jump_power, 0f);
            transform.position += vec * speed * Time.deltaTime;            
        }        
    }

    private void JumpMethod2()
    {
        if (Input.GetButtonDown("Jump") && isGround())
        {            
            rigid.velocity = new Vector2(0, jump_power);
        }
    }

    private void JumpMethod3()
    {
        if (Input.GetButtonDown("Jump") && isGround())
        {
            Vector2 vec = new Vector2(rigid.velocity.x, jump_power);
            rigid.AddForce(vec, ForceMode2D.Impulse);
        }
    }

    private void updateAnimationState()
    {
        if (isGround())
            ps = isStop ? PlayerState.idle : PlayerState.running;
        else
            ps = isGoingDown() ? PlayerState.falling : PlayerState.jumping;

        anim.SetInteger("state", (int)ps);
    }
}
