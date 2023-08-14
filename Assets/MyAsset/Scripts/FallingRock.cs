using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRock : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Transform startPoint;

    [SerializeField]
    private float upSpeed = 20f;

    [SerializeField]
    private float maxUpSpeed = 1f;

    [SerializeField]
    private float downSpeed = 30f;

    private bool isGoingUp = false;

    private enum RockState { ready, falling, up, idle };
    private RockState rs;
    private Rigidbody2D rigid;

    private void Awake()
    {
        rs = RockState.ready;
        rigid = GetComponent<Rigidbody2D>();
        rigid.gravityScale = 0f;
    }

    private void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (rs)
        {
            case RockState.ready:
                ReadyToFall();
                break;

            case RockState.up:
                GoingUp();
                break;
        }

        CheckDistance();
    }

    private void CheckDistance()
    {
        if (Vector2.Distance(startPoint.position, transform.position) < 0.1f && rs == RockState.up)
        {
            rs = RockState.ready;
            rigid.velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            gameObject.tag = "Ground";
            rs = RockState.idle;
            Invoke("ReadyToUp", 2f);
        }
    }

    private void ReadyToFall()
    {
        rs = RockState.idle;
        Invoke("Fall", 3f);
    }

    private void Fall()
    {
        gameObject.tag = "Trap";
        rigid.gravityScale = 20f;        
        rs = RockState.falling;
    }

    private void ReadyToUp()
    {
        rs = RockState.up;
        rigid.gravityScale = 0f;
    }

    private void GoingUp()
    {
        rigid.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
        if (rigid.velocity.y > 1f)
            rigid.velocity = new Vector2(0f, maxUpSpeed);
    }

}
