using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionExample : MonoBehaviour
{
    public Grid2D grid = new Grid2D();

    public Color axisColor = Color.white;
    public Color lineColor = Color.gray;
    public Color divisionColor = Color.yellow;

    public TriangleDrawObj triangle = new TriangleDrawObj();
    public Circle myCircle = new Circle();
    public RectDrawObj rectangle = new RectDrawObj();

    public bool insideCircle = false;
    public bool insideTri = false;
    public bool insideRect = false;

    public Vector3 Point => Input.mousePosition;

    private void Awake()
    {
        grid.origin = new Vector3(Screen.width / 2, Screen.height / 2);
        grid.screenSize = new Vector3(Screen.width, Screen.height);

        triangle.Initalize(new Vector3(-20, 20), Vector3.one * grid.gridSize, Color.cyan);

        rectangle.Initalize(new Vector3(12, -14), new Vector3(25, 15), Color.magenta);

        myCircle.Radius = 100f;
        myCircle.sides = 32;
        myCircle.center = DrawingTools.GridToScreen(new Vector3(15, 18), grid);
    }

    // Update is called once per frame
    void Update()
    {
        #region grid drawing
        float xStart = grid.gridSpaceOrigin.x - (grid.divisionCount * grid.gridSize);
        float yStart = grid.originGridSpace.y + (grid.divisionCount * grid.gridSize);

        for (int i = 0; i <= grid.divisionCount * grid.gridSize; i++)
        {
            Color correctColor = lineColor;

            // X Axis

            float xPos = xStart + (i * 2);

            if (xPos == grid.gridSpaceOrigin.x)
            {
                correctColor = axisColor;
            }
            else if (xPos % 5 == 0)
            {
                correctColor = divisionColor;
            }

            grid.DrawLine(new Line(
                new Vector3(xPos, yStart),
                new Vector3(xPos, grid.gridSpaceOrigin.y - grid.divisionCount * grid.gridSize),
                correctColor));

            // Y Axis

            float yPos = yStart - (i * 2);

            if (yPos == grid.gridSpaceOrigin.x)
            {
                correctColor = axisColor;
            }
            else if (yPos % 5 == 0)
            {
                correctColor = divisionColor;
            }

            grid.DrawLine(new Line(
                new Vector3(xStart, yPos),
                new Vector3(grid.gridSpaceOrigin.x + grid.divisionCount * grid.gridSize, yPos),
                correctColor));
        }
        #endregion

        #region solid drawing

        if (insideCircle)
        {
            for(int i = (int)myCircle.Radius; i > 0; i--)
            {
                DrawingTools.DrawCircle(myCircle.center, i, myCircle.sides, Color.blue);
            }
        }

        if (insideRect)
        {
            Vector3 cornerA_Screen = DrawingTools.GridToScreen(rectangle.CornerA,grid);
            Vector3 cornerC_Screen = DrawingTools.GridToScreen(rectangle.CornerC, grid);
            for (int i = (int)cornerA_Screen.y; i >= (int)cornerC_Screen.y; i--)
            {
                Glint.AddCommand(new Line(new Vector3(cornerA_Screen.x, i), new Vector3(cornerC_Screen.x, i), Color.blue));
            }
        }

        if (insideTri)
        {
            Vector3 PointA_Sc = DrawingTools.GridToScreen(triangle.PointA, grid);
            Vector3 PointB_Sc = DrawingTools.GridToScreen(triangle.PointB, grid);
            Vector3 PointC_Sc = DrawingTools.GridToScreen(triangle.PointC, grid);

            if (PointB_Sc.y < PointA_Sc.y) { Vector3 temp = PointB_Sc; PointB_Sc = PointA_Sc; PointA_Sc = temp; }
            if (PointC_Sc.y < PointA_Sc.y) { Vector3 temp = PointC_Sc; PointC_Sc = PointA_Sc; PointA_Sc = temp; }
            if (PointC_Sc.y < PointB_Sc.y) { Vector3 temp = PointC_Sc; PointC_Sc = PointB_Sc; PointB_Sc = temp; }

            List<float> x01 = DrawingTools.Interpolate(PointA_Sc.y, PointA_Sc.x, PointB_Sc.y, PointB_Sc.x);
            List<float> x12 = DrawingTools.Interpolate(PointB_Sc.y, PointB_Sc.x, PointC_Sc.y, PointC_Sc.x);
            List<float> x02 = DrawingTools.Interpolate(PointA_Sc.y, PointA_Sc.x, PointC_Sc.y, PointC_Sc.x);

            x01.RemoveAt(x01.Count - 1);
            x01.AddRange(x12);
            List<float> x012 = x01;

            List<float> x_left;
            List<float> x_right;

            int m = (int)Mathf.Floor(x012.Count / 2);
            if (x02[m] < x012[m])
            {
                x_left = x02;
                x_right = x012;
            }
            else
            {
                x_left = x012;
                x_right = x02;
            }

            for (float y = PointA_Sc.y; y < PointC_Sc.y - 1; y += 1)
            {
                Glint.AddCommand(new Line(
                    new Vector3(x_left[(int)(y - PointA_Sc.y)], y), 
                    new Vector3(x_right[(int)(y - PointA_Sc.y)], y), 
                    Color.blue));
            }
        }

        #endregion

        grid.DrawObject(triangle);
        grid.DrawObject(rectangle);

        myCircle.Draw();

        #region shape collision detection

        if (Vector3.Distance(Point,myCircle.center) <= myCircle.Radius) { insideCircle = true; }
        else { insideCircle = false; }

        Vector3 pointGridSpace = DrawingTools.ScreenToGrid(Point,grid);

        if((pointGridSpace.x >= rectangle.CornerA.x && pointGridSpace.x <= rectangle.CornerB.x) 
            && (pointGridSpace.y <= rectangle.CornerA.y && pointGridSpace.y >= rectangle.CornerD.y))
        {
            insideRect = true;
        }
        else { insideRect = false; }

        insideTri = DrawingTools.PointInsideTriangle(pointGridSpace, triangle);

        #endregion

        DrawingTools.DrawCircle(Point, 3f, 16, Color.red);
    }

}
