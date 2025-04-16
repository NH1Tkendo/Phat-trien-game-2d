using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [HideInInspector]
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float fireCooldown = 2f;
    private float fireTimer = 0f;

    public float fireRange = 8f;
    public Transform player;
    public LayerMask obstacleMask;

    [HideInInspector]
    public bool isChasing = false;
    //Hàm gán FirePoint
    void Awake()
    {
        if (firePoint == null)
        {
            firePoint = transform.Find("FirePoint");
            if (firePoint == null)
            {
                Debug.LogWarning($"{name}: Không tìm thấy FirePoint! Đảm bảo prefab có child tên chính xác.");
            }
        }

        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null || !isChasing) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= fireRange && HasLineOfSight() && IsFacingPlayer())
        {
            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0f)
            {
                FireAtPlayer();
                fireTimer = fireCooldown;
            }
        }
    }

    bool HasLineOfSight()
    {
        Vector2 direction = (player.position - firePoint.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, direction, fireRange, obstacleMask);

        Debug.DrawRay(firePoint.position, direction * fireRange, Color.red);

        return hit.collider == null || hit.collider.CompareTag("Player");
    }

    bool IsFacingPlayer()
    {
        float dirToPlayer = player.position.x - transform.position.x;
        bool playerOnRight = dirToPlayer > 0f;
        bool lookingRight = transform.localScale.x > 0f;
        return (playerOnRight && lookingRight) || (!playerOnRight && !lookingRight);
    }

    void FireAtPlayer()
    {
        Vector2 direction = (player.position - firePoint.position).normalized;
        Debug.Log("Firing bullet toward player: " + direction);

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
        }
    }
}