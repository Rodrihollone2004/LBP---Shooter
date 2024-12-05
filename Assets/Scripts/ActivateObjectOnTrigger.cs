using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObjectOnTrigger: MonoBehaviour
{
    [SerializeField] private GameObject objectToActivate; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (objectToActivate != null)
            {
                objectToActivate.SetActive(true);
            }
        }
    }
}
