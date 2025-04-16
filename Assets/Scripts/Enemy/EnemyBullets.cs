using UnityEngine;

public class EnemyBullets : MonoBehaviour
{
    public int damage = 10;
    public float lifetime = 5f;

    void Start()
    {
        Destroy(gameObject, lifetime); // tự hủy sau X giây
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            if (playerMovement != null && playerHealth != null)
            {
                // Bỏ qua nếu player đang invincible (khi dodge)
                if (playerHealth.IsInvincible()) return;

                playerMovement.KBCounter = playerMovement.KBTotalTime;

                playerMovement.KnockFromRight = collision.transform.position.x <= transform.position.x;

                playerHealth.TakeDamage(damage);
            }
        }
        // ✅ Nếu trúng Block → hủy ngay
        if (collision.CompareTag("Block"))        
            Destroy(gameObject);        
    }
}