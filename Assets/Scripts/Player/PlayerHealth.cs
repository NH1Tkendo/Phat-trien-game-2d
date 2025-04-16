using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //Máu
    public int maxHealth = 100;
    public int health;
    
    private bool isInvincible = false;//iFrame
    public GameObject deathEffect;
    private Animator animator;//Animation    
    [HideInInspector]
    public PlayerMovement playerMovement;//Chuyển động người chơi

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        health = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible)
            return;

        health -= damage;

        if (animator != null)
        {
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
            if (!currentState.IsName("Dash"))
            {
                animator.SetTrigger("Hurt");
            }
        }

        if (health <= 0)
        {
            Die();
        }
        //iFrame sau khi trúng đòn đánh
        if (playerMovement != null)
            playerMovement.TriggerTemporaryCollisionIgnore(); // 👇 gọi hàm xử lý
    }

    public void SetInvincible(bool value)
    {
        isInvincible = value;
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }
    void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
