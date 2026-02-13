using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class PlayerControl : MonoBehaviour
{
    [Header("설정")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("상태")]
    public float maxHealth = 100f;
    public float currentHealth = 100f;
    public bool isHurt = false;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool hasDiedFromFall = false;
    private bool isDead = false;

    public GameObject GameOverUI;
    private CinemachineImpulseSource impulseSource;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void FixedUpdate()
    {
        if (isDead) return;
        
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        
        if (transform.position.y < -20f && !hasDiedFromFall)
        {
            hasDiedFromFall = true;
            Die();
        }
    }

    public void Jump()
    {
        if (isDead) return;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    public void Attack()
    {
        if (isDead) return;
        Debug.Log("공격!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("obstacle"))
        {
            Debug.Log("장애물과 충돌!");
            TakeDamage();
        }
        if (collision.CompareTag("FinalLine"))
        {
            Debug.Log("최종라인과 충돌!");
            Clear();
        }
    }

    private void TakeDamage()
    {
        if (isHurt) return;

        currentHealth -= 40f;
        Debug.Log("현재 체력: " + currentHealth);
        if (impulseSource != null)
        {
            // 괄호 안에 숫자를 넣으면 강도를 곱해서 조절할 수 있습니다. 예: GenerateImpulse(0.5f);
            impulseSource.GenerateImpulse();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartHurtEffect();
        }
    }

    private void StartHurtEffect()
    {
        isHurt = true;

        // 0.2초 동안 변했다가(Duration), 20번 반복(Loops)하며, 다시 원래대로 돌아오는 방식(Yoyo)
        // 총 지속 시간: 0.2초 * 10회 = 2초
        spriteRenderer.DOFade(0.4f, 0.2f)
            .SetLoops(10, LoopType.Yoyo)
            .OnComplete(() => {
                isHurt = false;
                spriteRenderer.color = Color.white;
            });
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("죽음!");
        GameOverUI.SetActive(true);
        transform.DOKill(); 
    }

    private void Clear()
    {
        Debug.Log("승리!");
        transform.DOKill(); 
    }
    
    private void OnDestroy()
    {
        transform.DOKill();
    }
}