using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackRange = 1f;
    public int damage = 20;
    public float attackCooldown = 1.5f;
    public Vector2 attackBoxSize = new Vector2(2f, 1.5f);
    public Transform attackPoint;

    private float cooldownTimer = 0f;
    private Animator animator;
    private Transform player;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (attackPoint == null)
        {
            Debug.LogWarning("❌ attackPoint chưa được gán trong EnemyAttack");
        }
    }

    void Update()
    {
        if (player == null) return;

        cooldownTimer += Time.deltaTime;

        float dist = Vector2.Distance(transform.position, player.position);
        if (dist <= attackRange && cooldownTimer >= attackCooldown)
        {
            Attack();
            cooldownTimer = 0f;
        }
    }

    void Attack()
    {
        if (animator != null)
            animator.SetTrigger("Attack");

        Collider2D[] hits = Physics2D.OverlapBoxAll(
            attackPoint.position,
            attackBoxSize,
            0f,
            LayerMask.GetMask("Player") // hoặc dùng: ~0 nếu ko dùng LayerMask
        );

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                PlayerHealth ph = hit.GetComponent<PlayerHealth>();
                if (ph != null && !ph.IsInvincible())
                {
                    ph.TakeDamage(damage);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPoint.position, attackBoxSize);
    }
}
