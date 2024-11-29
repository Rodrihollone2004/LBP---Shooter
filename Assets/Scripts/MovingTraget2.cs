using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTraget2 : MonoBehaviour
{
    [Header("Movement")]
    private int slowSpeed = 3;
    private int normalSpeed = 5;
    private int fastSpeed = 8;
    private bool isMovingUp;
    [SerializeField] private float minY = 1.5f;
    [SerializeField] private float maxY = 3f;
    [SerializeField] private bool canMove = true;

    [Header("Controladores")]
    [SerializeField] private TargetData targetData;

    [Header("Vida")]
    [SerializeField] private int startLife;

    private void Start()
    {
        targetData.speed = normalSpeed;
        startLife = targetData.life;
    }

    private void Update()
    {
        if (canMove)
        {
            Move();

            if (Input.GetKeyDown(KeyCode.J))
            {
                targetData.speed = slowSpeed;
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                targetData.speed = normalSpeed;
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                targetData.speed = fastSpeed;
            }
        }
    }

    public void Move()
    {
        if (isMovingUp)
        {
            transform.Translate(Vector3.up * targetData.speed * Time.deltaTime);
            if (transform.position.y >= maxY)
                isMovingUp = false;
        }
        else
        {
            transform.Translate(Vector3.down * targetData.speed * Time.deltaTime);
            if (transform.position.y <= minY)
                isMovingUp = true;
        }
    }

    public void HitTarget()
    {
        gameObject.SetActive(false);

        targetData.life = startLife;
    }

    public void OnHitByRaycast()
    {
        targetData.life -= 1;

        if (targetData.life <= 0)
        {
            HitTarget();
        }
    }
    public void SetCanMove(bool value)
    {
        canMove = value;
    }
}

