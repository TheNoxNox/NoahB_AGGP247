using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    public float mass = 100f;

    public float rotation = 0f;

    public float speed = 1000f;

    public Vector3 force = Vector3.zero;

    public float maxVelocity = 15f;

    public Vector3 location;

    Vector3 nose, tailLeft, tailRight, tailCenter;

    Vector3 screenCenter;

    public Vector3 velocity = Vector3.zero;

    public Vector3 acceleration = Vector3.zero;


    private void Awake()
    {
        screenCenter = new Vector3(Screen.width / 2, Screen.height / 2);
        location = screenCenter;
    }

    // Update is called once per frame
    void Update()
    {
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
        else
        {
            force = Vector3.zero;
        }
        #endregion

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

        nose = DrawingTools.RotatePoint(location, rotation, location + new Vector3(0, 16));
        tailLeft = DrawingTools.RotatePoint(location, rotation, location + new Vector3(-8, -12));
        tailRight = DrawingTools.RotatePoint(location, rotation, location + new Vector3(8, -12));
        tailCenter = DrawingTools.RotatePoint(location, rotation, location + new Vector3(0, -8));

        DrawShip();
    }

    void DrawShip()
    {
        Glint.AddCommand(new Line(nose, tailLeft, Color.red));
        Glint.AddCommand(new Line(tailLeft, tailCenter, Color.red));
        Glint.AddCommand(new Line(tailCenter, tailRight, Color.red));
        Glint.AddCommand(new Line(tailRight, nose, Color.red));
    }
}
