using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PhotoTaggerOM
{
    public class Line
    {
        List<Point> linePoints = new List<Point>();
        Point lowestPoint;
        Line nextLine;
        Line previousLine;

        public Point[] Points
        {
            get
            {
                Point[] points;
                points = new Point[linePoints.Count];
                points = linePoints.ToArray();

                return points;
            }
        }

        public void AddPoint(int x, int y)
        {
            Point newPoint = new Point(x, y);
            AddPoint(newPoint);
        }

        public void AddPoint(Point point)
        {
            
            //set the lowest
            if (linePoints.Count == 0)
            {
                lowestPoint = point;
            }
            else
            {
                if (point.Y < lowestPoint.Y)
                    lowestPoint = point;
            }

            linePoints.Add(point);
        }

        public Point FirstPoint
        {
            get { return linePoints[0]; }
        }

        public Point EndPoint
        {
            get { return linePoints[linePoints.Count - 1]; }
        }

        //This function draws down from the lowest point, and then round the containing rectangle
        public void InvertArea(int width, int height)
        {            
            int insertIndex = linePoints.IndexOf(lowestPoint);
                
            linePoints.Insert(insertIndex + 1, new Point(lowestPoint.X, 0));
            linePoints.Insert(insertIndex + 2, new Point(0, 0));
            linePoints.Insert(insertIndex + 3, new Point(0, height));
            linePoints.Insert(insertIndex + 4, new Point(width, height));
            linePoints.Insert(insertIndex + 5, new Point(width, 0));
            linePoints.Insert(insertIndex + 6, new Point(lowestPoint.X, 0));
            linePoints.Insert(insertIndex + 7, lowestPoint);

        }
    }
}
