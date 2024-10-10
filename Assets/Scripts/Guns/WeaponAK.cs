using UnityEngine;
using TMPro;

public class WeaponAK : MonoBehaviour, IWeapon
{
    [SerializeField] private TextMeshProUGUI bulletAkCountText;
    [SerializeField] private int damageAk = 5;
    [SerializeField] private float shootCooldown = 0.1f;
    private int bulletAkCount;

    private bool canShoot = true;
    private float nextShootTime = 0f;

    private Camera mainCamera;

    private void Awake()
    {
        bulletAkCount = 20;

        mainCamera = Camera.main;
    }

    public void WeaponController()
    {
        if (canShoot && Input.GetButton("Fire1") && Time.time >= nextShootTime)
        {
            if (bulletAkCount > 0)
            {
                Shoot();
                bulletAkCount--;
                UpdateBulletsCount();

                nextShootTime = Time.time + shootCooldown;

                if (bulletAkCount <= 0)
                {
                    canShoot = false;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            bulletAkCount = 20;
            canShoot = true;
            UpdateBulletsCount();
        }
    }


    public void Shoot()
    {
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                IDamageable damageable = hit.collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.DealDamage(damageAk, hit.point);
                }
            }
        }
    }

    public void UpdateBulletsCount()
    {
        bulletAkCountText.text = "Bullets Ak: " + bulletAkCount;
    }
}
