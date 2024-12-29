using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CGUtilities.Enums;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremeSegments : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            List<Point> newPoints = new List<Point>();

            for (int i = 0; i < points.Count; i++)
            {
                if (!newPoints.Contains(points[i]))
                {
                    newPoints.Add(points[i]);
                }
            }

            if (points.Count < 3)
            {
                outPoints = new List<Point>(points);
                return;
            }

            List<Line> hullLines = new List<Line>();

            foreach (Point point1 in newPoints)
            {
                foreach (Point point2 in newPoints)
                {
                    if (point1 == point2) continue;

                    Line candidateLine = new Line(point1, point2);

                    bool hasLeftHalf = false;
                    bool hasRightHalf = false;
                    bool hascolinear = false;
                    foreach (Point pk in newPoints)
                    {
                        TurnType direction = HelperMethods.CheckTurn(candidateLine, pk);
                        if (direction == TurnType.Left)
                        {
                            hasLeftHalf = true;
                        }
                        else if (direction == TurnType.Right)
                        {
                            hasRightHalf = true;
                        }
                        else if (!HelperMethods.PointOnSegment(pk, candidateLine.Start, candidateLine.End))
                        {

                            hascolinear = true;
                            break;
                        }
                        if (hasLeftHalf && hasRightHalf)
                        {
                            break;
                        }
                    }

                    if (!(hasLeftHalf && hasRightHalf) && !hascolinear)
                    {
                        if (!hullLines.Contains(candidateLine))
                        {
                            hullLines.Add(candidateLine);
                        }
                    }

                }
            }

            HashSet<Point> hullPointsSet = new HashSet<Point>();
            foreach (Line line in hullLines)
            {
                hullPointsSet.Add(line.Start);
                hullPointsSet.Add(line.End);
            }

            outPoints = hullPointsSet.ToList();

        }


        public static double Distance(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Segments";
        }


    }
}