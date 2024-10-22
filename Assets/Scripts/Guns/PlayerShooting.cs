using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour
{
    WeaponAK weaponAK;
    WeaponPistol weaponPistol;
    WeaponKnife weaponKnife; 

    IWeapon weapon;

    private void Awake()
    {
        weaponPistol = FindObjectOfType<WeaponPistol>();
        weaponAK = FindObjectOfType<WeaponAK>();
        weaponKnife = FindObjectOfType<WeaponKnife>(); 
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
        else if (Input.GetKeyDown("3")) 
        {
            SetWeapon(weaponKnife); 
            weapon.UpdateBulletsCount();
        }
    }

    public void SetWeapon(IWeapon weapon)
    {
        this.weapon = weapon;
    }
}
