using TMPro;
using UnityEngine;

public class TriggerTextUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI uiText;
    [SerializeField] private string message = " ";
    [SerializeField] private float displayDuration = 5f;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            ShowMessage();
        }
    }

    private void ShowMessage()
    {
        if (uiText != null)
        {
            uiText.text = message;
            uiText.gameObject.SetActive(true);
            Invoke(nameof(HideMessage), displayDuration);
        }
    }

    private void HideMessage()
    {
        if (uiText != null)
        {
            uiText.gameObject.SetActive(false);
        }
    }
}
