using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] WeaponData startingWeapon; // Temp var
    [SerializeField] Transform weaponHolder;
    [SerializeField] PlayerAnimation playerAnimation;
    [SerializeField] PlayerCombat playerCombat;
    public WeaponData CurrentWeaponData { get; private set; }
    private void Awake()
    {
        InitWeapon(startingWeapon);
    }
    public void InitWeapon(WeaponData weaponData)
    {
        Instantiate(weaponData.WeaponPrefab, weaponHolder);

        CurrentWeaponData = weaponData;
        playerCombat.InitCombatData(weaponData);
        playerAnimation.SetWeaponStance(weaponData.WeaponType);
    }
}
