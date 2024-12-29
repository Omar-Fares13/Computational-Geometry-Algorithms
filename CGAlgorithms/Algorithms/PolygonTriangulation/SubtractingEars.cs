using System;
using System.Collections.Generic;
using System.Linq;
using CGUtilities;

namespace CGAlgorithms.Algorithms.PolygonTriangulation
{
    class SubtractingEars : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons,
            ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            Polygon polygon = polygons[0];
            List<Point> vertices = GetVerticesFromLines(polygon.lines);

            // Ensure the polygon is counterclockwise
            if (!IsCounterClockwise(vertices))
            {
                vertices.Reverse();
            }

            List<Line> diagonals = new List<Line>();
            int maxIterations = vertices.Count * vertices.Count; // Fail-safe for infinite loops
            int iteration = 0;

            while (vertices.Count > 3 && iteration < maxIterations)
            {
                for (int i = 0; i < vertices.Count; i++)
                {
                    int prev = (i - 1 + vertices.Count) % vertices.Count;
                    int next = (i + 1) % vertices.Count;

                    if (IsEar(vertices, i))
                    {
                        // Add diagonal
                        diagonals.Add(new Line(vertices[prev], vertices[next]));

                        // Remove ear vertex
                        vertices.RemoveAt(i);
                        i--; // Adjust index after removal
                        break;
                    }
                }
                iteration++;
            }

            if (iteration >= maxIterations)
            {
                throw new Exception("Ear clipping algorithm failed: potential infinite loop.");
            }

            outLines = diagonals;
        }

        private List<Point> GetVerticesFromLines(List<Line> lines)
        {
            List<Point> vertices = new List<Point>();
            foreach (Line line in lines)
            {
                if (vertices.Count == 0 || !vertices.Last().Equals(line.Start))
                {
                    vertices.Add(line.Start);
                }
            }
            return vertices;
        }

        private bool IsCounterClockwise(List<Point> vertices)
        {
            double area = 0;
            for (int i = 0; i < vertices.Count; i++)
            {
                Point current = vertices[i];
                Point next = vertices[(i + 1) % vertices.Count];
                area += (next.X - current.X) * (next.Y + current.Y);
            }
            return area < 0; // Counterclockwise if signed area is negative
        }

        private bool IsEar(List<Point> vertices, int earIndex)
        {
            int prev = (earIndex - 1 + vertices.Count) % vertices.Count;
            int next = (earIndex + 1) % vertices.Count;

            Point v1 = vertices[prev];
            Point v2 = vertices[earIndex];
            Point v3 = vertices[next];

            // Check if vertex forms convex angle
            if (!IsConvex(v1, v2, v3))
                return false;

            // Check if any other vertices lie inside the triangle
            for (int i = 0; i < vertices.Count; i++)
            {
                if (i != prev && i != earIndex && i != next)
                {
                    if (IsPointInTriangle(vertices[i], v1, v2, v3))
                        return false;
                }
            }

            return true;
        }

        private bool IsConvex(Point a, Point b, Point c)
        {
            double cross = (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);
            return cross > 0; // Convex if cross product > 0
        }

        private bool IsPointInTriangle(Point p, Point a, Point b, Point c)
        {
            bool b1 = ((p.X - a.X) * (b.Y - a.Y) - (p.Y - a.Y) * (b.X - a.X)) >= 0;
            bool b2 = ((p.X - b.X) * (c.Y - b.Y) - (p.Y - b.Y) * (c.X - b.X)) >= 0;
            bool b3 = ((p.X - c.X) * (a.Y - c.Y) - (p.Y - c.Y) * (a.X - c.X)) >= 0;

            return b1 == b2 && b2 == b3;
        }

        public override string ToString()
        {
            return "Subtracting Ears";
        }
    }
}
