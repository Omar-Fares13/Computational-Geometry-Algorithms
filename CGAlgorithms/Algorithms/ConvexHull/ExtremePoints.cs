using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CGUtilities.Enums;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremePoints : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            // test case 18    remove redandunt points 
            List<Point> newPoints = new List<Point>();

            for (int i = 0; i < points.Count; i++)
            {
                if (!newPoints.Contains(points[i]))
                    newPoints.Add(points[i]);
            }


            // corner cases
            if (points.Count <= 3 ) {
                outPoints = new List<Point>(points);
                return ;
            }


            // general code
            foreach (Point point in newPoints)
            {
                bool isExtreme = true;

                foreach (Point i in newPoints)
                {

                    foreach (Point j in newPoints)
                    {

                        foreach (Point k in newPoints)
                        {

                            if (point != i && point != j && point != k)
                            {
                                PointInPolygon isinside = HelperMethods.PointInTriangle(point, i, j, k); 

                                if (isinside  != PointInPolygon.Outside )
                                { 
                                    isExtreme = false;
                                    break ;
                                }
                            }
                        }

                        if (!isExtreme) break;

                    }

                    if (!isExtreme) break;
                }

                if (isExtreme)
                    outPoints.Add(point);
            }
            

        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Points";
        }
    }
}
