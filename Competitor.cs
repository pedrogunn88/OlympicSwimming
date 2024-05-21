using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlympicSwimming
{
    public class Competitor
    {
        //Fields for storing competitor information.
        private int compNumber;
        private string compName;
        private int compAge;
        private string hometown;
        private bool newPB;
        private CompHistory history;
        //Gets and sets the competitor number & name.
        public int CompNumber { get => compNumber; set => compNumber = value; }
        public string CompName { get => compName; set => compName = value; }
        //Gets and sets the competitor age. Throws an exception if the age is less than or equal to 0.
        public int CompAge
        {
            get => compAge; set
            {
                if (value <= 0) //Validating that the competitor age is greater than 0.
                {
                    throw new ArgumentOutOfRangeException(nameof(CompAge), "Competitor age must be greater than 0.");
                }
                compAge = value;
            }
        }
        //Gets and sets the competitor's hometown, newpb, and history.
        public string Hometown { get => hometown; set => hometown = value; }
        public bool NewPB { get => newPB; set => newPB = value; }
        public CompHistory History { get => history; set => history = value; }
        //Initializes the competitor with the given parameters. Throws an exception if the competitor age is less than or equal to 0.
        public Competitor(int compNumber, string compName, int compAge, string hometown, bool newPB, CompHistory history)
        {
            CompNumber = compNumber;
            CompName = compName;
            CompAge = compAge;
            Hometown = hometown;
            NewPB = newPB;
            History = history;
        }
        //Returns a string representation of the competitor.
        public override string ToString()
        {
            return $"Competitor Number: {CompNumber}\n" +
                   $"Name: {CompName}\n" +
                   $"Age: {CompAge}\n" +
                   $"Hometown: {Hometown}\n" +
                   $"New Personal Best: {NewPB}\n" +
                   $"History: {History}";
        }
        //Returns a string representation of the competitor for saving to a file.
        public string ToFile()
        {
            return $"{CompNumber},{CompName},{CompAge},{Hometown},{NewPB},{History.ToFile()}";
        }
        //Returns true if the competitor has a new personal best, false otherwise, by comparing the race time to the personal best.
        public bool IsNewPB(double raceTime)
        {
            if (history == null || raceTime < history.PersonalBest) 
            {
                history.PersonalBest = raceTime;
                NewPB = true;
                return true;
            }
            return false;
        }
    }
}
