using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int health;

    private bool isInvincible = false;
    private Animator animator;

    void Start()
    {
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
            Destroy(gameObject);
        }
    }

    public void SetInvincible(bool value)
    {
        isInvincible = value;
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }
}
