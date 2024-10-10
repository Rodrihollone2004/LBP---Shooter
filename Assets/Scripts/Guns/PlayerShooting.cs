using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour
{
    WeaponAK weaponAK;
    WeaponPistol weaponPistol;

    IWeapon weapon;

    private void Awake()
    {
        weaponPistol = FindObjectOfType<WeaponPistol>();
        weaponAK = FindObjectOfType<WeaponAK>();
    }

    private void Start()
    {
        weapon = weaponPistol;

        weapon.UpdateBulletsCount();
    }

    private void Update()
    {
        InputWeapons();

        weapon.WeaponController();
    }

    private void InputWeapons()
    {
        if (Input.GetKeyDown("1"))
        {
            SetWeapon(weaponPistol);
            weapon.UpdateBulletsCount();
        }
        else if (Input.GetKeyDown("2"))
        {
            SetWeapon(weaponAK);
            weapon.UpdateBulletsCount();
        }
    }

    public void SetWeapon(IWeapon weapon)
    {
        this.weapon = weapon;
    }
}
