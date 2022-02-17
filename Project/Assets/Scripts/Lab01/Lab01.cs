using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid2D
{
    public Vector3 screenSize;
    public Vector3 origin;
    public Vector3 gridSpaceOrigin { get { return DrawingTools.ScreenToGrid(origin, this); } }

    public float gridSize = 10f;
    public float minGridSize = 2f;
    public float originSize = 20f;

    public int divisionCount = 5;
    public int minDivisionCount = 2;

    public Vector3 originGridSpace { get { return DrawingTools.ScreenToGrid(origin, this); } }

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
    public bool isDrawGroupCircle = true;

    Grid2D grid = new Grid2D();

    Ellipse myEllipse = new Ellipse();
    Circle myCircle = new Circle();

    List<DrawingObject> drawObjects = new List<DrawingObject>();
    Diamond originDiamond = new Diamond();
    Diamond rotatingDiamond = new Diamond();
    ObjLetterE letterE = new ObjLetterE();
    Hexagon hex = new Hexagon();

    private void Awake()
    {
        grid.origin = new Vector3(Screen.width / 2, Screen.height / 2);
        grid.screenSize = new Vector3(Screen.width, Screen.height);

        Vector3 gridOrigin = DrawingTools.ScreenToGrid(grid.origin, grid) / grid.gridSize;
        originDiamond.Initalize(gridOrigin, new Vector3(grid.originSize,grid.originSize,grid.originSize), originColor);

        rotatingDiamond.Initalize(gridOrigin + new Vector3(0, 7.5f, 0) / (grid.gridSize / 2), Vector3.one * grid.gridSize, Color.cyan);

        letterE.Initalize(gridOrigin + new Vector3(-30, 10, 0), Vector3.one * grid.gridSize, Color.red);

        hex.Initalize(gridOrigin + new Vector3(10,-20,0), Vector3.one * grid.gridSize, Color.red);

        myEllipse.center = grid.origin;
        myEllipse.axis = new Vector3(85, 20);
        myEllipse.color = Color.red;

        myCircle.Radius = 130f;
        myCircle.sides = 16;
        myCircle.center = grid.origin;
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
        if (Input.GetKey(KeyCode.UpArrow))
        {
            MoveOrigin(MoveDir.up);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            MoveOrigin(MoveDir.left);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            MoveOrigin(MoveDir.right);
        }
        if (Input.GetKey(KeyCode.DownArrow))
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
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isDrawGroupCircle = !isDrawGroupCircle;
        }

        //(0,0)
        Vector3 gridOrigin = DrawingTools.ScreenToGrid(grid.origin, grid);

        
        float xStart = gridOrigin.x - (grid.divisionCount * grid.gridSize);
        float yStart = gridOrigin.y + (grid.divisionCount * grid.gridSize);

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

        Parabola1(xStart);
        Parabola2(xStart);
        Parabola3(xStart);
        Parabola4(yStart);

        if (isDrawingOrigin)
        {
            //DrawOrigin(gridOrigin);
            grid.DrawObject(originDiamond);
        }

        //Debug.Log("Center of screen is at " + ScreenOrigin.x + ", " + ScreenOrigin.y);
        rotatingDiamond.Initalize(DrawingTools.RotatePoint(gridOrigin, -72f * Time.deltaTime, rotatingDiamond.Location),
            Vector3.one * grid.gridSize, Color.cyan);
        grid.DrawObject(rotatingDiamond);
        grid.DrawObject(letterE);
        grid.DrawObject(hex);

        if (isDrawGroupCircle)
        {
            DrawingTools.DrawCircle(DrawingTools.GridToScreen(grid.originGridSpace + new Vector3(-5, -10), grid),
            50, 20, Color.magenta);
            myCircle.Draw();
        }
        else
        {
            DrawingTools.DrawEllipse(DrawingTools.GridToScreen(grid.originGridSpace + new Vector3(-20, -25), grid),
                new Vector3(75, 135), 12, Color.green);
            myEllipse.Draw();
        }
        
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
                grid.origin = new Vector3(grid.origin.x, grid.origin.y + 5 * 0.2f);
                break;
            case MoveDir.left:
                grid.origin = new Vector3(grid.origin.x - 5 * 0.2f, grid.origin.y);
                break;
            case MoveDir.right:
                grid.origin = new Vector3(grid.origin.x + 5 * 0.2f, grid.origin.y);
                break;
            case MoveDir.down:
                grid.origin = new Vector3(grid.origin.x, grid.origin.y - 5 * 0.2f);
                break;
        }
    }

    public void ScaleGridSize(MoveDir dir)
    {
        switch (dir)
        {
            case MoveDir.up:
                grid.gridSize += 2f;
                break;
            case MoveDir.down:
                grid.gridSize -= 2f;
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

    public void Parabola1(float gridLeftEdge)
    {
        Vector3 prevPoint = new Vector3(gridLeftEdge, Mathf.Pow(gridLeftEdge, 2));

        for (int i = 1; i <= grid.divisionCount * grid.gridSize * 2; i++)
        {
            float xPos = gridLeftEdge + (i);

            float yPos = Mathf.Pow(xPos, 2);

            Vector3 newPoint = new Vector3(xPos, yPos);

            grid.DrawLine(new Line(prevPoint, newPoint, Color.cyan));

            prevPoint = newPoint;
        }
    }

    public void Parabola2(float gridLeftEdge)
    {
        Vector3 prevPoint = new Vector3(gridLeftEdge, Mathf.Pow(gridLeftEdge, 2) + (gridLeftEdge * 2) + 1);

        for (int i = 1; i <= grid.divisionCount * grid.gridSize * 2; i++)
        {
            float xPos = gridLeftEdge + (i);

            float yPos = Mathf.Pow(xPos, 2) + (xPos * 2) + 1;

            Vector3 newPoint = new Vector3(xPos, yPos);

            grid.DrawLine(new Line(prevPoint, newPoint, Color.blue));

            prevPoint = newPoint;
        }
    }

    public void Parabola3(float gridLeftEdge)
    {
        Vector3 prevPoint = new Vector3(gridLeftEdge, (-2 * Mathf.Pow(gridLeftEdge, 2)) + (10 * gridLeftEdge) + 12);

        for (int i = 1; i <= grid.divisionCount * grid.gridSize * 2; i++)
        {
            float xPos = gridLeftEdge + (i);

            float yPos = (-2 * Mathf.Pow(xPos,2)) + (10 * xPos) + 12;

            Vector3 newPoint = new Vector3(xPos, yPos);

            grid.DrawLine(new Line(prevPoint, newPoint, Color.green));

            prevPoint = newPoint;
        }
    }

    public void Parabola4(float gridTopEdge)
    {
        Vector3 prevPoint = new Vector3(Mathf.Pow(-gridTopEdge,3), gridTopEdge);

        for (int i = 1; i <= grid.divisionCount * grid.gridSize * 2; i++)
        {
            float yPos = gridTopEdge - (i);

            float xPos = Mathf.Pow(-yPos, 3);

            Vector3 newPoint = new Vector3(xPos, yPos);

            grid.DrawLine(new Line(prevPoint, newPoint, Color.magenta));

            prevPoint = newPoint;
        }
    }
}
