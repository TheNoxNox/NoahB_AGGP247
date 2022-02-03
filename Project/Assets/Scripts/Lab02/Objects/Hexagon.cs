using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon : DrawingObject
{
    public override void Initalize(Vector3 origin, Vector3 scale, Color objectColor)
    {
        Location = origin;
        Scale = scale;

        Debug.Log(Location.ToString());

        Vector3 point1, point2, point3, point4, point5, point6;

        point1 = Location + new Vector3(-1, 0, 0);
        point2 = DrawingTools.RotatePoint(Location, 60f, point1);
        point3 = DrawingTools.RotatePoint(Location, 60f, point2);
        point4 = DrawingTools.RotatePoint(Location, 60f, point3);
        point5 = DrawingTools.RotatePoint(Location, 60f, point4);
        point6 = DrawingTools.RotatePoint(Location, 60f, point5);

        Lines.Add(new Line(point1, point2, objectColor));
        Lines.Add(new Line(point2, point3, objectColor));
        Lines.Add(new Line(point3, point4, objectColor));
        Lines.Add(new Line(point4, point5, objectColor));
        Lines.Add(new Line(point5, point6, objectColor));
        Lines.Add(new Line(point6, point1, objectColor));

    }
}
