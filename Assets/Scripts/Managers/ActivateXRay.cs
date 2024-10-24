using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateXRay: MonoBehaviour
{
    [SerializeField] LayerMask defaultLayer;
    [SerializeField] LayerMask xRayLayer;

    private bool xRayActive;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (xRayActive)
            {
                xRayActive = !xRayActive;
                int layerNum = (int)Mathf.Log(xRayActive ? xRayLayer.value : defaultLayer.value, 2);
                gameObject.layer = layerNum;

                if (transform.childCount > 0)
                    SetLayerAllChildren(transform, layerNum);
            }
            else
            {
                xRayActive = !xRayActive;
                int layerNum = (int)Mathf.Log(xRayActive ? xRayLayer.value : defaultLayer.value, 2);
                gameObject.layer = layerNum;

                if (transform.childCount > 0)
                    SetLayerAllChildren(transform, layerNum);
            }
        }
    }

    void SetLayerAllChildren(Transform root, int layer)
    {
        var children = root.GetComponentsInChildren<Transform>(includeInactive: true);

        foreach (var child in children)
        {
            child.gameObject.layer = layer;
        }
    }
}
