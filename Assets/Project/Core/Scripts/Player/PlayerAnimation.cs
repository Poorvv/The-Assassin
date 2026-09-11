using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int WeaponStance = Animator.StringToHash("WeaponStance");
    public void SetMovementSpeed(float speed)
    {
        animator.SetFloat(Speed, speed);
    }
    public void SetWeaponStance(WeaponType currentWeapon)
    {
        int stance = currentWeapon == WeaponType.Pistol ? 0 : 1;
        animator.SetInteger(WeaponStance, stance);
    }
}
