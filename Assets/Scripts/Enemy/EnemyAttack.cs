using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackRange = 3f;
    private Transform player;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning($"{name}: Không tìm thấy Player trong scene!");
    }

    void Update()
    {
        if (player == null || animator == null) return;

        float dist = Vector2.Distance(transform.position, player.position);
        if (dist <= attackRange)
        {
            animator.SetTrigger("Attack");
        }
    }
}
