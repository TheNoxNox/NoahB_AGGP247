using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarTwinkle
{
    public Vector3 center;

    public float size = 2.5f;

    public float twinkleSTR = 0f;

    public float twinkleCDMax = 10f;
    public float twinkleCDMin = 3f;
    public float twinkleCD = 6.5f;

    public float twinkleTimeDur = 1.2f;
    public float twinkleTime = 0f;

    public StarTwinkle()
    {
        center = new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height));
        twinkleSTR = Random.Range(0, 2 * Mathf.PI);
    }

    // Update is called once per frame
    public void Twinkle()
    {
        twinkleSTR += Time.deltaTime * 0.1f;
        DrawingTools.DrawCircle(center, size * Mathf.Sin(twinkleSTR), 4, Color.white);
        if(twinkleSTR >= 2 * Mathf.PI) { twinkleSTR = 0f; }
    }
}
