using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] WeaponManager weaponManager;
    [SerializeField] LayerMask _enemyLayer;
    [SerializeField] Transform aimTarget;
    private Collider _currentTarget;
    private float _nextFireTime;
    private float _detectionRange;

    public void InitCombatData(WeaponData weaponData)
    {
        _detectionRange = weaponData.Range;
    }
    private void Update()
    {
        DetectNearbyEnemies();
        if (weaponManager.CurrentWeapon != null)
        {
            //Transform aimPoint = weaponManager.CurrentWeapon.AimPoint;

            //Debug.DrawRay(
            //    aimPoint.position,
            //    aimPoint.forward * 5f,
            //    Color.blue
            //);
        }
        AimAtTarget();
        
        if (IsAimedAtTarget())
        {
            Shoot();
        }
    }
    void DetectNearbyEnemies()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _detectionRange, _enemyLayer);
        Collider closestEnemy = null;
        float closestDistanceSqr = Mathf.Infinity;
        foreach (var hitCollider in hitColliders)
        {
            EnemyHealth enemyHealth = hitCollider.GetComponent<EnemyHealth>();
            if (enemyHealth == null || !enemyHealth.IsAlive)
                continue;
            if (!CheckLOS(hitCollider))
                continue;
            float distanceSqr = (hitCollider.transform.position - transform.position).sqrMagnitude;
            if (distanceSqr < closestDistanceSqr)
            {
                closestDistanceSqr = distanceSqr;
                closestEnemy = hitCollider;
            }
        }
        _currentTarget = closestEnemy;
        Debug.Log($"Closest Enemy: {_currentTarget?.name ?? "None"}");
    }
    bool CheckLOS(Collider enemy)
    {
        Transform aimPoint = weaponManager.CurrentWeapon.AimPoint;
        Vector3 direction = (enemy.transform.position - aimPoint.position).normalized;
        if (Physics.Raycast(aimPoint.position, direction, out RaycastHit hit, _detectionRange) && hit.collider == enemy)
        {
            //Debugging purposes, visualize the raycast in the editor
            Debug.DrawRay(aimPoint.position, direction * _detectionRange, hit.collider == enemy ? Color.green : Color.red);
            return true;
        }
        return false;
    }
    void AimAtTarget()
    {
        if(_currentTarget == null)
            return;
        aimTarget.position = _currentTarget.transform.position;
        /*Vector3 direction = (_currentTarget.transform.position - transform.position).normalized;
        direction.y = 0; // Keep the player upright
        if(direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 720f * Time.deltaTime);
        }*/
    }
    bool IsAimedAtTarget()
    {
        if (_currentTarget == null)
            return false;
        //Vector3 directionToTarget = (_currentTarget.transform.position - lookingPoint.position).normalized;
        //float alignment = Vector3.Dot(lookingPoint.forward, directionToTarget);
        //return alignment > 0.99f; // Adjust the threshold as needed
        //Transform muzzle = weaponManager.CurrentWeapon.Muzzle;

        //Vector3 direction =
        //    (aimTarget.position - muzzle.position).normalized;

        //float alignment =
        //    Vector3.Dot(muzzle.forward, direction);

        //return alignment > 0.98f;
        Transform aimPoint = weaponManager.CurrentWeapon.AimPoint;

        Vector3 directionToTarget =
            (aimTarget.position - aimPoint.position).normalized;

        float alignment =
            Vector3.Dot(aimPoint.forward, directionToTarget);

        return alignment >= 0.98f;
    }
    void Shoot()
    {
        if (_currentTarget == null)
            return;
        if(Time.time < _nextFireTime)
            return;
        //AimAtTarget();
        weaponManager.CurrentWeapon.Fire(_currentTarget.transform.position);
        _nextFireTime = Time.time + weaponManager.CurrentWeaponData.FireInterval;
    }

    //Debugging purposes, visualize the detection range in the editor
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _detectionRange);
    }
}
