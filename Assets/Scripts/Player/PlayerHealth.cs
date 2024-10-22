using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealth: MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [SerializeField] private TMP_Text healthText;

    void Start()
    {
        currentHealth = maxHealth;  
        UpdateHealthUI();
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Vida actual: " + currentHealth);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // metodo por si queremos curar al jugador.
    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Vida actual después de curarse: " + currentHealth);
        UpdateHealthUI();
    }

    private void Die()
    {
        Debug.Log("Jugador ha muerto");
        Invoke("RestartLevel", 2f);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = currentHealth.ToString();
        }
    }
}
