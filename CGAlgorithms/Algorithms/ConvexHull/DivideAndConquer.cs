using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class DivideAndConquer : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count < 3)
            {
                outPoints = new List<Point>(points);
                return;
            }

            Point leftmostPoint = points.Aggregate((p1, p2) => p1.X < p2.X ? p1 : p2);
            Point rightmostPoint = points.Aggregate((p1, p2) => p1.X > p2.X ? p1 : p2);

            outPoints.Add(leftmostPoint);
            outPoints.Add(rightmostPoint);

            List<Point> aboveLine = points.Where(p => DetermineTurn(leftmostPoint, rightmostPoint, p) == Enums.TurnType.Left).ToList();
            List<Point> belowLine = points.Where(p => DetermineTurn(leftmostPoint, rightmostPoint, p) == Enums.TurnType.Right).ToList();

            ProcessHull(leftmostPoint, rightmostPoint, aboveLine, ref outPoints);
            ProcessHull(rightmostPoint, leftmostPoint, belowLine, ref outPoints);

            for (int i = 0; i < outPoints.Count; i++)
            {
                outLines.Add(new Line(outPoints[i], outPoints[(i + 1) % outPoints.Count]));
            }

            RemoveCollinearPoints(ref outPoints);

            Console.WriteLine("Final Convex Hull Points:");
            foreach (var point in outPoints)
            {
                Console.WriteLine($"({point.X}, {point.Y})");
            }

        }

        private static Enums.TurnType DetermineTurn(Point start, Point end, Point test)
        {
            double crossProduct = (end.X - start.X) * (test.Y - start.Y) - (end.Y - start.Y) * (test.X - start.X);

            if (Math.Abs(crossProduct) < 1e-9)
                return Enums.TurnType.Colinear;
            return crossProduct > 0 ? Enums.TurnType.Left : Enums.TurnType.Right;
        }

        private static void ProcessHull(Point start, Point end, List<Point> points, ref List<Point> hull)
        {
            if (points.Count == 0)
                return;

            Point farthest = points.OrderByDescending(p => CalculateDistanceToLine(start, end, p)).First();

            hull.Insert(hull.IndexOf(end), farthest);

            var leftSet = points.Where(p => DetermineTurn(start, farthest, p) == Enums.TurnType.Left).ToList();
            var rightSet = points.Where(p => DetermineTurn(farthest, end, p) == Enums.TurnType.Left).ToList();

            ProcessHull(start, farthest, leftSet, ref hull);
            ProcessHull(farthest, end, rightSet, ref hull);
        }

        private static double CalculateDistanceToLine(Point start, Point end, Point point)
        {
            double numerator = Math.Abs((end.Y - start.Y) * point.X - (end.X - start.X) * point.Y + end.X * start.Y - end.Y * start.X);
            double denominator = Math.Sqrt(Math.Pow(end.Y - start.Y, 2) + Math.Pow(end.X - start.X, 2));
            return numerator / denominator;
        }

        private static void RemoveCollinearPoints(ref List<Point> convexHull)
        {
            for (int i = 0; i < convexHull.Count; i++)
            {
                Point prev = convexHull[(i - 1 + convexHull.Count) % convexHull.Count];
                Point current = convexHull[i];
                Point next = convexHull[(i + 1) % convexHull.Count];

                if (HelperMethods.PointOnSegment(current, prev, next))
                {
                    convexHull.RemoveAt(i);
                    i--;
                }
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Divide & Conquer";
        }

    }
}
