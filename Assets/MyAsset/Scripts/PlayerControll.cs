using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControll : MonoBehaviour
{
    [SerializeField]
    private int moveMethod = 3;

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float max_speed = 7f;

    [SerializeField]
    private float jump_power = 10f;

    [SerializeField]
    private float grivity_weight = 1f;

    private Animator anim;
    private Rigidbody2D rigid;
    private SpriteRenderer sr;
        
    private enum PlayerState { idle, running, jumping, falling };
    private PlayerState ps;

    private void Awake()
    {        
        ps = PlayerState.idle;

        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
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



    // Update is called once per frame
    void Update()
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
        

        if (isGoingDown())
        {
            Debug.Log("going down!!!");
            ps = PlayerState.falling;
            anim.SetInteger("state", (int)ps);

            rigid.velocity += grivity_weight * Physics2D.gravity * Time.deltaTime;
        }
        else
        {
            JumpMethod2();
        }
    }

    void MoveMethod1()
    {
        float horiz = Input.GetAxis("Horizontal");
        Vector3 vec = new Vector3(horiz, 0f, 0f);
        transform.position += vec * speed * Time.deltaTime;
    }

    void MoveMethod2()
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

        if (!Jumped)
            ps = isStop2 ? PlayerState.idle : PlayerState.running;

        anim.SetInteger("state", (int)ps);
    }

    void MoveMethod3()
    {
        float horiz = Input.GetAxis("Horizontal");
        rigid.velocity = new Vector2(horiz * speed, rigid.velocity.y);
        sr.flipX = Mathf.Sign(horiz) > 0 ? false : true;

        if (isOverSpeed())
        {
            // 최대 속도 제한 
            rigid.velocity = new Vector2(Mathf.Sign(rigid.velocity.x)*max_speed, rigid.velocity.y);            
        }       

        if (!Jumped)
            ps = isStop ? PlayerState.idle : PlayerState.running;

        anim.SetInteger("state", (int)ps);
    }

    void JumpMethod1()
    {
        if (Input.GetButtonDown("Jump") && !Jumped)
        {
            ps = PlayerState.jumping;
            anim.SetInteger("state", (int)ps);

            Vector3 vec = new Vector3(0f, jump_power, 0f);
            transform.position += vec * speed * Time.deltaTime;            
        }        
    }

    void JumpMethod2()
    {
        if (Input.GetButtonDown("Jump") && !Jumped)
        {            
            rigid.velocity = new Vector2(0, jump_power);
            ps = PlayerState.jumping;
            anim.SetInteger("state", (int)ps);
        }
    }

    void JumpMethod3()
    {
        if (Input.GetButtonDown("Jump") && !Jumped)
        {
            Vector2 vec = new Vector2(rigid.velocity.x, jump_power);
            rigid.AddForce(vec, ForceMode2D.Impulse);
            ps = PlayerState.jumping;
            anim.SetInteger("state", (int)ps);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Debug.Log("Ground!!!");
            ps = PlayerState.idle;
            anim.SetInteger("state", (int)ps);
        }
    }
}
