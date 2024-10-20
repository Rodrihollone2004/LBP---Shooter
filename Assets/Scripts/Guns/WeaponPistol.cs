using UnityEngine;
using TMPro;
using System.Collections;

public class WeaponPistol : MonoBehaviour, IWeapon
{
    [SerializeField] private TextMeshProUGUI bulletPistolCountText;
    [SerializeField] private int damagePistol = 20;
    [SerializeField] private AudioClip shootSoundPistol;
    [SerializeField] private AudioClip reloadSoundPistol;
    private AudioSource audioSource;

    private int bulletPistolCount;
    private bool canShoot = true;
    private Camera mainCamera;

    private void Awake()
    {
        bulletPistolCount = 20;
        mainCamera = Camera.main;
        audioSource = GetComponent<AudioSource>();
    }

    public void WeaponController()
    {
        if (canShoot && Input.GetButtonDown("Fire1"))
        {
            if (bulletPistolCount > 0)
            {
                Shoot();
                bulletPistolCount--;
                UpdateBulletsCount();

                if (bulletPistolCount <= 0)
                {
                    canShoot = false;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            audioSource.PlayOneShot(reloadSoundPistol);

            StartCoroutine(ReloadGun());
        }
    }

    public IEnumerator ReloadGun()
    {
        canShoot = false;
        yield return new WaitForSeconds(2.4f);

        bulletPistolCount = 20;
        canShoot = true;
        UpdateBulletsCount();
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
                    damageable.DealDamage(damagePistol, hit.point);
                }
            }
        }
        audioSource.PlayOneShot(shootSoundPistol);
    }

    public void UpdateBulletsCount()
    {
        bulletPistolCountText.text = "Bullets Pistol: " + bulletPistolCount;
    }
}
