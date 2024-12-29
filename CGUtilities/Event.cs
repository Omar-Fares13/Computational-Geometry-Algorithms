using System;

namespace CGUtilities
{
    public enum EventType
    {
        Start,
        End,
        Intersection
    }

    public class Event : IComparable<Event>
    {
        public Point Location { get; set; }
        public EventType Type { get; set; }
        public Line Segment1 { get; set; }
        public Line? Segment2 { get; set; }

        public Event(Point location, EventType type, Line segment1)
        {
            Location = location;
            Type = type;
            Segment1 = segment1;
            Segment2 = null;
        }

        public Event(Point location, Line segment1, Line segment2)
        {
            if (segment2 == null)
            {
                throw new ArgumentException("Segment2 must be provided for intersection events.");
            }

            Location = location;
            Type = EventType.Intersection;
            Segment1 = segment1;
            Segment2 = segment2;
        }

        public int CompareTo(Event other)
        {
            return Location.CompareTo(other.Location); // Utilize Point's CompareTo
        }
    }
}
