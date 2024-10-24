using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour
{
    WeaponAK weaponAK;
    WeaponPistol weaponPistol;
    WeaponKnife weaponKnife; 

    IWeapon weapon;

    private Camera mainCamera;
    private float normalFieldOfView = 60f; 
    private float zoomedFieldOfView = 30f; 
    private bool isZoomed = false; 
    private Coroutine zoomCoroutine; 

    private void Awake()
    {
        weaponPistol = FindObjectOfType<WeaponPistol>();
        weaponAK = FindObjectOfType<WeaponAK>();
        weaponKnife = FindObjectOfType<WeaponKnife>();

        mainCamera = Camera.main;
    }

    private void Start()
    {
        weapon = weaponPistol;

        weapon.UpdateBulletsCount();
    }

    private void Update()
    {
        InputWeapons();


        if (Input.GetMouseButton(1))
        {
            if (!isZoomed)
            {
                ToggleZoom(true); 
            }
        }
        else
        {
            if (isZoomed)
            {
                ToggleZoom(false); 
            }
        }

        weapon.WeaponController();
    }

    private void ToggleZoom(bool zoomIn)
    {
        isZoomed = zoomIn; 
        if (zoomCoroutine != null)
        {
            StopCoroutine(zoomCoroutine);
        }
        zoomCoroutine = StartCoroutine(ZoomCoroutine(zoomIn));
    }

    private IEnumerator ZoomCoroutine(bool zoomIn)
    {
        float targetFOV = zoomIn ? zoomedFieldOfView : normalFieldOfView;
        float startFOV = mainCamera.fieldOfView;
        float elapsed = 0f;

        while (elapsed < 0.2f) 
        {
            mainCamera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, elapsed / 0.2f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.fieldOfView = targetFOV;
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
