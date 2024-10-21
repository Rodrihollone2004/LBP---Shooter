using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTarget : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float minX = -7f;
    [SerializeField] private float maxX = 7f;

    [Header("Respawn")]
    [SerializeField] private Vector2 respawnRangeX = new Vector2(-7f, 7f);
    [SerializeField] private Vector2 respawnRangeY = new Vector2(1.5f, 3f);
    [SerializeField] private float respawnZ = -10f;

    private object movement;

    private void Start()
    {
        movement = MovementFactory.CreateMovement("Normal", minX, maxX);
    }

    private void Update()
    {
        MoveTarget();

        if (Input.GetKeyDown(KeyCode.K))
        {
            ChangeMovementType("Fast"); 
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            ChangeMovementType("Slow");
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            ChangeMovementType("Normal"); 
        }
    }

    private void MoveTarget()
    {
        if (movement is NormalMovement normalMovement)
        {
            normalMovement.Move(transform, moveSpeed);
        }
        else if (movement is FastMovement fastMovement)
        {
            fastMovement.Move(transform, moveSpeed);
        }
        else if (movement is SlowMovement slowMovement)
        {
            slowMovement.Move(transform, moveSpeed);
        }
    }

    public void HitTarget()
    {
        gameObject.SetActive(false);
        Invoke("Respawn", 0.5f);
    }

    private void Respawn()
    {
        float randomX = Random.Range(respawnRangeX.x, respawnRangeX.y);
        float randomY = Random.Range(respawnRangeY.x, respawnRangeY.y);
        transform.position = new Vector3(randomX, randomY, respawnZ);
        gameObject.SetActive(true);
    }

    public void OnHitByRaycast()
    {
        HitTarget();
    }
    public void ChangeMovementType(string type)
    {
        movement = MovementFactory.CreateMovement(type, minX, maxX);
    }
}
