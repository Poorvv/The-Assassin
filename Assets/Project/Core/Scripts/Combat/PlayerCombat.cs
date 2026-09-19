using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] WeaponManager weaponManager;
    [SerializeField] LayerMask _enemyLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] Transform aimTarget;
    [SerializeField] float aimReturnSmoothTime = 0.2f;
    private Vector3 _defaultAimPointPos;
    private Collider _currentTarget;
    private float _nextFireTime;
    private float _detectionRange;
    private Vector3 _aimTargetVelocity;

    void Awake()
    {
        _defaultAimPointPos = aimTarget.localPosition;
    }
    public void InitCombatData(WeaponData weaponData)
    {
        _detectionRange = weaponData.Range;
    }
    private void Update()
    {
        DetectNearbyEnemies();
        if(_currentTarget == null)
        {
            ResetAimTarget();
            return;
        }
        AimAtTarget();
        if (CanWeaponSeeTarget())
        {
            if (IsAimedAtTarget())
            {
                Shoot();
            }
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
    private void AimAtTarget()
    {
        if (_currentTarget == null)
        {
            ResetAimTarget();
            return;
        }

        if (Time.time < _nextFireTime)
        {
            ResetAimTarget();
            return;
        }

        if (!CanWeaponSeeTarget())
        {
            ResetAimTarget();
            return;
        }

        aimTarget.position = _currentTarget.transform.position;
    }
    private bool IsAimedAtTarget()
    {
        if (_currentTarget == null)
            return false;

        Transform aimPoint = weaponManager.CurrentWeapon.AimPoint;

        Vector3 directionToTarget = (_currentTarget.transform.position - aimPoint.position).normalized;

        float alignment =
            Vector3.Dot(aimPoint.forward, directionToTarget);

        //Debug.Log($"Alignment: {alignment}");

        return alignment >= 0.98f;
    }
    private bool CanWeaponSeeTarget()
    {
        Transform muzzle = weaponManager.CurrentWeapon.Muzzle;

        Vector3 direction =
            _currentTarget.transform.position - muzzle.position;

        float distance = direction.magnitude;

        return !Physics.Raycast(
            muzzle.position,
            direction.normalized,
            distance,
            obstacleLayer
        );
    }
    void Shoot()
    {
        if (_currentTarget == null || Time.time < _nextFireTime)
            return;
            
        weaponManager.CurrentWeapon.Fire(_currentTarget.transform.position);
        _nextFireTime = Time.time + weaponManager.CurrentWeaponData.FireInterval;
    }
    void ResetAimTarget()
    {
        aimTarget.localPosition = Vector3.SmoothDamp(
        aimTarget.localPosition,
        _defaultAimPointPos,
        ref _aimTargetVelocity,
        aimReturnSmoothTime
        );
    }

    //Debugging purposes, visualize the detection range in the editor
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _detectionRange);
    }
}
