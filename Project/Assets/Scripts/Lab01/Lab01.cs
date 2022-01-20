using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lab01 : MonoBehaviour
{
    public class Grid2D
    {
        public Vector3 screenSize;
        public Vector3 origin;

        public float gridSize = 10f;
        public float minGridSize = 2f;
        public float originSize = 20f;

        public int divisionCount = 5;
        public int minDivisionCount = 2;
    }

    public Color axisColor = Color.white;
    public Color lineColor = Color.gray;
    public Color divisionColor = Color.yellow;
    public Color originColor = Color.red;

    public bool isDrawingOrigin = false;
    public bool isDrawingAxis = true;
    public bool isDrawingDivisions = true;

    Grid2D grid = new Grid2D();

    private void Start()
    {
        grid.origin = new Vector3(Screen.width / 2, Screen.height / 2);
        grid.screenSize = new Vector3(Screen.width, Screen.height);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isDrawingOrigin = !isDrawingOrigin;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isDrawingAxis = !isDrawingAxis;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            isDrawingDivisions = !isDrawingDivisions;
        }


        //(0,0)
        Vector3 gridOrigin = ScreenToGrid(grid.origin);

        
        float xStart = gridOrigin.x - grid.divisionCount * grid.gridSize;
        float yStart = gridOrigin.y + grid.divisionCount * grid.gridSize;

        //X Axis
        for (int i = 0; i <= grid.divisionCount * grid.gridSize * 2; i++)
        {
            Color correctColor = lineColor;

            float xPos = (xStart + i) * 2;

            if(xPos == gridOrigin.x && isDrawingAxis)
            {
                correctColor = axisColor;
            }
            else if(xPos % 5 == 0 && isDrawingDivisions)
            {
                correctColor = divisionColor;
            }

            DrawLine(new Line(
                new Vector3(xPos, yStart),
                new Vector3(xPos, gridOrigin.y - grid.divisionCount * grid.gridSize), 
                correctColor));
        }

        //Y Axis
        for(int i = 0; i <= grid.divisionCount * grid.gridSize * 2; i++)
        {
            Color correctColor = lineColor;

            float yPos = (yStart - i) * 2;

            if (yPos == gridOrigin.x && isDrawingAxis)
            {
                correctColor = axisColor;
            }
            else if (yPos % 5 == 0 && isDrawingDivisions)
            {
                correctColor = divisionColor;
            }

            DrawLine(new Line(
                new Vector3(xStart, yPos),
                new Vector3(gridOrigin.x + grid.divisionCount * grid.gridSize, yPos),
                correctColor));
        }

        if (isDrawingOrigin)
        {
            DrawOrigin(gridOrigin);
        }

        //Debug.Log("Center of screen is at " + ScreenOrigin.x + ", " + ScreenOrigin.y);
    }

    public Vector3 GridToScreen(Vector3 gridSpace)
    {
        float screenPosX = gridSpace.x * grid.gridSize + grid.origin.x;
        float screenPosY = gridSpace.y * grid.gridSize + grid.origin.y;

        return new Vector3(screenPosX,screenPosY);
    }

    public Vector3 ScreenToGrid(Vector3 screenSpace)
    {
        float gridPosX = (screenSpace.x - grid.origin.x) / grid.gridSize;
        float gridPosY = (screenSpace.y - grid.origin.y) / grid.gridSize;

        return new Vector3(gridPosX, gridPosY);
    }

    public void DrawLine(Line line)
    {
        Line screenLine = new Line(GridToScreen(line.start), GridToScreen(line.end), line.color);

        Glint.AddCommand(screenLine);        
    }

    public void DrawOrigin(Vector3 gridOrigin)
    {
        Vector3 topPoint = new Vector3(gridOrigin.x, gridOrigin.y + 0.2f) * grid.originSize;
        Vector3 leftPoint = new Vector3(gridOrigin.x - 0.2f, gridOrigin.y) * grid.originSize;
        Vector3 rightPoint = new Vector3(gridOrigin.x + 0.2f, gridOrigin.y) * grid.originSize;
        Vector3 bottomPoint = new Vector3(gridOrigin.x, gridOrigin.y - 0.2f) * grid.originSize;

        DrawLine(new Line(topPoint, leftPoint, originColor));
        DrawLine(new Line(leftPoint, bottomPoint, originColor));
        DrawLine(new Line(bottomPoint, rightPoint, originColor));
        DrawLine(new Line(rightPoint,topPoint, originColor));
    }
}
