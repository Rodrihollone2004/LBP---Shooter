using UnityEngine;

public class MovementFactory
{
    public static object CreateMovement(string type, float minX, float maxX)
    {
        switch (type)
        {
            case "Normal":
                return new NormalMovement(minX, maxX);
            case "Fast":
                return new FastMovement(minX, maxX);
            case "Slow":
                return new SlowMovement(minX, maxX);
            default:
                return new NormalMovement(minX, maxX);
        }
    }
}
