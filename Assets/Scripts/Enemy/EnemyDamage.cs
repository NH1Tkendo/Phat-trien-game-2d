using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage = 10;

    private void OnCollisionEnter2D(Collision2D collision)
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
    }
}
