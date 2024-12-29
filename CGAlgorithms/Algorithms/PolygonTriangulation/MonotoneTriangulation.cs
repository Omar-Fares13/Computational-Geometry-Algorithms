using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGUtilities;

namespace CGAlgorithms.Algorithms.PolygonTriangulation
{
    class MonotoneTriangulation : Algorithm
    {
        public override void Run(List<Point> Points, List<Line> inputLines, List<Polygon> inputPolygons, ref List<Point> outputPoints, ref List<Line> outputLines, ref List<Polygon> outputPolygons)
        {
            
        }

        public override string ToString()
        {
            return "Monotone Triangulation";
        }
    }
}