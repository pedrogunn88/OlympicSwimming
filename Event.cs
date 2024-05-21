using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlympicSwimming
{
    public abstract class Event
    {
        //Fields for storing data for an event.
        private int eventNo;
        private string venue;
        private int venueID;
        private DateTime eventDateTime;
        protected double record;
        //Constructor for an event using the venue name, includes the name of the venue, date and time of the event, record time and the event number,
        //an argument exception is thrown if the event number is not between 1 and 100.
        protected Event(int eventNo, string venue, DateTime eventDateTime, double record)
        {
            if (eventNo < 1 || eventNo > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(eventNo), "Event no must be between 1 and 100.");
            }
            this.eventNo = eventNo;
            this.venue = venue ?? throw new ArgumentNullException(nameof(venue));
            this.eventDateTime = eventDateTime;
            this.record = record;
        }
        //Constructor for an event using the venueID, includes the venueID, date and time of the event, record time and the event number,
        //an argument exception is thrown if the event number is not between 1 and 100. I only requested the venue name in the menu class.
        protected Event(int eventNo, int venueID, DateTime eventDateTime, double record)
        {
            if (eventNo < 1 || eventNo > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(eventNo), "Event no must be between 1 and 100.");
            }
            this.eventNo = eventNo;
            this.venueID = venueID;
            this.eventDateTime = eventDateTime;
            this.record = record;
        }
        //Gets and sets the event number, venue name, venueID, date and time of the event & record time.
        public int EventNo { get => eventNo; set => eventNo = value; }
        public string Venue { get => venue; set => venue = value; }
        public int VenueID { get => venueID; set => venueID = value; }
        public DateTime EventDateTime { get => eventDateTime; set => eventDateTime = value; }
        public double Record { get => record; set => record = value; }
    }
    public class BreastStroke : Event
    {
        //Fields for storing data for a breaststroke event.
        private string eventType;
        private int distance;
        private double winningTime;
        private bool newRecord;
        //Constructor for a breaststroke event using the venue name, includes the name of the venue, date and time of the event, rethe event number,
        //event type, distance, winning time and new record, an argument exception is thrown if the distance is not between 50 and 1500 meters &
        //the event number is not between 1 and 100.
        public BreastStroke(int eventNo, string venue, DateTime eventDateTime, double record, string eventType, int distance, double winningTime, bool newRecord)
            : base(eventNo, venue, eventDateTime, record)
        {
            if (distance < 50 || distance > 1500)
            {
                throw new ArgumentOutOfRangeException(nameof(distance), "Distance must be between 50 and 1500 meters.");
            }

            if (winningTime <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(winningTime), "Winning time must be a positive number.");
            }
            this.eventType = eventType;
            this.distance = distance;
            this.winningTime = winningTime;
            this.newRecord = newRecord;
        }
        //Constructor for a breaststroke event using the venue ID, includes the venueID, date and time of the event, rethe event number,
        //event type, distance, winning time and new record, an argument exception is thrown if the distance is not between 50 and 1500 meters &
        //the event number is not between 1 and 100. I only requested the venue name in the menu class.
        public BreastStroke(int eventNo, int venueID, DateTime eventDateTime, double record, string eventType, int distance, double winningTime, bool newRecord)
            : base(eventNo, venueID, eventDateTime, record)
        {
            if (distance < 50 || distance > 1500)
            {
                throw new ArgumentOutOfRangeException(nameof(distance), "Distance must be between 50 and 1500 meters.");
            }

            if (winningTime <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(winningTime), "Winning time must be a positive number.");
            }

            this.eventType = eventType;
            this.distance = distance;
            this.winningTime = winningTime;
            this.newRecord = newRecord;
        }
        //Gets and sets the event type, distance, winning time and new record for the breaststroke event.
        public string EventType { get => eventType; set => eventType = value; }
        public int Distance { get => distance; set => distance = value; }
        public double WinningTime { get => winningTime; set => winningTime = value; }
        public bool NewRecord { get => newRecord; set => newRecord = value; }
        //Returns a string representation of the event.
        public override string ToString()
        {
            return $"Event No: {EventNo}, Venue: {Venue}, Event Date: {EventDateTime}, EventType: {eventType}, Distance: {distance}, Winning Time: {winningTime}, New Record: {newRecord}";
        }
        //Returns a string representation of the event in a file format.
        public string ToFile()
        {
            return $"{EventNo},{Venue},{EventDateTime},{eventType},{distance},{winningTime},{newRecord}";
        }
        //Returns true if the winning time is a new record for the event or false if it is not.
        public bool IsNewRecord()
        {
            if (newRecord)
            {
                Record = winningTime;
                return true;
            }
            return false;
        }
    }
}