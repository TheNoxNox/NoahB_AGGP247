using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleDrawObj : DrawingObject
{
    public Vector3 PointA;
    public Vector3 PointB;
    public Vector3 PointC;

    public override void Initalize(Vector3 origin, Vector3 scale, Color objectColor)
    {
        Lines.Clear();

        Location = origin;
        Scale = scale;

        PointA = Location + new Vector3(Scale.x, 0, 0);
        PointB = DrawingTools.RotatePoint(Location, 120f, PointA);
        PointC = DrawingTools.RotatePoint(Location, 120f, PointB);

        Lines.Add(new Line(PointA, PointB, objectColor));
        Lines.Add(new Line(PointB, PointC, objectColor));
        Lines.Add(new Line(PointC, PointA, objectColor));
    }
}
