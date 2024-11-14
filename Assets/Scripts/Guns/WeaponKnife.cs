using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.VFX;

public class WeaponKnife : MonoBehaviour, IWeapon
{
    [SerializeField] private TextMeshProUGUI knifeCountText;
    [SerializeField] private int damageKnife = 50;
    [SerializeField] private float atackCooldown = 0.5f;
    [Header("Sound")]
    [SerializeField] private AudioClip slashSound;
    [Header("Hole")]
    [SerializeField] private LayerMask impactLayers;
    [SerializeField] private GameObject impactPrefab;
    [Header("Particles")]
    [SerializeField] private VisualEffect shootVisualEffect;
    [SerializeField] private VisualEffect impactVisualEffect;
    private AudioSource audioSource;

    private Camera mainCamera;
    private float nextAtackTime = 0f;

    private void Awake()
    {
        mainCamera = Camera.main;
        audioSource = GetComponent<AudioSource>();
    }

    public void WeaponController()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextAtackTime)
        {
            Shoot();
            nextAtackTime = Time.time + atackCooldown;
        }
    }

    public void Shoot()
    {
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2f))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                IDamageable damageable = hit.collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.DealDamage(damageKnife, hit.point);
                }
            }

            if ((impactLayers.value & (1 << hit.collider.gameObject.layer)) > 0)
            {
                impactVisualEffect.transform.position = hit.point + hit.normal * 0.01f;
                impactVisualEffect.transform.rotation = Quaternion.LookRotation(hit.normal);
                impactVisualEffect.Play();

                shootVisualEffect.Play();
                Vector3 impactPosition = hit.point + hit.normal * 0.01f;
                GameObject impactEffect = Instantiate(impactPrefab, impactPosition, Quaternion.LookRotation(hit.normal));
                impactEffect.transform.localScale = new Vector3(0.035f, 0.035f, 0.035f);
                StartCoroutine(DestroyImpactAfterDelay(impactEffect, 1f));
            }
        }

        audioSource.PlayOneShot(slashSound);
    }

    private IEnumerator DestroyImpactAfterDelay(GameObject impactEffect, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(impactEffect);
    }

    public void UpdateBulletsCount()
    {
        knifeCountText.text = " ";
    }
}
