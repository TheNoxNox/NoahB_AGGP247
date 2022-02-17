using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectDrawObj : DrawingObject
{
    public Vector3 CornerA, CornerB, CornerC, CornerD;

    public override void Initalize(Vector3 origin, Vector3 scale, Color objectColor)
    {
        Lines.Clear();

        Location = origin;
        Scale = scale;

        CornerA = Location + new Vector3(-Scale.x, Scale.y);
        CornerB = Location + new Vector3(Scale.x, Scale.y);
        CornerC = Location + new Vector3(Scale.x, -Scale.y);
        CornerD = Location + new Vector3(-Scale.x, -Scale.y);

        Lines.Add(new Line(CornerA, CornerB, objectColor));
        Lines.Add(new Line(CornerB, CornerC, objectColor));
        Lines.Add(new Line(CornerC, CornerD, objectColor));
        Lines.Add(new Line(CornerD, CornerA, objectColor));
    }
}
