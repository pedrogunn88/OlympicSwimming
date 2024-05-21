using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace OlympicSwimming
{
    public class Menu
    {
        //Creates a new instance of the Menu.
        private Competition competition;
        //The competition instance.
        public Menu(Competition competition)
        {
            this.competition = competition;
        }
        //Starts the menu.
        public void Start()
        {
            competition.LoadFromFile(); 
            while (true)
            {
                DisplayMenu();
            }
        }
        //Displays the menu & gets the user's choice.
        private void DisplayMenu()
        {
            Console.WriteLine(" ******* Olympic Swimming Qualifiers ******* ");
            Console.WriteLine("1. Add a Competitor");
            Console.WriteLine("2. Delete a Competitor");
            Console.WriteLine("3. Clear All Competitors");
            Console.WriteLine("4. Print All Competitors");
            Console.WriteLine("5. Get Competitors by Event");
            Console.WriteLine("6. Load Data from File");
            Console.WriteLine("7. Save Data to File");
            Console.WriteLine("8. Competitor Index");
            Console.WriteLine("9. Sort Competitors by Age");
            Console.WriteLine("10. Winners");
            Console.WriteLine("11. Get Qualifiers for an Event");
            Console.WriteLine("12. Exit");
            Console.Write("Enter your choice: ");
            int choice;
            if (int.TryParse(Console.ReadLine(), out choice))
            {
                switch (choice)
                {
                    case 1:
                        AddCompetitor(); //Calls the AddCompetitor method defined in Menu class.
                        break;
                    case 2:
                        DeleteCompetitor(); //Calls the DeleteCompetitor method defined in Menu class.
                        break;
                    case 3:
                        ClearAll(); //Calls the ClearAll method defined in Menu class.
                        break;
                    case 4:
                        PrintAllCompetitors(); //Calls the PrintAllCompetitors method defined in Menu class.
                        break;
                    case 5:
                        GetCompetitorsByEvent(); //Calls the GetCompetitorsByEvent method defined in Menu class.
                        break;
                    case 6:
                        competition.LoadFromFile();
                        break;
                    case 7:
                        competition.SaveToFile(); //Calls the SaveToFile method defined in Menu class.
                        break;
                    case 8:
                        DisplayCompetitorIndex(); //Calls the DisplayCompetitorIndex method defined in Menu class.
                        break;
                    case 9:
                        SortCompetitorsByAge(); //Calls the SortCompetitorsByAge method defined in Menu class.
                        break;
                    case 10:
                        DisplayWinners(); //Calls the DisplayWinners method defined in Menu class.
                        break;
                    case 11:
                        DisplayQualifiers(); //Calls the DisplayQualifiers method defined in Menu class.
                        break;
                    case 12:
                        Environment.Exit(0); //Exits the program.
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid option, please select between 1-12.");
            }
            Console.WriteLine();
        }
        //Prompts the user to enter the competitor's details and adds to the database
        private void AddCompetitor()
        {
            Console.WriteLine("Please enter the competitor details:");
            // Validating compNumber
            Console.Write("Comp No (100-999): ");
            int compNo;
            if (!int.TryParse(Console.ReadLine(), out compNo) || compNo < 100 || compNo > 999)
            {
                Console.WriteLine("Invalid comp no. Please enter a number between 100 and 999.");
                return;
            }
            // Validating compName
            Console.Write("Comp Name: ");
            string compName = Console.ReadLine();
            // Validating compAge
            Console.Write("Comp Age: ");
            int compAge;
            if (!int.TryParse(Console.ReadLine(), out compAge) || compAge <= 0)
            {
                Console.WriteLine("Invalid age.");
                return;
            }
            // Validating hometown
            Console.Write("Hometown: ");
            string hometown = Console.ReadLine();
            // Validating eventId
            Console.Write("Event No: ");
            int eventId;
            if (!int.TryParse(Console.ReadLine(), out eventId) || eventId <= 0 || eventId > 100)
            {
                Console.WriteLine("Invalid event no. An event number must be between 1 and 100.");
                return;
            }
            if (!competition.CheckEvent(eventId))
            {
                Console.WriteLine("Event does not exist.");
                return;
            }
            Event ev = competition.GetEvent(eventId);
            // Validating venueInput
            Console.Write("Please enter a venue name: ");
            string venueInput = Console.ReadLine();
            int venueId;
            string venueName;
            if (int.TryParse(venueInput, out venueId))
            {
                venueName = null;
            }
            else
            {
                venueName = venueInput;
                if (string.IsNullOrEmpty(venueName))
                {
                    Console.WriteLine("Venue name cannot be blank.");
                    return;
                }
                venueId = 0;
            }
            // Validating careerWins
            Console.Write("Career Wins: ");
            int careerWins;
            if (!int.TryParse(Console.ReadLine(), out careerWins) || careerWins < 0)
            {
                Console.WriteLine("Invalid career wins.");
                return;
            }
            // Validating mostRecentWin
            Console.Write("Most Recent Win: ");
            string mostRecentWin = Console.ReadLine();
            // Validating medals
            Console.Write("Medals (for example, 1G,2S,B): ");
            List<string> medals = new List<string>(Console.ReadLine().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            // Validating newPB
            Console.Write("New Personal Best (Y/N): ");
            string newPBInput = Console.ReadLine();
            bool newPB = newPBInput.ToUpper() == "Y";
            // Validating placed
            Console.Write("Placed (1-8): ");
            int placed;
            if (!int.TryParse(Console.ReadLine(), out placed) || placed < 1 || placed > 8)
            {
                Console.WriteLine("Invalid placed number. Must be between 1 and 8.");
                return;
            }
            // Validating raceTime
            Console.Write("Race Time: ");
            double raceTime;
            if (!double.TryParse(Console.ReadLine(), out raceTime) || raceTime <= 0)
            {
                Console.WriteLine("Invalid race time. Must be a positive number.");
                return;
            }
            // Validating personalBest
            Console.Write("Personal Best: ");
            double personalBest;
            if (!double.TryParse(Console.ReadLine(), out personalBest) || personalBest <= 0)
            {
                Console.WriteLine("Invalid personal best. Must be a positive number.");
                return;
            }
            // Adds the competitor to the database.
            competition.AddCompetitor(compNo, compName, compAge, hometown, newPB, ev, venueId, venueName, careerWins, mostRecentWin, medals, placed, raceTime, personalBest);
            Console.WriteLine("Added successfully.");
        }
        //Prompts the user to enter the competitor's number and deletes from the database.
        private void DeleteCompetitor()
        {
            Console.Write("Enter competitor no to delete: ");
            if (int.TryParse(Console.ReadLine(), out int compNo))
            {
                competition.RemoveCompetitor(compNo);
            }
            else
            {
                Console.WriteLine("Invalid comp no.");
            }
        }
        //This will clear all competitors from the database.
        private void ClearAll()
        {
            competition.ClearAll(); 
            Console.WriteLine("All competitors cleared.");
        }
        //This will print all competitors from the database.
        private void PrintAllCompetitors()
        {
            Console.WriteLine("All Competitors");
            List<Competitor> competitors = competition.GetAllCompetitors();
            foreach (Competitor comp in competitors)
            {
                Console.WriteLine($"Comp No: {comp.CompNumber} | Name: {comp.CompName} | Age: {comp.CompAge} | Hometown: {comp.Hometown} | New PB: {comp.NewPB}");
            }
        }
        //Prompts the user to enter the event number and prints all competitors for that event.
        private void GetCompetitorsByEvent()
        {
            Console.Write("Event No: ");
            if (!int.TryParse(Console.ReadLine(), out int eventId))
            {
                Console.WriteLine("Invalid Event No.");
                return;
            }

            List<Competitor> competitors = competition.GetCompetitorsByEvent(eventId);

            if (competitors.Count > 0)
            {
                Console.WriteLine($"Event Competitors {eventId}:");
                foreach (Competitor competitor in competitors)
                {
                    Console.WriteLine($"Comp No: {competitor.CompNumber} : Name: {competitor.CompName} : Age: {competitor.CompAge} : Hometown: {competitor.Hometown} : New PB: {competitor.NewPB}");
                }
            }
            else
            {
                Console.WriteLine("None found.");
            }
        }
        //Saves the database to a text file.
        private void SaveToFile()
        {
            competition.SaveToFile();
        }
        //Thus will display the index of competitors.
        private void DisplayCompetitorIndex()
        {
            Dictionary<int, string> compIndex = competition.GetCompetitorIndex();
            if (compIndex.Count == 0)
            {
                Console.WriteLine("Competitor index is empty.");
            }
            else
            {
                Console.WriteLine("Competitor Index:");
                foreach (var kvp in compIndex)
                {
                    Console.WriteLine($"Comp No: {kvp.Key}, Comp Name: {kvp.Value}");
                }
            }
        }
        //This sorts the competitors by age and displays them in ascending order.
        private void SortCompetitorsByAge()
        {
            List<Competitor> competitors = competition.GetCompetitorsSortedByAge();
            if (competitors.Count > 0)
            {
                Console.WriteLine("Competitors Sorted by Age:");
                foreach (var competitor in competitors)
                {
                    Console.WriteLine($"Comp No: {competitor.CompNumber} : Name: {competitor.CompName} : Age: {competitor.CompAge} : Hometown: {competitor.Hometown} : New PB: {competitor.NewPB}");
                }
            }
            else
            {
                Console.WriteLine("No competitors found.");
            }
        }
        //This will display the competitors with 20 or more career wins.
        private void DisplayWinners()
        {
            List<Competitor> winners = competition.Winners();
            if (winners.Count == 0)
            {
                Console.WriteLine("None found.");
                return;
            }
            Console.WriteLine("Competitors with 20 or more career wins:");
            foreach (var winner in winners)
            {
                Console.WriteLine($"Competitor Number: {winner.CompNumber}, Name: {winner.CompName}, Age: {winner.CompAge}, Hometown: {winner.Hometown}");
            }
        }
        //Prompts the user to enter the event number and displays the qualifiers for that event.
        private void DisplayQualifiers()
        {
            Console.Write("Event No: ");
            if (!int.TryParse(Console.ReadLine(), out int eventId))
            {
                Console.WriteLine("Invalid Event ID.");
                return;
            }
            List<(int CompNo, decimal RaceTime)> qualifiers = competition.GetQualifiers(eventId);
            if (qualifiers.Count == 0)
            {
                Console.WriteLine("No qualifiers found.");
                return;
            }
            Console.WriteLine("Qualifiers:");
            foreach (var qualifier in qualifiers)
            {
                Console.WriteLine($"Competitor Number: {qualifier.CompNo}, Race Time: {qualifier.RaceTime}");
            }
        }
    }
}