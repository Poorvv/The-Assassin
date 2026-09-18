using UnityEngine;

public class HitScanWeapon : Weapon
{

    // Implement the abstract method from Weapon
    public override void Fire(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity))
        {
            EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(weaponData.Damage);
            }
        }
        else
        {
            Debug.Log("Missed");
        }
    }
}
