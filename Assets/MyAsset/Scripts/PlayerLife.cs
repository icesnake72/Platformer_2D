using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    [SerializeField]
    private AudioSource deathSound;

    private Animator anim;
    private Rigidbody2D rigid;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            Die();
        }        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            Die();
        }
    }

    private void Die()
    {
        /*
        방지: 플레이어 캐릭터가 죽었을 때 물리 시뮬레이션이 계속해서 작용하면 캐릭터가 물리적인 힘에 의해 움직이거나 무작위로 튕기는 등의 현상이 발생할 수 있습니다. 이는 캐릭터의 죽음 상태에서 원치 않는 동작을 방지하기 위해 Static으로 설정하는 것입니다.
        게임 오버/리스폰: 대부분의 경우 캐릭터가 죽으면 게임 오버 또는 리스폰을 시행합니다. 이 때 물리 시뮬레이션에 의한 움직임은 필요하지 않으므로 Static으로 설정하여 물리적인 영향을 받지 않도록 만듭니다.
        효율성: 캐릭터가 죽은 상태에서 물리 시뮬레이션이 계속 돌아가면 게임 성능에 부하를 줄 수 있습니다. Static으로 설정하면 해당 객체에 대한 물리적 계산이 생략되므로 게임의 효율성이 향상됩니다.
        따라서 플레이어 캐릭터의 죽음을 처리할 때, Rigidbody2D를 Static으로 설정하여 물리적인 영향을 제어하고, 적절한 시기에 게임 오버 또는 리스폰을 수행하는 것이 보통 좋은 접근 방식입니다.
        */
        deathSound.Play();


        rigid.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("dead");
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
