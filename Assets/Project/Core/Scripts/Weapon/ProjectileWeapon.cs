using UnityEngine;

public class ProjectileWeapon : Weapon
{
    public override void Fire(Vector3 targetPosition)
    {
        // TODO: replace with actual firing logic (instantiate projectile, set velocity, etc.)
        Debug.Log($"Firing projectile at {targetPosition}");
    }
}
