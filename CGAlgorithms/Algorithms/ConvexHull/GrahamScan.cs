using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class GrahamScan : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            // Step 1: Remove duplicates
            points = points.Distinct().ToList();
            if (points.Count < 3)
            {
                outPoints = new List<Point>(points);
                return;
            }

            // Step 2: Sort points
            points.Sort((p1, p2) =>
            {
                if (p1.X != p2.X) return p1.X.CompareTo(p2.X);
                return p1.Y.CompareTo(p2.Y);
            });

            // Step 3: Build the lower hull
            List<Point> lowerHull = new List<Point>();
            foreach (var p in points)
            {
                while (lowerHull.Count >= 2 &&
                       CrossProduct(lowerHull[lowerHull.Count - 2], lowerHull[lowerHull.Count - 1], p) <= 0)
                {
                    lowerHull.RemoveAt(lowerHull.Count - 1);
                }
                lowerHull.Add(p);
            }

            // Step 4: Build the upper hull
            List<Point> upperHull = new List<Point>();
            for (int i = points.Count - 1; i >= 0; i--)
            {
                var p = points[i];
                while (upperHull.Count >= 2 &&
                       CrossProduct(upperHull[upperHull.Count - 2], upperHull[upperHull.Count - 1], p) <= 0)
                {
                    upperHull.RemoveAt(upperHull.Count - 1);
                }
                upperHull.Add(p);
            }

            // Step 5: Combine lower and upper hulls
            lowerHull.RemoveAt(lowerHull.Count - 1);
            upperHull.RemoveAt(upperHull.Count - 1);
            outPoints = new List<Point>(lowerHull.Concat(upperHull));
        }

        private static double CrossProduct(Point a, Point b, Point c)
        {
            return (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);
        }

        public override string ToString()
        {
            return "Convex Hull - Graham Scan";
        }
    }
}
