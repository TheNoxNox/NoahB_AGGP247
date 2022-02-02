using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingTools : MonoBehaviour
{
    public static Vector3 GridToScreen(Vector3 gridSpace, Grid2D grid)
    {
        float screenPosX = gridSpace.x * grid.gridSize + grid.origin.x;
        float screenPosY = gridSpace.y * grid.gridSize + grid.origin.y;

        return new Vector3(screenPosX, screenPosY);
    }

    public static Vector3 ScreenToGrid(Vector3 screenSpace, Grid2D grid)
    {
        float gridPosX = (screenSpace.x - grid.origin.x) / grid.gridSize;
        float gridPosY = (screenSpace.y - grid.origin.y) / grid.gridSize;

        return new Vector3(gridPosX, gridPosY);
    }

    public static float V3ToAngle(Vector3 start, Vector3 end)
    {
        float anglRadians = Mathf.Atan2((end.y - start.y), (end.x - start.x));

        return anglRadians * (180 / Mathf.PI);
    }

    public static float LineToAngle(Line line)
    {
        float lineAngleDeg = DrawingTools.V3ToAngle(line.start, line.end);

        return lineAngleDeg;
    }

    public static Vector3 RotatePoint(Vector3 Center, float angle, Vector3 pointIN)
    {
        Vector3 pointOUT;

        float radAngl = Mathf.Deg2Rad * angle;

        float rotX = pointIN.x * Mathf.Cos(radAngl) - pointIN.y * Mathf.Sin(radAngl);
        float rotY = pointIN.x * Mathf.Sin(radAngl) + pointIN.y * Mathf.Cos(radAngl);

        //Debug.Log("X Prime: " + rotX);
        //Debug.Log("Y prime: " + rotY);

        pointOUT = new Vector3(Center.x + rotX, Center.y + rotY);

        return pointOUT;
    }
}
