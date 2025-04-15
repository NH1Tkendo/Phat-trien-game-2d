using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    public float runSpeed = 40f;

    float horizontalMove = 0f;
    bool jump = false;

    public float KBForce;
    public float KBCounter;
    public float KBTotalTime;
    public bool KnockFromRight;

    [Header("Dash Settings")]
    public float dashSpeed = 75f;
    public float dashDuration = 0.2f;
    private bool isDashing = false;
    private float dashTimer = 0f;

    private Collider2D playerCollider;
    [SerializeField] private LayerMask enemyLayer;

    private PlayerHealth playerHealth;

    private bool skipSmoothingNextFrame = false;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerCollider = GetComponent<Collider2D>();
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
    private bool IsInHurtState()
    {
        if (animator == null) return false;
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Hurt");
    }

}
