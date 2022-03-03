using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallObj
{
    public Vector3 location = Vector3.zero;
    public float radius = 10f;
    public float speed = 400f;
    public float firingAngle = 90f;
    public float mass = 10f;

    public Vector3 force = Vector3.zero;
    public Vector3 velocity = Vector3.zero;
    public Vector3 acceleration = Vector3.zero;

    public bool isOnGround = false;

    public BallObj(float angle, float firingForce, Vector3 firingLocation)
    {
        location = firingLocation;
        firingAngle = angle;
        speed = firingForce;
        Debug.Log(firingAngle);
        force.y = speed * Mathf.Sin(firingAngle * Mathf.Deg2Rad);
        force.x = speed * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
    }

    public void DoPhysics()
    {
        force += Physics.gravity;

        acceleration = force / mass;

        velocity += acceleration * Time.deltaTime;

        location += velocity;

        if((location.y - radius) <= TankBall.groundLevel)
        {
            location = new Vector3(location.x, TankBall.groundLevel + radius);
            isOnGround = true;
        }
    }

    public void DrawBall()
    {
        DrawingTools.DrawCircle(location, radius, 10, Color.red);
    }
}

public class TankBall : MonoBehaviour
{
    public static float groundLevel = 0;

    public float firingAngle = 90f;

    public float firingForce = 200f;

    public Vector3 location = new Vector3(Screen.width / 2, groundLevel);

    public Vector3 leftBotCorner, rightBotCorner, leftTopCorner, rightTopCorner;
    public Vector3 firingSpot;

    private void Awake()
    {
        groundLevel = Screen.height / 10f;

        location = new Vector3(Screen.width / 2, groundLevel);

        leftBotCorner = location + new Vector3(-10f, 0);
        rightBotCorner = location + new Vector3(10f, 0);
        leftTopCorner = location + new Vector3(-10f, 50f);
        rightTopCorner = location + new Vector3(10f, 50f);
        firingSpot = location + new Vector3(0, 50f);
    }

    public List<BallObj> balls = new List<BallObj>();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            firingAngle -= 0.15f;
            if (firingAngle < 30) firingAngle = 30;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            firingAngle += 0.15f;
            if (firingAngle > 150f) firingAngle = 150;
        }
        
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            firingForce += 10f;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            firingForce -= 10f;
        }

        DrawCannon();

        DrawGround();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireBall();
        }            

        foreach(BallObj ball in balls)
        {
            if (!ball.isOnGround)
            {
                ball.DoPhysics();
            }
            ball.DrawBall();
        }
    }

    public void FireBall()
    {
        BallObj ball = new BallObj(180f - firingAngle, firingForce, firingSpot);
        balls.Add(ball);
    }

    void DrawGround()
    {
        Glint.AddCommand(new Line(new Vector3(0, groundLevel), new Vector3(Screen.width, groundLevel), Color.green));
    }

    void DrawCannon()
    {
        leftBotCorner = DrawingTools.RotatePoint(location, 90 - firingAngle, location + new Vector3(-10f, 0));
        rightBotCorner = DrawingTools.RotatePoint(location, 90 - firingAngle, location + new Vector3(10f, 0));
        leftTopCorner = DrawingTools.RotatePoint(location, 90 - firingAngle, location + new Vector3(-10f, 50f));
        rightTopCorner = DrawingTools.RotatePoint(location, 90 - firingAngle, location + new Vector3(10f, 50f));

        firingSpot = DrawingTools.RotatePoint(location, 90 - firingAngle, location + new Vector3(0, 50f));

        Glint.AddCommand(new Line(leftBotCorner, rightBotCorner, Color.magenta));
        Glint.AddCommand(new Line(rightBotCorner, rightTopCorner, Color.magenta));
        Glint.AddCommand(new Line(rightTopCorner, leftTopCorner, Color.magenta));
        Glint.AddCommand(new Line(leftTopCorner, leftBotCorner, Color.magenta));
    }
}
