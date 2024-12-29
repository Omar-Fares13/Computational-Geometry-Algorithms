using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class Incremental : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count < 3)
            {
                outPoints = new List<Point>(points);
                return;
            }

            // Sort points by x-coordinate, breaking ties by y-coordinate
            points = points.OrderBy(p => p.X).ThenBy(p => p.Y).ToList();

            // Initialize the convex hull with the first two points
            List<Point> convexHull = new List<Point> { points[0], points[1] };

            for (int i = 2; i < points.Count; i++)
            {
                Point currentPoint = points[i];

                // Add the current point to the hull
                convexHull.Add(currentPoint);

                // Fix the upper hull
                while (convexHull.Count > 2 && !IsTurnLeft(convexHull[convexHull.Count - 3], convexHull[convexHull.Count - 2], convexHull[convexHull.Count - 1]))
                {
                    convexHull.RemoveAt(convexHull.Count - 2); // Remove the second-to-last point
                }
            }

            // Close the loop by adding the points in reverse for the lower hull
            int lowerHullStart = convexHull.Count;
            for (int i = points.Count - 2; i >= 0; i--)
            {
                Point currentPoint = points[i];

                // Add the current point to the hull
                convexHull.Add(currentPoint);

                // Fix the lower hull
                while (convexHull.Count > lowerHullStart + 1 && !IsTurnLeft(convexHull[convexHull.Count - 3], convexHull[convexHull.Count - 2], convexHull[convexHull.Count - 1]))
                {
                    convexHull.RemoveAt(convexHull.Count - 2); // Remove the second-to-last point
                }
            }

            // Remove the duplicate of the starting point
            convexHull.RemoveAt(convexHull.Count - 1);

            outPoints = convexHull;

            // Generate lines for visualization
            for (int i = 0; i < outPoints.Count; i++)
            {
                outLines.Add(new Line(outPoints[i], outPoints[(i + 1) % outPoints.Count]));
            }
        }

        private bool IsTurnLeft(Point p1, Point p2, Point p3)
        {
            // Check the cross product to determine the turn direction
            double crossProduct = (p2.X - p1.X) * (p3.Y - p1.Y) - (p2.Y - p1.Y) * (p3.X - p1.X);
            return crossProduct > 0; // True if it's a left turn
        }

        public override string ToString()
        {
            return "Convex Hull - Incremental";
        }
    }
}
