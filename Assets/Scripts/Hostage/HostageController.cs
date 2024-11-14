using UnityEngine;

public class HostageController : MonoBehaviour
{
    Transform playerBehind;
    [SerializeField] float speed;
    bool isCaught;

    private void Start()
    {
        playerBehind = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        Inputs();

        if (isCaught)
        {
            FollowPlayer();
        }
        else
        {
            StopFollow();
        }
    }

    private void FollowPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerBehind.position, speed * Time.deltaTime);
    }

    private void StopFollow()
    {
        Debug.Log("Basta");
    }

    private void Inputs()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4) && !isCaught)
        {
            isCaught = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && isCaught)
        {
            isCaught = false;
        }
    }
}
