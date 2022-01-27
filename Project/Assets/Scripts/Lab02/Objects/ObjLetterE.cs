using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjLetterE : DrawingObject
{
    public override void Initalize(Vector3 origin, Vector3 scale, Color objectColor)
    {
        Location = origin;
        Scale = scale;

        // 8 Segment Display (10,20)
        Lines.Add(new Line(new Vector3(Location.x, Location.y), new Vector3(Location.x + 10, Location.y),objectColor));
        //Lines.Add(new Line(new Vector2(10, 0), new Vector2(10, 10) ));
        Lines.Add(new Line(new Vector3(Location.x, Location.y), new Vector3(Location.x, Location.y + 10),objectColor));
        Lines.Add(new Line(new Vector3(Location.x + 10, Location.y + 10), new Vector3(Location.x, Location.y + 10),objectColor));
        Lines.Add(new Line(new Vector3(Location.x, Location.y + 10), new Vector3(Location.x, Location.y + 20),objectColor));
        //Lines.Add(new Line(new Vector2(10, 10), new Vector2(10, 20) ));
        Lines.Add(new Line(new Vector3(Location.x, Location.y + 20), new Vector3(Location.x + 10,Location.y + 20),objectColor));
    }
}
