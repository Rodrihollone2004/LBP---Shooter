using UnityEngine;

public class SlowMovement
{
    private float minX;
    private float maxX;
    private bool movingRight = true;

    public SlowMovement(float minX, float maxX)
    {
        this.minX = minX;
        this.maxX = maxX;
    }

    public void Move(Transform target, float speed)
    {
        if (movingRight)
        {
            target.Translate(Vector3.right * (speed * 0.5f) * Time.deltaTime);
            if (target.position.x >= maxX)
                movingRight = false;
        }
        else
        {
            target.Translate(Vector3.left * (speed * 0.5f) * Time.deltaTime);
            if (target.position.x <= minX)
                movingRight = true;
        }
    }
}
