using System;
using System.Collections.Generic;
using System.Linq;
using CGUtilities;

namespace CGAlgorithms.Algorithms.SegmentIntersection
{
    class SweepLine : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons,
            ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            // Initialize event queue with line endpoints
            var events = new List<Event>();
            foreach (var line in lines)
            {
                // Ensure start point is to the left of end point
                if (line.Start.X > line.End.X)
                {
                    var temp = line.Start;
                    line.Start = line.End;
                    line.End = temp;
                }

                events.Add(new Event(line.Start, EventType.Start, line));
                events.Add(new Event(line.End, EventType.End, line));
            }

            // Sort events by x-coordinate (and y if x is equal)
            events.Sort();

            // Active segments sorted by y-coordinate at sweep line
            var activeSegments = new SortedSet<Line>(Comparer<Line>.Create((a, b) =>
            {
                // Get y-intersection of segments with sweep line
                double x = events[0].Location.X;  // Current sweep line x position
                double y1 = a.Start.Y + (a.End.Y - a.Start.Y) * (x - a.Start.X) / (a.End.X - a.Start.X);
                double y2 = b.Start.Y + (b.End.Y - b.Start.Y) * (x - b.Start.X) / (b.End.X - b.Start.X);

                if (Math.Abs(y1 - y2) < Constants.Epsilon)
                    return 0;
                return y1 < y2 ? -1 : 1;
            }));

            // Process events
            foreach (var evt in events)
            {
                switch (evt.Type)
                {
                    case EventType.Start:
                        // Check for intersections with segments above and below
                        CheckIntersections(evt.Segment1, activeSegments, outPoints);
                        activeSegments.Add(evt.Segment1);
                        break;

                    case EventType.End:
                        activeSegments.Remove(evt.Segment1);
                        break;

                    case EventType.Intersection:
                        // Handle intersection event
                        if (!outPoints.Any(p => p.Equals(evt.Location)))
                            outPoints.Add(evt.Location);
                        break;
                }
            }
        }

        private void CheckIntersections(Line newSegment, SortedSet<Line> activeSegments, List<Point> outPoints)
        {
            foreach (var segment in activeSegments)
            {
                Point intersection = FindIntersection(newSegment, segment);
                if (intersection != null && !outPoints.Any(p => p.Equals(intersection)))
                {
                    outPoints.Add(intersection);
                }
            }
        }

        private Point FindIntersection(Line line1, Line line2)
        {
            // Calculate line parameters
            double x1 = line1.Start.X, y1 = line1.Start.Y;
            double x2 = line1.End.X, y2 = line1.End.Y;
            double x3 = line2.Start.X, y3 = line2.Start.Y;
            double x4 = line2.End.X, y4 = line2.End.Y;

            // Calculate denominators
            double denom = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
            if (Math.Abs(denom) < Constants.Epsilon)
                return null; // Lines are parallel

            // Calculate intersection point
            double t = ((x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4)) / denom;
            double u = -((x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3)) / denom;

            // Check if intersection is within line segments
            if (t >= 0 && t <= 1 && u >= 0 && u <= 1)
            {
                double x = x1 + t * (x2 - x1);
                double y = y1 + t * (y2 - y1);
                return new Point(x, y);
            }

            return null;
        }

        public override string ToString()
        {
            return "Sweep Line";
        }
    }
}