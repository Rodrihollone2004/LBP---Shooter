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
    [SerializeField] private TargetSpeed targetSpeed;

    [Header("Respawn")]
    [SerializeField] private Vector2 respawnRangeX = new Vector2(-7f, 7f);
    [SerializeField] private Vector2 respawnRangeY = new Vector2(1.5f, 3f);
    [SerializeField] private float respawnZ = -10f;
    private int startLife;

    private void Start()
    {
        targetSpeed.speed = normalSpeed;
        startLife = targetSpeed.life;
    }

    private void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.J))
        {
            targetSpeed.speed = slowSpeed;
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            targetSpeed.speed = normalSpeed;
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            targetSpeed.speed = fastSpeed;
        }
    }

    public void Move()
    {
        if (isMovingRight)
        {
            transform.Translate(Vector3.right * (targetSpeed.speed) * Time.deltaTime);
            if (transform.position.x >= maxX)
                isMovingRight = false;
        }
        else
        {
            transform.Translate(Vector3.left * (targetSpeed.speed) * Time.deltaTime);
            if (transform.position.x <= minX)
                isMovingRight = true;
        }
    }

    public void HitTarget()
    {
        gameObject.SetActive(false);
        Invoke("Respawn", 0.5f);
        targetSpeed.life = startLife;
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
        targetSpeed.life -= 1;

        if (targetSpeed.life <= 0)
        {
            HitTarget();
        }
    }
}
