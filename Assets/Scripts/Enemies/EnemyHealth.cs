using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private Slider healthbarSlider;
    [SerializeField] private Image healthbarFillImage;
    [SerializeField] private Color maxHealthColor = Color.green;
    [SerializeField] private Color zeroHealthColor = Color.red;
    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private AudioClip damageSound;
    private AudioSource audioSource;

    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        SetHealthbarUi();
        audioSource = GetComponent<AudioSource>();
    }

    public void DealDamage(int damage, Vector3 originPosition)
    {
        currentHealth -= damage;

        if (damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);
        }

        Instantiate(damageTextPrefab, transform.position, Quaternion.identity).GetComponent<DamageText>().Initialise(damage);

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        SetHealthbarUi();

        CheckIfDead();
    }

    private void CheckIfDead()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void SetHealthbarUi()
    {
        float healthPercentage = CalculateHealthPercentage();
        healthbarSlider.value = healthPercentage;
        healthbarFillImage.color = Color.Lerp(zeroHealthColor, maxHealthColor, healthPercentage / 100f);
    }

    private float CalculateHealthPercentage()
    {
        return (currentHealth / maxHealth) * 100;
    }
}
