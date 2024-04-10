using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponsManager : MonoBehaviour
{
    public static PlayerWeaponsManager instance;

    ControlsMap controlsMap;

    private bool mainWeaponReloading;
    private int mainWeaponAmmunition;

    [SerializeField] private WeaponData mainWeapon;
    
    public WeaponData MainWeapon { get => mainWeapon; }

    private void OnEnable() => controlsMap.Gameplay.Enable();
    private void OnDisable() => controlsMap.Gameplay.Disable();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        controlsMap = new ControlsMap();

        controlsMap.Gameplay.Reload.performed += ctx => StartCoroutine(Reload());
    }

    private void Start()
    {
        mainWeaponAmmunition = mainWeapon.ammunitionMax;
    }

    public bool CanShoot()
    {
        if (!mainWeaponReloading)
        {
            if(mainWeaponAmmunition - mainWeapon.projectilesByShot >= 0)
                return true;
        }
        return false;
    }

    public void MainWeapondConsumeAmmunition()
    {
        mainWeaponAmmunition--;
        if (mainWeaponAmmunition < 0)
            Debug.LogError("Shouldn't have shot.");
    }

    private IEnumerator Reload()
    {
        print("RELOAD");
        mainWeaponReloading = true;
        yield return new WaitForSeconds(mainWeapon.reloadTime);
        mainWeaponAmmunition = mainWeapon.ammunitionMax;
        mainWeaponReloading = false;
    }
}
