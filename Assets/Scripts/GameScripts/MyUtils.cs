using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUtils
{
    public static Vector3 SnapToGrid(Vector3 position)
    {
        float x = Mathf.Round(position.x * 2) / 2;
        float y = Mathf.Round(position.y * 2) / 2;
        float z = Mathf.Round(position.z * 2) / 2;
        return new Vector3(x, y, z);
    }

    public static float DistanceBetweenObjects(Vector2 fromPos, Vector2 toPos)
    {
        float distance = Direction2D(fromPos, toPos).magnitude;
        return distance;
    }

    public static Vector2 Direction2D(Vector2 fromPos, Vector2 toPos)
    {
        Vector2 direction = toPos - fromPos;
        return direction;
    }

    public static Vector2 GetMouseDir2D(Vector2 fromPos)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseDirection = Direction2D(fromPos, mousePos);
        return mouseDirection;
    }

    public static float GetAngleTo(Vector2 towardPos)
    {
        float angle = Mathf.Atan2(towardPos.y, towardPos.x) * Mathf.Rad2Deg;
        return angle;
    }

    public static Vector3 Vec2DTo3D(Vector2 vecToChange)
    {
        Vector3 newVec3 = new Vector3(vecToChange.x, vecToChange.y, 0f);
        return newVec3;
    }
}
