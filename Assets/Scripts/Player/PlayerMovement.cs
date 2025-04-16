using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    public float runSpeed = 40f;

    float horizontalMove = 0f;
    bool jump = false;

    // các thuộc tính liên qua đến knockback
    public float KBForce = 20f;//Lực knockback
    public float KBCounter;
    public float KBTotalTime = 0.4f;// Player sẽ bị giật lùi
    public bool KnockFromRight;
    // các thuộc tính liên quan đến lướt (dash)
    [Header("Dash Settings")]
    public float dashSpeed = 75f;//Tốc độ dash
    public float dashDuration = 0.2f;//Thời gian dash
    private bool isDashing = false;//Check xem có đang dash ko
    private float dashTimer = 0f;//Thời lượng dash để ngăn ko bị lướt
    private Collider2D playerCollider;//Người chơi va chạm với vật thể
    [SerializeField] private LayerMask enemyLayer;//Layer kẻ thù

    private PlayerHealth playerHealth;
    // 
    private bool skipSmoothingNextFrame = false;
    //các thuộc tính liên quan đến iframe
    [Header("Invincibility Flash")]
    public float invincibleTime = 1f;
    public SpriteRenderer spriteRenderer; // ← gán sẵn từ Inspector
    public Color flashColor = new Color(1f, 1f, 1f, 0.3f);
    public int flashCount = 5;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerCollider = GetComponent<Collider2D>();
        playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth != null)
            playerHealth.playerMovement = this;
    }

    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJumping", true);
        }

        if (Input.GetButtonDown("Dash") && !isDashing && KBCounter <= 0 && !IsInHurtState())
        {
            isDashing = true;
            dashTimer = dashDuration;
            animator.SetTrigger("Dash");

            if (playerHealth != null)
                playerHealth.SetInvincible(true);

            SetEnemyCollision(false);
        }
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }
    void FixedUpdate()
    {
        if (KBCounter > 0)
        {
            Vector2 knockback = KnockFromRight
                ? new Vector2(-KBForce, KBForce)
                : new Vector2(KBForce, KBForce);
            controller.ApplyKnockback(knockback);
            KBCounter -= Time.deltaTime;
        }
        else if (isDashing)
        {
            float direction = controller.IsFacingRight() ? 1f : -1f;

            if (dashTimer == dashDuration)
            {
                controller.ApplyKnockback(new Vector2(direction * dashSpeed, 0f));
            }

            dashTimer -= Time.deltaTime;

            if (dashTimer <= 0f)
            {
                isDashing = false;

                if (playerHealth != null)
                    playerHealth.SetInvincible(false);

                SetEnemyCollision(true);

                skipSmoothingNextFrame = true; // ✅ skip smoothing 1 frame
            }
        }
        if (skipSmoothingNextFrame)
        {
            controller.Move(horizontalMove * Time.deltaTime, false, true);
            skipSmoothingNextFrame = false;
        }
        else
        {
            controller.Move(horizontalMove * Time.deltaTime, jump);
            jump = false;
        }

    }
    //Hàm va chạm với kẻ địch
    void SetEnemyCollision(bool enabled)
    {
        Collider2D[] playerColliders = GetComponents<Collider2D>();
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(transform.position, 2f, enemyLayer);

        foreach (var enemyCol in enemyColliders)
        {
            Collider2D[] enemyAll = enemyCol.GetComponents<Collider2D>();
            foreach (var pCol in playerColliders)
            {
                foreach (var eCol in enemyAll)
                {
                    Physics2D.IgnoreCollision(pCol, eCol, !enabled);
                }
            }
        }
    }
    //Các hàm iFrame
    public void TriggerTemporaryCollisionIgnore()
    {
        StartCoroutine(TempIgnoreCollisionCoroutine());
    }
    private IEnumerator TempIgnoreCollisionCoroutine()
    {
        SetEnemyCollision(false);
        if (playerHealth != null)
            playerHealth.SetInvincible(true);

        StartCoroutine(FlashEffect());

        yield return new WaitForSeconds(invincibleTime);

        if (playerHealth != null)
            playerHealth.SetInvincible(false);
        SetEnemyCollision(true);
    }
    private IEnumerator FlashEffect()
    {
        Color originalColor = spriteRenderer.color;

        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(invincibleTime / (flashCount * 2));
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(invincibleTime / (flashCount * 2));
        }
    }
    //hàm liên quan đến trạng thái bị hurt
    private bool IsInHurtState()
    {
        if (animator == null) return false;
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Hurt");
    }
}
