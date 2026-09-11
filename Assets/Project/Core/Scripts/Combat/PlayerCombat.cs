using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] LayerMask _enemyLayer;
    private float _detectionRange;

    public void InitCombatData(WeaponData weaponData)
    {
        _detectionRange = weaponData.Range;
    }
    private void Update()
    {
        DetectNearbyEnemies();
    }
    void DetectNearbyEnemies()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _detectionRange, _enemyLayer);
        foreach (var hitCollider in hitColliders)
        {
            CheckLOS(hitCollider);
        }
    }
    void CheckLOS(Collider enemy)
    {
        Vector3 direction = (enemy.transform.position - transform.position).normalized;
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, _detectionRange))
        {
            if (hit.collider == enemy)
            {
                Debug.Log($"Can see {enemy.name}");
            }
        }
    }
}
