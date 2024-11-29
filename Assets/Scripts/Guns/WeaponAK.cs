using UnityEngine;
using UnityEngine.VFX;
using TMPro;
using System.Collections;

public class WeaponAK : MonoBehaviour, IWeapon
{
    [SerializeField] private TextMeshProUGUI bulletAkCountText;
    [SerializeField] private int damageAk = 40;
    [SerializeField] private float shootCooldown = 0.1f;
    [Header("Sounds")]
    [SerializeField] private AudioClip shootSoundAk;
    [SerializeField] private AudioClip reloadSoundAk;
    [Header("ShootHole")]
    [SerializeField] private LayerMask impactLayers;
    [SerializeField] private GameObject impactPrefab;
    [Header("Particles")]
    [SerializeField] private VisualEffect shootVisualEffect;
    [SerializeField] private VisualEffect impactVisualEffect;
    private AudioSource audioSource;

    private int bulletAkCount;
    private bool canShoot = true;
    private float nextShootTime = 0f;

    private Camera mainCamera;

    private void Awake()
    {
        bulletAkCount = 30;
        mainCamera = Camera.main;
        audioSource = GetComponent<AudioSource>();
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
            audioSource.PlayOneShot(reloadSoundAk);
            StartCoroutine(ReloadGun());
        }
    }

    public IEnumerator ReloadGun()
    {
        canShoot = false;
        yield return new WaitForSeconds(2.4f);

        bulletAkCount = 30;
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
                    damageable.DealDamage(damageAk, hit.point);
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

        audioSource.PlayOneShot(shootSoundAk);
        shootVisualEffect.Play();
    }

    private IEnumerator DestroyImpactAfterDelay(GameObject impactEffect, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(impactEffect);
    }

    public void UpdateBulletsCount()
    {
        bulletAkCountText.text = $"{bulletAkCount.ToString()} <size=50%>30</size>";
    }
}
