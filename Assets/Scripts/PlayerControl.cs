using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // [핵심] DOTween 네임스페이스 추가

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

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }

    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    public void Attack()
    {
        Debug.Log("공격!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("obstacle"))
        {
            Debug.Log("장애물과 충돌!");
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        if (isHurt) return;

        currentHealth -= 50f;
        Debug.Log("현재 체력: " + currentHealth);

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

        // 기존 색상에서 투명도(Alpha)만 0.4로 변경하는 애니메이션
        // 0.2초 동안 변했다가(Duration), 20번 반복(Loops)하며, 다시 원래대로 돌아오는 방식(Yoyo)
        // 총 지속 시간: 0.2초 * 15회 = 3초
        spriteRenderer.DOFade(0.4f, 0.2f)
            .SetLoops(15, LoopType.Yoyo)
            .OnComplete(() => {
                // 애니메이션이 다 끝나면 실행될 코드 (람다식)
                isHurt = false;
                spriteRenderer.color = Color.white;
            });
    }

    private void Die()
    {
        Debug.Log("죽음!");
        transform.DOKill(); 
    }
    
    private void OnDestroy()
    {
        transform.DOKill();
    }
}