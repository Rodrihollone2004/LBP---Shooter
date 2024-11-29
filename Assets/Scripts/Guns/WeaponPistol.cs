using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.VFX;

public class WeaponPistol : MonoBehaviour, IWeapon
{
    [SerializeField] private TextMeshProUGUI bulletPistolCountText;
    [SerializeField] private int damagePistol = 20;
    [Header("Sounds")]
    [SerializeField] private AudioClip shootSoundPistol;
    [SerializeField] private AudioClip reloadSoundPistol;
    [Header("ShootHole")]
    [SerializeField] private LayerMask impactLayers;
    [SerializeField] private GameObject impactPrefab;
    [Header("Particles")]
    [SerializeField] private VisualEffect shootVisualEffect;
    [SerializeField] private VisualEffect impactVisualEffect;
    private AudioSource audioSource;

    private int bulletPistolCount;
    private bool canShoot = true;
    private Camera mainCamera;

    private void Awake()
    {
        bulletPistolCount = 12;
        mainCamera = Camera.main;
        audioSource = GetComponent<AudioSource>();
    }

    public void WeaponController()
    {
        Debug.DrawRay(mainCamera.transform.position, mainCamera.transform.forward * 200, Color.red);
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

        bulletPistolCount = 12;
        canShoot = true;
        UpdateBulletsCount();
    }

    public void Shoot()
    {
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        int layerMask = ~(1 << LayerMask.NameToLayer("NoHit"));

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider.CompareTag("Target"))  
            {
                ShootingRange shootingRange = FindObjectOfType<ShootingRange>();
                if (shootingRange != null)
                {
                    shootingRange.HitTarget(hit.collider.gameObject);  
                }
            }

            if (hit.collider.CompareTag("Enemy"))
            {
                IDamageable damageable = hit.collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.DealDamage(damagePistol, hit.point);

                    impactVisualEffect.transform.position = hit.point + hit.normal * 0.01f;
                    impactVisualEffect.transform.rotation = Quaternion.LookRotation(hit.normal);
                    impactVisualEffect.Play();
                }
            }

            if ((impactLayers.value & (1 << hit.collider.gameObject.layer)) > 0)
            {
                impactVisualEffect.transform.position = hit.point + hit.normal * 0.01f;
                impactVisualEffect.transform.rotation = Quaternion.LookRotation(hit.normal);
                impactVisualEffect.Play();

                Vector3 impactPosition = hit.point + hit.normal * 0.01f;
                GameObject impactEffect = Instantiate(impactPrefab, impactPosition, Quaternion.LookRotation(hit.normal));
                impactEffect.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                StartCoroutine(DestroyImpactAfterDelay(impactEffect, 1f));
            }

            MovingTarget target = hit.transform.GetComponent<MovingTarget>();

            if (target != null)
            {
                target.OnHitByRaycast();  
            }
            MovingTraget2 target2 = hit.transform.GetComponent<MovingTraget2>();

            if (target2 != null)
            {
                target2.OnHitByRaycast();
            }
        }
        audioSource.PlayOneShot(shootSoundPistol);
        shootVisualEffect.Play();
    }



    private IEnumerator DestroyImpactAfterDelay(GameObject impactEffect, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(impactEffect);
    }

    public void UpdateBulletsCount()
    {
        bulletPistolCountText.text = $"{bulletPistolCount.ToString()} <size=50%>12</size>";
    }
}
