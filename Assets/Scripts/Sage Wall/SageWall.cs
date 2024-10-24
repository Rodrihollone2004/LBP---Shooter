using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SageWall : MonoBehaviour
{
    [SerializeField] private Camera camWall;
    [SerializeField] private float detectRange = 20f;

    [Space]
    [Header("Wall Buliding Parameters")]
    [SerializeField] private GameObject wallMarker;

    [SerializeField] private int wallCubeAmount;
    [SerializeField] private GameObject wallCube;
    [SerializeField] private float wallCubeDistance;

    [SerializeField] private float wallDuration;
    [SerializeField] private float updateRate;

    private Vector3 destination;
    private Quaternion rotation;
    private bool isInRange;
    private bool isWallMarkerActive;
    private bool isWallBuliding;

    private void Start()
    {
        wallMarker.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isWallMarkerActive = true;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            isWallMarkerActive = false;
        }

        if (isWallMarkerActive)
        {
            UpdateWallMarker();
        }
        else
        {
            wallMarker.SetActive(false);
        }

        if (Input.GetButtonDown("Fire1") && isInRange && !isWallBuliding)
        {
            BuildWall();
        }
    }

    private void UpdateWallMarker()
    {
        Ray ray = camWall.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, detectRange))
        {
            isInRange = true;
            destination = hit.point;
            rotation = Quaternion.LookRotation(ray.direction);
        }
        else
        {
            isInRange = false;
        }

        if (isInRange)
        {
            wallMarker.transform.position = destination;
            rotation = new Quaternion(wallMarker.transform.rotation.x, rotation.y, wallMarker.transform.rotation.z, wallMarker.transform.rotation.w);
            wallMarker.transform.rotation = rotation;
            wallMarker.SetActive(true);
        }
        else
        {
            wallMarker.SetActive(false);
        }
    }

    private void BuildWall()
    {
        isWallBuliding = true;
        isWallMarkerActive = false;
        GameObject wall = new GameObject();
        wall.transform.position = destination;
        wall.name = "Wall";

        for (int i = 0; i < wallCubeAmount; i++)
        {
            GameObject cube = Instantiate(wallCube, destination + new Vector3(i * wallCubeDistance, 0, 0), Quaternion.Euler(-90, 0, 0)) as GameObject;
            cube.transform.SetParent(wall.transform);
        }

        wall.transform.rotation = rotation;
        wall.transform.Translate(new Vector3(-(int)(wallCubeAmount / 2) * wallCubeDistance, 0, 0), Space.Self);

        isWallBuliding = false;

        StartCoroutine(DestroyWall(wall));
    }

    private IEnumerator DestroyWall(GameObject wallToDestroy)
    {
        float duration = wallDuration;

        while(duration > 0)
        {
            duration -= updateRate;

            yield return new WaitForSeconds(updateRate);

            if(duration <= 0)
            {
                Destroy(wallToDestroy);
            }
        }
    }
}
