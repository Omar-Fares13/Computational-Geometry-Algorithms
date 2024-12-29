using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CGUtilities.Enums;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class JarvisMarch : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            List<Point> newPoints = points.Distinct().ToList();

            if (newPoints.Count < 3)
            {
                outPoints = new List<Point>(newPoints);
                return;
            }

            Point start = newPoints.OrderBy(p => p.Y).ThenBy(p => p.X).First();
            List<Point> hull = new List<Point>();
            Point lastHullPoint = start;

            do
            {
                hull.Add(lastHullPoint);

                Point nextPoint = newPoints[0];
                foreach (Point candidate in newPoints)
                {
                    if (candidate == lastHullPoint) continue;

                    TurnType turn = HelperMethods.CheckTurn(new Line(lastHullPoint, nextPoint), candidate);
                    if (turn == TurnType.Left ||
                        (turn == TurnType.Colinear && Distance(lastHullPoint, candidate) > Distance(lastHullPoint, nextPoint)))
                    {
                        nextPoint = candidate;
                    }
                }

                lastHullPoint = nextPoint;

            } while (lastHullPoint != start);

            outPoints = hull;


        }


        public static double Distance(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        }


        public override string ToString()
        {
            return "Convex Hull - Jarvis March";
        }
    }
}
