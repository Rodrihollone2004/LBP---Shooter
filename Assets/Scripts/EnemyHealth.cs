using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth2 : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private Slider healthbarSlider;
    [SerializeField] private Image healthbarFillImage;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private bool isImmortal = false;
    [SerializeField] private EnemyData enemyData;
    public static int contadorGeneral = 0;
    private AudioSource audioSource;

    private float currentHealth;
    private Vector3 originalPosition;

    private void Start()
    {
        currentHealth = maxHealth;
        SetHealthbarUi();
        audioSource = GetComponent<AudioSource>();
        originalPosition = transform.position;
    }

    public void DealDamage(int damage, Vector3 originPosition)
    {
        ShowDamageEffects(damage);

        if (isImmortal) return;

        currentHealth -= damage;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        SetHealthbarUi();

        CheckIfDead();
    }

    private void ShowDamageEffects(int damage)
    {
        if (damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);
        }

        Instantiate(enemyData.damageTextPrefab, transform.position, Quaternion.identity).GetComponent<DamageText>().Initialise(damage);
    }

    private void CheckIfDead()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            contadorGeneral++;
        }
    }

    private void SetHealthbarUi()
    {
        float healthPercentage = CalculateHealthPercentage();
        healthbarSlider.value = healthPercentage;
        healthbarFillImage.color = Color.Lerp(enemyData.zeroHealthColor, enemyData.maxHealthColor, healthPercentage / 100f);
    }

    private float CalculateHealthPercentage()
    {
        return (currentHealth / maxHealth) * 100;
    }
}
