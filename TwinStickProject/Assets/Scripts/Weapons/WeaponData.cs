using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Weapon")]
public class WeaponData : ScriptableObject
{
    public Weapons weapon;
    [Tooltip("Time between each shoot (in seconds).")]
    public float fireRate;
    public int damage;
    public float bulletSpeed;
    public int ammunitionMax;
    public int projectilesByShot;
    [Tooltip("Time between each projectile of one shoot (in seconds), so it also increase fireRate.")]
    public float timeBetweenProjectiles;
    public float range;
    public GameObject projectile;
    public float inaccuracyAngle;
    public float reloadTime;

    public enum Weapons
    {
        AK, M16, Pistol
    }
}
