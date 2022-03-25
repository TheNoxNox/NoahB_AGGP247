using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : GamePiece
{
    

    Vector3 nose, tailLeft, tailRight, tailCenter;  

    


    

    // Update is called once per frame
    void Update()
    {
        

        force = Vector3.zero;

        #region input

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            rotation += 0.1f;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            rotation -= 0.1f;
        }

        
        if (Input.GetKey(KeyCode.W))
        {
            force.y = speed * Mathf.Cos(rotation * Mathf.Deg2Rad);
            force.x = speed * -Mathf.Sin(rotation * Mathf.Deg2Rad);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            velocity *= 0.999f;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            velocity *= 0f;
        }
        #endregion

        foreach(GravitySource source in externalForces)
        {
            float angleToSourceY = Mathf.Atan((location.y - source.location.y) / (location.x - source.location.x));
            float angleToSourceX = Mathf.Acos((location.x - source.location.x) / 
                Mathf.Sqrt(Mathf.Pow(location.y - source.location.y, 2) + Mathf.Pow(location.x - source.location.x, 2)));

            force.y += source.gravityStrength * Mathf.Sin(angleToSourceY);
            force.x += source.gravityStrength * -Mathf.Cos(angleToSourceX);
        }

        acceleration = force / mass;

        if(Mathf.Abs(velocity.magnitude) < maxVelocity * Time.deltaTime)
        {
            velocity += acceleration * Time.deltaTime;
        }    

        location += velocity;

        if(location.x < 0) { location = new Vector3(Screen.width, location.y); }
        else if(location.x > Screen.width) { location = new Vector3(0, location.y); }

        if(location.y < 0) { location = new Vector3(location.x, Screen.height); }
        else if(location.y > Screen.height) { location = new Vector3(location.x, 0); }

        nose = DrawingTools.RotatePoint(location, rotation, location + new Vector3(0, 32));
        tailLeft = DrawingTools.RotatePoint(location, rotation, location + new Vector3(-16, -24));
        tailRight = DrawingTools.RotatePoint(location, rotation, location + new Vector3(16, -24));
        tailCenter = DrawingTools.RotatePoint(location, rotation, location + new Vector3(0, -16));

        DrawShip();
        externalForces.Clear();
    }

    void DrawShip()
    {
        Glint.AddCommand(new Line(nose, tailLeft, pieceColor));
        Glint.AddCommand(new Line(tailLeft, tailCenter, pieceColor));
        Glint.AddCommand(new Line(tailCenter, tailRight, pieceColor));
        Glint.AddCommand(new Line(tailRight, nose, pieceColor));
    }

    public override void CollideWith(GamePiece piece)
    {
        dead = true;
        base.CollideWith(piece);

        GameManager.Main.DestroyGamePiece(this);
    }
}
