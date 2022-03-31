using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySource : GamePiece
{
    public float gravityStrength = 10f;

    public float gravityRadius = 150f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        DrawingTools.DrawCircle(location, gravityRadius, 16, pieceColor);

        foreach(GamePiece otherPiece in GameManager.Main.gamePieces)
        {
            if(otherPiece != this)
            {
                if (location.x + gravityRadius + otherPiece.hitboxSize > otherPiece.location.x
                            && location.x < otherPiece.location.x + gravityRadius + otherPiece.hitboxSize
                            && location.y + gravityRadius + otherPiece.hitboxSize > otherPiece.location.y
                            && location.y < otherPiece.location.y + gravityRadius + otherPiece.hitboxSize)
                {
                    //Is colliding with other piece
                    
                    if (!otherPiece.externalForces.Contains(this))
                    {
                        Debug.Log("Adding grav force to " + otherPiece.gameObject.name);
                        otherPiece.externalForces.Add(this);
                    }

                }
            }
        }
    }
}
