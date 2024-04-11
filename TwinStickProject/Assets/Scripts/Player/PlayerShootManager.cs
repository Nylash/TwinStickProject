using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShootManager : MonoBehaviour
{
    public static PlayerShootManager instance;

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
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

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
        while (PlayerWeaponsManager.instance.CanShoot()) 
        {
            for (int i = 0; i < PlayerWeaponsManager.instance.MainWeapon.projectilesByShot; i++)
            {
                PlayerWeaponsManager.instance.MainWeapondConsumeAmmunition();
                currentBullet = Instantiate(bullet, bulletSpawn.position, Quaternion.identity);
                currentBulletBehaviorRef = currentBullet.GetComponent<ProjectileBehavior>();
                currentBulletBehaviorRef.direction = new Vector3(PlayerAimManager.instance.AimDirection.x, 0, PlayerAimManager.instance.AimDirection.y);
                currentBullet.layer = (int)Mathf.Log(bulletLayer.value, 2);//To get real value from bitMask
                currentBullet.SetActive(true);
                if (PlayerWeaponsManager.instance.MainWeapon.projectilesByShot > 1)
                    yield return new WaitForSeconds(PlayerWeaponsManager.instance.MainWeapon.timeBetweenProjectiles);
            }
            yield return new WaitForSeconds(PlayerWeaponsManager.instance.MainWeapon.fireRate);
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
