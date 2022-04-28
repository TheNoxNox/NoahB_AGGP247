using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : GamePiece
{
    public List<KeyCode> inputs = new List<KeyCode>();

    Vector3 nose, tailLeft, tailRight, tailCenter;

    public int score = 0;

    public int playerNum;

    protected override void Awake()
    {
        base.Awake();
        GameManager.Main.players.Add(this);
    }



    // Update is called once per frame
    protected override void Update()
    {
        

        force = Vector3.zero;

        #region input

        if (inputs.Contains(KeyCode.A))
        {
            rotation += 2f;
        }
        if (inputs.Contains(KeyCode.D))
        {
            rotation -= 2f;
        }

        
        if (inputs.Contains(KeyCode.W))
        {
            force.y = speed * Mathf.Sin(rotation * Mathf.Deg2Rad);
            force.x = speed * Mathf.Cos(rotation * Mathf.Deg2Rad);
        }
        else if (inputs.Contains(KeyCode.S))
        {
            velocity *= 0.989f;
        }

        if (inputs.Contains(KeyCode.Q))
        {
            Shoot();
        }
        #endregion

        foreach (GravitySource source in externalForces)
        {
            float angleToSourceY = Mathf.Atan((location.y - source.location.y) / (location.x - source.location.x));
            float angleToSourceX = Mathf.Acos((location.x - source.location.x) /
                Mathf.Sqrt(Mathf.Pow(location.y - source.location.y, 2) + Mathf.Pow(location.x - source.location.x, 2)));

            float forceToApplyY = (source.gravityStrength * -Mathf.Sin(angleToSourceY)) /
                Mathf.Sqrt(Mathf.Pow(location.y - source.location.y, 2) + Mathf.Pow(location.x - source.location.x, 2));
            float forceToApplyX = (source.gravityStrength * -Mathf.Cos(angleToSourceX)) /
                Mathf.Sqrt(Mathf.Pow(location.y - source.location.y, 2) + Mathf.Pow(location.x - source.location.x, 2));

            if (location.x < source.location.x)
            {
                forceToApplyY *= -1;
            }

            this.force.y += forceToApplyY;
            this.force.x += forceToApplyX;

            
        }

        this.acceleration = force / mass;

        if (Mathf.Abs(velocity.magnitude) < maxVelocity * Time.deltaTime)
        {
            this.velocity += acceleration * Time.deltaTime;
        }
        

        this.location += velocity;

        if (location.x < 0) { location = new Vector3(Screen.width, location.y); }
        else if (location.x > Screen.width) { location = new Vector3(0, location.y); }

        if (location.y < 0) { location = new Vector3(location.x, Screen.height); }
        else if (location.y > Screen.height) { location = new Vector3(location.x, 0); }

        nose = DrawingTools.RotatePoint(location, rotation, location + new Vector3(18, 0));
        tailLeft = DrawingTools.RotatePoint(location, rotation, location + new Vector3(-12, 8));
        tailRight = DrawingTools.RotatePoint(location, rotation, location + new Vector3(-12, -8));
        tailCenter = DrawingTools.RotatePoint(location, rotation, location + new Vector3(-8, 0));

        DrawShip();
        externalForces.Clear();
        inputs.Clear();
    }

    public void Shoot()
    {
        Bullet proj = Instantiate(GameManager.Main.bulletPrefab).GetComponent<Bullet>();
        proj.location = nose;
        proj.source = this;
        proj.rotation = rotation;
        proj.pieceColor = pieceColor;
        proj.dead = false;
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
        if (piece.GetType() == typeof(Bullet))
        {
            if ((piece as Bullet).source == this) { return; }
            else
            {
                dead = true;
                GameManager.Main.PlayerScored((piece as Bullet).source);
                return;
            }
        }
        if(piece.GetType() != typeof(Spaceship))
        {
            dead = true;
            //base.CollideWith(piece);

            GameManager.Main.PlayerWipeout(this);
        }
        
    }
}
