using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    int _currentHealth;
    public bool IsAlive => _currentHealth > 0;

    private void Awake()
    {
        _currentHealth = maxHealth;
    }
    public void TakeDamage(int damage)
    {
        if(!IsAlive) return;
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }
    }
    void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject);
        //Ragdoll implementation.
    }
}
