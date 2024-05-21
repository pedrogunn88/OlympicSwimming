using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlympicSwimming
{
    //Creates the new instance of the Competition class using the database connection string.
    public class Competition
    {
        private string connectionString; 
        public Competition(string connectionString) //The connection string to connect to the database.
        {
            this.connectionString = connectionString;
        }
        //For adding a competitor and all the relevant details to the database.
        public void AddCompetitor(int compNo, string name, int age, string hometown, bool isNewPB, Event ev, int venueId, string venueName, int careerWins, string mostRecentWin, List<string> medals, int placed, double raceTime, double personalBest)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                //SQL query to add the competitor to the database using the competitors table.
                string query = "INSERT INTO competitors (CompNo, CompName, CompAge, Hometown, NewPB, CareerWins, MostRecentWin, Medals, PersonalBest) " +
                               "VALUES (@CompNo, @Name, @Age, @Hometown, @NewPB, @CareerWins, @MostRecentWin, @Medals, @PersonalBest)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@CompNo", compNo);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Age", age);
                command.Parameters.AddWithValue("@Hometown", hometown);
                command.Parameters.AddWithValue("@NewPB", isNewPB);
                command.Parameters.AddWithValue("@CareerWins", careerWins);
                command.Parameters.AddWithValue("@MostRecentWin", mostRecentWin);
                command.Parameters.AddWithValue("@Medals", string.Join(", ", medals));
                command.Parameters.AddWithValue("@PersonalBest", personalBest);
                //SQL query to add the competitor to the database using the performances table.
                string performanceQuery = "INSERT INTO performances (EventNo, CompNo, RaceTime, Placed, Qualified) " +
                                          "VALUES (@EventNo, @CompNo, @RaceTime, @Placed, @Qualified)";
                MySqlCommand performanceCommand = new MySqlCommand(performanceQuery, connection);
                performanceCommand.Parameters.AddWithValue("@EventNo", ev.EventNo);
                performanceCommand.Parameters.AddWithValue("@CompNo", compNo);
                performanceCommand.Parameters.AddWithValue("@RaceTime", raceTime);
                performanceCommand.Parameters.AddWithValue("@Placed", placed);
                performanceCommand.Parameters.AddWithValue("@Qualified", placed <= 3);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    performanceCommand.ExecuteNonQuery();
                    Console.WriteLine("Competitor and performance data added successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error adding competitor and performance data: {ex.Message}");
                }
            }
        }
        // For removing a competitor from the database using the relevant competitor number (CompNo).
        public void RemoveCompetitor(int compNo)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                // Check if the competitor exists in the database
                string checkQuery = "SELECT COUNT(*) FROM competitors WHERE CompNo = @CompNo";
                MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@CompNo", compNo);
                try
                {
                    connection.Open();
                    int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                    if (count == 0)
                    {
                        Console.WriteLine("Invalid comp no. Competitor does not exist.");
                        return;
                    }
                    // SQL query to remove the competitor from the database using the competitors table.
                    string query = "DELETE FROM competitors WHERE CompNo = @CompNo";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CompNo", compNo);
                    command.ExecuteNonQuery();

                    Console.WriteLine("Competitor deleted successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting competitor: {ex.Message}");
                }
            }
        }
        //For clearing all the competitors from the database.
        public void ClearAll()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "DELETE FROM competitors";
                MySqlCommand command = new MySqlCommand(query, connection);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    Console.WriteLine("All competitors cleared successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error clearing competitors: {ex.Message}");
                }
            }
        }
        //Retrieves all the competitors from the database and displays in a user friendly format.
        public List<Competitor> GetAllCompetitors()
        {
            List<Competitor> competitors = new List<Competitor>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM competitors";
                MySqlCommand command = new MySqlCommand(query, connection);
                try
                {
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                Competitor competitor = new Competitor(
                                    Convert.ToInt32(reader["CompNo"]), reader["CompName"].ToString(),
                                    Convert.ToInt32(reader["CompAge"]), reader["Hometown"].ToString(),
                                    Convert.ToBoolean(reader["NewPB"]),
                                    new CompHistory(
                                    reader["MostRecentWin"].ToString(), Convert.ToInt32(reader["CareerWins"]),
                                    new List<string>(reader["Medals"].ToString().Split(new[] { ", " }, StringSplitOptions.None)), Convert.ToDouble(reader["PersonalBest"])
                                    )
                                );
                                competitors.Add(competitor);
                            }
                            catch (ArgumentOutOfRangeException ex)
                            {
                                Console.WriteLine($"Error processing competitor {reader["CompNo"]}: {ex.Message}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error retrieving competitors: {ex.Message}");
                }
            }
            return competitors;
        }
        //Retrieves all the competitors for a specific event from the database using the event number (EventNo).
        public List<Competitor> GetCompetitorsByEvent(int eventNo)
        {
            // Check if the event exists
            if (!CheckEvent(eventNo))
            {
                Console.WriteLine("Invalid event no. Event does not exist.");
                return new List<Competitor>();
            }
            List<Competitor> competitors = new List<Competitor>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                //SQL query to retrieve all the competitors for a specific event from the database using the competitors table and joining the performances table.
                string query = "SELECT c.CompNo, c.CompName, c.CompAge, c.Hometown, c.NewPB, c.MostRecentWin, c.CareerWins, c.Medals, c.PersonalBest " + "FROM competitors c " +
                               "INNER JOIN performances p ON c.CompNo = p.CompNo " + "WHERE p.EventNo = @EventNo";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@EventNo", eventNo);
                try
                {
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                Competitor competitor = new Competitor(
                                    Convert.ToInt32(reader["CompNo"]), reader["CompName"].ToString(),
                                    Convert.ToInt32(reader["CompAge"]), reader["Hometown"].ToString(),
                                    Convert.ToBoolean(reader["NewPB"]),
                                    new CompHistory(
                                    reader["MostRecentWin"].ToString(), Convert.ToInt32(reader["CareerWins"]),
                                    new List<string>(reader["Medals"].ToString().Split(new[] { ", " }, StringSplitOptions.None)), Convert.ToDouble(reader["PersonalBest"])
                                    )
                                );
                                competitors.Add(competitor);
                            }
                            catch (ArgumentOutOfRangeException ex)
                            {
                                Console.WriteLine($"Error processing competitor {reader["CompNo"]}: {ex.Message}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error retrieving competitors: {ex.Message}");
                }
            }
            return competitors;
        }
        //Not sure if i need to implement this as im using the database?
        public void LoadFromFile()
        {
            Console.WriteLine("Loading data from file...");
        }
        //For saving all the competitor information from the database to a txt file called olympicswimmingdata.txt on the desktop.
        public void SaveToFile()
        {
            Console.WriteLine("Saving data to file...");
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktopPath, "olympicswimmingdata.txt");
            List<string> lines = new List<string>();
            List<Competitor> competitors = GetAllCompetitors();
            foreach (Competitor comp in competitors)
            {
            lines.Add(comp.ToFile());
            }
            File.WriteAllLines(filePath, lines);
        }
        //This creates a dictionary of competitors using their numbers and names.
        public Dictionary<int, string> GetCompetitorIndex()
        {
            Dictionary<int, string> compIndex = new Dictionary<int, string>();
            foreach (Competitor competitor in GetAllCompetitors())
            {
            compIndex.Add(competitor.CompNumber, competitor.CompName);
            }
            return compIndex;
        }
        //This will display the list of competitors sorted by their age in ascending order.
        public List<Competitor> GetCompetitorsSortedByAge()
        {
            List<Competitor> competitors = GetAllCompetitors();
            competitors.Sort((x, y) => x.CompAge.CompareTo(y.CompAge));
            return competitors;
        }
        //Displays the list of competitors with 20 or more career wins.
        public List<Competitor> Winners()
        {
            List<Competitor> winners = new List<Competitor>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                //SQL query to retrieve all the competitors with 20 or more career wins from the database using the competitors table.
                string query = "SELECT * FROM competitors WHERE CareerWins > 20";
                MySqlCommand command = new MySqlCommand(query, connection);
                try
                {
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                Competitor competitor = new Competitor(
                                    Convert.ToInt32(reader["CompNo"]), reader["CompName"].ToString(),
                                    Convert.ToInt32(reader["CompAge"]), reader["Hometown"].ToString(),
                                    Convert.ToBoolean(reader["NewPB"]),
                                    new CompHistory(
                                    reader["MostRecentWin"].ToString(), Convert.ToInt32(reader["CareerWins"]),
                                    new List<string>(reader["Medals"].ToString().Split(new[] { ", " }, StringSplitOptions.None)), Convert.ToDouble(reader["PersonalBest"])
                                    )
                                );
                                winners.Add(competitor);
                            }
                            catch (ArgumentOutOfRangeException ex)
                            {
                                Console.WriteLine($"Error processing competitor {reader["CompNo"]}: {ex.Message}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error retrieving winners: {ex.Message}");
                }
            }
            return winners;
        }
        // Displays all qualified competitors and their race times for a specific event from the database using the event number (EventNo).
        public List<(int CompNo, decimal RaceTime)> GetQualifiers(int eventNo)
        {
            List<(int CompNo, decimal RaceTime)> qualifiers = new List<(int CompNo, decimal RaceTime)>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                // SQL query to retrieve the competitor number (CompNo) and race time (RaceTime) for a specific event from the database.
                string query = @"
            SELECT c.CompNo, p.RaceTime FROM competitors c JOIN performances p ON c.CompNo = p.CompNo WHERE p.EventNo = @EventNo AND p.Qualified = 1";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@EventNo", eventNo);
                try
                {
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        // Loop through the results and add the competitor number and race time to the list of qualifiers.
                        while (reader.Read())
                        {
                            int compNo = Convert.ToInt32(reader["CompNo"]);
                            decimal raceTime = Convert.ToDecimal(reader["RaceTime"]);
                            qualifiers.Add((compNo, raceTime));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error retrieving qualifiers: {ex.Message}");
                }
            }
            return qualifiers;
        }
        //This will check if a competitor already exists in the database using the competitor number, it will return true if they do and false if they dont.
        public bool CheckCompetitor(int compNo)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM competitors WHERE CompNo = @CompNo";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@CompNo", compNo);
                try
                {
                    connection.Open();
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error checking competitor: {ex.Message}");
                    return false;
                }
            }
        }
        //This will retrive the event from the database using the event number, it will return the event if it exists and null if it doesn't.
        public Event GetEvent(int eventId)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                //SQL query to check if an event already exists in the database using the event number from the database events table.
                string query = "SELECT * FROM events WHERE EventNo = @EventNo";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@EventNo", eventId);
                try
                {
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string venue = reader["Venue"].ToString();
                            DateTime eventDate = Convert.ToDateTime(reader["EventDate"]);
                            string eventType = reader["EventType"].ToString();
                            int distance = Convert.ToInt32(reader["Distance"]);
                            double winningTime = Convert.ToDouble(reader["WinningTime"]);
                            bool newRecord = Convert.ToBoolean(reader["NewRecord"]);
                            Event ev;
                            switch (eventType)
                            {
                                case "Breaststroke":
                                ev = new BreastStroke(eventId, venue, eventDate, 0, eventType, distance, winningTime, newRecord);
                                break;
                                //other event types? (remember to implement dumb dumb!)
                                default:
                                ev = null;
                                 break;
                            }
                            return ev;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error retrieving event: {ex.Message}");
                }
            }
            return null;
        }
        //This will check if an event already exists in the database using the event number, it will return true if it does and false if it doesn't.
        public bool CheckEvent(int eventId)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            { //SQL query to check if an event already exists in the database using the events table.
                string query = "SELECT COUNT(*) FROM events WHERE EventNo = @EventNo";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@EventNo", eventId);
                try
                {
                    connection.Open();
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error checking event: {ex.Message}");
                    return false;
                }
            }
        }
    }
}