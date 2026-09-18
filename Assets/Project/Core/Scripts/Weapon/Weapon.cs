using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected WeaponData weaponData;
    [SerializeField] protected Transform muzzle;
    [SerializeField] protected Transform aimPoint;

    public WeaponData Data => weaponData;
    public Transform Muzzle => muzzle;
    public Transform AimPoint => aimPoint;
    public abstract void Fire(Vector3 targetPosition);

}
