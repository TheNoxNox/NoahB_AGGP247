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

        float rotX = (pointIN.x - Center.x) * Mathf.Cos(radAngl) - (pointIN.y - Center.y) * Mathf.Sin(radAngl);
        float rotY = (pointIN.x - Center.x) * Mathf.Sin(radAngl) + (pointIN.y - Center.y) * Mathf.Cos(radAngl);

        //Debug.Log("X Prime: " + rotX);
        //Debug.Log("Y prime: " + rotY);

        pointOUT = new Vector3(Center.x + rotX, Center.y + rotY);

        return pointOUT;
    }

    public static float ToDegrees(float radian)
    {
        return radian * Mathf.Rad2Deg;
    }

    public static float ToRadians(float degree)
    {
        return degree * Mathf.Deg2Rad;
    }

    public static Vector3 CircleRadiusPoint(Vector3 origin, float angle, float radius)
    {
        return new Vector3(origin.x + radius * Mathf.Cos(ToRadians(angle)), origin.y + radius * Mathf.Sin(ToRadians(angle)));
    }

    /// <summary>
    /// Draws a circle independantly of any grid. As such, coordinates and radius should be screenspace.
    /// </summary>
    /// <param name="pos">Coordinates in screen space</param>
    /// <param name="radius">In screenspace units.</param>
    /// <param name="sides">Anything below 3 will be truncated and replaced with 3.</param>
    /// <param name="color"></param>
    public static void DrawCircle(Vector3 pos, float radius, int sides, Color color)
    {
        int adjustedSides;
        if(sides > 3) { adjustedSides = sides; }
        else { adjustedSides = 3; }

        float incrementAngle = 360f / adjustedSides;

        Vector3 lastPoint = CircleRadiusPoint(pos, 0, radius);

        for (int i = 1; i <= sides; i++)
        {
            Vector3 nextPoint = CircleRadiusPoint(pos, incrementAngle * i, radius);

            Line circleSide = new Line(lastPoint, nextPoint, color);

            Glint.AddCommand(circleSide);

            lastPoint = nextPoint;
        }
    }

    public static Vector3 EllipseRadiusPoint(Vector3 origin, float angle, Vector3 axis)
    {
        return new Vector3(
            origin.x + axis.x * Mathf.Cos(ToRadians(angle)),
            origin.y + axis.y * Mathf.Sin(ToRadians(angle)));
    }

    public static void DrawEllipse(Vector3 pos, Vector3 axis, int sides, Color color)
    {
        int adjustedSides;
        if (sides > 3) { adjustedSides = sides; }
        else { adjustedSides = 3; }

        float incrementAngle = 360f / adjustedSides;

        Vector3 lastPoint = EllipseRadiusPoint(pos, 0, axis);

        for (int i = 1; i <= sides; i++)
        {
            Vector3 nextPoint = EllipseRadiusPoint(pos,incrementAngle * i, axis);

            Line circleSide = new Line(lastPoint, nextPoint, color);

            Glint.AddCommand(circleSide);

            lastPoint = nextPoint;
        }
    }

    /// <summary>
    /// both MUST be in either gridspace or screenspace coordinates.
    /// </summary>
    /// <param name="point">E</param>
    /// <param name="triangle"></param>
    /// <returns></returns>
    public static bool PointInsideTriangle(Vector3 point, TriangleDrawObj triangle)
    {
        // https://blackpawn.com/texts/pointinpoly/

        Vector3 v0 = triangle.PointC - triangle.PointA;
        Vector3 v1 = triangle.PointB - triangle.PointA;
        Vector3 v2 = point - triangle.PointA;

        float dot00 = Vector3.Dot(v0, v0);
        float dot01 = Vector3.Dot(v0, v1);
        float dot02 = Vector3.Dot(v0, v2);
        float dot11 = Vector3.Dot(v1, v1);
        float dot12 = Vector3.Dot(v1, v2);

        float invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
        float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
        float v = (dot00 * dot12 - dot01 * dot02) * invDenom;

        if (u >= 0 && v >= 0 && u + v < 1) { return true; }

        return false;
    }
}

public class Ellipse
{
    /// <summary>
    /// The center point IN SCREEN SPACE UNITS of the ellipse. Make sure to convert!
    /// </summary>
    public Vector3 center = Vector3.zero;
    public Vector3 axis = Vector3.one;
    public float rotation = 0;
    public int sides = 32;
    public float width = 2.0f;
    public Color color = Color.white;

    public Ellipse() { }

    public Ellipse(Vector3 cent, Vector3 ax, float rot, int side, float wid, Color col)
    {
        center = cent;
        axis = ax;
        rotation = rot;
        sides = side;
        width = wid;
        color = col;
    }

    public virtual void Draw()
    {
        DrawingTools.DrawEllipse(center, axis, sides, color);
    }

    // I did not include an overload for drawing with a Grid2D as it isn't needed given how I've set up my DrawEllipse function.
}

public class Circle : Ellipse
{
    public float Radius { get { return axis.x; } set { axis = new Vector3(value, value); } }

    public override void Draw()
    {
        DrawingTools.DrawCircle(center, Radius, sides, color);
    }
}
