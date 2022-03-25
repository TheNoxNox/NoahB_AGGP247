using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
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

    private void Awake()
    {
        //location = GameManager.Main.screenCenter;
        GameManager.Main.gamePieces.Add(this);
    }

    // https://gamedevelopment.tutsplus.com/tutorials/when-worlds-collide-simulating-circle-circle-collisions--gamedev-769
    private void FixedUpdate()
    {
        if (dead) { return; }
        foreach (GamePiece otherPiece in GameManager.Main.gamePieces)
        {
            if (dead) return;
            if (otherPiece != this)
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

    public virtual void CollideWith(GamePiece piece)
    {
        Debug.Log("Object " + gameObject.name + " is colliding with object " + piece.gameObject.name);
    }
}
