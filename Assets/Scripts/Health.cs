using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamagable
{
    [SerializeField] private int maxHealth = 100;
    private int health = 100;

    public UnityEvent<int, int> HealthChanged;
    public UnityEvent Died;

    public void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        HealthChanged.Invoke(health, maxHealth);

        if (health == 0)
            Die();
    }

    private void Die()
    {
        Died.Invoke();
    }
}

public interface IDamagable
{
    void TakeDamage(int damage);
}
