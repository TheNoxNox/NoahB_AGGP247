using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidTest : GamePiece
{
    private void Update()
    {
        DrawingTools.DrawCircle(location, hitboxSize, 6, pieceColor);
    }
}
