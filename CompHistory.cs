using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlympicSwimming
{
    public class CompHistory
    {
        //Gets and sets the most recent win, career wins, medals, and personal best of the competitor.
        public string MostRecentWin { get; set; }
        public int CareerWins { get; set; }
        public List<string> Medals { get; set; }
        public double PersonalBest { get; set; }

        //Constructor for the CompHistory class, takes in the most recent win, career wins, medals, and personal best of the competitor,
        //will throw an exception if the career wins or personal best are not positive.
        public CompHistory(string mostRecentWin, int careerWins, List<string> medals, double personalBest)
        {
            if (careerWins < 0) //Validating the career wins.
            {
                throw new ArgumentOutOfRangeException(nameof(careerWins), "Career wins must be higher than 0.");
            }

            if (personalBest <= 0) //Validating the personal best.
            {
                throw new ArgumentOutOfRangeException(nameof(personalBest), "Personal best must be a positive number.");
            }
            //Initializing the fields.
            MostRecentWin = mostRecentWin;
            CareerWins = careerWins;
            Medals = medals ?? new List<string>();
            PersonalBest = personalBest;
        }
        //Returns a string representation of the CompHistory object.
        public override string ToString()
        {
            return $"Most Recent Win: {MostRecentWin}, Career Wins: {CareerWins}, Medals: {string.Join(", ", Medals)}, Personal Best: {PersonalBest}";
        }
        //Returns a string representation of the CompHistory object in a format that can be saved to a file.
        public string ToFile()
        {
            return $"{MostRecentWin},{CareerWins},{string.Join(",", Medals)},{PersonalBest}";
        }
    }
}