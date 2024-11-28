using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMoveObject : MonoBehaviour
{
    [SerializeField] private GameObject objectToMove;
    [SerializeField] private float moveDistance = 2f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int damage = 60;
    private bool isMoving = false;
    private bool damageApplied = false;
    private Vector3 targetPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!damageApplied)
            {
                PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                }
                damageApplied = true;
            }

            if (!isMoving) 
            {
                isMoving = true;
                targetPosition = objectToMove.transform.position + new Vector3(0, 0, moveDistance);
                StartCoroutine(MoveObject());
            }
        }
    }

    private IEnumerator MoveObject()
    {
        while (Vector3.Distance(objectToMove.transform.position, targetPosition) > 0.01f)
        {
            objectToMove.transform.position = Vector3.MoveTowards(
                objectToMove.transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        objectToMove.transform.position = targetPosition;
    }
}
