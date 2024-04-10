using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShootManager : MonoBehaviour
{
    public static PlayerShootManager instance;

    ControlsMap controlsMap;

    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private LayerMask bulletLayer;

    private bool isTryingToShoot;
    private GameObject currentBullet;
    private ProjectileBehavior currentBulletBehaviorRef;

    private void OnEnable() => controlsMap.Gameplay.Enable();
    private void OnDisable() => controlsMap.Gameplay.Disable();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        controlsMap = new ControlsMap();

        controlsMap.Gameplay.Shoot.performed += ctx => isTryingToShoot = true;
        controlsMap.Gameplay.Shoot.canceled += ctx => isTryingToShoot = false;
    }

    private void Update()
    {
        if (isTryingToShoot)
        {
            currentBullet = Instantiate(bullet,bulletSpawn.position,Quaternion.identity);
            currentBulletBehaviorRef = currentBullet.GetComponent<ProjectileBehavior>();
            currentBulletBehaviorRef.direction = new Vector3(PlayerAimManager.instance.AimDirection.x,0,PlayerAimManager.instance.AimDirection.y);
            currentBullet.layer = (int)Mathf.Log(bulletLayer.value, 2);//To get real value from bitMask
            currentBullet.SetActive(true);
        }
    }
}
