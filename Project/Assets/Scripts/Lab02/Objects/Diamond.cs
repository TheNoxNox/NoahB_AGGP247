using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : DrawingObject
{
    public override void Initalize(Vector3 origin, Vector3 scale, Color objectColor)
    {
        Lines.Clear();
        Location = origin;
        Scale = scale;
       
        Vector3 topPoint = new Vector3(Location.x * Scale.x, (Location.y + 0.2f) * Scale.y);
        Vector3 leftPoint = new Vector3((Location.x - 0.2f) * Scale.x, Location.y * Scale.y);
        Vector3 rightPoint = new Vector3((Location.x + 0.2f) * Scale.x, Location.y * Scale.y);
        Vector3 bottomPoint = new Vector3(Location.x * Scale.x, (Location.y - 0.2f) * Scale.y);
        Lines.Add(new Line(topPoint, leftPoint, objectColor));
        Lines.Add(new Line(leftPoint, bottomPoint, objectColor));
        Lines.Add(new Line(bottomPoint, rightPoint, objectColor));
        Lines.Add(new Line(rightPoint, topPoint, objectColor));
    }
}
