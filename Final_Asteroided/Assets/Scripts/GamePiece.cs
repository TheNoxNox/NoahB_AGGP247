using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
    public int health = 5;

    public float hitboxSize;

    public Vector3 location;

    public Color pieceColor;

    public float mass = 100f;

    public float rotation = 0f;

    public float speed = 1000f;

    public Vector3 force = Vector3.zero;

    public float maxVelocity = 15f;

    public Vector3 velocity = Vector3.zero;

    public Vector3 acceleration = Vector3.zero;

    public bool dead = false;

    public List<GravitySource> externalForces = new List<GravitySource>();

    protected virtual void Awake()
    {
        //location = GameManager.Main.screenCenter;
        GameManager.Main.gamePieces.Add(this);
    }

    protected virtual void Update()
    {
        force = Vector3.zero;

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

        externalForces.Clear();
    }

    // https://gamedevelopment.tutsplus.com/tutorials/when-worlds-collide-simulating-circle-circle-collisions--gamedev-769
    protected virtual void FixedUpdate()
    {
        if (dead) { return; }
        foreach (GamePiece otherPiece in GameManager.Main.gamePieces)
        {
            if (dead) return;
            if (otherPiece != this && otherPiece.GetType() != typeof(GravitySource))
            {

                if (location.x + hitboxSize + otherPiece.hitboxSize > otherPiece.location.x
                            && location.x < otherPiece.location.x + hitboxSize + otherPiece.hitboxSize
                            && location.y + hitboxSize + otherPiece.hitboxSize > otherPiece.location.y
                            && location.y < otherPiece.location.y + hitboxSize + otherPiece.hitboxSize)
                {
                    //Is colliding with other piece

                    //otherPiece.CollideWith(this);
                    this.CollideWith(otherPiece);
                    return;
                }
            }

        }

    }

    public virtual void CollideWith(GamePiece piece)
    {
        Debug.Log("Object " + gameObject.name + " is colliding with object " + piece.gameObject.name);
    }
}
