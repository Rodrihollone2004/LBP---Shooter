﻿using UnityEngine;
using TMPro;
using System.Collections;

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
            bulletPistolCount = 20;
            canShoot = true;
            UpdateBulletsCount();

            audioSource.PlayOneShot(reloadSoundPistol);
        }
    }

    public void Shoot()
    {
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if ((impactLayers.value & (1 << hit.collider.gameObject.layer)) > 0)
            {
                
                Vector3 impactPosition = hit.point + hit.normal * 0.01f; 
                GameObject impactEffect = Instantiate(impactPrefab, impactPosition, Quaternion.LookRotation(hit.normal));
                impactEffect.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                StartCoroutine(DestroyImpactAfterDelay(impactEffect, 1f));
            }

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


    private IEnumerator DestroyImpactAfterDelay(GameObject impactEffect, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(impactEffect);
    }

    public void UpdateBulletsCount()
    {
        bulletPistolCountText.text = "Bullets Pistol: " + bulletPistolCount;
    }
}
