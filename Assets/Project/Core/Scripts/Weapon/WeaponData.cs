using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public WeaponType WeaponType;
    public float Range;
    public int Damage;
    public int ShotsPerFire;
    public float FireInterval;
    public GameObject WeaponPrefab;
}
