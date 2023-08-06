using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;

    [SerializeField]
    private float max_speed = 3f;

    [SerializeField]
    private float jump_power = 5f;

    [SerializeField]
    private float grivity_weight = 1f;

    private Animator anim;
    private Rigidbody2D rigid;
    private SpriteRenderer sr;

    private bool jumped = false;

    private enum PlayerState { idle, running, jumping, falling };
    private PlayerState playerState;

    private void Awake()
    {
        jumped = false;
        playerState = PlayerState.idle;

        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }


    private void FixedUpdate()
    {
        // rigidbody를 이용한 움직임은 FixedUpdate()에서 처리해주는게 좋다         
        MoveMethod3();

        if (jumped)
        {
            if (rigid.velocity.y < 0)
            {
                Debug.Log("폴 ");
                rigid.velocity += grivity_weight * Physics2D.gravity * Time.deltaTime;
                playerState = PlayerState.falling;
                anim.SetInteger("state", (int)playerState);
            }
                
            //else
            //    rigid.velocity += Physics2D.gravity * Time.deltaTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //SimpleMove();
        //JumpMethod1();
        // MoveMethod2();
        JumpMethod2();
        // JumpMethod3();        
    }

    void SimpleMove()
    {
        float horiz = Input.GetAxis("Horizontal");
        Vector3 vec = new Vector3(horiz, 0f, 0f);
        transform.position += vec * speed * Time.deltaTime;
    }

    void MoveMethod2()
    {
        // horiz 값은 왼쪽 방향키가 눌리면 음수, 반대인 경우 양수가 된다 
        float horiz = Input.GetAxis("Horizontal");

        // Vector2.right는 (1, 0)이기 때문에 horiz값에 의해 방향이 결정된다(right에 너무 집중할 필요가 없고, rigth는 상수이기때문에 right가 갖고 있는 값에 집중해야 한다 )
        // AddForce() 메소드는 rigidbody2d의 velocity 값에 영향을 준다 (velocity 값을 바꾼다는 뜻임 )
        rigid.AddForce(Vector2.right * horiz * speed, ForceMode2D.Impulse);
        if (rigid.velocity.x > max_speed)
            rigid.velocity = new Vector2(max_speed, rigid.velocity.y);
        else if (rigid.velocity.x < max_speed * -1)
            rigid.velocity = new Vector2(max_speed * -1, rigid.velocity.y);
    }

    void MoveMethod3()
    {
        float horiz = Input.GetAxis("Horizontal");

        rigid.velocity = new Vector2(horiz * speed, rigid.velocity.y);
        if (rigid.velocity.x > max_speed)
        {
            if (playerState != PlayerState.jumping && playerState != PlayerState.falling)
                playerState = PlayerState.running;
            rigid.velocity = new Vector2(max_speed, rigid.velocity.y);
            sr.flipX = false;            
        }                
        else if (rigid.velocity.x < max_speed * -1)
        {
            if (playerState!=PlayerState.jumping && playerState!=PlayerState.falling)
                playerState = PlayerState.running;

            rigid.velocity = new Vector2(max_speed * -1, rigid.velocity.y);
            sr.flipX = true;            
        }
        else
        {
            if (playerState != PlayerState.jumping && playerState != PlayerState.falling)
                playerState = PlayerState.idle;

            Debug.Log("플레이어 정지됨 !!!!");
        }

        anim.SetInteger("state", (int)playerState);
        //if (!jumped)
        //    anim.SetFloat("Run", Mathf.Abs(rigid.velocity.x));
    }

    void JumpMethod1()
    {
        if (Input.GetButtonDown("Jump") && jumped==false)
        {
            jumped = true;
            Vector3 vec = new Vector3(0f, jump_power, 0f);
            transform.position += vec * speed * Time.deltaTime;
            playerState = PlayerState.jumping;
            anim.SetInteger("state", (int)playerState);
        }        
    }

    void JumpMethod2()
    {
        if (Input.GetButtonDown("Jump") && jumped==false)
        {
            Debug.Log("점핑 ");
            jumped = true;            
            rigid.velocity = new Vector2(0, jump_power);
            playerState = PlayerState.jumping;
            anim.SetInteger("state", (int)playerState);
        }
    }

    void JumpMethod3()
    {
        if (Input.GetButtonDown("Jump") && jumped == false)
        {
            jumped = true;
            Vector2 vec = new Vector2(rigid.velocity.x, jump_power);
            rigid.AddForce(vec, ForceMode2D.Impulse);
            playerState = PlayerState.jumping;
            anim.SetInteger("state", (int)playerState);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Debug.Log("바닥에 착지!!!");
            jumped = false;
            playerState = PlayerState.idle;
            anim.SetInteger("state", (int)playerState);
        }
    }
}
