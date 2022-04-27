using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : GamePiece
{
    

    public float invincibilityTimer = 0.05f;

    public float timerCount = 0f;

    public bool invuln = false;

    protected override void Awake()
    {
        base.Awake();
        rotation = Random.Range(0f, 359f);

        speed = Random.Range(5f, 11f);
    }

    protected override void Update()
    {
        //base.Update();

        force.y = speed * Mathf.Sin(rotation * Mathf.Deg2Rad);
        force.x = speed * Mathf.Cos(rotation * Mathf.Deg2Rad);

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

        DrawingTools.DrawCircle(location, hitboxSize, 10, pieceColor);

        if (invuln) { HitRecovery(); }
    }

    public override void CollideWith(GamePiece piece)
    {
        //base.CollideWith(piece);
        if (invuln) { return; }
        health -= 1;
        if(health <= 0)
        {
            GameManager.Main.DestroyGamePiece(this);
        }
    }

    void HitRecovery()
    {
        timerCount += Time.deltaTime;
        if(timerCount >= invincibilityTimer)
        {
            invuln = false;
            timerCount = 0;
        }
    }
}
