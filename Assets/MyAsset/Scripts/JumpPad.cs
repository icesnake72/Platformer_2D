using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField]
    private float jumpPower = 10f;

    private Animator anim;
    private bool readyToJump;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        readyToJump = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ( collision.gameObject.CompareTag("Player"))
        {
            anim.SetBool("Jump", true);            
            collision.rigidbody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            readyToJump = false;
        }
    }     

    private void ReadyToJump()
    {
        anim.SetBool("Jump", false);
        readyToJump = true;
    }
}
