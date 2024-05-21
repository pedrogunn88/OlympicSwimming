using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlympicSwimming
{
    public class Result
    {
        //Fields for storing result data.
        private int placed;
        private double raceTime;
        private bool qualified;
        //Constructor for creating a result object,placed must be between 1 and 8 and race time must be a positive value, raceTime must be
        //a positive value & qualified must be true or false, if placed is not between 1 and 8 or race time is not a positive value then
        //the argumentoutofrangeexception will be thrown.
        public Result(int placed, double raceTime, bool qualified) 
        {
            if (placed < 1 || placed > 8) 
            {
                throw new ArgumentOutOfRangeException(nameof(placed), "Placed must be between 1 and 8.");
            }
            if (raceTime <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(raceTime), "Race time must be a positive number.");
            }
            //Initialize the fields.
            this.placed = placed;
            this.raceTime = raceTime;
            this.qualified = qualified;
        }
        //Properties for accessing the result data.
        public int Placed { get => placed; }
        public double RaceTime { get => raceTime; }
        public bool Qualified { get => qualified; }

        //Returns a string representation of the result object.
        public override string ToString()
        {
            return $"Placed: {Placed}, Race Time: {RaceTime}, Qualified: {Qualified}";
        }
        //For checking if the competitor has qualified, >3 hasn't qualified & <=3 has qualified.
        public bool IsQualified()
        {
            return Placed <= 3;
        }
    }
}