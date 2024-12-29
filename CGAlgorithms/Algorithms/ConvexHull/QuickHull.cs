using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class QuickHull : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if ( points.Count < 3)
            {
                outPoints = new List<Point>(points);
                return;
            }

            outPoints = new List<Point>();

            Point leftMost = points[0];
            Point rightMost = points[0];

            foreach (var point in points)
            {
                if (point.X < leftMost.X)
                    leftMost = point;
                if (point.X > rightMost.X)
                    rightMost = point;
            }

            outPoints.Add(leftMost);
            outPoints.Add(rightMost);

            List<Point> leftSet = new List<Point>();
            List<Point> rightSet = new List<Point>();

            foreach (var point in points)
            {
                if (point != leftMost && point != rightMost)
                {
                    if (HelperMethods.CheckTurn(new Line(leftMost, rightMost), point) == Enums.TurnType.Left)
                        leftSet.Add(point);
                    else
                        rightSet.Add(point);
                }
            }

            FindHull(leftMost, leftSet, rightMost, ref outPoints);
            FindHull(rightMost, rightSet, leftMost, ref outPoints);
        }


        private void FindHull(Point p1, List<Point> points, Point p2, ref List<Point> outPoints)
        {
            if (points.Count == 0)
                return;

            double maxDistance = double.MinValue;
            Point farthestPoint = points[0];
            foreach (var point in points)
            {
                double distance = DistanceFromLine(p1, p2, point);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    farthestPoint = point;
                }
            }

            outPoints.Add(farthestPoint);

            List<Point> leftSet1 = new List<Point>();
            List<Point> leftSet2 = new List<Point>();

            foreach (var point in points)
            {
                if (point != farthestPoint)
                {
                    if (HelperMethods.CheckTurn(new Line(p1, farthestPoint), point) == Enums.TurnType.Left)
                        leftSet1.Add(point);
                    if (HelperMethods.CheckTurn( new Line(farthestPoint, p2), point) == Enums.TurnType.Left)
                        leftSet2.Add(point);
                }
            }

            FindHull(p1, leftSet1, farthestPoint, ref outPoints);
            FindHull(farthestPoint, leftSet2, p2, ref outPoints);

        }
        public double DistanceFromLine(Point p1, Point p2, Point p)
        {
            return Math.Abs((p.Y - p1.Y) * (p2.X - p1.X) - (p.X - p1.X) * (p2.Y - p1.Y)) / Math.Sqrt(Math.Pow(p2.Y - p1.Y, 2) + Math.Pow(p2.X - p1.X, 2));
        }

        public override string ToString()
        {
            return "Convex Hull - Quick Hull";
        }
    }
}
