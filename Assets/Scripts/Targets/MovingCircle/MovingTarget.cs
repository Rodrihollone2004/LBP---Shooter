using UnityEngine;

public class MovingTarget : MonoBehaviour
{
    [Header("Movement")]
    private int slowSpeed = 3;
    private int normalSpeed = 7;
    private int fastSpeed = 11;
    private bool isMovingRight;
    [SerializeField] private float minX = -7f;
    [SerializeField] private float maxX = 7f;
    [SerializeField] private bool canMove = true;

    [Header("Controladores")]
    [SerializeField] private TargetData targetData;
    [SerializeField] private ActivateTraining activateTraining;

    [Header("Respawn")]
    [SerializeField] private float respawnX = 63f;
    [SerializeField] private Vector2 respawnRangeY = new Vector2(1.5f, 3f);
    [SerializeField] private Vector2 respawnRangeZ = new Vector2(22f, 36f);
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
        if (isMovingRight)
        {
            transform.Translate(Vector3.forward * (targetData.speed) * Time.deltaTime);
            if (transform.position.z >= maxX)
                isMovingRight = false;
        }
        else
        {
            transform.Translate(Vector3.back * (targetData.speed) * Time.deltaTime);
            if (transform.position.z <= minX)
                isMovingRight = true;
        }
    }

    public void HitTarget()
    {
        gameObject.SetActive(false);
        Invoke("Respawn", 0.5f);

        targetData.life = startLife;
        activateTraining.CurrentTargets += 1;
    }

    private void Respawn()
    {
        float randomZ = Random.Range(respawnRangeZ.x, respawnRangeZ.y);
        float randomY = Random.Range(respawnRangeY.x, respawnRangeY.y);
        transform.position = new Vector3(respawnX, randomY, randomZ);
        gameObject.SetActive(true);
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
