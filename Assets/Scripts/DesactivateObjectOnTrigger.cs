using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesactivateObjectOnTrigger : MonoBehaviour
{
    [SerializeField] private GameObject objectToDeactivate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (objectToDeactivate != null)
            {
                objectToDeactivate.SetActive(false);
            }
        }
    }
}
