using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : GamePiece
{
    public Spaceship source;

    protected override void Update()
    {
        force.y = speed * Mathf.Sin(rotation * Mathf.Deg2Rad);
        force.x = speed * Mathf.Cos(rotation * Mathf.Deg2Rad);

        this.acceleration = force / mass;

        if (Mathf.Abs(velocity.magnitude) < maxVelocity * Time.deltaTime)
        {
            this.velocity += acceleration * Time.deltaTime;
        }

        this.location += velocity;

        if (location.x < 0) { GameManager.Main.DestroyGamePiece(this); }
        else if (location.x > Screen.width) { GameManager.Main.DestroyGamePiece(this); }

        if (location.y < 0) { GameManager.Main.DestroyGamePiece(this); }
        else if (location.y > Screen.height) { GameManager.Main.DestroyGamePiece(this); }

        DrawingTools.DrawCircle(location, hitboxSize, 4, pieceColor);
    }

    protected override void FixedUpdate()
    {
        if (dead) { return; }
        foreach (GamePiece otherPiece in GameManager.Main.gamePieces)
        {
            if (dead) return;
            if (otherPiece != this && otherPiece != source && otherPiece.GetType() != typeof(GravitySource))
            {

                if (location.x + hitboxSize + otherPiece.hitboxSize > otherPiece.location.x
                            && location.x < otherPiece.location.x + hitboxSize + otherPiece.hitboxSize
                            && location.y + hitboxSize + otherPiece.hitboxSize > otherPiece.location.y
                            && location.y < otherPiece.location.y + hitboxSize + otherPiece.hitboxSize)
                {
                    //Is colliding with other piece

                    this.CollideWith(otherPiece);
                    return;
                }
            }

        }
    }

    public override void CollideWith(GamePiece piece)
    {
        if (piece.GetType() == typeof(Bullet))
        {
            if ((piece as Bullet).source == source) { return; }
        }
        if(piece.GetType() == typeof(Spaceship)) 
        {
            if((Spaceship)piece == source)
            {
                return;
            }
        }
        //base.CollideWith(piece);
        GameManager.Main.DestroyGamePiece(this);
    }
}
