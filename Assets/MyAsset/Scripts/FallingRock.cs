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

    // Update is called once per frame
    void Update()
    {
        switch(rs)
        {
            case RockState.ready:
                rs = RockState.idle;
                rigid.gravityScale = 0f;                
                Invoke("Fall", 3f);
                break;


            case RockState.up:
                // rigid.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
                if (rigid.velocity.y > 1f)
                    rigid.velocity = new Vector2(0f, maxUpSpeed);


                //transform.position = Vector2.MoveTowards(transform.position,
                //    startPoint.position, upSpeed * Time.deltaTime);
                break;            
        }

        if (Vector2.Distance(startPoint.position, transform.position) < 0.1f && rs==RockState.up)
        {            
            rs = RockState.ready;
            rigid.velocity = Vector2.zero;
        }
    }

    private void Fall()
    {        
        gameObject.tag = "Trap";
        rigid.gravityScale = 1f;
        rigid.AddForce(Vector2.down * downSpeed, ForceMode2D.Impulse);        
        rs = RockState.falling;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            rs = RockState.idle;
            Invoke("GoingUp", 2f);
        }
    }

    private void GoingUp()
    {        
        rs = RockState.up;
        gameObject.tag = "Dummy";
    }

}
