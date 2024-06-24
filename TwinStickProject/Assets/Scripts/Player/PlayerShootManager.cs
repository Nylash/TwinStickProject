using System.Collections;
using UnityEngine;

public class PlayerShootManager : Singleton<PlayerShootManager>
{
    ControlsMap controlsMap;

    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private LayerMask bulletLayer;

    private GameObject currentBullet;
    private ProjectileBehavior currentBulletBehaviorRef;
    private bool stopShooting;
    private Coroutine shootingCoroutine;

    private void OnEnable() => controlsMap.Gameplay.Enable();
    private void OnDisable() => controlsMap.Gameplay.Disable();

    private void Awake()
    {
        controlsMap = new ControlsMap();

        controlsMap.Gameplay.Shoot.performed += ctx => StartShooting();
        controlsMap.Gameplay.Shoot.canceled += ctx => StopShooting();
    }

    private void StartShooting()
    {
        if (!stopShooting)
            shootingCoroutine = StartCoroutine(Shoot());
        else
            stopShooting = false;
    }

    private void StopShooting()
    {
        if (shootingCoroutine != null)
            stopShooting = true;
    }

    private IEnumerator Shoot()
    {
        while (PlayerWeaponsManager.Instance.CanShoot()) 
        {
            PlayerWeaponsManager.Instance.MainWeaponConsumeAmmunition();
            for (int i = 0; i < PlayerWeaponsManager.Instance.MainWeapon.projectilesByShot; i++)
            {
                currentBullet = Instantiate(bullet, bulletSpawn.position, Quaternion.identity);
                currentBulletBehaviorRef = currentBullet.GetComponent<ProjectileBehavior>();
                float randomAngle = Random.Range(-PlayerWeaponsManager.Instance.MainWeapon.inaccuracyAngle, PlayerWeaponsManager.Instance.MainWeapon.inaccuracyAngle);

                Vector3 shootDirection = Quaternion.AngleAxis(randomAngle, Vector3.up) 
                    * new Vector3(PlayerAimManager.Instance.AimDirection.x, 0, PlayerAimManager.Instance.AimDirection.y);
                currentBulletBehaviorRef.direction = shootDirection.normalized;
                currentBulletBehaviorRef.speed = PlayerWeaponsManager.Instance.MainWeapon.bulletSpeed;
                currentBulletBehaviorRef.range = PlayerWeaponsManager.Instance.MainWeapon.range;
                currentBullet.layer = (int)Mathf.Log(bulletLayer.value, 2);//To get real value from bitMask
                currentBullet.SetActive(true);
                if (PlayerWeaponsManager.Instance.MainWeapon.projectilesByShot > 1)
                    yield return new WaitForSeconds(PlayerWeaponsManager.Instance.MainWeapon.timeBetweenProjectiles);
            }
            yield return new WaitForSeconds(PlayerWeaponsManager.Instance.MainWeapon.fireRate);
            if (stopShooting)
            {
                stopShooting = false;
                shootingCoroutine = null;
                yield break;
            }
        }
        stopShooting = false;
        shootingCoroutine = null;
        yield break;
    }
}
