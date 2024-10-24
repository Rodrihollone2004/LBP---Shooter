using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [SerializeField] private int damageAmount = 10;
    private bool playerInZone = false;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && !playerInZone)
        {
            playerInZone = true;
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInZone = false;
        }
    }
}
