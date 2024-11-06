using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Image pistolIcon;
    [SerializeField] private Image akIcon;
    [SerializeField] private Image knifeIcon;    
    
    [SerializeField] private GameObject pistolObject;
    [SerializeField] private GameObject akObject;
    [SerializeField] private GameObject knifeObject;

    WeaponAK weaponAK;
    WeaponPistol weaponPistol;
    WeaponKnife weaponKnife; 

    IWeapon weapon;
    private Camera mainCamera;
    private float normalFieldOfView = 60f; 
    private float zoomedFieldOfView = 50f; 
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
        weapon = weaponAK;

        weapon.UpdateBulletsCount();

        UpdateWeaponIcon();
    }

    private void Update()
    {
        InputWeapons();


        if (ReferenceEquals(weapon, weaponPistol) || ReferenceEquals(weapon, weaponAK))
        {
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
            SetWeapon(weaponAK);
            weapon.UpdateBulletsCount();
        }
        else if (Input.GetKeyDown("2"))
        {
            SetWeapon(weaponPistol);
            weapon.UpdateBulletsCount();
        }
        else if (Input.GetKeyDown("3")) 
        {
            SetWeapon(weaponKnife); 
            weapon.UpdateBulletsCount();

            if (isZoomed)
            {
                ToggleZoom(false);
            }
        }
    }


    private void SetWeapon(IWeapon newWeapon)
    {
        weapon = newWeapon;
        weapon.UpdateBulletsCount();

        akObject.SetActive(ReferenceEquals(weapon, weaponAK));
        pistolObject.SetActive(ReferenceEquals(weapon, weaponPistol));
        knifeObject.SetActive(ReferenceEquals(weapon, weaponKnife));

        UpdateWeaponIcon();
    }

    private void UpdateWeaponIcon()
    {
        pistolIcon.color = Color.gray;
        akIcon.color = Color.gray;
        knifeIcon.color = Color.gray;

        if (ReferenceEquals(weapon, weaponPistol))
        {
            pistolIcon.color = Color.white;
        }
        else if (ReferenceEquals(weapon, weaponAK))
        {
            akIcon.color = Color.white;
        }
        else if (ReferenceEquals(weapon, weaponKnife))
        {
            knifeIcon.color = Color.white;
        }
    }
}
