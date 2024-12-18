using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TriggerTextUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI uiText;
    [SerializeField] private Image uiImage;
    [SerializeField] private string message = " ";
    [SerializeField] private float displayDuration = 5f;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;

            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.AllowMovement(false);
            }

            ShowMessage();
        }
    }

    private void ShowMessage()
    {
        if (uiText != null)
        {
            uiText.text = message;
            uiText.gameObject.SetActive(true);
        }

        if (uiImage != null)
        {
            uiImage.enabled = true;
        }
        Invoke(nameof(HideMessage), displayDuration);
    }

    private void HideMessage()
    {
        if (uiText != null)
        {
            uiText.gameObject.SetActive(false);
        }

        if (uiImage != null)
        {
            uiImage.enabled = false;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.AllowMovement(true); 
            }
        }
    }
}
