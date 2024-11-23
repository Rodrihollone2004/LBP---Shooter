using Unity.VisualScripting;
using UnityEngine;

public class RopeClimbing : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private LayerMask whatIsRope;

    [Header("Climbing")]
    [SerializeField] private float climbSpeed, maxClimbTime;
    private float climbTimer;

    [Header("Rope Limits")]
    [SerializeField] private Transform ropeTop;
    [SerializeField] private bool isClimbing;
    [SerializeField] private bool isOnRope;
    [SerializeField] private bool isOnTop;


    private void Update()
    {
        RopeCheck();
        StateMachine();

        if (isClimbing) HandleRopeMovement();
    }

    private void RopeCheck()
    {
        isOnRope = Physics.CheckSphere(transform.position, 0.7f, whatIsRope);

        if (isOnRope)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 1f, whatIsRope);
            if (colliders.Length > 0)
            {
                Transform rope = colliders[0].transform;

                ropeTop = rope.Find("Top");
            }
        }

        if (playerController.IsGrounded())
        {
            climbTimer = maxClimbTime;
            isClimbing = false;
        }
    }

    private void StateMachine()
    {
        if (Input.GetKeyDown(KeyCode.F) && isClimbing)
        {
            isClimbing = false;
            StopClimbing();
        }

        if (isOnRope && Input.GetKeyDown(KeyCode.F))
        {
            if (playerController.IsGrounded())
            {
                rb.position += Vector3.up * 2f;
                isOnTop = false;
                isClimbing = true;
                rb.useGravity = false;
                rb.velocity = Vector3.zero;
                rb.isKinematic = true;
                playerController.Gravity = 0f;
            }
            else
            {
                isOnTop = false;
                isClimbing = true;
                rb.useGravity = false;
                rb.velocity = Vector3.zero;
                rb.isKinematic = true;
                playerController.Gravity = 0f;
            }
        }

        if (playerController.IsGrounded())
        {
            StopClimbing();
        }

        if (transform.position.y >= ropeTop.position.y && !playerController.IsDashing)
        {
            isOnTop = true;
            StopClimbing();

            if (isOnTop == true)
                rb.AddForce(Vector3.up * 3f, ForceMode.Impulse);
        }
    }

    private void HandleRopeMovement()
    {
        if (isClimbing)
        {
            Vector3 moveDirection = Vector3.zero;

            if (Input.GetKey(KeyCode.W))
            {
                moveDirection = Vector3.up * climbSpeed;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                moveDirection = Vector3.down * climbSpeed;
            }

            transform.position += moveDirection * Time.deltaTime;
        }
    }

    private void StopClimbing()
    {
        isOnTop = false;
        isClimbing = false;
        isOnRope = false;
        rb.useGravity = true;
        rb.isKinematic = false;

        playerController.Gravity = -9.81f;

        if (transform.position.y >= ropeTop.position.y)
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.7f);
    }
}