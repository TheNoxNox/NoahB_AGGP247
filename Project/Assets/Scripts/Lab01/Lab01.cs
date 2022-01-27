using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid2D
{
    public Vector3 screenSize;
    public Vector3 origin;

    public float gridSize = 10f;
    public float minGridSize = 2f;
    public float originSize = 20f;

    public int divisionCount = 5;
    public int minDivisionCount = 2;

    public void DrawLine(Line line, bool DrawOnGrid = true)
    {
        Vector3 lineStart;
        Vector3 lineEnd;

        if (DrawOnGrid)
        {
            lineStart = DrawingTools.GridToScreen(line.start, this);
            lineEnd = DrawingTools.GridToScreen(line.end, this);
        }
        else
        {
            lineStart = line.start;
            lineEnd = line.end;
        }

        Line screenLine = new Line(lineStart, lineEnd, line.color);

        Glint.AddCommand(screenLine);
    }

    public void DrawObject(DrawingObject lineObj, bool DrawOnGrid = true)
    {
        lineObj.Draw(this);
    }
}

public class Lab01 : MonoBehaviour
{
    public enum MoveDir
    {
        up = 1,
        left = 2,
        right = 3,
        down = 4
    }

    

    public Color axisColor = Color.white;
    public Color lineColor = Color.gray;
    public Color divisionColor = Color.yellow;
    public Color originColor = Color.red;

    public bool isDrawingOrigin = false;
    public bool isDrawingAxis = true;
    public bool isDrawingDivisions = true;

    Grid2D grid = new Grid2D();

    List<DrawingObject> drawObjects = new List<DrawingObject>();
    DrawingObject diamond = new DrawingObject();

    private void Start()
    {
        grid.origin = new Vector3(Screen.width / 2, Screen.height / 2);
        grid.screenSize = new Vector3(Screen.width, Screen.height);

        Vector3 gridOrigin = DrawingTools.ScreenToGrid(grid.origin, grid);
        Vector3 topPoint = new Vector3(gridOrigin.x, gridOrigin.y + 0.2f) * grid.originSize;
        Vector3 leftPoint = new Vector3(gridOrigin.x - 0.2f, gridOrigin.y) * grid.originSize;
        Vector3 rightPoint = new Vector3(gridOrigin.x + 0.2f, gridOrigin.y) * grid.originSize;
        Vector3 bottomPoint = new Vector3(gridOrigin.x, gridOrigin.y - 0.2f) * grid.originSize;
        diamond.Lines.Add(new Line(topPoint, leftPoint, originColor));
        diamond.Lines.Add(new Line(leftPoint, bottomPoint, originColor));
        diamond.Lines.Add(new Line(bottomPoint, rightPoint, originColor));
        diamond.Lines.Add(new Line(rightPoint, topPoint, originColor));
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
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveOrigin(MoveDir.up);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveOrigin(MoveDir.left);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveOrigin(MoveDir.right);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveOrigin(MoveDir.down);
        }
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            ScaleGridSize(MoveDir.down);
        }
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            ScaleGridSize(MoveDir.up);
        }
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            ScaleDivisionSize(MoveDir.down);
        }
        if (Input.GetKeyDown(KeyCode.Period))
        {
            ScaleDivisionSize(MoveDir.up);
        }

        //(0,0)
        Vector3 gridOrigin = DrawingTools.ScreenToGrid(grid.origin, grid);

        
        float xStart = gridOrigin.x - (grid.divisionCount) * grid.gridSize;
        float yStart = gridOrigin.y + (grid.divisionCount) * grid.gridSize;

        for (int i = 0; i <= grid.divisionCount * grid.gridSize; i++)
        {
            Color correctColor = lineColor;

            // X Axis

            float xPos = xStart + (i * 2);

            if(xPos == gridOrigin.x && isDrawingAxis)
            {
                correctColor = axisColor;
            }
            else if(xPos % 5 == 0 && isDrawingDivisions)
            {
                correctColor = divisionColor;
            }

            grid.DrawLine(new Line(
                new Vector3(xPos, yStart),
                new Vector3(xPos, gridOrigin.y - grid.divisionCount * grid.gridSize), 
                correctColor));

            // Y Axis

            float yPos = yStart - (i * 2);

            if (yPos == gridOrigin.x && isDrawingAxis)
            {
                correctColor = axisColor;
            }
            else if (yPos % 5 == 0 && isDrawingDivisions)
            {
                correctColor = divisionColor;
            }

            grid.DrawLine(new Line(
                new Vector3(xStart, yPos),
                new Vector3(gridOrigin.x + grid.divisionCount * grid.gridSize, yPos),
                correctColor));
        }

        if (isDrawingOrigin)
        {
            //DrawOrigin(gridOrigin);
            grid.DrawObject(diamond);
        }

        //Debug.Log("Center of screen is at " + ScreenOrigin.x + ", " + ScreenOrigin.y);
    }

    

    public void DrawLine(Line line)
    {
        Line screenLine = new Line(DrawingTools.GridToScreen(line.start, grid), DrawingTools.GridToScreen(line.end, grid), line.color);

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

        Debug.Log("drawing line from " + topPoint.ToString() + " to " + leftPoint.ToString());
        Debug.Log("drawing line from " + leftPoint.ToString() + " to " + bottomPoint.ToString());
    }

    public void MoveOrigin(MoveDir dir)
    {
        switch (dir)
        {
            case MoveDir.up:
                grid.origin = new Vector3(grid.origin.x, grid.origin.y + 5);
                break;
            case MoveDir.left:
                grid.origin = new Vector3(grid.origin.x - 5, grid.origin.y);
                break;
            case MoveDir.right:
                grid.origin = new Vector3(grid.origin.x + 5, grid.origin.y);
                break;
            case MoveDir.down:
                grid.origin = new Vector3(grid.origin.x, grid.origin.y - 5);
                break;
        }
    }

    public void ScaleGridSize(MoveDir dir)
    {
        switch (dir)
        {
            case MoveDir.up:
                grid.gridSize += 1f;
                break;
            case MoveDir.down:
                grid.gridSize -= 1;
                if(grid.gridSize < grid.minGridSize) { grid.gridSize = grid.minGridSize; }
                break;
        }
    }

    public void ScaleDivisionSize(MoveDir dir)
    {
        switch (dir)
        {
            case MoveDir.up:
                grid.divisionCount += 1;
                break;
            case MoveDir.down:
                grid.divisionCount -= 1;
                if(grid.divisionCount < grid.minDivisionCount) { grid.divisionCount = grid.minDivisionCount; }
                break;
        }
    }
}
