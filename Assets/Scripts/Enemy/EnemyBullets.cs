using UnityEngine;

public class EnemyBullets : MonoBehaviour
{
    public int damage = 10;
    public float lifetime = 5f;

    void Start()
    {
        Destroy(gameObject, lifetime); // tự hủy sau X giây
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // ✅ Nếu trúng đạn người chơi → bỏ qua
        if (other.GetComponent<Bullet>() != null)
        {
            return;
        }

        // ✅ Nếu trúng Player
        if (other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null && !ph.IsInvincible())
            {
                ph.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        // ✅ Nếu trúng Block → hủy ngay
        else if (other.CompareTag("Block"))
        {
            Destroy(gameObject);
        }
        // ✅ Nếu trúng thứ khác không phải Enemy → cũng hủy
        else if (!other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}