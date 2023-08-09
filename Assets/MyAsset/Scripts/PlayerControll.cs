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

    [SerializeField]
    private AudioSource jumpSound;

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

    //private bool Jumped
    //{
    //    get
    //    {
    //        return (ps == PlayerState.jumping) || (ps == PlayerState.falling);
    //    }
    //}

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
        /*
        이 문장은 Unity에서 Physics2D.BoxCast 함수를 사용하여 물리적인 상자 형태의 캐스트를 수행하는 것을 나타냅니다. 이 함수는 2D 공간에서 충돌을 검출하는 데 사용되며, 특히 지정한 경로를 따라 레이캐스팅을 수행하여 충돌 여부를 판단합니다.

        coll.bounds.center: coll이라는 객체(일반적으로 Collider2D)의 중심점을 나타내는 벡터입니다. 이는 캐스팅의 시작점으로 사용됩니다.
        coll.bounds.size: coll 객체의 크기를 나타내는 벡터로, 상자의 너비와 높이를 의미합니다.
        0f: 상자 캐스팅의 회전 각도를 나타내며, 여기서는 회전 없이 직선 캐스팅을 하기 때문에 0도로 설정합니다.
        Vector2.down: 상자 캐스팅의 방향을 나타내는 벡터입니다.여기서는 아래쪽 방향(떨어지는 방향)을 나타내는 벡터로 설정합니다.
        1f: 상자 캐스팅의 최대 거리를 나타냅니다.이 경우 1 유닛 거리까지만 캐스팅을 수행합니다.
        jumpableGround: 충돌을 검출할 레이어 마스크(Layer Mask)를 나타냅니다. jumpableGround 레이어 마스크에 해당하는 레이어와만 충돌을 검출하게 됩니다.
        따라서 위의 코드는 coll 객체의 바운딩 박스를 기준으로 아래쪽으로 1 유닛 거리까지 상자 캐스팅을 수행하며, jumpableGround 레이어와의 충돌 여부를 검출합니다.이를 통해 지면과의 접촉 여부 등을 확인하고, 점프 가능한 상태인지 판단하는 데 사용될 수 있습니다.
        */
        return Physics2D.BoxCast(coll.bounds.center,    // 캐스팅의 시작점 
                                 coll.bounds.size,      // 박스 콜라이더의 사이즈 
                                 0f,                    // 박스 콜라이더의 기울임(앵글)
                                 Vector2.down,          // 캐스트할 방향 
                                 1f,                    // 캐스트할 거리 (1유닛)
                                 jumpableGround);       // 충돌을 체크할 레이어 
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
            jumpSound.Play();
            Vector3 vec = new Vector3(0f, jump_power, 0f);
            transform.position += vec * speed * Time.deltaTime;            
        }        
    }

    private void JumpMethod2()
    {
        if (Input.GetButtonDown("Jump") && isGround())
        {
            jumpSound.Play();
            rigid.velocity = new Vector2(0, jump_power);
        }
    }

    private void JumpMethod3()
    {
        if (Input.GetButtonDown("Jump") && isGround())
        {
            jumpSound.Play();
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
