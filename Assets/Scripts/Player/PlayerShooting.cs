using UnityEngine;
using TMPro;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bulletCountText;
    [SerializeField] private int damage = 10;
    private bool canShoot = true;
    private int bulletCount;

    private Camera mainCamera;

    private void Awake()
    {
        bulletCount = 20;
        UpdateBulletCountText();

        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (canShoot && Input.GetButtonDown("Fire1"))
        {
            if (bulletCount > 0)
            {
                Shoot(); 
                bulletCount--;
                UpdateBulletCountText();

                if (bulletCount <= 0)
                {
                    canShoot = false;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            bulletCount = 20;
            canShoot = true;
            UpdateBulletCountText();
        }

    }

    private void Shoot()
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
                    damageable.DealDamage(damage, hit.point);
                }
            }
        }
    }

    private void UpdateBulletCountText()
    {
        bulletCountText.text = "Bullets: " + bulletCount;
    }
}
