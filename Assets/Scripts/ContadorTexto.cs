using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContadorTexto : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI uiText;
    [SerializeField] private Image uiImage;
    [SerializeField] private string message = " ";
    [SerializeField] private float displayDuration = 5f;
    void Update()
    {
        if (EnemyHealth2.contadorGeneral == 2)
        {
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

        EnemyHealth2.contadorGeneral = 0;
    }
}
