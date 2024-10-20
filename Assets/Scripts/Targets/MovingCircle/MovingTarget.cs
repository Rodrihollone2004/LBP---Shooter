using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTarget : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float minX = -7f;
    [SerializeField] private float maxX = 7f;

    [Header("Respawn")]
    [SerializeField] private Vector2 respawnRangeX = new Vector2(-7f, 7f); 
    [SerializeField] private Vector2 respawnRangeY = new Vector2(1.5f, 3f);  
    [SerializeField] private float respawnZ = -10f; 

    private Vector3 initialPosition;
    private bool movingRight = true;

    private void Start()
    {
        initialPosition = transform.position; 
    }

    private void Update()
    {
        MoveTarget(); 
    }
    private void MoveTarget()
    {
        if (movingRight)
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);  

            if (transform.position.x >= maxX)
                movingRight = false; 
        }
        else
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

            if (transform.position.x <= minX)
                movingRight = true; 
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
}
