using System.Collections;
using UnityEngine;

public class HealthOrb : MonoBehaviour
{
    [SerializeField] private int healAmount = 20;
    private OrbManager orbManager;

    private void Start()
    {
        orbManager = FindObjectOfType<OrbManager>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);
                gameObject.SetActive(false); 
                orbManager.StartCoroutine(orbManager.RespawnOrb(gameObject, 2f));
            }
        }
    }
}
