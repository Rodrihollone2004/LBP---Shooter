using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMovement
{
    private float minX;
    private float maxX;
    private bool movingRight = true;

    public NormalMovement(float minX, float maxX)
    {
        this.minX = minX;
        this.maxX = maxX;
    }

    public void Move(Transform target, float speed)
    {
        if (movingRight)
        {
            target.Translate(Vector3.right * speed * Time.deltaTime);
            if (target.position.x >= maxX)
                movingRight = false;
        }
        else
        {
            target.Translate(Vector3.left * speed * Time.deltaTime);
            if (target.position.x <= minX)
                movingRight = true;
        }
    }
}
